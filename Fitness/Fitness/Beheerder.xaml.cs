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
        public Machine SelectedMachine { get; private set; }

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
            List<string> status = DbContext.ShowStatus();
            foreach (string item in status)
            {
                Cmb_Status.Items.Add(item);
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Dtg_Machines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedMachine = (Machine)Dtg_Machines.SelectedItem;
            this.DataContext = SelectedMachine;
        }
    }
}