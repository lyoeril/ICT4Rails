using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Onderhoud
    {
        private int id;
        private Medewerker medewerker;
        private Tram tram;
        private DateTime starttijd;
        private DateTime eindtijd;
        private string opmerking;
        private string soort;

        public int ID { get { return id; } }
        public Medewerker Medewerker { get { return medewerker; } }
        public Tram Tram { get { return tram; } }
        public DateTime Starttijd { get { return Starttijd; } }
        public DateTime Eindtijd { get { return eindtijd; } }
        public string Opmerking { get { return opmerking; } }
        public string Soort { get { return soort; } }

        public Onderhoud(int id, Medewerker medewerker, Tram tram, DateTime starttijd, DateTime eindtijd, string opmerking, string soort)
        {
            this.id = id;
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
            string idstr;
            if(id == 0)
            {
                idstr = "Onbekend";
            }
            else
            {
                idstr = id.ToString();
            }

            string tramstr;
            if (tram == null)
            {
                tramstr = "Onbekend";
            }
            else
            {
                tramstr = tram.Id.ToString();
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
            

            string info = "ID: " + idstr +
                " - Tram: " + tramstr +
                " - Medewerker: " + medewerkerstr +
                " - Soort: " + soortstr +
                " - Starttijd: " + starttijdstr +
                " - Eindtijd: " + eindtijdstr +
                " - Opmerking: " + opmerking;

            return info;
        }

    }
}
