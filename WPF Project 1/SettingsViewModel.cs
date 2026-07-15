using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project_1
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Tuner _analyzer;

   
        public ObservableCollection<string> Microphones { get; set; }

        public int SelectedMicrophoneIndex
        {
            get => _analyzer.SelectedDeviceNumber;
            set
            {
                if (_analyzer.SelectedDeviceNumber != value && value >= 0)
                {
 
                    _analyzer.SelectedDeviceNumber = value;
                    OnPropertyChanged();


                    _analyzer.RestartRecording();
                }
            }
        }

        public SettingsViewModel(Tuner sharedAnalyzer)
        {
            _analyzer = sharedAnalyzer;

            Microphones = new ObservableCollection<string>(_analyzer.GetAvailableMicrophones());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
