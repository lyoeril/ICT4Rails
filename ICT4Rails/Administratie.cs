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
        /* alles voor de beheerder */
        public void AddMedewerker(Medewerker medewerker)
        {
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
        public void TramToevoegen(Tram tram)
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
        }
        public bool TramVerwijderen(Tram tram)
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
        public void AddOnderhoudsbeurt(Onderhoud onderhoudsbeurt)
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
