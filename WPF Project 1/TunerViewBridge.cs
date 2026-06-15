using NAudio.Wave;
using NAudio.Lame;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WPF_Project_1
{
    class TunerViewBridge
    {
        static readonly string[] NoteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        static void Main(string[] args)
        {
            using var waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            waveIn.DataAvailable += OnDataAvailable;

            try
            {
                waveIn.StartRecording();
                Console.WriteLine("Гітарний тюнер увімкнено! Зіграйте ноту...");
                Console.WriteLine("Натисніть [ENTER] для виходу.\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            int sampleCount = e.BytesRecorded / 2;
            float[] samples = new float[sampleCount];
            float sumOfSquares = 0;

            // 1. Декодування та Нормалізація (як ми робили раніше)
            for (int i = 0; i < sampleCount; i++)
            {
                short sample32 = BitConverter.ToInt16(e.Buffer, i * 2);
                samples[i] = sample32 / 32768f;
                sumOfSquares += samples[i] * samples[i];
            }

            // 2. Розрахунок гучності (Noise Gate)
            float rms = (float)Math.Sqrt(sumOfSquares / sampleCount);

            // Якщо тиша або фоновий шум комп'ютера - нічого не рахуємо
            if (rms < 0.01f)
            {
                Console.Write("\rОчікування звуку...".PadRight(40));
                return;
            }

            // 3. Знаходимо частоту (Герци) за допомогою алгоритму Автокореляції
            float frequency = FindFundamentalFrequency(samples, 44100);

            // 4. Перетворюємо частоту в музичну ноту
            if (frequency > 60 && frequency < 1200) // Діапазон гітари (приблизно)
            {
                string note = GetNoteName(frequency);
                Console.Write($"\rЗвучить: [ {note} ]  Частота: {frequency:0.0} Гц".PadRight(40));
            }
        }

        // =========================================================
        // МАТЕМАТИЧНИЙ БЛОК
        // =========================================================

        /// <summary>
        /// Алгоритм Автокореляції. Шукає патерн, що повторюється у звуковій хвилі.
        /// </summary>
        static float FindFundamentalFrequency(float[] samples, int sampleRate)
        {
            // Нас цікавлять частоти від 60 Гц (найнижча струна) до 1200 Гц
            int minLag = sampleRate / 1200;
            int maxLag = sampleRate / 60;

            float maxCorrelation = 0;
            int bestLag = 0;

            // Зсовуємо хвилю саму на себе і шукаємо збіг
            for (int lag = minLag; lag <= maxLag; lag++)
            {
                float correlation = 0;
                for (int i = 0; i < samples.Length - lag; i++)
                {
                    correlation += samples[i] * samples[i + lag];
                }

                // Знаходимо найкращий збіг (пік автокореляції)
                if (correlation > maxCorrelation)
                {
                    maxCorrelation = correlation;
                    bestLag = lag;
                }
            }

            if (bestLag == 0) return 0;

            // Частота = Частота дискретизації / знайдений крок повторення
            return (float)sampleRate / bestLag;
        }

        /// <summary>
        /// Переводить частоту (Герци) у музичну ноту.
        /// </summary>
        static string GetNoteName(float frequency)
        {
            // Формула перетворення частоти у номер MIDI-ноти (стандарт A4 = 440 Гц)
            int pitch = (int)Math.Round(12.0 * Math.Log2(frequency / 440.0) + 69.0);

            // Якщо сталася помилка розрахунку
            if (pitch < 0) return "Н/Д";

            // Дізнаємося назву ноти (залишок від ділення на 12)
            string noteName = NoteNames[pitch % 12];

            // Дізнаємося октаву
            int octave = (pitch / 12) - 1;

            return $"{noteName}{octave}";
        }


    }
}
