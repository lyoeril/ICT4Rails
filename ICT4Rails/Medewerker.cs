using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Medewerker
    {
        private string wachtwoord;
        private string naam;
        private string email;
        private string functie;
        private string adres;
        private string postcode;

        public string Wachtwoord { get { return wachtwoord; } }
        public string Naam { get { return naam; } }
        public string Email { get { return email; } }
        public string Functie { get { return functie; } }
        public string Adres { get { return adres; } }
        public string Postcode { get { return postcode; } }

        public Medewerker(string naam, string email, string functie, string adres, string postcode, string wachtwoord)
        {
            this.naam = naam;
            this.email = email;
            this.functie = functie;
            this.adres = adres;
            this.postcode = postcode;
            this.wachtwoord = wachtwoord;
        }

        public void LogIn()
        {
            return;
        }

        public void LogOut()
        {
            return;
        }
    }
}
