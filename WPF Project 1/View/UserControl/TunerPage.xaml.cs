using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

//using System.Windows.Forms;
using Microsoft.Win32;

namespace WPF_Project_1.View.UserControl
{
    /// <summary>
    /// Interaction logic for TunerPage.xaml
    /// </summary>
    public partial class TunerPage : System.Windows.Controls.UserControl
    {
        public TunerPage(Tuner sharedAnalyzer)
        {
            InitializeComponent();

            this.DataContext = new TunerViewModel(sharedAnalyzer);

            this.Unloaded += TunerPage_Unloaded;
        }

        private void AnyButton_Checked(object sender, RoutedEventArgs e)
        {
            if (tbStringDisplay == null) return;

            if (sender is RadioButton clickedButton)
            {
                if (clickedButton.Tag != null)
                {
                    tbStringDisplay.Text = clickedButton.Tag.ToString();
                }
            }
        }


        private void TunerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is TunerViewModel vm)
            {
                vm.Cleanup();
            }
        }


    }
}
