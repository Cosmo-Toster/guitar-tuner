using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project_1
{
    public class Tuner
    {
        private WaveInEvent _waveIn;

        static readonly string[] NoteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        public event EventHandler<float> ChangedFrequency;


        public void StartGenerating()
        {
            if (_waveIn != null) return;
            try
            {
                _waveIn = new WaveInEvent();
                _waveIn.DeviceNumber = 0;
                _waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
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
                Debug.WriteLine("Запис зупинено");
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

            
            float frequency = FindFundamentalFrequency(samples, 44100);

            //if (frequency > 60 && frequency < 1200)
            //{/
                //string note = GetNoteName(frequency);
                //Debug.Write($"\rЗвучить: [ {note} ]  Частота: {frequency:0.0} Гц".PadRight(40));
                ChangedFrequency?.Invoke(this, frequency);

            //}
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

        static string GetNoteName(float frequency)
        {
            int pitch = (int)Math.Round(12.0 * Math.Log2(frequency / 440.0) + 69.0);

            if (pitch < 0) return "Н/Д";

            string noteName = NoteNames[pitch % 12];

            int octave = (pitch / 12) - 1;

            return $"{noteName}{octave}";
        }
    }
}
