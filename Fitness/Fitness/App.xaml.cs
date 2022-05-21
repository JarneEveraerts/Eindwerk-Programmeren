using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Main;

namespace Fitness
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LoginWindow _loginWindow = new();

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Services.Configurator = new Configurator(e.Args);
            _loginWindow.Show();
        }
    }
}