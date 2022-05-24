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
using Microsoft.VisualBasic;

namespace Fitness
{
    /// <summary>
    /// Interaction logic for Beheerder.xaml
    /// </summary>
    public partial class Beheerder : Window
    {
        public string Username { get; private set; }

        public Beheerder(string username)
        {
            Username = username;
            InitializeComponent();
            Setup();
        }

        public static void Setup()
        {
        }

        private void Btn_RemoveMachine_Click(object sender, RoutedEventArgs e)
        {
            RemoveMachine removeMachine = new();
            removeMachine.Show();
        }

        private void Btn_AddMachine_Click(object sender, RoutedEventArgs e)
        {
            AddMachine addMachine = new();
            addMachine.Show();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Btn_Status_Click(object sender, RoutedEventArgs e)
        {
            Onderhoud onderhoud = new();
            onderhoud.Show();
        }
    }
}