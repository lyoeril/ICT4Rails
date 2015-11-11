using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ICT4Rails
{

    public class Administratie
    {
        private List<Gebruiker> gebruikers;
        private List<Medewerker> medewerkers;
        private List<Onderhoud> onderhoudslijst;
        private List<Spoor> sporen;
        private List<Tram> trams;
        private List<Status> statuslijst;
        private List<TramType> tramtypes;
        public List<Gebruiker> Gebruikers { get { return gebruikers; } }
        public List<Medewerker> Medewerkers { get { return medewerkers; } }
        public List<Onderhoud> Onderhoudslijst { get { return onderhoudslijst; } }
        public List<Spoor> Sporen { get { return sporen; } }
        public List<Tram> Trams { get { return trams; } }
        public List<Status> Statuslijst { get { return statuslijst; } }
        public List<TramType> Tramtypes { get { return tramtypes; } }
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
            this.statuslijst = data.GetAllStatus();
            this.tramtypes = data.GetAllTramtypes();
        }

        private void RefreshClass()
        {
            this.onderhoudslijst = data.GetAllOnderhoud();
            this.sporen = data.GetAllSporen();
            this.trams = data.GetAllTrams();
            this.gebruikers = data.GetAllGebruikers();
            this.medewerkers = data.GetAllMedewerkers();
            this.statuslijst = data.GetAllStatus();
            this.tramtypes = data.GetAllTramtypes();
        }
        /* alles voor de beheerder */
        public bool AddMedewerker(Medewerker medewerker)
        {
            if (FindMedewerker(medewerker.ID) != null)
            {
                return false;
            }
            data.InsertMedewerker(medewerker);
            RefreshClass();
            return true;
        }

        public bool RemoveMedewerker(Medewerker medewerker)
        {
            if (FindMedewerker(medewerker.ID) != null)
            {
                data.RemoveMedewerker(medewerker);
                RefreshClass();
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
            RefreshClass();
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

        public Gebruiker FindGebruikerByName(string naam)
        {
            if (gebruikers != null)
            {
                foreach (Gebruiker g in gebruikers)
                {
                    if (g.GebruikersNaam == naam)
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
                RefreshClass();
                return true;
            }

            return false;
        }
        public bool RemoveGebruiker(Gebruiker gebruiker)
        {
            if (FindGebruiker(gebruiker.Medewerker_ID) != null)
            {
                data.RemoveGebruiker(gebruiker);
                RefreshClass();
                return true;
            }
            return false;
        }
        public bool ChangeMedewerker(Medewerker medewerker)
        {
            if (FindMedewerker(medewerker.ID) != null)
            {
                data.UpdateMedewerker(medewerker);
                RefreshClass();
                return true;
            }
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
            data.InsertTram(tram);
            RefreshClass();
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
                    RefreshClass();
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
            data.InsertOnderhoud(onderhoudsbeurt);
            RefreshClass();
            return true;
        }

        public bool UpdateOnderhoudsbeurt(Onderhoud onderhoudsbeurt)
        {
            // bij deze methode wordt er een nieuwe onderhoudsbeurt toegevoegd
            foreach (Onderhoud Selected_Onderhoudsbeurt in onderhoudslijst)
            {
                if (onderhoudsbeurt == Selected_Onderhoudsbeurt)
                {
                    throw new Exception("De onderhoudsbeurt bestaat al!");
                }
            }
            data.UpdateOnderhoud(onderhoudsbeurt);
            RefreshClass();
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
                    RefreshClass();
                    return true;
                }
            }
            return false;
        }

        public List<Onderhoud> GetOnderhoudslijst(string soort)
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
            RefreshClass();
        }

        public bool AddOnderhoudsbeurt(Medewerker medewerker, Tram tram, string opmerking, string soort, DateTime starttijd, DateTime eindtijd)
        {
            // bij deze methode wordt er een nieuwe onderhoudsbeurt toegevoegd
            //----------------------------------------------------------------------------------

            data.InsertOnderhoud(new Onderhoud(1, medewerker, tram, starttijd, eindtijd, opmerking, soort));
            RefreshClass();
            return true;
            //----------------------------------------------------------------------------------
        }


        public void TramStatusVeranderen(int tramNummer, string statusnaam)
        {
            foreach (Tram tram in Trams)
            {
                if (tram.Id == tramNummer)
                {
                    foreach (Status status in data.GetAllStatus())
                    {
                        if (statusnaam.ToUpper() == status.Naam)
                        {
                            tram.Status = status;
                            data.UpdateTram(tram);
                            RefreshClass();
                        }
                    }
                    data.UpdateTram(tram);
                    RefreshClass();
                }
            }

        }

        public void TramBewerken(int tramNummer, string typeNaam, string statusNaam, string lijn)
        {
            TramType type = null;
            Status status = null;

            foreach (TramType t in Tramtypes)
            {
                if (t.Naam == typeNaam)
                {
                    type = t;
                    break;
                }
            }

            foreach (Status s in Statuslijst)
            {
                if (s.Naam == statusNaam)
                {
                    status = s;
                    break;
                }
            }

            Tram tram = new Tram(tramNummer, type, status, lijn);
            data.UpdateTram(tram);
            RefreshClass();
        }

        public void TramVerwijderen(int tramNummer)
        {
            Tram t = null;
            foreach (Tram tram in trams)
            {
                if (tram.Id == tramNummer)
                {
                    t = tram;
                }
            }
            data.RemoveTram(t);
            RefreshClass();
        }

        public void AddTramType(TramType type)
        {
            data.InsertTramType(type);
            RefreshClass();
        }

        public Point? GetRowColIndex(TableLayoutPanel tlp, Point point)
        {
            if (point.X > tlp.Width || point.Y > tlp.Height)
            {
                return null;
            }

            int w = tlp.Width;
            int h = tlp.Height;
            int[] widths = tlp.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
            {
                w -= widths[i];
            }
            int col = i + 1;

            int[] heights = tlp.GetRowHeights();

            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
            {
                h -= heights[i];
            }
            int row = i + 1;

            return new Point(col, row);
        }

        public List<Trampositie> GetTramPositie()
        {
            List<Trampositie> positie = data.GetAllTramposities();
            return positie;
        }
    }
}
