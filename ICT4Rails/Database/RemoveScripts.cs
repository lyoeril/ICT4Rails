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
        public void RemoveMedewerker(Medewerker mederwerker)
        {
            using (OracleConnection connection = Connection)
            {
                string Delete = "DELETE FROM MEDEWERKER WHERE ID =" + mederwerker.ID;

                using (OracleCommand command = new OracleCommand(Delete, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void RemoveTram(Tram tram)
        {
            using (OracleConnection connection = Connection)
            {
                string Delete = "DELETE FROM TRAM WHERE ID = " + tram.Id;
                using (OracleCommand command = new OracleCommand(Delete, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveGebruiker(Gebruiker gebruiker)
        {
            using (OracleConnection connection = Connection)
            {
                string Delete = "DELETE FROM GEBRUIKER WHERE MedewerkerID =" + gebruiker.Medewerker_ID;

                using (OracleCommand command = new OracleCommand(Delete, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
