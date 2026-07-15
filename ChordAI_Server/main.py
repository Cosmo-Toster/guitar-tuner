import joblib
import numpy as np
from typing import List
import librosa
import uvicorn
from fastapi import FastAPI
from pydantic import BaseModel
import os
import sys

app = FastAPI()

# Визначаємо шлях
if getattr(sys, 'frozen', False):
    application_path = os.path.dirname(sys.executable)
else:
    application_path = os.path.dirname(os.path.abspath(__file__))

model_path = os.path.join(application_path, "chord_model.pkl")
model_error = "Невідома помилка"

# Спроба завантажити модель
try:
    model = joblib.load(model_path)
except Exception as e:
    model = None
    model_error = str(e) # Запам'ятовуємо точний текст помилки!

class AudioData(BaseModel):
    samples: List[float]

@app.post("/predict")
async def predict_chord(data: AudioData):
    # ДІАГНОСТИКА: Якщо моделі немає, виводимо помилку прямо у відповідь
    if model is None:
        return {"chord": f"Помилка: {model_error} | Шлях: {model_path}", "confidence": 0}
    
    # ДІАГНОСТИКА: Якщо звук не дійшов
    if not data.samples:
        return {"chord": "Тиша (звук не дійшов до сервера)", "confidence": 0}

    y = np.array(data.samples, dtype=np.float32)

    try:
        y_harmonic, _ = librosa.effects.hpss(y)
        chroma = librosa.feature.chroma_cqt(
            y=y_harmonic, 
            sr=48000, 
            fmin=librosa.note_to_hz('E2'), 
            n_chroma=12
        )
        chroma_vector = np.mean(chroma, axis=1)
        
        max_val = np.max(chroma_vector)
        if max_val > 0:
            chroma_vector = chroma_vector / max_val
            
        chroma_vector = chroma_vector.reshape(1, -1)

        prediction = model.predict(chroma_vector)[0]
        probabilities = model.predict_proba(chroma_vector)[0]
        confidence = float(max(probabilities)) * 100.0

        return {"chord": str(prediction), "confidence": round(confidence, 1)}
    except Exception as e:
        return {"chord": f"Помилка аналізу: {e}", "confidence": 0}

if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000, log_config=None)