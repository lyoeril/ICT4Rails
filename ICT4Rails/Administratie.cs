using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Administratie
    {
        private List<Medewerker> medewerkers;
        private List<Onderhoud> onderhoudslijst;
        private List<Tram> trams;

        public List<Medewerker> Medewerkers { get { return medewerkers; } }
        public List<Onderhoud> Onderhoudslijst { get { return onderhoudslijst; } }
        public List<Tram> Trams { get { return trams; } }

        public Administratie()
        {
            this.medewerkers = new List<Medewerker>();
            this.onderhoudslijst = new List<Onderhoud>();
            this.trams = new List<Tram>();
        }
        public bool AddMedewerker(Medewerker medewerker)
        {
            foreach(Medewerker Selected_Medewerker in medewerkers)
            {
                if(medewerker == Selected_Medewerker)
                {
                    medewerkers.Remove(Selected_Medewerker);
                    return true;
                }
            }
            return false;
        }
        public bool RemoveMedewerker(Medewerker medewerker)
        {
            foreach (Medewerker Selected_Medewerker in medewerkers)
            {
                if (medewerker == Selected_Medewerker)
                {
                    medewerkers.Add(Selected_Medewerker);
                    return true;
                }
            }
            return false;
        }


    }
}
