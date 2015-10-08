using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Type
    {
        private string naam;
        private string beschrijving;
        private int lengte;

        public string Naam { get { return naam; } }
        public string Beschrijving { get { return beschrijving; } }
        public int Lengte { get { return lengte; } }

        public Type(string naam, string beschrijving, int lengte)
        {
            this.naam = naam;
            this.beschrijving = beschrijving;
            this.lengte = lengte;
        }
    }
}
