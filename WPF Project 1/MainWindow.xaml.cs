using System.Windows;
using WPF_Project_1.View.UserControl;
using System.Diagnostics;


namespace WPF_Project_1
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

      

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            
            Process[] servers = Process.GetProcessesByName("ChordServer");

            foreach (Process server in servers)
            {
                try
                {
                    server.Kill(); 
                    server.WaitForExit(); 
                }
                catch
                {
                    
                }
            }
        }
    }
}