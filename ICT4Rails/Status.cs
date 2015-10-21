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

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string naamstr;
            if (naam == "" || naam == null)
            {
                naamstr = "Onbekend";
            }
            else
            {
                naamstr = naam;
            }

            string bijzonderhedenstr;
            if (bijzonderheden == "" || bijzonderheden == null)
            {
                bijzonderhedenstr = "Onbekend";
            }
            else
            {
                bijzonderhedenstr = bijzonderheden;
            }

            string info = "Naam: " + naamstr +
                "Bijzonderheden: " + bijzonderhedenstr;

            return info;
        }
    }
}
