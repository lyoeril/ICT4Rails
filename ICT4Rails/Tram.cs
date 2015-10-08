using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Tram
    {
        public Type Type { get; }
        public Status Status { get; set; }
        public int Lijn { get; set; }
        public int Nummer { get; }
        public bool Beschikbaar { get; set; }
        public DateTime Uitrijdtijd { get; set; }

        public Tram (Type type, Status status,int lijn, int nummer, bool beschikbaar, DateTime uitrijdtijd)
        {
            Type = type;
            Status = status;
            Lijn = lijn;
            Nummer = nummer;
            Beschikbaar = beschikbaar;
            Uitrijdtijd = uitrijdtijd;
        }


        
    }
}
