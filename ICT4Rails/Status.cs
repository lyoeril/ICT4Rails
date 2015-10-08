using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Status
    {
        // Fields
        public string Naam { get; set; }

        public string Bijzonderheden { get; set; }

        public Status(string naam, string bijzonderheden)
        {
            Naam = naam;
            Bijzonderheden = bijzonderheden;
        }
    }
}
