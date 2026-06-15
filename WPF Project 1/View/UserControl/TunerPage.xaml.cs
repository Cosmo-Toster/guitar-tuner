using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
//using System.Windows.Forms;
using Microsoft.Win32;

namespace WPF_Project_1.View.UserControl
{
    /// <summary>
    /// Interaction logic for TunerPage.xaml
    /// </summary>
    public partial class TunerPage : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        public TunerPage()
        {
            DataContext = this;
            InitializeComponent();

            NAudioClass NAclass = new NAudioClass();

            
        }

        private string boundText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string BoundText
        {
            get { return boundText; }
            set 
            {
                boundText = value;
                OnPropertyChanged();
            }
        }

        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            bool? success = fileDialog.ShowDialog();

            if (success == true)
            {
                string filePath = fileDialog.FileName;

                tbResult.Text = filePath;
            }
        }

        private void OnPropertyChanged( [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
