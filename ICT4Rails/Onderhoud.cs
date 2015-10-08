using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Onderhoud
    {
        private Medewerker medewerker;
        private Tram tram;
        private DateTime starttijd;
        private DateTime eindtijd;
        private string soort;

        public Medewerker Medewerker { get { return medewerker; } }
        public Tram Tram { get { return tram; } }
        public DateTime Starttijd { get { return Starttijd; } }
        public DateTime Eindtijd { get { return eindtijd; } }
        public string Soort { get { return soort; } }

        public Onderhoud(Medewerker medewerker, Tram tram, DateTime starttijd, DateTime eindtijd, string soort)
        {
            this.medewerker = medewerker;
            this.tram = tram;
            this.starttijd = starttijd;
            this.eindtijd = eindtijd;
            this.soort = soort;
        }

        public override string ToString()
        {
            return "TO DO";
        }

    }
}
