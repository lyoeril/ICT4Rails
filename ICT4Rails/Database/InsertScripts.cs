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
                string Insert = "INSERT INTO MEDEWERKER (ID, Naam, Email, Functie, Adres, Postcode) VALUES (seq_Medewerker_ID.nextval" + ",':NAAM',':EMAIL',':FUNCTIE',':ADRES',':POSTCODE')";
                using (OracleCommand command = new OracleCommand(Insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("NAAM", medewerker.Naam));
                    command.Parameters.Add(new OracleParameter("EMAIL", medewerker.Email));
                    command.Parameters.Add(new OracleParameter("FUNCTIE", medewerker.Functie));
                    command.Parameters.Add(new OracleParameter("ADRES", medewerker.Adres));
                    command.Parameters.Add(new OracleParameter("POSTCODE", medewerker.Postcode));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertGebruiker(Gebruiker gebruiker)
        {
            using (OracleConnection connection = Connection)
            {
                string Insert = "INSERT INTO GEBRUIKER (Gebruikersnaam, MedewerkerID, Wachtwoord) VALUES (':NAAM',:ID,':PASS')";
                using (OracleCommand command = new OracleCommand(Insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("NAAM", gebruiker.GebruikersNaam));
                    command.Parameters.Add(new OracleParameter("ID", gebruiker.Medewerker_ID));
                    command.Parameters.Add(new OracleParameter("PASS", gebruiker.Wachtwoord));
                    command.ExecuteNonQuery();
                }

            }
        }
        public void InsertReservering(Reservering reservering)
        {
            using (OracleConnection connection = Connection)
            {
                string insert = "insert into reservering (ID, SpoorID, TramID, Datum) values(seq_Reservering_ID.nextval, :SPOORID, :DATUM)";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("SPOORID", reservering.Spoor.Spoorid));
                    command.Parameters.Add(new OracleParameter("TRAMID", reservering.Tram.Id));
                    command.Parameters.Add(new OracleParameter("DATUM", reservering.Datum));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertTram(Tram tram)
        {
            using (OracleConnection connection = Connection)
            {
                string insert = "insert into tram (ID, TYPENAAM, STATUSNAAM, LIJN, BESCHIKBAAR) values(:TRAMID, ':TYPENAAM', ':STATUSNAAM', ':LIJN', ':BESCHIKBAAR')";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("TRAMID", tram.Id));
                    command.Parameters.Add(new OracleParameter("TYPENAAM", tram.Type.Naam));
                    command.Parameters.Add(new OracleParameter("STATUSNAAM", tram.Status.Naam));
                    command.Parameters.Add(new OracleParameter("LIJN", tram.Lijn));
                    char beschikbaar = 'N';
                    if (tram.Beschikbaar)
                    {
                        beschikbaar = 'Y';
                    }
                    command.Parameters.Add(new OracleParameter("BESCHIKBAAR", beschikbaar));
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
