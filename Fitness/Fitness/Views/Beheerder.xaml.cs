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
using Data;
using Domain;
using Microsoft.VisualBasic;

namespace Fitness
{
    /// <summary>
    /// Interaction logic for Beheerder.xaml
    /// </summary>
    public partial class Beheerder : Window
    {
        public Machine SelectedMachine { get; private set; }

        public Beheerder()
        {
            InitializeComponent();
            Setup();
        }

        public void Setup()
        {
            Txt_MachineName.Clear();
            Cmb_Status.Items.Clear();
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

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_MachineName.Text == "") return;
            if (Cmb_Status.SelectedItem == SelectedMachine.Status)
            {
                MessageBox.Show($"No changes detected", "Input Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult check = MessageBox.Show($"are u sure u want to chanche status off: {SelectedMachine.Id} {SelectedMachine.Name}?", "Update confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (check == MessageBoxResult.Yes)
            {
                DbContext.UpdateStatus(SelectedMachine, DbContext.CheckStatus(Cmb_Status.SelectedValue.ToString()));
            }
            Setup();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_MachineName.Text == "") return;
            MessageBoxResult check = MessageBox.Show($"are u sure u want to delete: {SelectedMachine.Id} {SelectedMachine.Name}?", "Delete confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (check == MessageBoxResult.Yes)
            {
                DbContext.RemoveMachine(SelectedMachine);
                Setup();
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string input = Txt_Input.Text;
            if (input == "")
            {
                MessageBox.Show("empty field detected", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (input.Any(char.IsDigit) || input.Any(char.IsPunctuation))
            {
                MessageBox.Show("Use of illigal characters", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
                Txt_Input.Text = "";
                return;
            }
            MessageBoxResult check = MessageBox.Show($"are u sure u want to Add: {input}?", "Add confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (check == MessageBoxResult.Yes)
            {
                DbContext.AddMachine(input);
                Setup();
            }
        }
    }
}