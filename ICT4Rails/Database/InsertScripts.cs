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
        public void InsertMedewerker(Medewerker medewerker)
        {
            using (OracleConnection connection = Connection)
            {
                string Insert = "INSERT INTO MEDEWERKER (ID, Naam, Email, Functie, Adres, Postcode) VALUES (seq_Medewerker_ID.nextval" + ",'" + medewerker.Naam + "','" + medewerker.Email + "','" + medewerker.Functie + "','" + medewerker.Adres + "','" + medewerker.Postcode + "')";
                using (OracleCommand command = new OracleCommand(Insert, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertGebruiker(Gebruiker gebruiker)
        {
            using (OracleConnection connection = Connection)
            {
                string Insert = "INSERT INTO GEBRUIKER (Gebruikersnaam, MedewerkerID, Wachtwoord) VALUES (" + "'" + gebruiker.GebruikersNaam + "'," + gebruiker.Medewerker_ID + ",'" + gebruiker.Wachtwoord + "')";
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
        public void InsertOnderhoud(Medewerker medewerker, int tramnummerID, string opmerking, string soort, DateTime starttijd, DateTime eindtijd)
        {
            int medewerkerID = medewerker.ID;
            string Uppersoort = soort.ToUpper();

            using (OracleConnection connection = Connection)
            {
                string Insert = "INSERT INTO ONDERHOUD(ID, MedewerkerID, TramnummerID, Opmerking, Soort, Starttijd, Eindtijd) VALUES (seq_Onderhoud_ID.nextval," + medewerkerID + "," + tramnummerID + ",'" + opmerking + "','" + Uppersoort + "','" + starttijd + "','" + eindtijd + "')";
                using (OracleCommand command = new OracleCommand(Insert, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
