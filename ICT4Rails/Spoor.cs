using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Spoor
    {
        private int spoornummer;
        private int lijn;
        private bool beschikbaar;

        public int Spoornummer { get { return spoornummer; } }
        public int Lijn { get { return lijn; } }
        public bool Beschikbaar { get { return beschikbaar; } }

        public Spoor(int spoornr, int lijn, bool beschikbaar)
        {
            this.spoornummer = spoornr;
            this.lijn = lijn;
            this.beschikbaar = beschikbaar;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string lijnstr;
            if (lijn == 0)
            {
                lijnstr = "Onbekend";
            }
            else
            {
                lijnstr = lijn.ToString();
            }

            string spoorstr;
            if (spoornummer == 0)
            {
                spoorstr = "Onbekend";
            }
            else
            {
                spoorstr = spoornummer.ToString();
            }

            string beschikbaarstr;
            if (beschikbaar)
            {
                beschikbaarstr = "Ja";
            }
            else
            {
                beschikbaarstr = "Nee";
            }

            string info = "Spoornummer: " + spoorstr +
                "Lijn: " + lijnstr +
                "Beschikbaar: " + beschikbaarstr;

            return info;
        }
    }
}
