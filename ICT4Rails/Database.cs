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
        private string connStr = "User Id=" + "--username--" + ";Password=" + "--password--" + ";Data Source=" + "//localhost:1521/XE" + ";";
        private string dataString;

        public List<string> SelectListFromDatabase(string query)
        {
            List<string> dataList = new List<string>();

            try
            {
                using (OracleConnection oracleConn = new OracleConnection(connStr))
                using (OracleCommand cmd = new OracleCommand(query, oracleConn))
                using (OracleDataReader odr = cmd.ExecuteReader())
                {
                    while (odr.Read())
                    {
                        dataList.Add(odr.ToString());
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

        public string SelectStringFromDatabase(string query)
        {      
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(connStr))
                using (OracleCommand cmd = new OracleCommand(query, oracleConn))
                using (OracleDataReader odr = cmd.ExecuteReader())
                {
                    while (odr.Read())
                    {
                        dataString = odr.ToString();
                    }
                }
                return dataString;
            }

            catch (OracleException e)
            {
                errorMessage = "Code: " + e.Data + "\n" + "Message: " + e.Message;
                Console.WriteLine(errorMessage);
                return null;
            }
        }
    }
}
