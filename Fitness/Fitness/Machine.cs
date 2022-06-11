using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness
{
    public class Machine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public Machine()
        { }

        public Machine(Machine machine)
        {
            Id = machine.Id;
            Name = machine.Name;
            Status = machine.Status;
        }
    }
}