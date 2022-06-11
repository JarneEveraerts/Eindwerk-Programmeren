using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness
{
    public class Costumer
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string Place { get; set; }
        public string BirthDate { get; set; }
        public string Interest { get; set; }
        public string Subsciption { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }

        public Costumer(List<string> data)
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
}