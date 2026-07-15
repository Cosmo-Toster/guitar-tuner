using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project_1
{
    class ChordsViewModel : INotifyPropertyChanged
    {
        private Tuner _analyzer;
        private ChordRecognitionService _chordService = new ChordRecognitionService();

        private bool _isProcessing = false;

        private string _currentChord = "Готовий до гри!";
        public string CurrentChord
        {
            get => _currentChord;
            set { _currentChord = value; OnPropertyChanged(); }
        }

        private string _confidenceText = "";
        public string ConfidenceText
        {
            get => _confidenceText;
            set { _confidenceText = value; OnPropertyChanged(); }
        }

        public ChordsViewModel(Tuner sharedAnalyzer)
        {
            _analyzer = sharedAnalyzer;


            _analyzer.AudioChunkReady += OnAudioChunkReady;
        }

        private async void OnAudioChunkReady(object sender, float[] samples)
        {
            System.Diagnostics.Debug.WriteLine($"[ChordsViewModel] Отримано аудіо. Розмір: {samples.Length}. IsProcessing: {_isProcessing}");

            if (_isProcessing) return;

            _isProcessing = true;

            try
            {
     
                ChordResponse result = await _chordService.PredictChordAsync(samples);

                if (result != null )
                {
                    CurrentChord = result.chord;
                    ConfidenceText = $"Точність: {result.confidence}%";
                }
            }
            finally
            {

                _isProcessing = false;
            }
        }

        public void Cleanup()
        {
            if (_analyzer != null)
            {
                _analyzer.AudioChunkReady -= OnAudioChunkReady;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}