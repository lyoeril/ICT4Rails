using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;
using System.Drawing;

namespace ICT4Rails
{
    public class InputBox
    {

        public void InputBoxVeranderGebruiker(Administratie administratie, Object obj, string title)
        {

            Form form = new Form();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            form.Text = title;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            if (obj is Gebruiker)
            {
                Gebruiker g = obj as Gebruiker;
                form.ClientSize = new Size(396, 140);
                buttonOk.SetBounds(228, 105, 75, 23);
                buttonCancel.SetBounds(309, 105, 75, 23);
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                Label Gebruikersnaam = new Label();
                Label Wachtwoord = new Label();
                TextBox Gebruikersnaamtxt = new TextBox();
                TextBox Wachtwoordtxt = new TextBox();

                Gebruikersnaam.SetBounds(9, 20, 372, 13);
                Gebruikersnaamtxt.SetBounds(12, 36, 372, 20);
                Wachtwoord.SetBounds(12, 60, 372, 13);
                Wachtwoordtxt.SetBounds(12, 74, 372, 20);

                Gebruikersnaam.Text = "Gebruikersnaam:";
                Wachtwoord.Text = "Wachtwoord:";
                Gebruikersnaamtxt.Text = g.GebruikersNaam;
                Wachtwoordtxt.Text = "(Wachtwoord)";

                Gebruikersnaam.AutoSize = true;
                Wachtwoord.AutoSize = true;
                Gebruikersnaamtxt.Anchor = Gebruikersnaamtxt.Anchor | AnchorStyles.Right;
                Wachtwoordtxt.Anchor = Wachtwoordtxt.Anchor | AnchorStyles.Right;
                form.Controls.AddRange(new Control[] { Gebruikersnaam, Wachtwoord, Gebruikersnaamtxt, Wachtwoordtxt, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, Gebruikersnaam.Right + 10), form.ClientSize.Height);

                DialogResult dialogResult = form.ShowDialog();

                try
                {
                    Gebruiker UpdateGebruiker = new Gebruiker(Gebruikersnaamtxt.Text, g.Medewerker_ID, Wachtwoordtxt.Text);
                    administratie.ChangeGebruiker(UpdateGebruiker);

                }

                catch (OracleException e)
                {
                    MessageBox.Show(e.Message);
                }
                return;
            }

            else if (obj is Medewerker)
            {
                Medewerker m = obj as Medewerker;

                form.ClientSize = new Size(396, 360);
                buttonOk.SetBounds(228, 250, 75, 23);
                buttonCancel.SetBounds(309, 250, 75, 23);
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                Label lblNaam = new Label();
                Label lblEmail = new Label();
                Label lblFunctie = new Label();
                Label lblStraatNr = new Label();
                Label lblPostcode = new Label();
                TextBox txtNaam = new TextBox();
                TextBox txtEmail = new TextBox();
                ComboBox CBFunctie = new ComboBox();
                CBFunctie.Items.Add("SCHOONMAKER");
                CBFunctie.Items.Add("REPARATEUR");
                CBFunctie.Items.Add("BEHEERDER");
                CBFunctie.Items.Add("WAGENPARKBEHEERDER");
                TextBox txtStraatNR = new TextBox();
                TextBox txtPostcode = new TextBox();

                lblNaam.SetBounds(9, 20, 372, 13);
                txtNaam.SetBounds(12, 36, 372, 20);
                lblEmail.SetBounds(12, 60, 372, 13);
                txtEmail.SetBounds(12, 76, 372, 20);
                lblFunctie.SetBounds(12, 100, 372, 13);
                CBFunctie.SetBounds(12, 116, 372, 20);
                lblStraatNr.SetBounds(12, 140, 372, 13);
                txtStraatNR.SetBounds(12, 156, 372, 20);
                lblPostcode.SetBounds(12, 190, 372, 13);
                txtPostcode.SetBounds(12, 206, 372, 20);

                lblNaam.Text = "Naam:";
                lblEmail.Text = "Email:";
                lblFunctie.Text = "Functie:";
                lblStraatNr.Text = "Straatnaam en nummer:";
                lblPostcode.Text = "Postcode:";

                lblNaam.AutoSize = true; lblEmail.AutoSize = true; lblFunctie.AutoSize = true; lblStraatNr.AutoSize = true; lblPostcode.AutoSize = true;

                txtNaam.Anchor = txtNaam.Anchor | AnchorStyles.Right;
                txtEmail.Anchor = txtEmail.Anchor | AnchorStyles.Right;
                CBFunctie.Anchor = CBFunctie.Anchor | AnchorStyles.Right;
                txtStraatNR.Anchor = txtStraatNR.Anchor | AnchorStyles.Right;
                txtPostcode.Anchor = txtPostcode.Anchor | AnchorStyles.Right;
                form.Controls.AddRange(new Control[] { lblNaam, lblEmail, lblFunctie, lblStraatNr, lblPostcode, txtNaam, txtEmail, CBFunctie, txtStraatNR, txtPostcode, buttonOk, buttonCancel });
                form.ClientSize = new Size(Math.Max(300, lblNaam.Right + 10), form.ClientSize.Height);
                DialogResult dialogResult = form.ShowDialog();


                if (!string.IsNullOrWhiteSpace(txtNaam.Text) && !string.IsNullOrWhiteSpace(txtEmail.Text)
                                                                 && !string.IsNullOrWhiteSpace(txtStraatNR.Text))
                {
                    if (administratie.ValidatePostcode(txtPostcode.Text))
                    {
                        if (txtEmail.Text.Contains('@') && txtEmail.Text.Contains('.'))
                        {
                            if (CBFunctie.SelectedItem == null)
                            {
                                MessageBox.Show("Geef een functie op voor de medewerker die u aan het toevoegen bent.");
                                return;
                            }
                            else
                            {
                                string Cbkeuze = CBFunctie.SelectedItem.ToString();
                                Medewerker Updatemedewerker = new Medewerker(m.ID, txtNaam.Text, txtEmail.Text, Cbkeuze, txtStraatNR.Text, txtPostcode.Text);
                                administratie.ChangeMedewerker(Updatemedewerker);
                                administratie.RefreshClass();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Vul een geldige emailadres op.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Is geen goede postcode voor nl");
                    }
                }
                else
                {
                    MessageBox.Show("Vul alle gegevens in om een account te updaten.");
                }
            }
        }
    }
}
                            



