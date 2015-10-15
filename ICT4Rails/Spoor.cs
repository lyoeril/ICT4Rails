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
    }
}
