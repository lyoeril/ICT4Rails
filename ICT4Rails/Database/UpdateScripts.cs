﻿using System;
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
        public void UpdateTramStatus(Tram tram)
        {
            string UpperStatus = tram.Status.ToString().ToUpper();

            using (OracleConnection connection = Connection)
            {
                string Update = "UPDATE TRAM SET STATUSNAAM =:Status, LIJN=:Lijnnummer, BESCHIKBAAR=:Beschikbaar  WHERE ID =:IDTRAM ";
                using (OracleCommand command = new OracleCommand(Update, connection))
                {
                    command.Parameters.Add(new OracleParameter("Status", tram.Status.Naam.ToUpper()));
                    command.Parameters.Add(new OracleParameter("Lijnnummer", tram.Lijn));
                    char beschikbaar;
                    if (tram.Beschikbaar)
                    {
                        beschikbaar = 'Y';
                    }
                    else
                    {
                        beschikbaar = 'N';
                    }
                    command.Parameters.Add(new OracleParameter("Beschikbaar", beschikbaar));
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
    }
}
