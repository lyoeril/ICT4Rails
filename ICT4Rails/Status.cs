using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Status
    {
        private string naam;
        private string bijzonderheden;

        public string Naam { get { return naam; } }
        public string Bijzonderheden { get { return bijzonderheden; } }

        public Status(string naam, string bijzonderheden)
        {
            this.naam = naam;
            this.bijzonderheden = bijzonderheden;
        }
    }
}
