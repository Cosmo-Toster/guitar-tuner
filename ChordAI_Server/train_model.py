import numpy as np
from sklearn.ensemble import RandomForestClassifier
import joblib

# 1. Ідеальні шаблони акордів (Індекси: C, C#, D, D#, E, F, F#, G, G#, A, A#, B)
ideal_chords = {
    # Мажорні акорди (Major)
    "C Major": [1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0], # C, E, G
    "D Major": [0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0], # D, F#, A
    "E Major": [0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 1.0], # E, G#, B
    "F Major": [1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0], # F, A, C
    "G Major": [0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0], # G, B, D
    "A Major": [0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0], # A, C#, E
    
    # Мінорні акорди (Minor)
    "A Minor": [1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0], # A, C, E
    "E Minor": [0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0], # E, G, B
    "D Minor": [0.0, 0.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0], # D, F, A
    "B Minor": [0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0]  # B, D, F# (часто грається з баре)
}

X = [] # Тут будуть зберігатися звукові вектори
y = [] # Тут будуть зберігатися правильні відповіді (назви акордів)

print("Генеруємо синтетичний датасет...")

# 2. Створюємо по 1000 шумних варіацій для кожного акорду
for chord_name, template in ideal_chords.items():
    template_array = np.array(template)
    
    for _ in range(1000):
        # Додаємо сильний випадковий шум (імітація реальної гітари)
        noise = np.random.normal(0.0, 0.35, 12) 
        noisy_vector = np.clip(template_array + noise, 0.0, 1.0)
        
        X.append(noisy_vector)
        y.append(chord_name)

# 3. Додаємо клас "Тиша / Незрозумілий шум", щоб модель не вгадувала акорди там, де їх немає
for _ in range(1000):
    noise_vector = np.random.uniform(0.0, 0.4, 12)
    X.append(noise_vector)
    y.append("Тиша")

print(f"Створено {len(X)} прикладів для навчання.")

# 4. Створюємо та навчаємо ШІ
print("Навчаємо нейромережу (Random Forest)...")
model = RandomForestClassifier(n_estimators=100, max_depth=10, random_state=42)
model.fit(X, y)

# 5. Зберігаємо "мозок" у файл
joblib.dump(model, "chord_model.pkl")
print("Готово! Модель збережено у файл 'chord_model.pkl'.")