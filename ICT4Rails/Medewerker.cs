using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Medewerker
    {
        private string Wachtwoord;
        public string Naam { get; set; }
        public string Email { get;}
        public string Functie { get; set; }
        public string Adres { get; set; }
        public string Postcode { get; set; }

        public Medewerker(string naam, string email, string functie, string adres, string postcode, string wachtwoord)
        {
            Naam = naam;
            Email = email;
            Functie = functie;
            Adres = adres;
            Postcode = postcode;
            Wachtwoord = wachtwoord;
        }

        public void LogIn()
        {
            ;
        }

        public void LogOut()
        {
            ;
        }
    }
}
