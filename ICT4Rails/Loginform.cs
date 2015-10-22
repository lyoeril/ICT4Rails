using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICT4Rails
{
    public partial class LoginForm : Form
    {
        Administratie administratie;
        public LoginForm()
        {
            InitializeComponent();
            administratie = new Administratie();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // "asdf" is temp admin user because reasons
            foreach (Gebruiker g in administratie.Gebruikers)
            {
                if (g.GebruikersNaam == tbxUsername.Text)
                {
                    if (g.LogIn(tbxPassword.Text))
                    {
                        Program.loggedIn = g;
                        MainForm remise = new MainForm();
                        remise.FormClosing += MainForm_FormClosing;
                        remise.Show();
                        this.Hide();
                        tbxPassword.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Ongeldig Wachtwoord");
                    }
                }
                else
                {
                    MessageBox.Show("Ongeldige Gebruikersnaam");
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Show();
                Program.loggedIn = null;
            }
        }
    }
}
