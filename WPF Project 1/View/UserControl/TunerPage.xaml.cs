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
    public partial class TunerPage : System.Windows.Controls.UserControl
    {
        public TunerPage()
        {
            InitializeComponent();

            this.DataContext = new TunerViewModel();


        }

    }
}
