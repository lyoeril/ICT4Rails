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
        private string user = "system";//Wachtwoord van de server
        private string pw = "password";//Wachtwoord van de server
        private string errorMessage;
        private string connStr;

        public List<string> SelectFrommDatabase(string query)
        {
            List<string> dataList = new List<string>();

            try
            {
                connStr = "User Id=" + user + ";Password=" + pw + ";Data Source=" + "//localhost:1521/XE" + ";";

                using (OracleConnection oracleConn = new OracleConnection(connStr))
                {
                    using (OracleCommand cmd = new OracleCommand(query, oracleConn))
                    {
                        using (OracleDataReader odr = cmd.ExecuteReader())
                        {
                            while (odr.Read())
                            {
                                dataList.Add(odr.ToString());
                            }
                        }
                    }
                }                
                return dataList;
            }

            catch(OracleException e)
            {
                errorMessage = "Code: " + e.Data + "\n" + "Message: " + e.Message;
                Console.WriteLine(errorMessage);
                return null;
            }
        }
    }
}
