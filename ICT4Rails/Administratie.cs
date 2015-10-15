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
        /* Hieronder vind je alles in verband met de medewerkers*/
        public void AddMedewerker(Medewerker medewerker)
        {
            // Moet veranderd worden ivm database link
            foreach (Medewerker Selected_Medewerker in medewerkers)
            {
                if(medewerker == Selected_Medewerker)
                {
                    throw new Exception("De medewerker bestaat al!");
                }
            }
            medewerkers.Add(medewerker);
        }
        public bool RemoveMedewerker(Medewerker medewerker)
        {
            // Moet veranderd worden ivm database link
            foreach (Medewerker Selected_Medewerker in medewerkers)
            {
                if (medewerker == Selected_Medewerker)
                {
                    medewerkers.Remove(Selected_Medewerker);
                    return true;
                }
            }
            return false;
        }

        public void ChangeMedewerker(Medewerker medewerker)
        {
            // TO DO
        }

        /* Hieronder vind je alles in verband met de trams*/
        public void AddTram(Tram tram)
        {
            // Moet veranderd worden ivm database link
            foreach (Tram Selected_Tram in trams)
            {
                if (tram == Selected_Tram)
                {
                    throw new Exception("De tram bestaat al!");
                }
            }
            trams.Add(tram);
        }
        public bool RemoveTram(Tram tram)
        {
            // Moet veranderd worden ivm database link
            foreach (Tram Selected_Tram in trams)
            {
                if (tram == Selected_Tram)
                {
                    trams.Remove(Selected_Tram);
                    return true;
                }
            }
            return false;
        }
        /* Hieronder ...*/
        public void AddOnderhoudbeurt(Onderhoud onderhoudsbeurt)
        {
            // Moet veranderd worden ivm database link
            foreach (Onderhoud Selected_Onderhoudsbeurt in onderhoudslijst)
            {
                if (onderhoudsbeurt == Selected_Onderhoudsbeurt)
                {
                    throw new Exception("De onderhoudsbeurt bestaat al!");
                }
            }
            onderhoudslijst.Add(onderhoudsbeurt);
        }
        public bool RemoveOnderhoudsbeurt(Onderhoud onderhoudsbeurt)
        {
            foreach (Onderhoud Selected_Onderhoudsbeurt in onderhoudslijst)
            {
                if (onderhoudsbeurt == Selected_Onderhoudsbeurt)
                {
                    onderhoudslijst.Remove(onderhoudsbeurt);
                    return true;
                }
            }
            return false;
        }

    }
}
