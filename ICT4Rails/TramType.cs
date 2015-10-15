using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class TramType
    {
        private string naam;
        private string beschrijving;
        private int lengte;

        public string Naam { get { return naam; } }
        public string Beschrijving { get { return beschrijving; } }
        public int Lengte { get { return lengte; } }

        public TramType(string naam, string beschrijving, int lengte)
        {
            this.naam = naam;
            this.beschrijving = beschrijving;
            this.lengte = lengte;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string naamstr;
            if (naam == "")
            {
                naamstr = "Onbekend";
            }
            else
            {
                naamstr = naam;
            }

            string lengtestr;
            if (lengte == 0)
            {
                lengtestr = "Onbekend";
            }
            else
            {
                lengtestr = lengte.ToString();
            }

            string beschrijvingstr;
            if (beschrijving == "")
            {
                beschrijvingstr = "Onbekend";
            }
            else
            {
                beschrijvingstr = beschrijving;
            }

            string info = "Naam: " + naamstr +
                "Beschrijving: " + beschrijvingstr +
                "Lengte: " + lengtestr;

            return info;
        }
    }
}
