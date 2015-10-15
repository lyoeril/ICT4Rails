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
        private OracleConnection oracleConn;

        private string user = "system";//Wachtwoord van de server
        private string pw = "password";//Wachtwoord van de server

        private void Connect()
        {
            try
            {
                oracleConn = new OracleConnection("User Id=" + user + ";Password=" + pw + ";Data Source=" + "//localhost:1521/XE" + ";");
            }

            catch (Exception e)
            {
                
            }
            
        }

        
        public List<string> SelectFromDatabase(string Querry)
        {
            List<string> dataList = new List<string>();
            try
            {
                Connect();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = oracleConn;
                cmd.CommandText = Querry;
                cmd.CommandType = CommandType.Text;
                oracleConn.Open();
                OracleDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    dataList.Add(reader.GetString(0));
                }
                return dataList;
            }

            catch (Exception e)
            {
                return null;
            }
        }
    }
}
