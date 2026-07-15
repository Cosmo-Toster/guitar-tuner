using System.Windows;
using WPF_Project_1.View.UserControl;


namespace WPF_Project_1
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.DataContext is MainViewModel vm)
            {
                vm.Cleanup(); 
            }
            base.OnClosed(e);
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}