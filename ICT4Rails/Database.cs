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
        private static readonly string connectionString = "User Id=" + "--username--" + ";Password=" + "--password--" + ";Data Source=" + "//localhost:1521/XE" + ";";
        

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
                Convert.ToInt32(reader["Id"]),
                Convert.ToString(reader["Naam"]),
                Convert.ToString(reader["Email"]),
                Convert.ToString(reader["Functie"]),
                Convert.ToString(reader["Adres"]),
                Convert.ToString(reader["Postcode"])
                );
        }
    }
}
