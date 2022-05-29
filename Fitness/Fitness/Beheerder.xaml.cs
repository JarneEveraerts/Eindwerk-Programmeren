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

        public void Setup()
        {
            List<Machine> machines = new();
            foreach (List<string> machine in DbContext.MachineData())
            {
                machines.Add(new() { Id = machine[0], Name = machine[1], Status = machine[2] });
            }
            Dtg_Machines.ItemsSource = machines;
        }

        private void Btn_RemoveMachine_Click(object sender, RoutedEventArgs e)
        {
            RemoveMachine removeMachine = new(this);
            removeMachine.Show();
        }

        private void Btn_AddMachine_Click(object sender, RoutedEventArgs e)
        {
            AddMachine addMachine = new(this);
            addMachine.Show();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Btn_Status_Click(object sender, RoutedEventArgs e)
        {
            Onderhoud onderhoud = new(this);
            onderhoud.Show();
        }
    }

    public class Machine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}