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
        private string opmerking;
        private string soort;

        public Medewerker Medewerker { get { return medewerker; } }
        public Tram Tram { get { return tram; } }
        public DateTime Starttijd { get { return Starttijd; } }
        public DateTime Eindtijd { get { return eindtijd; } }
        public string Opmerking { get { return opmerking; } }
        public string Soort { get { return soort; } }

        public Onderhoud(Medewerker medewerker, Tram tram, DateTime starttijd, DateTime eindtijd, string opmerking, string soort)
        {
            this.medewerker = medewerker;
            this.tram = tram;
            this.starttijd = starttijd;
            this.eindtijd = eindtijd;
            this.opmerking = opmerking;
            this.soort = soort;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string tramstr;
            if (tram == null)
            {
                tramstr = "Onbekend";
            }
            else
            {
                tramstr = tram.Nummer.ToString();
            }

            string medewerkerstr;
            if (medewerker == null)
            {
                medewerkerstr = "Onbekend";
            }
            else
            {
                medewerkerstr = medewerker.Naam.ToString();
            }

            string soortstr;
            if (soort == "")
            {
                soortstr = "Onbekend";
            }
            else
            {
                soortstr = soort;
            }

            
            string starttijdstr;
            if (starttijd == null)
            {
                starttijdstr = "Onbekend";
            }
            else
            {
                starttijdstr = starttijd.TimeOfDay.ToString();
            }

            string eindtijdstr;
            if (eindtijd == null)
            {
                eindtijdstr = "Onbekend";
            }
            else
            {
                eindtijdstr = eindtijd.TimeOfDay.ToString();
            }
            

            string info = "Tram: " + tramstr +
                ", Medewerker: " + medewerkerstr +
                ", Soort: " + soortstr +
                ", Starttijd: " + starttijdstr +
                ", Eindtijd: " + eindtijdstr +
                ", Opmerking: " + opmerking;

            return info;
        }

    }
}
