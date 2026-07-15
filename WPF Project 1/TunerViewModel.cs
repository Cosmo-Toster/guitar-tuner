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
<<<<<<< HEAD
        private Tuner _analyzer;
=======
        private Tuner _tuner;
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203

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
<<<<<<< HEAD
        }

        private float _centsDeviation;

        public float CentsDeviation
        {
            get { return _centsDeviation; }
            set { _centsDeviation = value;
                OnPropertyChanged();
            }
        }

        private bool  _isRecording = false;

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

        private bool _isAutoMode = true; 
        public bool IsAutoMode
        {
            get => _isAutoMode;
            set
            {
                _isAutoMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsManualMode)); 
            }
        }
        public bool IsManualMode => !IsAutoMode;

       
        

        
        private readonly Dictionary<string, float> _standardStrings = new Dictionary<string, float>
{
    { "E2 (6)", 82.41f },
    { "A2 (5)", 110.00f },
    { "D3 (4)", 146.83f },
    { "G3 (3)", 196.00f },
    { "B3 (2)", 246.94f },
    { "E4 (1)", 329.63f }
};

=======
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

>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

<<<<<<< HEAD
        public TunerViewModel(Tuner sharedAnalyzer)
        {


            _analyzer = sharedAnalyzer;

            _analyzer.ChangedFrequency += OnTunerChangedFrequency;

            _analyzer.ChangedFrequency += LogicModel_FrequencyGenerated;

            SetAutoModeCommand = new RelayCommand(() => IsAutoMode = true);
            SetManualModeCommand = new RelayCommand(() => IsAutoMode = false);

            ToggleTunerCommand = new RelayCommand(() => ExecuteToggleTuner(null));
            SelectStringCommand = new RelayCommand<string>(ExecuteSelectedString);
            IsRecording = _analyzer.IsRecordingActive;
=======
        public TunerViewModel()
        {

            _tuner = new Tuner();
            _tuner.ChangedFrequency += OnTunerChangedFrequency;

            _tuner.ChangedFrequency += LogicModel_FrequencyGenerated;

            ToggleTunerCommand = new RelayCommand(() => ExecuteToggleTuner(null));
            SelectStringCommand = new RelayCommand<string>(ExecuteSelectedString);
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
        }
        private void LogicModel_FrequencyGenerated(object sender, float generatedNumber)
        {
            CurrentFrequency = generatedNumber;
        }

        public ICommand ToggleTunerCommand { get; }
        public ICommand SelectStringCommand { get; }

<<<<<<< HEAD
        public ICommand SetAutoModeCommand { get; }
        public ICommand SetManualModeCommand { get; }

        private void ExecuteSelectedString(string data)
        {
            IsAutoMode = false;
=======
        private void ExecuteSelectedString(string data)
        {
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
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
<<<<<<< HEAD
                _analyzer.StopGenerating();
=======
                _tuner.StopGenerating();
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
                IsRecording = false;
                SliderPosition = 0; 
            }
            else
            {
<<<<<<< HEAD
                _analyzer.StartGenerating();
=======
                _tuner.StartGenerating();
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
                IsRecording = true;
            }
        }

        public void UpdatePitch(float measuredHz)
        {
<<<<<<< HEAD
            if (IsAutoMode && CurrentFrequency > 0)
            {
                var closestString = _standardStrings
                    .OrderBy(s => Math.Abs(s.Value - CurrentFrequency))
                    .First();

       
                CurrentNoteName = closestString.Key;
                TargetFrequency = closestString.Value;
            }

=======
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
            if (measuredHz <= 0 || TargetFrequency <= 0)
                return;
            double cents = 1200.0 * Math.Log(measuredHz / TargetFrequency, 2);
            cents = Math.Clamp(cents, -500, 500);
            double visualMultiplier = 0.46;

<<<<<<< HEAD
            if(cents < -25) CurrentTuneState = TuneStatus.TooLow;
            else if (cents > 25) CurrentTuneState = TuneStatus.TooHigh;
            else CurrentTuneState = TuneStatus.Perfect;

=======
            if(cents < -5) CurrentTuneState = TuneStatus.TooLow;
            else if (cents > 5) CurrentTuneState = TuneStatus.TooHigh;
            else CurrentTuneState = TuneStatus.Perfect;


            //float targetPixels = (float)(cents * visualMultiplier);
            //SliderPosition += (float)((targetPixels - SliderPosition) * 0.2);
>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
            SliderPosition = (float)(cents * visualMultiplier);
        }

        private void OnTunerChangedFrequency(object sender, float frequency)
        {
            if (Application.Current?.Dispatcher != null)
                Application.Current.Dispatcher.InvokeAsync(() => UpdatePitch(frequency));
            else
                UpdatePitch(frequency);
        }

<<<<<<< HEAD
        public void Cleanup()
        {

            if (_analyzer != null)
            {
                _analyzer.ChangedFrequency -= OnTunerChangedFrequency;
                _analyzer.ChangedFrequency -= LogicModel_FrequencyGenerated;
            }
        }
=======

>>>>>>> 6ab6927727a28b4fc7c6abe5bd94786c279c3203
    }

    public enum TuneStatus
    {
        Waiting,
        TooLow,
        Perfect,
        TooHigh
    }
}
