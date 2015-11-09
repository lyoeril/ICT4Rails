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
    public partial class ConducteurForm : Form
    {
        Administratie administratie;
        public ConducteurForm()
        {
            InitializeComponent();
            administratie = new Administratie();
        }

        private void btnCheckStatus_Click(object sender, EventArgs e)
        {
           bool gevonden = false;
           int tramnummer = 0;
           if (Int32.TryParse(tbxTramnummer.Text, out tramnummer))
           {
               tbxStatusHuidig.Text = null;
               foreach (Tram t in administratie.Trams)
               {
                   if (tramnummer == t.Id)
                   {
                       tbxStatusHuidig.Text = t.Status.Naam;
                       cbxStatusNieuw.Enabled = true;
                       gevonden = true;
                       break;
                   }
               }

               if (gevonden == false)
               {
                   MessageBox.Show("Tram is niet gevonden!");
               }
           }
           else
           {
               MessageBox.Show("Vul eerst de gegevens correct in");
           }
        }

        private void btnBevestigStatus_Click(object sender, EventArgs e)
        {
            int tramnummer = 0;
            bool gevonden = false;
            if (Int32.TryParse(tbxTramnummer.Text, out tramnummer) && cbxStatusNieuw.SelectedItem != null)
            {
                foreach (Tram tram in administratie.Trams)
                {
                    if (tram.Id == tramnummer)
                    {
                        try
                        {
                            administratie.TramStatusVeranderen(tramnummer, cbxStatusNieuw.Text);
                            MessageBox.Show("De status van tram " + tbxTramnummer.Text + " is veranderd in '" + cbxStatusNieuw.Text + "'");
                            tbxTramnummer.Text = null;
                            tbxStatusHuidig.Text = null;
                            cbxStatusNieuw.SelectedItem = null;
                            cbxStatusNieuw.Enabled = false;
                            gevonden = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }

                if (gevonden == false)
                {
                    MessageBox.Show("Tram niet gevonden!");
                }
            }
            else
            {
                MessageBox.Show("Vul eerst alle velden correct in!");
            }
        }
    }
}
