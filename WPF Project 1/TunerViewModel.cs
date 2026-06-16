using NAudio.Wave;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Project_1
{
    class TunerViewModel : INotifyPropertyChanged
    {

        private Tuner _tuner;
        private float _currentFrequency;

        // Властивість, до якої буде прив'язаний інтерфейс
        public float CurrentFrequency
        {
            get => _currentFrequency;
            set
            {
                if (_currentFrequency != value)
                {
                    _currentFrequency = value;
                    OnPropertyChanged(); // Сповіщаємо WPF про зміну
                }
            }
        }

        public TunerViewModel()
        {
            _tuner = new Tuner();

            // 1. Підписуємося на подію з нашого класу логіки
            _tuner.ChangedFrequency += LogicModel_FrequencyGenerated;

            // 2. Запускаємо логіку
            _tuner.StartGenerating();
        }

        // Цей метод викликається щоразу, коли логіка генерує нове число
        private void LogicModel_FrequencyGenerated(object sender, float generatedNumber)
        {
            // Оновлюємо властивість. OnPropertyChanged спрацює автоматично.
            CurrentFrequency = generatedNumber;
        }

        // --- Стандартна реалізація INotifyPropertyChanged ---
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
