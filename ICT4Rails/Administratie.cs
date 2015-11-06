using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICT4Rails
{

    public class Administratie
    {
        private List<Gebruiker> gebruikers;
        private List<Medewerker> medewerkers;
        private List<Onderhoud> onderhoudslijst;
        private List<Spoor> sporen;
        private List<Tram> trams;

        public List<Gebruiker> Gebruikers { get { return gebruikers; } }
        public List<Medewerker> Medewerkers { get { return medewerkers; } }
        public List<Onderhoud> Onderhoudslijst { get { return onderhoudslijst; } }
        public List<Spoor> Sporen { get { return sporen; } }
        public List<Tram> Trams { get { return trams; } }

        Database data;

        public Administratie()
        {
            data = new Database();
            this.gebruikers = data.GetAllGebruikers();
            this.gebruikers.Add(new Gebruiker("", 0, ""));         
            this.medewerkers = data.GetAllMedewerkers();
            this.onderhoudslijst = data.GetAllOnderhoud();
            this.sporen = data.GetAllSporen();
            this.trams = data.GetAllTrams();
        }

        public void RefreshClass()
        {
            Database data = new Database();
            this.gebruikers = data.GetAllGebruikers();
            this.medewerkers = data.GetAllMedewerkers();
        }
        /* alles voor de beheerder */
        public bool AddMedewerker(Medewerker medewerker)
        {
            if (FindMedewerker(medewerker.ID) != null)
            {
                return false;
            }             
            data.InsertMedewerker(medewerker);
            return true;
        }

        public bool RemoveMedewerker(Medewerker medewerker)
        {
            if (FindMedewerker(medewerker.ID) != null)
            {
                data.RemoveMedewerker(medewerker);
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

        public bool AddGebruiker(Gebruiker gebruiker)
        {
            if (FindGebruiker(gebruiker.Medewerker_ID) != null)
            {
                return false;
            }
            data.InsertGebruiker(gebruiker);
            return true;
        }

        public bool ValidatePostcode(string postcode)
        {
            Regex regex = new Regex("^[1-9]{1}[0-9]{3}?[A-Z]{2}$");
            return regex.IsMatch(postcode);
        }

        public Gebruiker FindGebruiker(int id)
        {
            if (gebruikers != null)
            {
                foreach (Gebruiker g in gebruikers)
                {
                    if (g.Medewerker_ID == id)
                    {
                        return g;
                    }
                }
            }
            return null;
        }




        public bool ChangeGebruiker(Gebruiker gebruiker)
        {
            if (FindGebruiker(gebruiker.Medewerker_ID) != null)
            {
                data.UpdateGebruiker(gebruiker);
                return true;
            }
            return false;
        }
        public bool RemoveGebruiker(Gebruiker gebruiker)
        {
            if (FindGebruiker(gebruiker.Medewerker_ID) != null)
            {
                data.RemoveGebruiker(gebruiker);
                return true;
            }
            return false;
        }
        public bool ChangeMedewerker(Medewerker medewerker)
        {
            // TO DO
            throw new NotImplementedException();
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
            data.InsertTram(tram);
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

        public void SpoorStatusVeranderen(int id, int spoornr, int sectornr, bool beschikbaar)
        {
            Spoor spoor = new Spoor(id, spoornr, sectornr, beschikbaar);
            data.UpdateSpoor(spoor);
        }

        public bool AddOnderhoudsbeurt(Medewerker medewerker, int tramnummerID, string opmerking, string soort, DateTime starttijd, DateTime eindtijd)
        {
            // bij deze methode wordt er een nieuwe onderhoudsbeurt toegevoegd
            //----------------------------------------------------------------------------------

            data.InsertOnderhoud(medewerker, tramnummerID, opmerking, soort, starttijd, eindtijd);
            return true;
            //----------------------------------------------------------------------------------
        }


        public void TramStatusVeranderen(int tramNummer, string statusnaam)
        {
            foreach(Tram tram in Trams)
            {
                if(tram.Id == tramNummer)
                {
                    foreach(Status status in data.GetAllStatus())
                    {
                        if(statusnaam.ToUpper() == status.Naam)
                        {
                            tram.Status = status;
                            data.UpdateTram(tram);
                        }
                    }
                    
                    data.UpdateTram(tram);
                }
            }

        }

        public void TramBewerken(int tramNummer, string typeNaam, string statusNaam, string lijn, bool beschikbaarheid)
        {
            TramType type = null;
            Status status = null;

            foreach (TramType t in data.GetAllTramtypes())
            {
                if (t.Naam == typeNaam)
                {
                    type = t;
                    break;
                }
            }

            foreach (Status s in data.GetAllStatus())
            {
                if (s.Naam == statusNaam)
                {
                    status = s;
                    break;
                }
            }

            Tram tram = new Tram(tramNummer, type, status, lijn, beschikbaarheid);
            data.UpdateTram(tram);
        }

        public void TramVerwijderen(int tramNummer)
        {
            Tram t = null;
            foreach(Tram tram in trams)
            {
                if(tram.Id == tramNummer)
                {
                    t = tram;
                }
            }

            data.RemoveTram(t);
        }

        public List<Medewerker> GetAllMedewerkers()
        {
            if (data.GetAllMedewerkers() != null)
            {
                return data.GetAllMedewerkers();
            }
            return null;
        }

        public List<TramType> GetTypes()
        {
            return data.GetAllTramtypes();
        }

        public void AddTramType(TramType type)
        {
            data.InsertTramType(type);
        }
    }
}
