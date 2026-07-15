import joblib
import numpy as np
import librosa
import uvicorn
from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI()

try:
    model = joblib.load("chord_model.pkl")
    print("Модель ШІ успішно завантажена!")
except Exception as e:
    print("Помилка: файл моделі не знайдено. Спочатку запустіть train_model.py")
    model = None

class AudioData(BaseModel):
    samples: list[float]

@app.post("/predict")
async def predict_chord(data: AudioData):
    if not data.samples or model is None:
        return {"chord": "Тиша", "confidence": 0}

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
        print(f"Помилка аналізу: {e}")
        return {"chord": "Помилка", "confidence": 0}


if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=8000, log_config=None)