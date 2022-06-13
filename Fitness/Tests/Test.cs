using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal class Test
    {
        public static bool BeheerderLogin(string? username, string? password)
        {
            string usernameCheck = "admin";
            string passwordCheck = "admin";
            if (username == null || password == null) throw new ArgumentException("Ongeldige tekst");

            return (usernameCheck == username && password == passwordCheck);
        }

        public static bool KlantLogin(string? username)
        {
            string usernamecheck = "Brian.Thorn@hotmail.com";
            if (username == null) throw new ArgumentException("leeg veld");
            return (username == usernamecheck);
        }

        public static bool AddMachine(string? add)
        {
            if (add == "" || add == null) { throw new ArgumentException("empty field"); }
            else if (add.Any(char.IsDigit) || add.Any(char.IsPunctuation)) { throw new ArgumentException("illigal char"); }
            else { return true; }
        }

        public static bool EditStatus(string change, string current)
        {
            if (change == current)
            {
                throw new ArgumentException("no change");
            }
            return true;
        }
    }
}