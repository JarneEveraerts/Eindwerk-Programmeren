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
    /// Interaction logic for RemoveMachine.xaml
    /// </summary>
    public partial class RemoveMachine : Window
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public RemoveMachine()
        {
            InitializeComponent();
            Setup();
        }

        private void Btn_Remove_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult check = MessageBox.Show($"are u sure u want to delete: {Id} {Name}?", "Delete confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (check == MessageBoxResult.Yes)
            {
                DbContext.RemoveMachine(Id, Name);
                Cmb_Machines.Items.Clear();
                Setup();
                Beheerder.Setup();
            }
        }

        private void Btn_Cancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cmb_Machines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Cmb_Machines.HasItems) return;
            string[] data = Cmb_Machines.SelectedValue.ToString().Split(':');
            Id = int.Parse(data[0]);
            Name = data[1].Replace(" ", "");
        }

        private void Setup()
        {
            List<KeyValuePair<int, string>> machines = DbContext.AllMachines();
            foreach (KeyValuePair<int, string> item in machines)
            {
                Cmb_Machines.Items.Add($"{item.Key}: {item.Value}");
            }
        }
    }
}