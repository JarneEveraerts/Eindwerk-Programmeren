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

        public LoginBeheerder(LoginWindow loginWindow)
        {
            _loginWindow = loginWindow;
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string password = Txt_Password.Text;
            string username = Txt_Username.Text;
            DbContext.LoginBeheerder(username, password);
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _loginWindow.Show();
            this.Hide();
        }
    }
}