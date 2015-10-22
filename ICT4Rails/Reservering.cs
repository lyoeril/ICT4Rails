using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Reservering
    {
        private Tram tram;
        private Spoor spoor;
        private DateTime datum;

        public DateTime Datum { get { return datum; } }
        public Reservering(Tram tram, Spoor spoor, DateTime datum)
        {
            this.tram = tram;
            this.spoor = spoor;
            this.datum = datum;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string tramstr;
            if(tram == null)
            {
                tramstr = "Onbekend";
            }
            else
            {
                tramstr = tram.Nummer.ToString();
            }

            string spoorstr;
            if (spoor == null)
            {
                spoorstr = "Onbekend";
            }
            else
            {
                spoorstr = spoor.Spoornummer.ToString();
            }

            string datumstr;
            if (datum == null)
            {
                datumstr = "Onbekend";
            }
            else
            {
                datumstr = datum.ToString();
            }

            string info = "Tram: " + tramstr +
                " - Spoor: " + spoorstr +
                " - Datum: " + datumstr;

            return info;
        }
    }
}
