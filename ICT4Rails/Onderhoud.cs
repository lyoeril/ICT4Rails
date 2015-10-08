using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Onderhoud
    {
        public Medewerker Medewerker { get; set; }
        public Tram Tram { get; set; }
        public DateTime Starttijd { get; set; }
        public DateTime Eindtijd { get; set; }

        public string Soort { get; }

        public Onderhoud(Medewerker medewerker, Tram tram, DateTime starttijd, DateTime eindtijd, string soort)
        {
            Medewerker = medewerker;
            Tram = tram;
            Starttijd = starttijd;
            Eindtijd = eindtijd;
            Soort = soort;
        }

        public override string ToString()
        {
            return "TO DO";
        }

    }
}
