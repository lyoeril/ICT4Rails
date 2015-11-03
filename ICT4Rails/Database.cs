using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace ICT4Rails
{
    public class Database
    {
        //username en password moeten nog worden ingevuld voor je eigen databaseinstellingen;
        private static string dataUser = "S24B";
        private static string dataPass = "S24B";
        private static string dataSrc = "//localhost:1521/XE";
        private static readonly string connectionString = "User Id=" + dataUser + ";Password=" + dataPass + ";Data Source=" + dataSrc + ";";


        public static OracleConnection Connection
        {
            get
            {
                OracleConnection connection = new OracleConnection(connectionString);
                connection.Open();
                return connection;
            }
        }

        public List<Medewerker> GetAllMedewerkers()
        {
            List<Medewerker> Medewerkers = new List<Medewerker>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM MEDEWERKER Order by Id";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Medewerkers.Add(CreateMedewerkerFromReader(reader));
                        }
                    }
                }
            }
            return Medewerkers;
        }
        public void InsertMedewerker(string naam, string email, string functie, string adres, string postcode)
        {
            using (OracleConnection connection = Connection)
            {
                string Insert = "INSERT INTO MEDEWERKER (ID, Naam, Email, Functie, Adres, Postcode) VALUES (seq_Medewerker_ID.nextval" + ",'" + naam + "','" + email + "','" + functie + "','" + adres + "','" + postcode + "')";
                using (OracleCommand command = new OracleCommand(Insert, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertGebruiker(string gebruikersnaam, int medewerkerID, string wachtwoord)
        {
            using (OracleConnection connection = Connection)
            {
                string Insert = "INSERT INTO GEBRUIKER (Gebruikersnaam, MedewerkerID, Wachtwoord) VALUES (" + "'" + gebruikersnaam + "'," + medewerkerID + ",'" + wachtwoord + "')";
                using (OracleCommand command = new OracleCommand(Insert, connection))
                {
                    command.ExecuteNonQuery();
                }

            }
        }
        public void InsertReservering(int spoorID, int tramID, DateTime datum)
        {
            using (OracleConnection connection = Connection)
            {
                string insert = "insert into reservering (ID, SpoorID, TramID, Datum) values(seq_Reservering_ID.nextval, " + Convert.ToString(spoorID) + ", " + Convert.ToString(tramID) + ", " + Convert.ToString(datum) + ")";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveMedewerker(Medewerker mederwerker)
        {
            using (OracleConnection connection = Connection)
            {
                string Delete = "DELETE FROM MEDEWERKER WHERE ID ="+ mederwerker.ID;

                using (OracleCommand command = new OracleCommand(Delete, connection))
                {
                    command.ExecuteNonQuery();
                }

            }
        }

        public void InsertTramPositie(int spoorID, int tramID, DateTime aankomst, DateTime vertrek)
        {
            using (OracleConnection connection = Connection)
            {
                //ID IN DE STRING MOET NOG TOEGEVOEGD WORDEN MAAR IDK OF DIT MET SEQUENCE KAN
                string insert = "insert into trampositie (ID, SpoorID, TramID, AankomstTijd, VertrekTijd) values(ID, " + Convert.ToString(spoorID) + ", " + Convert.ToString(tramID) + ", " + Convert.ToString(aankomst) + ", " + Convert.ToString(vertrek) + ")";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

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

        public List<Onderhoud> GetAllOnderhoud()
        {
            List<Onderhoud> Onderhoudslijst = new List<Onderhoud>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM ONDERHOUD Order by Id";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Onderhoudslijst.Add(CreateOnderhoudFromReader(reader));
                        }
                    }
                }
            }
            return Onderhoudslijst;
        }

        private Onderhoud CreateOnderhoudFromReader(OracleDataReader reader)
        {
            int id = Convert.ToInt32(reader["ID"]);
            int medewerker = Convert.ToInt32(reader["MedewerkerID"]);
            int tram = Convert.ToInt32(reader["TramnummerID"]);
            DateTime starttijd = Convert.ToDateTime(reader["Starttijd"]);
            DateTime eindtijd = Convert.ToDateTime(reader["Eindtijd"]);
            string opmerking = Convert.ToString(reader["Opmerking"]);
            string soort = Convert.ToString(reader["Soort"]);

            Medewerker me = null;
            Tram tr = null;
            Administratie a = new Administratie();
            foreach (Medewerker m in a.Medewerkers)
            {
                if (m.ID == medewerker)
                {
                    me = m;
                }
            }
            foreach (Tram t in a.Trams)
            {
                if (t.Id == tram)
                {
                    tr = t; ;
                }
            }

            Onderhoud onderhoud = new Onderhoud(id, me, tr, starttijd, eindtijd, opmerking, soort);
            return onderhoud;
        }

        public List<Tram> GetAllTrams()
        {
            List<Tram> Trams = new List<Tram>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM TRAM";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        List<TramType> tramtypes = GetAllTramtypes();
                        List<Status> statuslist = GetAllStatus();
                        while (reader.Read())
                        {
                            Trams.Add(CreateTramFromReader(reader, tramtypes, statuslist));
                        }
                    }
                }
            }
            return Trams;
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

        public List<Status> GetAllStatus()
        {
            List<Status> Statuslist = new List<Status>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM STATUS";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Statuslist.Add(CreateStatusFromReader(reader));
                        }
                    }
                }
            }
            return Statuslist;
        }
        private Status CreateStatusFromReader(OracleDataReader reader)
        {
            return new Status(
                Convert.ToString(reader["NAAM"]),
                Convert.ToString(reader["BIJZONDERHEDEN"])
                );
        }


        public List<TramType> GetAllTramtypes()
        {
            List<TramType> Tramtypes = new List<TramType>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM TYPE";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tramtypes.Add(CreateTramTypeFromReader(reader));
                        }
                    }
                }
            }
            return Tramtypes;
        }

        private TramType CreateTramTypeFromReader(OracleDataReader reader)
        {
            return new TramType(
                Convert.ToString(reader["NAAM"]),
                Convert.ToString(reader["BESCHRIJVING"]),
                Convert.ToInt32(reader["LENGTE"])
                );
        }
        public List<Gebruiker> GetAllGebruikers()
        {
            List<Gebruiker> Gebruikers = new List<Gebruiker>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM GEBRUIKER";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gebruikers.Add(CreateGebruikerFromReader(reader));
                        }
                    }
                }
            }
            return Gebruikers;
        }

        private Gebruiker CreateGebruikerFromReader(OracleDataReader reader)
        {
            return new Gebruiker(
                Convert.ToString(reader["GEBRUIKERSNAAM"]),
                Convert.ToInt32(reader["MEDEWERKERID"]),
                Convert.ToString(reader["WACHTWOORD"])
                );
        }

        public List<Spoor> GetAllSporen()
        {
            List<Spoor> Sporen = new List<Spoor>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT SPOORNUMMER, SPOORSECTOR, BESCHIKBAAR FROM SPOOR";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Sporen.Add(CreateSpoorFromReader(reader));
                        }
                    }
                }
            }
            return Sporen;
        }

        private Spoor CreateSpoorFromReader(OracleDataReader reader)
        {
            int spoorid = Convert.ToInt32(reader["SPOORNUMMER"]);
            int spoorsector = Convert.ToInt32(reader["SPOORSECTOR"]);
            char Beschikbaar = Convert.ToChar(reader["BESCHIKBAAR"]);
            bool beschikbaar;
            if ( Beschikbaar == 'Y')
            {
                beschikbaar = true;
            }
            else
            {
                beschikbaar = false;
            }
            return new Spoor(spoorid, spoorsector, beschikbaar);
        }
        public List<Reservering> GetAllReserveringen()
        {
            List<Reservering> Reserveringen = new List<Reservering>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM Reservering";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        List<Tram> trams = GetAllTrams();
                        List<Spoor> sporen = GetAllSporen();
                        while (reader.Read())
                        {
                            Reserveringen.Add(CreateReserveringFromReader(reader, trams,sporen));
                        }
                    }
                }
            }
            return Reserveringen;
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
            if(actief == 'Y')
            {
                Actief = true;
            }
            else
            {
                Actief = false;
            }
            return new Reservering(id,Tram,Spoor,datetime, Actief);
        }
    }
}
