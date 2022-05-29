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
    public class Klant
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Place { get; set; }
        public string BirthDate { get; set; }
        public string Interest { get; set; }
        public string Subsciption { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }

        public Klant(List<string> data)
        {
            Id = data[0];
            FirstName = data[1];
            LastName = data[2];
            Email = data[3];
            Place = data[4];
            BirthDate = data[5];
            Interest = data[6];
            Subsciption = data[7];
        }
    }

    public partial class Reservatie : Window
    {
        public List<int> SelectedSlots { get; private set; } = new();
        public List<int> ReservedSlots { get; private set; }
        public Klant Klant { get; set; }

        public Reservatie(List<string> dataList)
        {
            Klant = new Klant(dataList);
            this.DataContext = Klant;
            InitializeComponent();
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
                DateTime date = Dpr_Date.SelectedDate.Value;
                ReservedSlots = DbContext.ReservedSlots(Klant.Email, date);
                Lsb_TimeSlot.Items.Clear();
                if (DbContext.ReservatieCount(Klant.Email, date) == 4)
                {
                    MessageBox.Show("Max 4 slots per day", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                };
                List<int> Slots = DbContext.AvailableSlots(date, Cmb_Machines.Text, Klant.Email, ReservedSlots);
                foreach (int s in Slots)
                {
                    Lsb_TimeSlot.Items.Add($"{s}u: 60min");
                }
            }
        }

        private void Dpr_Date_DateChanged(Object sender, EventArgs e)
        {
            if (!Dpr_Date.SelectedDate.HasValue) return;
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
            if (Lsb_TimeSlot.SelectedItems.Count == 0) return;
            string selectedText = Lsb_TimeSlot.SelectedItems[Lsb_TimeSlot.SelectedItems.Count - 1].ToString();
            if (Lsb_TimeSlot.SelectedItems.Count + DbContext.ReservatieCount(Klant.Email, Dpr_Date.SelectedDate.Value) > 4)
            {
                MessageBox.Show("Max 4 slots per day", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
                Lsb_TimeSlot.SelectedItems.Remove(selectedText);
            }
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
            SelectedSlots = selectedList;
        }

        private void Btn_Reserveer_Click(object sender, RoutedEventArgs e)
        {
            foreach (int item in SelectedSlots)
            {
                DbContext.Reserveer(Klant, Cmb_Machines.SelectedValue.ToString(), Dpr_Date.SelectedDate.Value, item);
            }
            SelectedSlots.Clear();
            Lsb_TimeSlot.SelectedItems.Clear();
            Dpr_Date.Text = string.Empty;
            SetupSlots();
        }
    }
}