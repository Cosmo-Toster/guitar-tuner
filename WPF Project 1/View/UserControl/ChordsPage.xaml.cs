using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Project_1.View.UserControl
{
    /// <summary>
    /// Interaction logic for ChordsPage.xaml
    /// </summary>
    public partial class ChordsPage : System.Windows.Controls.UserControl
    {
        public ChordsPage(Tuner sharedAnalyzer)
        {
            InitializeComponent();

            this.DataContext = new ChordsViewModel(sharedAnalyzer);
            this.Unloaded += ChordsPage_Unloaded;
        }

        private void ChordsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is ChordsViewModel vm)
            {
                vm.Cleanup();
            }
        }
    }
}
