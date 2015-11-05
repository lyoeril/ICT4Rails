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


        public List<Medewerker> GetAllMedewerkers()
        {
            List<Medewerker> Medewerkers = new List<Medewerker>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM MEDEWERKER Order by Id";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Medewerkers.Add(CreateMedewerkerFromReader(reader));
                        }
                    }
                }
            }
            return Medewerkers;
        }

        public List<Onderhoud> GetAllOnderhoud()
        {
            List<Onderhoud> Onderhoudslijst = new List<Onderhoud>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM ONDERHOUD Order by Id";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Onderhoudslijst.Add(CreateOnderhoudFromReader(reader));
                        }
                    }
                }
            }
            return Onderhoudslijst;
        }

        public List<Tram> GetAllTrams()
        {
            List<Tram> Trams = new List<Tram>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM TRAM";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        List<TramType> tramtypes = GetAllTramtypes();
                        List<Status> statuslist = GetAllStatus();
                        while (reader.Read())
                        {
                            Trams.Add(CreateTramFromReader(reader, tramtypes, statuslist));
                        }
                    }
                }
            }
            return Trams;
        }

        public List<Status> GetAllStatus()
        {
            List<Status> Statuslist = new List<Status>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM STATUS";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Statuslist.Add(CreateStatusFromReader(reader));
                        }
                    }
                }
            }
            return Statuslist;
        }

        public List<TramType> GetAllTramtypes()
        {
            List<TramType> Tramtypes = new List<TramType>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM TYPE";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tramtypes.Add(CreateTramTypeFromReader(reader));
                        }
                    }
                }
            }
            return Tramtypes;
        }

        public List<Gebruiker> GetAllGebruikers()
        {
            List<Gebruiker> Gebruikers = new List<Gebruiker>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM GEBRUIKER";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gebruikers.Add(CreateGebruikerFromReader(reader));
                        }
                    }
                }
            }
            return Gebruikers;
        }

        public List<Spoor> GetAllSporen()
        {
            List<Spoor> Sporen = new List<Spoor>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM SPOOR";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sporen.Add(CreateSpoorFromReader(reader));
                        }
                    }
                }
            }
            return Sporen;
        }

        public List<Reservering> GetAllReserveringen()
        {
            List<Reservering> Reserveringen = new List<Reservering>();
            using (OracleConnection connection = Connection)
            {
                string query = "SELECT * FROM Reservering";
                using (OracleCommand command = new OracleCommand(query, connection))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        List<Tram> trams = GetAllTrams();
                        List<Spoor> sporen = GetAllSporen();
                        while (reader.Read())
                        {
                            Reserveringen.Add(CreateReserveringFromReader(reader, trams, sporen));
                        }
                    }
                }
            }
            return Reserveringen;
        }


    }
}
