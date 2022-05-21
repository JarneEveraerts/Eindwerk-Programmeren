using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.Data.SqlClient;
using Main;

Services.Configurator = new Configurator(args);
List<string> klanten = new();
List<string> toestellen = new();
string input = "";
using (var reader = new StreamReader("klanten.txt"))
{
    while (!reader.EndOfStream)
    {
        klanten.Add(reader.ReadLine());
    }
}

using (var reader = new StreamReader("FitnessToestellen.txt"))
{
    while (!reader.EndOfStream)
    {
        toestellen.Add(reader.ReadLine());
    }
}
using (SqlConnection connect = new SqlConnection(Services.Configurator.DbConnection))
{
    string query = "";
    connect.Open();
    foreach (string klant in klanten)
    {
        string[] klantSplit = klant.Split(",");
        query =
            $"INSERT INTO KLANT(K_FirstName,K_Name,K_Email,K_Gemeente,K_GeboorteDatum,K_Intresse,K_Subscription) VALUES ({klantSplit[0]},{klantSplit[1]},{klantSplit[2]},{klantSplit[3]},{klantSplit[4]},{CheckInteresse(klantSplit[5])},{CheckSubscription(klantSplit[6])});";
        SqlCommand cmd = new SqlCommand(query, connect);

        cmd.ExecuteNonQuery();
    }
    connect.Close();
}
using (SqlConnection connect = new SqlConnection(Services.Configurator.DbConnection))
{
    string query = "";
    connect.Open();
    foreach (string toestel in toestellen)
    {
        string[] toestelSplit = toestel.Split(",");
        query =
            $"INSERT INTO toestellen(T_Id,T_Name,T_Status) VALUES ('{toestelSplit[0]}','{toestelSplit[1]}','1');";
        SqlCommand cmd = new SqlCommand(query, connect);

        cmd.ExecuteNonQuery();
    }
    connect.Close();
}

string CheckInteresse(string? interesse)
{
    if (interesse == "")
    {
        return "null";
    }
    else
    {
        using (SqlConnection connect = new SqlConnection(Services.Configurator.DbConnection))
        {
            connect.Open();
            string query = $"SELECT I_Id FROM interesse WHERE I_Name = {interesse.ToLower()}";
            SqlCommand cmd = new SqlCommand(query, connect);
            string output = Convert.ToString(cmd.ExecuteScalar());
            return $"'{output}'";
        }
    }
}

string CheckSubscription(string subscription)
{
    using (SqlConnection connect = new SqlConnection(Services.Configurator.DbConnection))
    {
        connect.Open();
        string query = $"SELECT S_Id FROM subscription WHERE S_Name = {subscription.ToLower()}";
        SqlCommand cmd = new SqlCommand(query, connect);
        string output = Convert.ToString(cmd.ExecuteScalar());
        connect.Close();
        return $"'{output}'";
    }
}