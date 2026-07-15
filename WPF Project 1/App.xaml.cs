using System.Diagnostics; 
using System.Windows;

namespace WPF_Project_1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Process _pythonServer;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                _pythonServer = new Process();
                _pythonServer.StartInfo.FileName = "ChordServer.exe";
                _pythonServer.StartInfo.UseShellExecute = false;
                _pythonServer.StartInfo.CreateNoWindow = true;
                _pythonServer.Start();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Не вдалося запустити сервер акордів: {ex.Message}");
                // ЗАПОБІЖНИК: обнуляємо змінну, бо процес не створився
                _pythonServer = null;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                // Тепер ця перевірка повністю безпечна
                if (_pythonServer != null && !_pythonServer.HasExited)
                {
                    _pythonServer.Kill();
                    _pythonServer.Dispose();
                }
            }
            catch
            {
                // Мовчки ігноруємо, якщо процес був примусово закритий системою
            }

            base.OnExit(e);
        }
    }

}
