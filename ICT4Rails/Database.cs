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
                if (t.Nummer == tram)
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


        private Tram CreateTramFromReader(OracleDataReader reader,List<TramType> tramtypes,List<Status> statuslist)
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
                if(status.Naam == statusnaam)
                {
                    Status = status;
                    break;
                }
            }
            if(beschikbaar == "Y")
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
        public List<Gebruiker> GetAllGebruiker()
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
    }
}
