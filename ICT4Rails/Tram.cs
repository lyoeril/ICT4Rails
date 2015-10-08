using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Tram
    {
        private Type type;
        private Status status;
        private int lijn;
        private int nummer;
        private bool beschikbaar;
        private DateTime uitrijdtijd;

        public Type Type { get { return type; } }
        public Status Status { get { return status; } }
        public int Lijn { get { return lijn; } }
        public int Nummer { get { return nummer; } }
        public bool Beschikbaar { get { return beschikbaar; } }
        public DateTime Uitrijdtijd { get { return uitrijdtijd; } }

        public Tram (Type type, Status status,int lijn, int nummer, bool beschikbaar, DateTime uitrijdtijd)
        {
            this.type = type;
            this.status = status;
            this.lijn = lijn;
            this.nummer = nummer;
            this.beschikbaar = beschikbaar;
            this.uitrijdtijd = uitrijdtijd;
        }


        
    }
}
