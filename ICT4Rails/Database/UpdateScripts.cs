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
        public void UpdateTram(Tram tram)
        {
            using (OracleConnection connection = Connection)
            {

                string Update = "UPDATE TRAM SET STATUSNAAM =:STATUS, LIJN=:LIJNNUMMER, TYPENAAM =:TYPE  WHERE ID =:IDTRAM ";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("STATUS", tram.Status.Naam));
                    command.Parameters.Add(new OracleParameter("LIJNNUMMER", tram.Lijn));
                    command.Parameters.Add(new OracleParameter("TYPE", tram.Type.Naam));
                    command.Parameters.Add(new OracleParameter("IDTRAM", tram.Id));
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateGebruiker(Gebruiker gebruiker)
        {
            using (OracleConnection connection = Connection)
            {
                string Update = "UPDATE GEBRUIKER SET GEBRUIKERSNAAM =:Gebruikersnaam, WACHTWOORD =:Wachtwoord WHERE MedewerkerID =:ID";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("Gebruikersnaam", gebruiker.GebruikersNaam));
                    command.Parameters.Add(new OracleParameter("Wachtwoord", gebruiker.Wachtwoord));
                    command.Parameters.Add(new OracleParameter("ID", gebruiker.Medewerker_ID));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMedewerker(Medewerker medewerker)
        {
            using (OracleConnection connection = Connection)
            {
                string Update = "UPDATE MEDEWERKER SET NAAM =:Naam, EMAIL =:Email, FUNCTIE =:Functie, ADRES =:Adres, POSTCODE =:Postcode WHERE ID =:ID";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("Naam", medewerker.Naam));
                    command.Parameters.Add(new OracleParameter("Email", medewerker.Email));
                    command.Parameters.Add(new OracleParameter("Functie", medewerker.Functie));
                    command.Parameters.Add(new OracleParameter("Adres", medewerker.Adres));
                    command.Parameters.Add(new OracleParameter("Postcode", medewerker.Postcode));
                    command.Parameters.Add(new OracleParameter("ID", medewerker.ID));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateSpoor(Spoor spoor)
        {
            using (OracleConnection connection = Connection)
            {
                string Update = "UPDATE SPOOR SET ID =:id, SpoorNummer =:spnr, SpoorSector =:spsctr, Beschikbaar =:beschkbr WHERE ID =:id";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("id", spoor.Spoorid));
                    command.Parameters.Add(new OracleParameter("spnr", spoor.Spoornummer));
                    command.Parameters.Add(new OracleParameter("spsctr", spoor.Sectornummer));
                    char beschikbaar = 'N';
                    if (spoor.Beschikbaar)
                    {
                        beschikbaar = 'Y';
                    }
                    command.Parameters.Add(new OracleParameter("beschkbr", beschikbaar));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTrampositie(Trampositie trampositie)
        {
            string aankomstijd1 = trampositie.Aankomstijd.Day.ToString("00") + trampositie.Aankomstijd.Month.ToString("00") + trampositie.Aankomstijd.Year.ToString("0000") + " " + trampositie.Aankomstijd.Hour.ToString("00") + ":" + trampositie.Aankomstijd.Minute.ToString("00")+":"+ trampositie.Aankomstijd.Second.ToString("00");
            string vertrektijd = trampositie.Vertrektijd.Day.ToString("00") + trampositie.Vertrektijd.Month.ToString("00") + trampositie.Vertrektijd.Year.ToString("0000") + " " + trampositie.Vertrektijd.Hour.ToString("00") + ":" + trampositie.Vertrektijd.Minute.ToString("00") + ":" + trampositie.Vertrektijd.Second.ToString("00");

            using (OracleConnection connection = Connection)
            {
                string Update = "UPDATE TRAMPOSITIE SET Spoorid =:spnr, TRAMID =:tramid, AANKOMSTIJD =  TO_TIMESTAMP('" + aankomstijd1 + "','DDMMYYYY HH24:MI:SS'), VERTREKTIJD =  TO_TIMESTAMP('" + vertrektijd + "','DDMMYYYY HH24:MI:SS')  WHERE ID =:id";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("spnr", trampositie.Spoor.Spoorid));
                    command.Parameters.Add(new OracleParameter("tramid", trampositie.Tram.Id));
                    
                    command.Parameters.Add(new OracleParameter("id", trampositie.Id));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateOnderhoud(Onderhoud o)
        {
            using (OracleConnection connection = Connection)
            {
                string starttijd1 = o.Starttijd.Day.ToString("00") + o.Starttijd.Month.ToString("00") + o.Starttijd.Year.ToString("0000") + " " + o.Starttijd.Hour.ToString("00") + ":" + o.Starttijd.Minute.ToString("00");
                string eindtijd1 = o.Eindtijd.Day.ToString("00") + o.Eindtijd.Month.ToString("00") + o.Eindtijd.Year.ToString("0000") + " " + o.Eindtijd.Hour.ToString("00") + ":" + o.Eindtijd.Minute.ToString("00");
                string Update = "UPDATE ONDERHOUD SET MedewerkerID =:MEDEWERKERID, TRAMNUMMERID =:TRAMNUMMER, SOORT =:SOORT, STARTTIJD = TO_TIMESTAMP('" + starttijd1 + "','DDMMYYYY HH24:MI'), EINDTIJD = TO_TIMESTAMP('" + eindtijd1 + "','DDMMYYYY HH24:MI') WHERE ID =:id";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("MEDEWERKERID", o.Medewerker.ID));
                    command.Parameters.Add(new OracleParameter("TRAMNUMMER", o.Tram.Id));
                    command.Parameters.Add(new OracleParameter("SOORT", o.Soort));
                    /*command.Parameters.Add(new OracleParameter("STARTTIJD", "TO_TIMESTAMP('"+starttijd1+"','DDMMYYYY HH24:MI')"));
                    command.Parameters.Add(new OracleParameter("EINDTIJD", "TO_TIMESTAMP('" + eindtijd1 + "','DDMMYYYY HH24:MI')"));*/
                    command.Parameters.Add(new OracleParameter("id", o.ID));
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
