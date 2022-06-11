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

namespace Fitness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginKlant loginKlant;
        private LoginBeheerder loginBeheerder;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Btn_LoginBeheerder_Click(object sender, RoutedEventArgs e)
        {
            loginBeheerder = new();
            loginBeheerder.Show();
            this.Close();
        }

        private void Btn_LoginKlant_Click(object sender, RoutedEventArgs e)
        {
            loginKlant = new();
            loginKlant.Show();
            this.Close();
        }
    }
}