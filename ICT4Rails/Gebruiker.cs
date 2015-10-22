using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Rails
{
    public class Gebruiker
    {
        private int id;
        private string gebruikersNaam;
        private string wachtwoord;

        public int ID { get { return id; } }
        public string GebruikersNaam { get { return gebruikersNaam; } }

        public Gebruiker(int id, string gebruikersNaam, string wachtwoord)
        {
            this.id = id;
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

        public bool LogOut()
        {
            if(Program.loggedIn == this)
            {
                Program.loggedIn = null;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
