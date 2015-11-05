using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Reservering
    {
        private int id;
        private Tram tram;
        private Spoor spoor;
        private DateTime datum;
        private bool actief;

        public int ID { get { return id; } }
        public Tram Tram { get { return tram; } }
        public Spoor Spoor { get { return spoor; } }
        public DateTime Datum { get { return datum; } }

        public bool Actief { get { return actief; } }
        public Reservering(int id,Tram tram, Spoor spoor, DateTime datum, bool actief)
        {
            this.id = id;
            this.tram = tram;
            this.spoor = spoor;
            this.datum = datum;
            this.actief = actief;
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
                tramstr = tram.Id.ToString();
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
