using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace ICT4Rails
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            if (Program.loggedIn == "Bestuurder")
            {
                tabcontrolRemise.Controls.Remove(tabpageRemiseOverzicht);
                tabcontrolRemise.Controls.Remove(tabpageRemiseBeheer);
                tabcontrolRemise.TabPages.Insert(0, tabpageStatusBeheer);
                tabcontrolRemise.Controls.Remove(tabpageAccountBeheer);
                tabcontrolRemise.Controls.Remove(tabPage4);
            }
            else
            {
                tabcontrolRemise.TabPages.Insert(0, tabpageRemiseOverzicht);
                tabcontrolRemise.TabPages.Insert(1, tabpageRemiseBeheer);
                tabcontrolRemise.TabPages.Insert(2, tabpageStatusBeheer);
                tabcontrolRemise.TabPages.Insert(3, tabpageAccountBeheer);
                tabcontrolRemise.TabPages.Insert(4, tabPage4);
            }
            //tabcontrolRemise.Controls.Remove(tabpageRemiseOverzicht);
        }

        // Overzichttab

        // [insert code here]

        // Beheertab

        private void btnBeheerBevestig_Click(object sender, EventArgs e)
        {
            //tabcontrolRemise.TabPages.Insert(0, tabpageRemiseOverzicht);
        }

        private void btnRemiseBeheerBevestig_Click(object sender, EventArgs e)
        {
            tbxRemiseBeheerTramNummer.Text = "";
            tbxRemiseBeheerTramLijn.Text = "";
            cbxRemiseBeheerTramType.SelectedItem = null;
        }

        private void cbxRemiseBeheerTramBewerking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRemiseBeheerTramBewerking.SelectedText == "Voeg toe")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                tbxRemiseBeheerTramLijn.Enabled = true;
                cbxRemiseBeheerTramType.Enabled = true;
            }
            else if (cbxRemiseBeheerTramBewerking.SelectedText == "Verwijder")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                tbxRemiseBeheerTramLijn.Enabled = false;
                cbxRemiseBeheerTramType.Enabled = false;
            }
            else if (cbxRemiseBeheerTramBewerking.SelectedText == "Bewerk")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                tbxRemiseBeheerTramLijn.Enabled = true;
                cbxRemiseBeheerTramType.Enabled = true;
            }
            btnRemiseBeheerTramBeheerBevestig.Enabled = true;
        }

        private void cbxRemiseBeheerSpoorBeheerBewerking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbxRemiseBeheerSpoorBeheerBewerking.SelectedText == "Blokkeer")
            {
                tbxRemiseBeheerSpoorBeheerSpoorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerSectorNummer.Enabled = false;
                tbxRemiseBeheerSpoorBeheerTramNummer.Enabled = false;
            }
            else if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedText == "Reserveer")
            {
                tbxRemiseBeheerSpoorBeheerSpoorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerSectorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerTramNummer.Enabled = true;
            }
            btnRemiseBeheerSpoorBeheerBevestig.Enabled = true;
        }

        private void btnRemiseBeheerSpoorBeheerBevestig_Click(object sender, EventArgs e)
        {
            tbxRemiseBeheerSpoorBeheerSpoorNummer.Text = "";
            tbxRemiseBeheerSpoorBeheerSectorNummer.Text = "";
            tbxRemiseBeheerSpoorBeheerTramNummer.Text = "";
        }

        private void btnBevestigTramStatus_Click(object sender, EventArgs e)
        {
            cbxStatusbeheerTramStatus.SelectedItem = null;
            tbxStatusbeheerTramNummer.Text = "";
        }
    }
}
