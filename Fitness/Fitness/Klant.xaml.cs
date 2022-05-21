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
    /// Interaction logic for Klant.xaml
    /// </summary>
    ///
    public partial class Klant : Window
    {
        public record ListBoxTimeItem(int Value, string Label);//class zonder logic bedoeld voor data bij te houde
        public int SelectedTimeslot { get; set; }

        public Klant(List<string> dataList)
        {
            this.DataContext = this;
            InitializeComponent();
            Txt_FirstName.Text = dataList[0];
            Txt_Name.Text = dataList[1];
            Txt_Place.Text = dataList[3];
            Txt_Date.Text = dataList[4];
            Txt_Interest.Text = dataList[5];
            Txt_Subsciption.Text = dataList[6];
            SetupMachineSelection();
            SetupDate();
        }

        public void SetupMachineSelection()
        {
            List<string> machines = new(DbContext.CheckMachine());
            foreach (var item in machines)
            {
                Cmb_Machines.Items.Add(item);
            }
        }

        public void SetupDate()
        {
            Dpr_Date.DisplayDateStart = DateTime.Today.AddDays(1);
            Dpr_Date.DisplayDateEnd = DateTime.Today.AddDays(7);
        }

        public void SetupSlots()
        {
            if (Dpr_Date.SelectedDate.HasValue)
            {
                List<int> Slots = DbContext.AvailableSlots(Dpr_Date.SelectedDate.Value, Cmb_Machines.Text);
                foreach (int s in Slots)
                {
                    Lsb_TimeSlot.Items.Add($"{s}u: 60min");
                }
            }
        }

        private void Dpr_Date_DateChanged(Object sender, EventArgs e)
        {
            DateTime selected = Dpr_Date.SelectedDate.Value;
            if (DateTime.Compare(selected, DateTime.Today.AddDays(1)) >= 0 && DateTime.Compare(selected, DateTime.Today.AddDays(7)) <= 0)
            {
                SetupSlots();
            }
            else
            {
                MessageBox.Show("Invalid Date", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cmb_Machines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetupSlots();
        }

        private void Lsb_TimeSlot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = Lsb_TimeSlot.SelectedItems[Lsb_TimeSlot.SelectedItems.Count - 1].ToString();
            if (Lsb_TimeSlot.SelectedItems.Count < 5)
            {
                List<int> selectedList = new();
                foreach (string item in Lsb_TimeSlot.SelectedItems)
                {
                    selectedList.Add(int.Parse(item.Substring(0, item.IndexOf('u'))));
                }
                selectedList.Sort();
                int i = 0;
                foreach (int s in selectedList)
                {
                    i = 0;
                    foreach (int check in selectedList)
                    {
                        if (s + 1 == check || s - 1 == check) i++;
                    }
                    if (i == 2) break;
                }
                if (i == 2)
                {
                    MessageBox.Show("Max 2 connected time-slots", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
                    Lsb_TimeSlot.SelectedItems.Remove(selectedText);
                }
            }
            else
            {
                MessageBox.Show("Max 4 slots per day", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
                Lsb_TimeSlot.SelectedItems.Remove(selectedText);
            }
        }

        private void Btn_Reserveer_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Lsb_TimeSlot.SelectedItems)
            {
                MessageBox.Show(item.ToString());
            }
        }
    }
}