using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml;
using Main;
using Microsoft.Data.SqlClient;

namespace Fitness;

public static class DbContext
{
    #region Login

    public static void LoginBeheerder(string? username, string? password)
    {
        string checkPass = "";
        if (username == "" || password == "")
        {
            MessageBox.Show("Invalid Username/Password", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            using (SqlConnection connect = new(Services.Configurator.DbConnection))
            {
                connect.Open();
                string query = $"SELECT B_Password FROM beheerder WHERE B_Name = '{username}';";
                SqlCommand cmd = new(query, connect);
                checkPass = cmd.ExecuteScalar().ToString();
                connect.Close();
            }

            if (password != checkPass)
            {
                MessageBox.Show("Invalid Username/Password", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
            }
        }
    }

    public static void LoginKlant(LoginWindow loginWindow, LoginKlant loginKlant, string? email)
    {
        List<string> dataList = new();
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"SELECT * FROM klant WHERE K_Email = '{email}';";
        SqlCommand cmd = new(query, connect);
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                for (int i = 1; i < 8; i++)
                {
                    switch (i)
                    {
                        case 6:
                            dataList.Add(CheckInterest(reader.GetSqlValue(i).ToString()));
                            break;

                        case 7:
                            dataList.Add(CheckSubscription(reader.GetSqlValue(i).ToString()));
                            break;

                        default:
                            dataList.Add(reader.GetSqlValue(i).ToString());
                            break;
                    }
                }
            }
            connect.Close();
            reader.Close();
            Klant klant = new(dataList);
            klant.Show();
            loginKlant.Close();
            loginWindow.Close();
        }
        else
        {
            MessageBox.Show("Invalid Email", "Error Detected in input", MessageBoxButton.OK, MessageBoxImage.Error);
            connect.Close();
        }
    }

    #endregion Login

    #region Conversion

    public static string? CheckInterest(string id)
    {
        string? output = "";
        if (id == "Null") return output;
        using (SqlConnection connect = new(Services.Configurator.DbConnection))
        {
            connect.Open();
            string query = $"SELECT I_Name FROM interesse WHERE I_Id = '{id}';";
            SqlCommand cmd = new(query, connect);
            output = cmd.ExecuteScalar() as string;
            connect.Close();
        }

        return output;
    }

    public static string CheckSubscription(string id)
    {
        string output = "";
        if (id == "Null") return output;
        using (SqlConnection connect = new(Services.Configurator.DbConnection))
        {
            connect.Open();
            string query = $"SELECT S_Name FROM subscription WHERE S_Id = '{id}';";
            SqlCommand cmd = new(query, connect);
            output = cmd.ExecuteScalar().ToString();
            connect.Close();
        }
        return output;
    }

    #endregion Conversion

    #region setup

    public static List<string> CheckMachine()
    {
        List<string> machines = new();
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"SELECT DISTINCT T_Name FROM toestellen;";
        SqlCommand cmd = new(query, connect);
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                machines.Add(reader.GetString(0));
            }
        }
        return machines;
    }

    public static List<int> AvailableSlots(DateTime date, string? machine)
    {
        List<int> slots = new();
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();

        for (int i = 8; i < 23; i++)
        {
            string query = $"SELECT COUNT(R_Id) FROM reservatie WHERE R_Date = '{date:yyyyMMdd}' AND R_Toestel = '{machine}' AND R_Slot = '{i}' ;";
            SqlCommand cmd = new(query, connect);
            if ((int)cmd.ExecuteScalar() < MachineCount(machine))
            {
                slots.Add(i);
            }
        }
        return slots;
    }

    #endregion setup

    public static int MachineCount(string machine)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"SELECT COUNT(T_Id) FROM toestellen WHERE T_Name = '{machine}';";
        SqlCommand cmd = new(query, connect);
        return (int)cmd.ExecuteScalar();
    }
}