using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Gebruiker
    {
        private int mederwerker_ID;
        private string gebruikersNaam;
        private string wachtwoord;

        public int Medewerker_ID { get { return mederwerker_ID; } }
        public string GebruikersNaam { get { return gebruikersNaam; } }
        public string Wachtwoord { get { return wachtwoord; } }

        public Gebruiker(string gebruikersNaam, int Medewerker_id, string wachtwoord)
        {
            this.mederwerker_ID = Medewerker_id;
            this.gebruikersNaam = gebruikersNaam;
            this.wachtwoord = wachtwoord;
        }

        public bool LogIn(string wachtwoordCheck)
        {
            if(wachtwoordCheck == wachtwoord)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
