using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace WPF_Project_1
{
    class TunerViewModel : INotifyPropertyChanged
    {
        private Tuner _tuner;

        private float _sliderPosition;

        public float SliderPosition
        {
            get { return _sliderPosition; }
            set { _sliderPosition = value;
                OnPropertyChanged();
            }
        }

        private float _currentFrequency;

        public float CurrentFrequency
        {
            get => _currentFrequency;
            set
            {
                if (_currentFrequency != value)
                {
                    _currentFrequency = value;
                    OnPropertyChanged();
                   
                }
            }
        }

        private string _currentNoteName;

        public string CurrentNoteName
        {
            get { return _currentNoteName; }
            set { _currentNoteName = value;
                OnPropertyChanged();
            }
        }

        private float _targetFrequency;

        public float TargetFrequency
        {
            get { return _targetFrequency; }
            set { _targetFrequency = value;
                OnPropertyChanged();
            }
        }

        private float _centsDeviation;

        public float CentsDeviation
        {
            get { return _centsDeviation; }
            set { _centsDeviation = value;
                OnPropertyChanged();
            }
        }

        private bool  _isRecording;

        public bool IsRecording
        {
            get { return _isRecording; }
            set { _isRecording = value;
                OnPropertyChanged();
            }
        }

        private TuneStatus _currentTuneState = TuneStatus.Waiting;
        public TuneStatus CurrentTuneState
        {
            get => _currentTuneState;
            set
            {
                _currentTuneState = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TunerViewModel()
        {

            _tuner = new Tuner();
            _tuner.ChangedFrequency += OnTunerChangedFrequency;

            _tuner.ChangedFrequency += LogicModel_FrequencyGenerated;

            ToggleTunerCommand = new RelayCommand(() => ExecuteToggleTuner(null));
            SelectStringCommand = new RelayCommand<string>(ExecuteSelectedString);
        }
        private void LogicModel_FrequencyGenerated(object sender, float generatedNumber)
        {
            CurrentFrequency = generatedNumber;
        }

        public ICommand ToggleTunerCommand { get; }
        public ICommand SelectStringCommand { get; }

        private void ExecuteSelectedString(string data)
        {
            var parts = data.Split('|');
            if (parts.Length == 2)
            {
                CurrentNoteName = parts[0];
                TargetFrequency = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                CentsDeviation = 0;
            }
        }

        private void ExecuteToggleTuner(object parameter)
        {
            if (IsRecording)
            {
                _tuner.StopGenerating();
                IsRecording = false;
                SliderPosition = 0; 
            }
            else
            {
                _tuner.StartGenerating();
                IsRecording = true;
            }
        }

        public void UpdatePitch(float measuredHz)
        {
            if (measuredHz <= 0 || TargetFrequency <= 0)
                return;
            double cents = 1200.0 * Math.Log(measuredHz / TargetFrequency, 2);
            cents = Math.Clamp(cents, -500, 500);
            double visualMultiplier = 0.46;

            if(cents < -5) CurrentTuneState = TuneStatus.TooLow;
            else if (cents > 5) CurrentTuneState = TuneStatus.TooHigh;
            else CurrentTuneState = TuneStatus.Perfect;


            //float targetPixels = (float)(cents * visualMultiplier);
            //SliderPosition += (float)((targetPixels - SliderPosition) * 0.2);
            SliderPosition = (float)(cents * visualMultiplier);
        }

        private void OnTunerChangedFrequency(object sender, float frequency)
        {
            if (Application.Current?.Dispatcher != null)
                Application.Current.Dispatcher.InvokeAsync(() => UpdatePitch(frequency));
            else
                UpdatePitch(frequency);
        }


    }

    public enum TuneStatus
    {
        Waiting,
        TooLow,
        Perfect,
        TooHigh
    }
}
