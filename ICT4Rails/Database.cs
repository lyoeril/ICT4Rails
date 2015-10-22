using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ICT4Rails
{
    class Database
    {
        //username en password moeten nog worden ingevuld voor je eigen databaseinstellingen;
        private string errorMessage;
        private static readonly string connectionString = "User Id=" + "S24B" + ";Password=" + "S24B" + ";Data Source=" + "//localhost:1521/XE" + ";";
        

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
            return null;
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
            List<Medewerker> Medewerkers = new List<Medewerker>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM ONDERHOUD Order by Id";
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
            return null;
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
    }
}
