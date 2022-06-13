using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Xml;
using Microsoft.Data.SqlClient;
using Domain;

namespace Data;

public static class DbContext
{
    #region Login

    public static bool LoginBeheerder(string? username, string? password)
    {
        if (username == "" || password == "")
        {
            return false;
        }
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();

        string query = $"SELECT 1 FROM beheerder WHERE B_Name = @username AND B_Password = @password;";
        SqlCommand cmd = new(query, connect);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        var validLogin = cmd.ExecuteScalar() as int?;
        connect.Close();
        if (validLogin.HasValue && validLogin.Value == 1)
        {
            return true;
        }
        return false;
    }

    public static Costumer LoginKlant(string? email)
    {
        List<string> dataKlant = new();
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"SELECT * FROM klant WHERE K_Email = @email;";
        SqlCommand cmd = new(query, connect);
        cmd.Parameters.AddWithValue("@email", email);
        using SqlDataReader reader = cmd.ExecuteReader();
        if (!reader.HasRows)
        {
            connect.Close();
            return null;
        }

        while (reader.Read())
        {
            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 6:
                        dataKlant.Add(CheckInterest(reader.GetSqlValue(i).ToString()));
                        break;

                    case 7:
                        dataKlant.Add(CheckSubscription(reader.GetSqlValue(i).ToString()));
                        break;

                    default:
                        dataKlant.Add(reader.GetSqlValue(i).ToString());
                        break;
                }
            }
        }
        Costumer costumer = new(dataKlant);
        connect.Close();
        reader.Close();
        return costumer;
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

    public static string CheckStatusId(string id)
    {
        string output = "";

        using (SqlConnection connect = new(Services.Configurator.DbConnection))
        {
            connect.Open();
            string query = $"SELECT S_Name FROM status WHERE S_Id = '{id}';";
            SqlCommand cmd = new(query, connect);
            output = (string)cmd.ExecuteScalar();
            connect.Close();
        }
        return output;
    }

    public static int CheckStatus(string status)
    {
        int output = 0;

        using (SqlConnection connect = new(Services.Configurator.DbConnection))
        {
            connect.Open();
            string query = $"SELECT S_Id FROM status WHERE S_Name = '{status}';";
            SqlCommand cmd = new(query, connect);
            output = (int)cmd.ExecuteScalar();
            connect.Close();
        }
        return output;
    }

    #endregion Conversion

    #region Setup

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

    public static List<int> AvailableSlots(DateTime date, string? machine, string email, List<int> reserverdSlots)
    {
        List<int> slots = new();
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        int j = 0;
        for (int i = 8; i < 23; i++)
        {
            string query = $"SELECT COUNT(*) FROM reservatie WHERE R_Date = '{date:yyyyMMdd}' AND R_Toestel = @machine AND R_Slot = @slot AND R_Email= @machine ;";
            SqlCommand cmd = new(query, connect);
            cmd.Parameters.AddWithValue("@machine", machine);
            cmd.Parameters.AddWithValue("@slot", i);
            cmd.Parameters.AddWithValue("@email", email);
            if ((int)cmd.ExecuteScalar() == 1) continue;

            if (reserverdSlots.Count > 0)
            {
                List<int> checkList = new(reserverdSlots);
                checkList.Add(i);
                checkList.Sort();
                // Comment: ????
                foreach (int s in checkList)
                {
                    j = 0;
                    foreach (int check in checkList)
                    {
                        if (s + 1 == check || s - 1 == check) j++;
                    }
                    if (j == 2) break;
                }
            }
            if (j == 2) continue;

            query = $"SELECT COUNT(R_Id) FROM reservatie WHERE R_Date = '{date:yyyyMMdd}' AND R_Toestel = @machine AND R_Slot = @slot ;";
            cmd = new(query, connect);
            cmd.Parameters.AddWithValue("@slot", i);
            cmd.Parameters.AddWithValue("@machine", machine);
            if ((int)cmd.ExecuteScalar() < MachineCount(machine)) slots.Add(i);
        }
        return slots;
    }

    public static List<int> ReservedSlots(string email, DateTime date)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        List<int> slots = new();
        string query = $"SELECT R_Slot FROM reservatie WHERE R_Date = '{date:yyyyMMdd}' AND R_Email = @email;";
        SqlCommand cmd = new(query, connect);
        cmd.Parameters.AddWithValue("@email", email);
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                slots.Add(reader.GetInt32(0));
            }
        }
        return slots;
    }

    #endregion Setup

    #region Count

    public static int MachineCount(string machine)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"SELECT COUNT(T_Id) FROM toestellen WHERE T_Name = '{machine}' AND T_Status = '1';";
        SqlCommand cmd = new(query, connect);
        return (int)cmd.ExecuteScalar();
    }

    public static int ReservatieCount(string email, DateTime date)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"SELECT COUNT(R_Id) FROM reservatie WHERE R_Email = '{email}' AND R_Date = '{date:yyyyMMdd}';";
        SqlCommand cmd = new(query, connect);
        return (int)cmd.ExecuteScalar();
    }

    #endregion Count

    public static void Reserveer(Costumer costumer, string machine, DateTime date, int slot)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"INSERT INTO Reservatie(R_Kid,R_FirstName,R_Name,R_Email,R_Toestel,R_Date,R_Slot) VALUES(@id,@firstName,@lastName,@email,@machine,'{date:yyyyMMdd}',@slot);";
        SqlCommand cmd = new(query, connect);
        cmd.Parameters.AddWithValue("@id", costumer.Id);
        cmd.Parameters.AddWithValue("@firstName", costumer.FirstName);
        cmd.Parameters.AddWithValue("@lastName", costumer.LastName);
        cmd.Parameters.AddWithValue("@email", costumer.Email);
        cmd.Parameters.AddWithValue("@slot", slot);
        cmd.Parameters.AddWithValue("@machine", machine);
        cmd.ExecuteNonQuery();
    }

    public static void AddMachine(string input)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"INSERT INTO Toestellen(T_Name,T_Status) VALUES('{input}','1');";
        SqlCommand cmd = new(query, connect);
        cmd.ExecuteNonQuery();
    }

    public static void RemoveMachine(Machine machine)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"DELETE FROM Toestellen WHERE T_Id = @id AND T_Name = @name ;";
        SqlCommand cmd = new(query, connect);
        cmd.Parameters.AddWithValue("@id", machine.Id);
        cmd.Parameters.AddWithValue("@name", machine.Name);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateStatus(Machine machine, int status)
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        connect.Open();
        string query = $"UPDATE Toestellen SET T_Status = '{status}'WHERE T_Id ='{machine.Id}' AND T_Name = '{machine.Name}' ;";
        SqlCommand cmd = new(query, connect);
        cmd.ExecuteNonQuery();
    }

    public static List<KeyValuePair<int, string>> AllMachines()
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        var machines = new List<KeyValuePair<int, string>>();
        string query = $"SELECT T_Id,T_Name FROM Toestellen;";
        SqlCommand cmd = new(query, connect);
        connect.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                machines.Add(new KeyValuePair<int, string>(reader.GetInt32(0), reader.GetString(1)));
            }
        }
        return machines;
    }

    public static List<string> ShowStatus()
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        List<string> status = new();
        string query = $"SELECT S_Name FROM status;";
        SqlCommand cmd = new(query, connect);
        connect.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                status.Add(reader.GetString(0));
            }
        }
        return status;
    }

    public static List<List<string>> MachineData()
    {
        using SqlConnection connect = new(Services.Configurator.DbConnection);
        List<List<string>> Reservations = new();
        string query = $"SELECT * FROM toestellen;";
        SqlCommand cmd = new(query, connect);
        connect.Open();
        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Reservations.Add(new() { reader.GetInt32(0).ToString(), reader.GetString(1), CheckStatusId(reader.GetInt32(2).ToString()) });
            }
        }
        return Reservations;
    }
}