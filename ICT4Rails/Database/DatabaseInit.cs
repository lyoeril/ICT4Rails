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
    }
}
