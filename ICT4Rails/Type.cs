using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Type
    {
        public string Naam { get; }
        public string Beschrijving { get; }
        public int Lengte { get; }

        public Type(string naam, string beschrijving, int lengte)
        {
            Naam = naam;
            Beschrijving = beschrijving;
            Lengte = lengte;
        }
    }
}
