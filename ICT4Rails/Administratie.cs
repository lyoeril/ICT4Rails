using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Administratie
    {
        public List<Medewerker> Medewerkers { get; set; }
        public List<Onderhoud> Onderhoudslijst { get; set; }
        public List<Tram> Trams { get; set; }

        public Administratie()
        {
            Medewerkers = new List<Medewerker>();
            Onderhoudslijst = new List<Onderhoud>();
            Trams = new List<Tram>();
        }
    }
}
