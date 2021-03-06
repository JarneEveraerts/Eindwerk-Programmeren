using Data;
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
using System.Windows.Shapes;

namespace Fitness
{
    /// <summary>
    /// Interaction logic for LoginKlant.xaml
    /// </summary>
    public partial class LoginKlant : Window
    {
        private LoginWindow _loginWindow;

        public LoginKlant()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string email = Txt_Email.Text;
            if (email == "")
            {
                MessageBox.Show("Empty field", "Error detected!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = DbContext.LoginKlant(email);
            if (result == null)
            {
                MessageBox.Show("Invalid Email", "Error detected!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Reservatie r = new(result);
            this.Close();
            r.Show();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _loginWindow = new();
            this.Close();
            _loginWindow.Show();
        }
    }
}