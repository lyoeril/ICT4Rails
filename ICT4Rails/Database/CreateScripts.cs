using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
namespace ICT4Rails
{
    public partial class Database
    {
        private Medewerker CreateMedewerkerFromReader(OracleDataReader reader)
        {
            return new Medewerker(
                Convert.ToInt32(reader["ID"]),
                Convert.ToString(reader["Naam"]),
                Convert.ToString(reader["Email"]),
                Convert.ToString(reader["Functie"]),
                Convert.ToString(reader["Adres"]),
                Convert.ToString(reader["Postcode"])
                );
        }

        private Onderhoud CreateOnderhoudFromReader(OracleDataReader reader, List<Tram> trams, List<Medewerker> medewerkers)
        {
            
            int id = Convert.ToInt32(reader["ID"]);
            var medewerkerid = reader["MedewerkerID"];
            int tram = Convert.ToInt32(reader["TramnummerID"]);
            string soort = Convert.ToString(reader["Soort"]);
            string opmerking = Convert.ToString(reader["Opmerking"]);
            Medewerker me = null;
            Tram tr = null;
            foreach (Tram t in trams)
            {
                if (t.Id == tram)
                {
                    tr = t;
                    break;
                }
            }
            Onderhoud onderhoud;
            
            if(medewerkerid != null)
            {
                DateTime starttijd = Convert.ToDateTime( reader["Starttijd"]);
                DateTime eindtijd = Convert.ToDateTime( reader["Eindtijd"]);
                
                foreach (Medewerker m in medewerkers)
                {
                    int medewerker = Convert.ToInt32(medewerkerid);
                    if (m.ID == medewerker)
                    {
                        me = m;
                    }
                }


                onderhoud = new Onderhoud(id, me, tr, starttijd, eindtijd, opmerking, soort);
            }
            else
            {
                onderhoud = new Onderhoud(id, tr, soort, opmerking);
            }
            
            return onderhoud;
        }

        private Tram CreateTramFromReader(OracleDataReader reader, List<TramType> tramtypes, List<Status> statuslist)
        {
            int id = Convert.ToInt32(reader["ID"]);
            string typenaam = Convert.ToString(reader["TYPENAAM"]);
            string statusnaam = Convert.ToString(reader["STATUSNAAM"]);
            string lijn = Convert.ToString(reader["LIJN"]);
            string beschikbaar = Convert.ToString(reader["BESCHIKBAAR"]);
            TramType Type = null;
            Status Status = null;
            bool trueorfalse;
            // eerst zal status en tramtype uitgewerkt moeten worden
            foreach (TramType type in tramtypes)
            {
                if (type.Naam == typenaam)
                {
                    Type = type;
                    break;
                }
            }
            foreach (Status status in statuslist)
            {
                if (status.Naam == statusnaam)
                {
                    Status = status;
                    break;
                }
            }
            if (beschikbaar == "Y")
            {
                trueorfalse = true;
            }
            else
            {
                trueorfalse = false;
            }
            return new Tram(id, Type, Status, lijn, trueorfalse);
        }

        private Status CreateStatusFromReader(OracleDataReader reader)
        {
            return new Status(
                Convert.ToString(reader["NAAM"]),
                Convert.ToString(reader["BIJZONDERHEDEN"])
                );
        }

        private TramType CreateTramTypeFromReader(OracleDataReader reader)
        {
            return new TramType(
                Convert.ToString(reader["NAAM"]),
                Convert.ToString(reader["BESCHRIJVING"]),
                Convert.ToInt32(reader["LENGTE"])
                );

        }

        private Trampositie CreateTramPositieFromReader(OracleDataReader reader, List<Spoor> sporenlijst, List<Tram> Tramlijst)
        {
            int id = Convert.ToInt32(reader["ID"]);
            int spoorid = Convert.ToInt32(reader["SPOORID"]);
            int tramid = Convert.ToInt32(reader["TRAMID"]);
            DateTime aankomstijd = Convert.ToDateTime(reader["AANKOMSTIJD"]);
            DateTime vertrektijd = Convert.ToDateTime(reader["VERTREKTIJD"]);

            Tram Tram = null;
            Spoor Spoor = null;
            foreach (Tram tram in Tramlijst)
            {
                if (tram.Id == tramid)
                {
                    Tram = tram;
                    break;
                }
            }

            foreach (Spoor spoor in sporenlijst)
            {
                if (spoor.Spoornummer == spoorid)
                {
                    Spoor = spoor;
                    break;
                }
            }
            return new Trampositie(id, Spoor, Tram, aankomstijd, vertrektijd);
        }
        private Gebruiker CreateGebruikerFromReader(OracleDataReader reader)
        {
            return new Gebruiker(
                Convert.ToString(reader["GEBRUIKERSNAAM"]),
                Convert.ToInt32(reader["MEDEWERKERID"]),
                Convert.ToString(reader["WACHTWOORD"])
                );
        }

        private Spoor CreateSpoorFromReader(OracleDataReader reader)
        {
            int spoorid = Convert.ToInt32(reader["ID"]);
            int spoornummer = Convert.ToInt32(reader["SPOORNUMMER"]);
            int spoorsector = Convert.ToInt32(reader["SPOORSECTOR"]);
            char Beschikbaar = Convert.ToChar(reader["BESCHIKBAAR"]);
            bool beschikbaar;
            if (Beschikbaar == 'Y')
            {
                beschikbaar = true;
            }
            else
            {
                beschikbaar = false;
            }
            return new Spoor(spoorid, spoornummer, spoorsector, beschikbaar);
        }

        private Reservering CreateReserveringFromReader(OracleDataReader reader, List<Tram> trams, List<Spoor> sporen)
        {
            int id = Convert.ToInt32(reader["ID"]);
            int spoorid = Convert.ToInt32(reader["SPOORID"]);
            int tramid = Convert.ToInt32(reader["TRAMID"]);
            DateTime datetime = Convert.ToDateTime(reader["DATUM"]);
            char actief = Convert.ToChar(reader["ACTIEF"]);

            Tram Tram = null;
            Spoor Spoor = null;
            foreach (Tram tram in trams)
            {
                if (tram.Id == id)
                {
                    Tram = tram;
                    break;
                }
            }
            foreach (Spoor spoor in sporen)
            {
                if (spoor.Spoornummer == spoorid)
                {
                    Spoor = spoor;
                    break;
                }
            }
            bool Actief;
            if (actief == 'Y')
            {
                Actief = true;
            }
            else
            {
                Actief = false;
            }
            return new Reservering(id, Tram, Spoor, datetime, Actief);
        }
    }
}
