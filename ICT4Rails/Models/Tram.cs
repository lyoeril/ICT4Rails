using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Tram
    {
        private int id;
        private TramType type;
        private Status status;
        private string lijn;


        public TramType Type { get { return type; } }
        public Status Status { get { return status; } set { status = value; } }
        public string Lijn { get { return lijn; } }
        public int Id { get { return id; } }


        public Tram(int id, TramType type, Status status, string lijn)
        {
            this.id = id;
            this.type = type;
            this.status = status;
            this.lijn = lijn;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            string nummerstr;
            if (id == 0)
            {
                nummerstr = "Onbekend";
            }
            else
            {
                nummerstr = id.ToString();
            }

            string lijnstr;
            if (lijn == null)
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

            string info = "Nummer: " + nummerstr +
                " - Lijn: " + lijnstr +
                " - Type: " + typestr +
                " - Status: " + statusstr;

            return info;
        }

    }
}
