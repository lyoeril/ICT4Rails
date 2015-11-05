using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Trampositie
    {
        private int id;
        private Spoor spoor;
        private Tram tram;
        DateTime aankomstijd;
        DateTime vertrektijd;

        public int Id { get { return id; } }
        public Spoor Spoor { get { return spoor; } }
        public Tram Tram { get { return tram; } }
        public DateTime Aankomstijd { get { return aankomstijd; } set { vertrektijd = value; } }
        public DateTime Vertrektijd { get; }

        public Trampositie(int id, Spoor spoor, Tram tram, DateTime aankomstijd, DateTime vertrektijd)
        {
            this.id = id;
            this.spoor = spoor;
            this.tram = tram;
            this.aankomstijd = aankomstijd;
            this.vertrektijd = vertrektijd;
        }

        public override string ToString()
        {
            string tramstr;
            if (tram == null)
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
            if (aankomstijd == null)
            {
                datumstr = "Onbekend";
            }
            else
            {
                datumstr = aankomstijd.ToString();
            }
            string datumstr2;
            if (vertrektijd == null)
            {
                datumstr2 = "Onbekend";
            }
            else
            {
                datumstr2 = vertrektijd.ToString();
            }
            string info = "Tram: " + tramstr +
                " - Spoor: " + spoorstr +
                " - Aankomstijd: " + datumstr +
                " - Vertrektijd: " + datumstr2;

            return info;
        }
    }
}
