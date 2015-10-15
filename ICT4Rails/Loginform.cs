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
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //if (tbxPassword.Text != "")
            //{
            //    MainForm remise = new MainForm();
            //    remise.FormClosing += MainForm_FormClosing;
            //    remise.Show();
            //    this.Hide();
            //    tbxPassword.Text = "";
            //}
            //else { MessageBox.Show("Incorrect wachtwoord"); }

            // "asdf" is temp admin user because reasons
            if (tbxUsername.Text == "Bestuurder" || 
                tbxUsername.Text == "asdf")
            {
                Program.loggedIn = tbxUsername.Text;
                MainForm remise = new MainForm();
                remise.FormClosing += MainForm_FormClosing;
                remise.Show();
                this.Hide();
                tbxPassword.Text = "";
            }
            else { MessageBox.Show("Incorrecte username"); }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Show();
                Program.loggedIn = "";
            }
        }
    }
}
