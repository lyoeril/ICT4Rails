using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Medewerker
    {
        private int id;
        private string wachtwoord;
        private string naam;
        private string email;
        private string functie;
        private string adres;
        private string postcode;

        public int ID { get { return id; } }
        public string Naam { get { return naam; } }
        public string Email { get { return email; } }
        public string Functie { get { return functie; } }
        public string Adres { get { return adres; } }
        public string Postcode { get { return postcode; } }

        public Medewerker(int id, string naam, string email, string functie, string adres, string postcode)
        {
            this.naam = naam;
            this.email = email;
            this.functie = functie;
            this.adres = adres;
            this.postcode = postcode;
            this.id = id;
        }

        public void LogIn()
        {
            return;
        }

        public void LogOut()
        {
            return;
        }

        public override string ToString()
        {
            //override tostring methode om gegevens gemakkelijk weer te geven
            //wachtwoord staat niet in tostring methode (voor nu)    

            string naamstr;
            if(naam == "")
            {
                naamstr = "Onbekend";
            }
            else
            {
                naamstr = naam;
            }

            string emailstr;
            if(email == "")
            {
                emailstr = "Onbekend";
            }
            else
            {
                emailstr = email;
            }

            string functiestr;
            if(functie == "")
            {
                functiestr = "Onbekend";
            }
            else
            {
                functiestr = functie;
            }

            string adresstr;
            if(adres == "")
            {
                adresstr = "Onbekend";
            }
            else
            {
                adresstr = adres;
            }

            string postcodestr;
            if(postcode == "")
            {
                postcodestr = "Onbekend";
            }
            else
            {
                postcodestr = postcode;
            }

            string info = "ID: " + ID.ToString() +
                "Naam: " + naamstr +
                "Emailadres: " + emailstr +
                "Functie: " + functiestr +
                "Adres: " + adresstr +
                "Postcode: " + postcodestr;

            return info;
        }
    }
}
