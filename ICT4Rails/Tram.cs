using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Tram
    {
        private TramType type;
        private Status status;
        private int lijn;
        private int nummer;
        private bool beschikbaar;
        private DateTime uitrijdtijd;

        public TramType Type { get { return type; } }
        public Status Status { get { return status; } }
        public int Lijn { get { return lijn; } }
        public int Nummer { get { return nummer; } }
        public bool Beschikbaar { get { return beschikbaar; } }
        public DateTime Uitrijdtijd { get { return uitrijdtijd; } }

        public Tram (TramType type, Status status,int lijn, int nummer, bool beschikbaar, DateTime uitrijdtijd)
        {
            this.type = type;
            this.status = status;
            this.lijn = lijn;
            this.nummer = nummer;
            this.beschikbaar = beschikbaar;
            this.uitrijdtijd = uitrijdtijd;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string nummerstr;
            if (nummer == 0)
            {
                nummerstr = "Onbekend";
            }
            else
            {
                nummerstr = nummer.ToString();
            }

            string lijnstr;
            if (lijn == 0)
            {
                lijnstr = "Onbekend";
            }
            else
            {
                lijnstr = lijn.ToString();
            }

            string typestr;
            if (type == null)
            {
                typestr = "Onbekend";
            }
            else
            {
                typestr = type.Naam.ToString();
            }

            string statusstr;
            if (status == null)
            {
                statusstr = "Onbekend";
            }
            else
            {
                statusstr = status.Naam.ToString();
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

            string uitrijdatestr;
            if (uitrijdtijd == null)
            {
                uitrijdatestr = "Onbekend";
            }
            else
            {
                uitrijdatestr = uitrijdtijd.Date.ToString();
            }

            string uitrijtimestr;
            if (uitrijdtijd == null)
            {
                uitrijtimestr = "Onbekend";
            }
            else
            {
                uitrijtimestr = uitrijdtijd.TimeOfDay.ToString();
            }

            string info = "Nummer: " + nummerstr +
                "Lijn: " + lijnstr +
                "Type: " + typestr +
                "Status: " + statusstr +
                "Beschikbaar: " + beschikbaarstr +
                "Uitrijdatum: " + uitrijdatestr +
                "Uitrijtijd: " + uitrijtimestr;

            return info;
        }
        
    }
}
