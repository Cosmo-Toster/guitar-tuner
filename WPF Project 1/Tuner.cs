using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project_1
{
    public enum AnalyzerMode
    {
        Tuner,
        ChordRecognition
    }
    public class Tuner
    {
        private WaveInEvent _waveIn;

      

        public bool IsRecordingActive => _waveIn != null;

        public int SelectedDeviceNumber { get; set; } = 0;

        public event EventHandler<float> ChangedFrequency;

        public event EventHandler<float[]> AudioChunkReady;

        public AnalyzerMode CurrentMode { get; set; } = AnalyzerMode.Tuner;

        public List<string> GetAvailableMicrophones()
        {
            var devices = new List<string>();
            for (int i = 0; i < WaveInEvent.DeviceCount; i++)
            {
                devices.Add(WaveInEvent.GetCapabilities(i).ProductName);
            }
            return devices;
        }

        public void RestartRecording()
        {
          
            if (_waveIn != null)
            {
                StopGenerating();
                StartGenerating();
            }
        }

        public void StartGenerating()
        {
            if (_waveIn != null) return;
            try
            {
                _waveIn = new WaveInEvent();
                _waveIn.DeviceNumber = SelectedDeviceNumber;
                _waveIn.WaveFormat = new WaveFormat(48000, 16, 1);
                _waveIn.BufferMilliseconds = 200;
                _waveIn.DataAvailable += OnDataAvailable;
                _waveIn.StartRecording();

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Помилка: {ex.Message}");
            }

        }

        public void StopGenerating()
        {
            if (_waveIn != null)
            {
                _waveIn.StopRecording();
                _waveIn.DataAvailable -= OnDataAvailable; 
                _waveIn.Dispose();
                _waveIn = null;
        
            }
        }

        public void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            int sampleCount = e.BytesRecorded / 2;
            float[] samples = new float[sampleCount];
            float sumOfSquares = 0;

            for (int i = 0; i < sampleCount; i++)
            {
                short sample32 = BitConverter.ToInt16(e.Buffer, i * 2);
                samples[i] = sample32 / 32768f;
                sumOfSquares += samples[i] * samples[i];
            }

            float rms = (float)Math.Sqrt(sumOfSquares / sampleCount);

            if (rms < 0.008f)
            {
                return;
            }


            if (CurrentMode == AnalyzerMode.Tuner)
            {
                float frequency = FindFundamentalFrequency(samples, 48000);
                if (frequency > 0)
                {
                    ChangedFrequency?.Invoke(this, frequency);
                }
            }
            else if (CurrentMode == AnalyzerMode.ChordRecognition)
            {
                AudioChunkReady?.Invoke(this, samples);
            }

        }

        static float FindFundamentalFrequency(float[] samples, int sampleRate)
        {
            int minLag = sampleRate / 1200;
            int maxLag = sampleRate / 60;

            float maxCorrelation = 0;
            int bestLag = 0;

            for (int lag = minLag; lag <= maxLag; lag++)
            {
                float correlation = 0;
                for (int i = 0; i < samples.Length - lag; i++)
                {
                    correlation += samples[i] * samples[i + lag];
                }

                if (correlation > maxCorrelation)
                {
                    maxCorrelation = correlation;
                    bestLag = lag;
                }
            }

            if (bestLag == 0) return 0;


            return (float)Math.Round(((float)sampleRate / bestLag),2);
            
        }

      
    }
}
