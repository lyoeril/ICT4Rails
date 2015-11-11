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
                string Insert = "INSERT INTO MEDEWERKER (ID, Naam, Email, Functie, Adres, Postcode) VALUES (seq_Medewerker_ID.nextval, :NAAM, :EMAIL, :FUNCTIE, :ADRES, :POSTCODE)";
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
                string Insert = "INSERT INTO GEBRUIKER (Gebruikersnaam, MedewerkerID, Wachtwoord) VALUES (:NAAM, :ID, :PASS)";
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
            string date = reservering.Datum.Day.ToString("00") + reservering.Datum.Month.ToString("00") + reservering.Datum.Year.ToString("0000");
            using (OracleConnection connection = Connection)
            {
                string insert = "insert into reservering (ID, SpoorID, TramID, Datum, ACTIEF) values(seq_Reservering_ID.nextval, :SPOORID, :TRAMID, TO_TIMESTAMP('" + date + "','DDMMYYYY') ,'Y')";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("SPOORID", reservering.Spoor.Spoorid));
                    command.Parameters.Add(new OracleParameter("TRAMID", reservering.Tram.Id));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertTram(Tram tram)
        {
            using (OracleConnection connection = Connection)
            {
                string insert = "insert into tram (ID, TYPENAAM, STATUSNAAM, LIJN) values(:TRAMID, :TYPENAAM, :STATUSNAAM, :LIJN)";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("TRAMID", tram.Id));
                    command.Parameters.Add(new OracleParameter("TYPENAAM", tram.Type.Naam));
                    command.Parameters.Add(new OracleParameter("STATUSNAAM", tram.Status.Naam));
                    command.Parameters.Add(new OracleParameter("LIJN", tram.Lijn));
                    command.ExecuteNonQuery();
                }
            }
        }
        public void InsertTramPositie(int spoorID, int tramID, DateTime aankomst, DateTime? vertrek)
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
        public void InsertOnderhoud(Onderhoud onderhoud)
        {
            using (OracleConnection connection = Connection)
            {
                try
                {
                    if (onderhoud.Medewerker == null)
                    {
                        string Insert =
                        "INSERT INTO ONDERHOUD(ID, TramnummerID, Soort, Opmerking)" +
                        " VALUES (seq_Onderhoud_ID.nextval, :TRAMNUMMERID, :SOORT, :OPMERKING)";
                        using (OracleCommand command = new OracleCommand(Insert, connection))
                        {
                            command.Parameters.Add(new OracleParameter("TRAMNUMMERID", onderhoud.Tram.Id));
                            command.Parameters.Add(new OracleParameter("SOORT", onderhoud.Soort));
                            command.Parameters.Add(new OracleParameter("OPMERKING", onderhoud.Opmerking));
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string Insert =
                        "INSERT INTO ONDERHOUD(ID, MedewerkerID, TramnummerID, Opmerking, Soort, Starttijd, Eindtijd)" +
                        " VALUES (seq_Onderhoud_ID.nextval, :MEDEWERKERID, :TRAMNUMMERID, :OPMERKING,:SOORT,:STARTTIJD,:EINDTIJD)";
                        using (OracleCommand command = new OracleCommand(Insert, connection))
                        {
                            command.Parameters.Add(new OracleParameter("MEDEWERKERID", onderhoud.Medewerker.ID));
                            command.Parameters.Add(new OracleParameter("TRAMNUMMERID", onderhoud.Tram.Id));
                            command.Parameters.Add(new OracleParameter("OPMERKING", onderhoud.Opmerking));
                            command.Parameters.Add(new OracleParameter("SOORT", onderhoud.Soort));
                            command.Parameters.Add(new OracleParameter("STARTTIJD", onderhoud.Starttijd));
                            command.Parameters.Add(new OracleParameter("EINDTIJD", onderhoud.Eindtijd));
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (OracleException oexc)
                {
                    Console.WriteLine(oexc.Message);
                }
            }
        }

        public void InsertTramType(TramType type)
        {
            using (OracleConnection connection = Connection)
            {
                string insert = "insert into type (naam, beschrijving, lengte) values(:NAAM, :BESCHRIJVING, :LENGTE)";
                using (OracleCommand command = new OracleCommand(insert, connection))
                {
                    command.Parameters.Add(new OracleParameter("NAAM", type.Naam));
                    command.Parameters.Add(new OracleParameter("BESCHRIJVING", type.Beschrijving));
                    command.Parameters.Add(new OracleParameter("LENGTE", type.Lengte));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
