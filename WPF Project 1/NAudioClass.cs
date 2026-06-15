using NAudio.Wave;
using NAudio.Lame;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WPF_Project_1
{
    class NAudioClass
    {
        public void PlayAudio()
        {
            using var waveIn = new WaveInEvent();

            waveIn.DeviceNumber = 1;
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);

            //waveIn.DataAvailable += OnDataAvailable();

            try
            {
                waveIn.StartRecording();
                Thread.Sleep(3000);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Виникла помилка");
            }
            finally
            {
                waveIn.StopRecording();
            }

        }

        static void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            int sCount = e.BytesRecorded / 2;
            float sumOfSquares = 0;
        }

        
    }
}
