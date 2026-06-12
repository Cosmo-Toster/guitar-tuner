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
    /// Interaction logic for ClearableTextBox.xaml
    /// </summary>
    public partial class ClearableTextBox : System.Windows.Controls.UserControl
    {
        public ClearableTextBox()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtText.Clear();
            txtText.Focus();
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtText.Text))
                tbPlaceHolder.Visibility = Visibility.Visible;
            else
                tbPlaceHolder.Visibility = Visibility.Hidden;
        }
    }
}
