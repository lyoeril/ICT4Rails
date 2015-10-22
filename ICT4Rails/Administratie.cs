using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{

    public class Administratie
    {
        private List<Gebruiker> gebruikers;
        private List<Medewerker> medewerkers;
        private List<Onderhoud> onderhoudslijst;
        private List<Tram> trams;

        public List<Gebruiker> Gebruikers { get { return gebruikers; } }
        public List<Medewerker> Medewerkers { get { return medewerkers; } }
        public List<Onderhoud> Onderhoudslijst { get { return onderhoudslijst; } }
        public List<Tram> Trams { get { return trams; } }

        public Administratie()
        {
            this.gebruikers = new List<Gebruiker>();
            this.medewerkers = new List<Medewerker>();
            this.onderhoudslijst = new List<Onderhoud>();
            this.trams = new List<Tram>();
        }

        /* alles voor de beheerder */
        public bool AddMedewerker(Medewerker medewerker)
        {
            if(FindMedewerker(medewerker.ID) != null)
            {
                //throw new Exception("De medewerker bestaat al!"); exception kan hier niet omdat een exception de methode break't, de return false code wordt niet uitgevoerd en dan wordt er niets gereturnd
                return false;
            }             
            medewerkers.Add(medewerker);
            return true;
        }

        public bool RemoveMedewerker(Medewerker medewerker)
        {
            if (FindMedewerker(medewerker.ID) != null)
            {
                medewerkers.Remove(FindMedewerker(medewerker.ID));
                return true;
            }
            return false;
        }

        public Medewerker FindMedewerker(int id)
        {
            if (medewerkers != null)
            {
                foreach (Medewerker m in medewerkers)
                {
                    if (m.ID == id)
                    {
                        return m;
                    }                    
                }
            }
            return null;
            throw new Exception("Er zijn geen medewerkers");
        }

        public bool ChangeMedewerker(Medewerker medewerker)
        {
            // TO DO
            return false;

        }

        public bool AddTram(Tram tram)
        {
            // er wordt hier een tram toegevoegd uit het systeem
            foreach (Tram Selected_Tram in trams)
            {
                if (tram == Selected_Tram)
                {
                    throw new Exception("De tram bestaat al!");
                }
            }
            trams.Add(tram);
            return true;
        }

        public bool RemoveTram(Tram tram)
        {
            // er wordt hier een tram verwijderd uit het systeem
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
        public bool AddOnderhoudsbeurt(Onderhoud onderhoudsbeurt)
        {
            // bij deze methode wordt er een nieuwe onderhoudsbeurt toegevoegd
            foreach (Onderhoud Selected_Onderhoudsbeurt in onderhoudslijst)
            {
                if (onderhoudsbeurt == Selected_Onderhoudsbeurt)
                {
                    throw new Exception("De onderhoudsbeurt bestaat al!");
                }
            }
            onderhoudslijst.Add(onderhoudsbeurt);
            return true;
        }

        public bool RemoveOnderhoudsbeurt(Onderhoud onderhoudsbeurt)
        {
            // bij deze methode wordt er een nieuwe onderhoudsbeurt verwijderd
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

        public List<Onderhoud> GetOnderhoudslijst (string soort)
        {
            // bij deze methode wordt er een lijst geretourneerd gefiltert door een soort
            return null;
        }

        public void OnderhoudsbeurtAfronden(DateTime starttijd, DateTime eindtijd)
        {
            //TO DO
            // de onderhoudsbeurt wordt hier afgerond en de tramstatus wordt veranderd naar remise waardoor het een ander spoort krijgt toegewezen
        }

        public void SpoorReserveren()
        {
            // TO DO
            // hierbij word een reservering geplaatst tussen een tram en een spoor.
        }

        public void SpoorStatusVeranderen()
        {
            // TO DO
            // bij deze methode wordt de status van het spoor verandert naar ....
        }
    }
}
