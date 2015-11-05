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
        private int sectornummer;
        private bool beschikbaar;

        public int Spoornummer { get { return spoornummer; } }
        public int Sectornummer { get { return sectornummer; } }
        public bool Beschikbaar { get { return beschikbaar; } set { beschikbaar = value; } }

        public Spoor(int spoornr, int sectornummer, bool beschikbaar)
        {
            this.spoornummer = spoornr;
            this.sectornummer = sectornummer;
            this.beschikbaar = beschikbaar;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string sectorstr;
            if (sectornummer == 0)
            {
                sectorstr = "Onbekend";
            }
            else
            {
                sectorstr = sectornummer.ToString();
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
                " - Sectornummer: " + sectorstr +
                " - Beschikbaar: " + beschikbaarstr;

            return info;
        }
    }
}
