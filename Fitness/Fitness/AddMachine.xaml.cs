﻿using System;
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
    /// Interaction logic for AddMachine.xaml
    /// </summary>
    public partial class AddMachine : Window
    {
        private Beheerder beheerder;

        public AddMachine(Beheerder beheer)
        {
            beheerder = beheer;
            InitializeComponent();
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            string input = Txt_Input.Text;
            if (input == "")
            {
                Error("empty field detected");
                Txt_Input.Text = "";
                return;
            }
            if (input.Any(char.IsDigit) || input.Any(char.IsPunctuation))
            {
                Error("Use of illigal characters");
                Txt_Input.Text = "";
                return;
            }
            MessageBoxResult check = MessageBox.Show($"are u sure u want to Add: {input}?", "Add confirmation!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (check == MessageBoxResult.Yes)
            {
                DbContext.AddMachine(input);
                Txt_Input.Text = "";
            }
        }

        private void Error(string reason)
        {
            MessageBox.Show($"{reason}", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            beheerder.Setup();
            this.Close();
        }
    }
}