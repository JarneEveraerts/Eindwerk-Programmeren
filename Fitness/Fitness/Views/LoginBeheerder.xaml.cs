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
    /// Interaction logic for LoginBeheerder.xaml
    /// </summary>
    public partial class LoginBeheerder : Window
    {
        private LoginWindow _loginWindow;

        public LoginBeheerder()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string password = Txt_Password.Text;
            string username = Txt_Username.Text;
            if (password == "" || username == "")
            {
                MessageBox.Show("Empty field", "Error detected!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            password.Any(c => char.IsPunctuation(c));
            var result = DbContext.LoginBeheerder(username, password);
            if (!result)
            {
                MessageBox.Show("Invalid username/password", "Error detected!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Beheerder b = new();
            this.Close();
            b.Show();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _loginWindow = new();
            _loginWindow.Show();
            this.Hide();
        }
    }
}