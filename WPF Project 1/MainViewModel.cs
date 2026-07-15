using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Project_1.View.UserControl;

namespace WPF_Project_1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Tuner _sharedAnalyzer = new Tuner();
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

 
        public ICommand ShowTunerPageCommand { get; }
        public ICommand ShowChordsPageCommand { get; }

        public ICommand ShowSettingsPageCommand { get; }

        public MainViewModel()
        {
            _sharedAnalyzer.StartGenerating();

            ShowTunerPageCommand = new RelayCommand(() =>
            {
                _sharedAnalyzer.CurrentMode = AnalyzerMode.Tuner;
                CurrentViewModel = new TunerPage(_sharedAnalyzer);
            });

            ShowChordsPageCommand = new RelayCommand(() =>
            {
                _sharedAnalyzer.CurrentMode = AnalyzerMode.ChordRecognition;
                CurrentViewModel = new ChordsPage(_sharedAnalyzer);
            });

            ShowSettingsPageCommand = new RelayCommand(() =>
            {
                CurrentViewModel = new SettingsPage(_sharedAnalyzer);
            });

            _sharedAnalyzer.CurrentMode = AnalyzerMode.Tuner;
            CurrentViewModel = new TunerPage(_sharedAnalyzer);
        }

        public void Cleanup()
        {
            if (_sharedAnalyzer != null)
            {
                _sharedAnalyzer.StopGenerating();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
