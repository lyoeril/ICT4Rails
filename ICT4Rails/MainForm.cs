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
using System.Text.RegularExpressions;
using Phidgets;
using Phidgets.Events;

namespace ICT4Rails
{
    public partial class MainForm : Form
    {
        private Label[][] Sporen;

        //Medewerker        
       
        private Database DataMed = new Database();
        private Administratie administratie;
        private int MedID;
        private Medewerker Fullmedewerker;
        private Gebruiker gebruiker;
        private bool heeftaccount = false;
        private List<Gebruiker> gebruikers;
        private List<Medewerker> medewerkers;
        private RFID rfid;

        public MainForm(Administratie administratie)
        {
            InitializeComponent();
            LogIn();
            VulSporen();
            Sporen = SporenArray();
            vullMederwerkerList();
            OpenAccountUI();
            loadComboboxes();

            this.administratie = administratie;

            this.tableLayoutPanel1.CellPaint += new TableLayoutCellPaintEventHandler(tableLayoutPanel1_CellPaint);

            rfid = new RFID();
            rfid.Attach += new AttachEventHandler(rfid_Attach);
            rfid.Detach += new DetachEventHandler(rfid_Detach);
            rfid.Tag += new TagEventHandler(rfid_Tag);
            rfid.TagLost += new TagEventHandler(rfid_TagLost);
            rfid.open();
            if (rfid.outputs.Count > 0)
            {
                rfid.Antenna = true;
                rfid.LED = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rfid.Attach -= new AttachEventHandler(rfid_Attach);
            rfid.Detach -= new DetachEventHandler(rfid_Detach);
            rfid.Tag -= new TagEventHandler(rfid_Tag);
            rfid.TagLost -= new TagEventHandler(rfid_TagLost);

            if (rfid.outputs.Count > 0)
            {
                rfid.Antenna = false;
                rfid.LED = false;
            }

            rfid.close();
        }

        //e.Tag = RFID-tag ID
        private void rfid_Tag(object sender, TagEventArgs e)
        {
            MessageBox.Show(e.Tag);
        }

        private void rfid_TagLost(object sender, TagEventArgs e)
        {
            //
        }

        private void rfid_Attach(object sender, AttachEventArgs e)
        {
            if (rfid.outputs.Count > 0)
            {
                rfid.Antenna = true;
                rfid.LED = true;
            }
        }

        private void rfid_Detach(object sender, DetachEventArgs e)
        {
            if (rfid.outputs.Count > 0)
            {
                rfid.Antenna = false;
                rfid.LED = false;
            }
        }

        private void LogIn()
        {
            if (Program.loggedIn.GebruikersNaam == "qwer")
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
        }

        // Overzichttab

        // Vult TableLayoutPanel met labels op de correcte locaties
        private void VulSporen()
        {
            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            int labelCount = 1;
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 23; y++)
                {
                    if (x >= 3 && x <= 16 && y == 22 ||
                        x >= 6 && x <= 16 && y == 21 ||
                        x >= 7 && x <= 9 && y == 5 ||
                        x >= 7 && x <= 16 && y == 20 ||
                        x >= 8 && x <= 16 && y == 19 ||
                        x == 9 && y <= 5 ||
                        x <= 9 && y >= 6 && y <= 12 ||
                        x >= 9 && x <= 11 && y == 18 ||
                        x == 10 && y >= 9 && y <= 12 ||
                        x >= 11 && x <= 15 && y >= 6 && y <= 12 ||
                        x == 11 && y >= 13 && y <= 17 ||
                        x == 13 && y == 18 ||
                        x == 15 && y <= 5 ||
                        x == 16 && y >= 11 && y <= 18 ||
                        x == 17 && y <= 6 ||
                        x >= 17 && y >= 7 && y <= 12)
                    {
                        // niks
                    }
                    else
                    {
                        Label label = new Label();
                        label.Anchor = System.Windows.Forms.AnchorStyles.None;
                        label.AutoSize = true;
                        label.Click += new EventHandler(label_Click);
                        label.Name = "label" + Convert.ToString(labelCount);
                        label.Tag = Convert.ToString(x) + ", " + Convert.ToString(y);
                        label.Text = "20" + Convert.ToString(x); // + ", " + Convert.ToString(y);
                        tableLayoutPanel1.Controls.Add(label, x, y);
                        labelCount++;
                    }
                }
            }



            // Geeft alle spoor labels de correcte text
            foreach (Label l in tableLayoutPanel1.Controls)
            {
                if ((string)l.Tag == "0, 0") { l.Text = "38"; }
                else if ((string)l.Tag == "1, 0") { l.Text = "37"; }
                else if ((string)l.Tag == "2, 0") { l.Text = "36"; }
                else if ((string)l.Tag == "3, 0") { l.Text = "35"; }
                else if ((string)l.Tag == "4, 0") { l.Text = "34"; }
                else if ((string)l.Tag == "5, 0") { l.Text = "33"; }
                else if ((string)l.Tag == "6, 0") { l.Text = "32"; }
                else if ((string)l.Tag == "7, 0") { l.Text = "31"; }
                else if ((string)l.Tag == "8, 0") { l.Text = "30"; }
                else if ((string)l.Tag == "10, 0") { l.Text = "40"; }
                else if ((string)l.Tag == "11, 0") { l.Text = "41"; }
                else if ((string)l.Tag == "12, 0") { l.Text = "42"; }
                else if ((string)l.Tag == "13, 0") { l.Text = "43"; }
                else if ((string)l.Tag == "14, 0") { l.Text = "44"; }
                else if ((string)l.Tag == "16, 0") { l.Text = "45"; }
                else if ((string)l.Tag == "18, 0") { l.Text = "58"; }

                else if ((string)l.Tag == "0, 13") { l.Text = "57"; }
                else if ((string)l.Tag == "1, 13") { l.Text = "56"; }
                else if ((string)l.Tag == "2, 13") { l.Text = "55"; }
                else if ((string)l.Tag == "3, 13") { l.Text = "54"; }
                else if ((string)l.Tag == "4, 13") { l.Text = "53"; }
                else if ((string)l.Tag == "5, 13") { l.Text = "52"; }
                else if ((string)l.Tag == "6, 13") { l.Text = "51"; }
                else if ((string)l.Tag == "7, 13") { l.Text = "64"; }
                else if ((string)l.Tag == "8, 13") { l.Text = "63"; }
                else if ((string)l.Tag == "9, 13") { l.Text = "62"; }
                else if ((string)l.Tag == "10, 13") { l.Text = "61"; }
                else if ((string)l.Tag == "12, 13") { l.Text = "74"; }
                else if ((string)l.Tag == "13, 13") { l.Text = "75"; }
                else if ((string)l.Tag == "14, 13") { l.Text = "76"; }
                else if ((string)l.Tag == "15, 13") { l.Text = "77"; }

                else if ((string)l.Tag == "17, 13") { l.Text = "12"; }
                else if ((string)l.Tag == "17, 14") { l.Text = "13"; }
                else if ((string)l.Tag == "17, 15") { l.Text = "14"; }
                else if ((string)l.Tag == "17, 16") { l.Text = "15"; }
                else if ((string)l.Tag == "17, 17") { l.Text = "16"; }
                else if ((string)l.Tag == "17, 18") { l.Text = "17"; }
                else if ((string)l.Tag == "17, 19") { l.Text = "18"; }
                else if ((string)l.Tag == "17, 20") { l.Text = "19"; }
                else if ((string)l.Tag == "17, 21") { l.Text = "20"; }
                else if ((string)l.Tag == "17, 22") { l.Text = "21"; }
            }
        }

        //de methode die aangeroepen wordt als er op een label geklikt wordt
        private void label_Click(object sender, EventArgs ee)
        {
            Label label = (Label)sender;
            if (label.Text != "")
            {
                foreach (Tram t in administratie.Trams)
                {
                    if (Convert.ToString(t.Id) == label.Text)
                    {
                        for (int spoor = 0; spoor < Sporen.Length; spoor++)
                        {
                            if (Sporen[spoor] != null)
                            {
                                for (int sector = 1; sector < Sporen[spoor].Length; sector++)
                                {
                                    if (Sporen[spoor][sector] == label)
                                    {
                                        this.tabcontrolRemise.SelectedTab = tabpageRemiseBeheer;
                                        tbxRemiseBeheerTramNummer.Text = Convert.ToString(t.Id);
                                        tbxRemiseBeheerTramLijn.Text = Convert.ToString(t.Lijn);
                                        //tbxRemiseBeheerTramType.SelectedItem = 
                                        //moet nog een methode toegevoegd worden om alle bestaande types uit de db te halen
                                        tbxRemiseBeheerSpoorBeheerSpoorNummer.Text = Convert.ToString(spoor);
                                        tbxRemiseBeheerSpoorBeheerSectorNummer.Text = Convert.ToString(sector);
                                        tbxRemiseBeheerSpoorBeheerTramNummer.Text = Convert.ToString(t.Id);
                                        tbxStatusbeheerTramNummer.Text = Convert.ToString(t.Id);
                                        tbxStatusbeheerOnderhoudTramnr.Text = Convert.ToString(t.Id);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Deze methode is er zodat de onderstaande code niet nog langer wordt
        private Label GetLabel(int x, int y)
        {
            Label label = (Label)tableLayoutPanel1.GetControlFromPosition(x, y);
            return label;
        }

        // Stopt alle sectorLabels in spoorArrays en de spoor arrays in een collectieve sporenArray
        private Label[][] SporenArray()
        {
            Label[][] sporenArray = new Label[78][];

            sporenArray[12] = new Label[1] { GetLabel(18, 13) };
            sporenArray[13] = new Label[1] { GetLabel(18, 14) };
            sporenArray[14] = new Label[1] { GetLabel(18, 15) };
            sporenArray[15] = new Label[1] { GetLabel(18, 16) };
            sporenArray[16] = new Label[1] { GetLabel(18, 17) };
            sporenArray[17] = new Label[1] { GetLabel(18, 18) };
            sporenArray[18] = new Label[1] { GetLabel(18, 19) };
            sporenArray[19] = new Label[1] { GetLabel(18, 20) };
            sporenArray[20] = new Label[1] { GetLabel(18, 21) };
            sporenArray[21] = new Label[1] { GetLabel(18, 22) };
            sporenArray[30] = new Label[4] { GetLabel(8, 1), GetLabel(8, 2), GetLabel(8, 3), GetLabel(8, 4) };
            sporenArray[31] = new Label[4] { GetLabel(7, 1), GetLabel(7, 2), GetLabel(7, 3), GetLabel(7, 4) };
            sporenArray[32] = new Label[5] { GetLabel(6, 1), GetLabel(6, 2), GetLabel(6, 3), GetLabel(6, 4), GetLabel(6, 5) };
            sporenArray[33] = new Label[5] { GetLabel(5, 1), GetLabel(5, 2), GetLabel(5, 3), GetLabel(5, 4), GetLabel(5, 5) };
            sporenArray[34] = new Label[5] { GetLabel(4, 1), GetLabel(4, 2), GetLabel(4, 3), GetLabel(4, 4), GetLabel(4, 5) };
            sporenArray[35] = new Label[5] { GetLabel(3, 1), GetLabel(3, 2), GetLabel(3, 3), GetLabel(3, 4), GetLabel(3, 5) };
            sporenArray[36] = new Label[5] { GetLabel(2, 1), GetLabel(2, 2), GetLabel(2, 3), GetLabel(2, 4), GetLabel(2, 5) };
            sporenArray[37] = new Label[5] { GetLabel(1, 1), GetLabel(1, 2), GetLabel(1, 3), GetLabel(1, 4), GetLabel(1, 5) };
            sporenArray[38] = new Label[5] { GetLabel(0, 1), GetLabel(0, 2), GetLabel(0, 3), GetLabel(0, 4), GetLabel(0, 5) };
            sporenArray[40] = new Label[8] { GetLabel(10, 1), GetLabel(10, 2), GetLabel(10, 3), GetLabel(10, 4), GetLabel(10, 5), GetLabel(10, 6), GetLabel(10, 7), GetLabel(10, 8) };
            sporenArray[41] = new Label[5] { GetLabel(11, 1), GetLabel(11, 2), GetLabel(11, 3), GetLabel(11, 4), GetLabel(11, 5) };
            sporenArray[42] = new Label[5] { GetLabel(12, 1), GetLabel(12, 2), GetLabel(12, 3), GetLabel(12, 4), GetLabel(12, 5) };
            sporenArray[43] = new Label[5] { GetLabel(13, 1), GetLabel(13, 2), GetLabel(13, 3), GetLabel(13, 4), GetLabel(13, 5) };
            sporenArray[44] = new Label[5] { GetLabel(14, 1), GetLabel(14, 2), GetLabel(14, 3), GetLabel(14, 4), GetLabel(14, 5) };
            sporenArray[45] = new Label[10] { GetLabel(16, 1), GetLabel(16, 2), GetLabel(16, 3), GetLabel(16, 4), GetLabel(16, 5), GetLabel(16, 6), GetLabel(16, 7), GetLabel(16, 8), GetLabel(16, 9), GetLabel(16, 10) };
            sporenArray[51] = new Label[7] { GetLabel(6, 14), GetLabel(6, 15), GetLabel(6, 16), GetLabel(6, 17), GetLabel(6, 18), GetLabel(6, 19), GetLabel(6, 20) };
            sporenArray[52] = new Label[8] { GetLabel(5, 14), GetLabel(5, 15), GetLabel(5, 16), GetLabel(5, 17), GetLabel(5, 18), GetLabel(5, 19), GetLabel(5, 20), GetLabel(5, 21) };
            sporenArray[53] = new Label[8] { GetLabel(4, 14), GetLabel(4, 15), GetLabel(4, 16), GetLabel(4, 17), GetLabel(4, 18), GetLabel(4, 19), GetLabel(4, 20), GetLabel(4, 21) };
            sporenArray[54] = new Label[8] { GetLabel(3, 14), GetLabel(3, 15), GetLabel(3, 16), GetLabel(3, 17), GetLabel(3, 18), GetLabel(3, 19), GetLabel(3, 20), GetLabel(3, 21) };
            sporenArray[55] = new Label[9] { GetLabel(2, 14), GetLabel(2, 15), GetLabel(2, 16), GetLabel(2, 17), GetLabel(2, 18), GetLabel(2, 19), GetLabel(2, 20), GetLabel(2, 21), GetLabel(2, 22) };
            sporenArray[56] = new Label[9] { GetLabel(1, 14), GetLabel(1, 15), GetLabel(1, 16), GetLabel(1, 17), GetLabel(1, 18), GetLabel(1, 19), GetLabel(1, 20), GetLabel(1, 21), GetLabel(1, 22) };
            sporenArray[57] = new Label[9] { GetLabel(0, 14), GetLabel(0, 15), GetLabel(0, 16), GetLabel(0, 17), GetLabel(0, 18), GetLabel(0, 19), GetLabel(0, 20), GetLabel(0, 21), GetLabel(0, 22) };
            sporenArray[58] = new Label[6] { GetLabel(18, 1), GetLabel(18, 2), GetLabel(18, 3), GetLabel(18, 4), GetLabel(18, 5), GetLabel(18, 6) };
            sporenArray[61] = new Label[4] { GetLabel(10, 14), GetLabel(10, 15), GetLabel(10, 16), GetLabel(10, 17) };
            sporenArray[62] = new Label[4] { GetLabel(9, 14), GetLabel(9, 15), GetLabel(9, 16), GetLabel(9, 17) };
            sporenArray[63] = new Label[5] { GetLabel(8, 14), GetLabel(8, 15), GetLabel(8, 16), GetLabel(8, 17), GetLabel(8, 18) };
            sporenArray[64] = new Label[6] { GetLabel(7, 14), GetLabel(7, 15), GetLabel(7, 16), GetLabel(7, 17), GetLabel(7, 18), GetLabel(7, 19) };
            sporenArray[74] = new Label[5] { GetLabel(12, 14), GetLabel(12, 15), GetLabel(12, 16), GetLabel(12, 17), GetLabel(12, 18) };
            sporenArray[75] = new Label[4] { GetLabel(13, 14), GetLabel(13, 15), GetLabel(13, 16), GetLabel(13, 17) };
            sporenArray[76] = new Label[5] { GetLabel(14, 14), GetLabel(14, 15), GetLabel(14, 16), GetLabel(14, 17), GetLabel(14, 18) };
            sporenArray[77] = new Label[5] { GetLabel(15, 14), GetLabel(15, 15), GetLabel(15, 16), GetLabel(15, 17), GetLabel(15, 18) };

            return sporenArray;
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            foreach (Label l in tableLayoutPanel1.Controls)
            {
                if (e.Row == tableLayoutPanel1.GetRow(l) && e.Column == tableLayoutPanel1.GetColumn(l))
                {
                    if (l.Text == "1")
                    {
                        g.FillRectangle(Brushes.LightGreen, r);
                    }
                    else if (l.Text == "2")
                    {
                        g.FillRectangle(Brushes.Yellow, r);
                    }
                    else if (l.Text == "5")
                    {
                        g.FillRectangle(Brushes.MediumPurple, r);
                    }
                    else if (l.Text == "10")
                    {
                        g.FillRectangle(Brushes.LightGray, r);
                    }
                    else if (l.Text == "16/24")
                    {
                        g.FillRectangle(Brushes.Brown, r);
                    }
                }
            }
        }

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
            if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedText == "Blokkeer")
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
            if (!string.IsNullOrWhiteSpace(cbxStatusbeheerTramStatus.Text))
            {
                administratie.TramStatusVeranderen(Convert.ToInt32(tbxStatusbeheerTramNummer.Text), cbxStatusbeheerTramStatus.Text);
                MessageBox.Show("De status van de tram is veranderd in: " + cbxStatusbeheerTramStatus.Text);
            }
        }

        //AccountBeheer

        private void vullMederwerkerList()
        {
            lbAccountMedewerkers.Items.Clear();
            lbAccountGebruiker.Items.Clear();
            medewerkers = DataMed.GetAllMedewerkers();
            gebruikers = DataMed.GetAllGebruikers();
            // Haal alle medewerkers op van database
            foreach (Medewerker medewerker in medewerkers)
            {
                lbAccountMedewerkers.Items.Add(medewerker);
            }

            foreach (Gebruiker gebruiker in gebruikers)
            {
                lbAccountGebruiker.Items.Add(gebruiker);
            }
            
        }

        private void btnAccountToevoegen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxAccountNaam.Text) && !string.IsNullOrWhiteSpace(tbxAccountPostcode.Text)
                                                                 && !string.IsNullOrWhiteSpace(tbxAccountStrtNR.Text))
            {
                bool rekt = ValidatePostcode(tbxAccountPostcode.Text);
                if (rekt) 
                {
                    if (tbxAccountEmail.Text.Contains('@') && tbxAccountEmail.Text.Contains('.'))
                    {
                        if (cbAccountFunctie.SelectedItem == null)
                        {
                            MessageBox.Show("Geef een functie op voor de medewerker die u aan het toevoegen bent.");
                        }
                        else
                        {
                            string Cbkeuze = cbAccountFunctie.SelectedItem.ToString();
                            Medewerker medewerker = new Medewerker(0, tbxAccountNaam.Text, tbxAccountEmail.Text, Cbkeuze, tbxAccountStrtNR.Text, tbxAccountPostcode.Text);
                            administratie.AddMedewerker(medewerker);
                        }
                        //Medewerker toevoegen aan datbase
                        vullMederwerkerList();
                        clearTextboxes();
                        administratie.RefreshClass();
                    }
                    else
                    {
                        MessageBox.Show("Email is niet geldig gevonden door het systeem.");
                    }
                }
                else
                {
                    MessageBox.Show("Vul alle gegevens in om een account aan te maken.");
                }
            }
            else
            {
                MessageBox.Show("Is geen goede postcode voor nl");
            }
        }

        private void OpenAccountUI()
        {
            btnAccountToevoegen.Enabled = true;
            tbxAccountEmail.Enabled = true;
            tbxAccountNaam.Enabled = true;
            tbxAccountPostcode.Enabled = true;
            tbxAccountStrtNR.Enabled = true;
            cbAccountFunctie.Enabled = true;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (administratie.FindGebruiker(MedID) != null)
            {
                MessageBox.Show("Gebruiker heeft al een inlog account");
            }
            else
            {
                Gebruiker gebruiker = new Gebruiker(tbxAccountUsername.Text, MedID, tbxAccountWachtwoord.Text);
                administratie.AddGebruiker(gebruiker);
                MessageBox.Show("Account is toegevoegd en kan gebruikt worden", "ICT4Rails",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                administratie.RefreshClass();
                vullMederwerkerList();
                clearTextboxes();
            }
        }

        private void lbAccountMedewerkers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbAccountMedewerkers.SelectedItem != null)
            {
                Medewerker medewerker = lbAccountMedewerkers.SelectedItem as Medewerker;
                Fullmedewerker = lbAccountMedewerkers.SelectedItem as Medewerker;
                MedID = medewerker.ID;
                enableButtons();

                if (administratie.FindGebruiker(medewerker.ID) != null)
                {
                    gebruiker = administratie.FindGebruiker(medewerker.ID);
                    heeftaccount = true;
                }
            }
        }

        private void lbAccountGebruiker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbAccountGebruiker.SelectedItem != null)
            {
                gebruiker = lbAccountGebruiker.SelectedItem as Gebruiker;
                btnAccountGebrkerverw.Enabled = true;
            }
        }

        private void enableButtons()
        {
            tbxAccountUsername.Enabled = true;
            tbxAccountWachtwoord.Enabled = true;
            BtnAccountInlogToevoegen.Enabled = true;
            BttnAccountRemoveMedewerker.Enabled = true;
        }
        private void clearTextboxes()
        {
            tbxAccountEmail.Text = "";
            tbxAccountNaam.Text = "";
            tbxAccountPostcode.Text = "";
            tbxAccountStrtNR.Text = "";
            cbAccountFunctie.Text = "";
        }
        private bool ValidatePostcode(string postcode)
        {
            Regex regex = new Regex("^[1-9]{1}[0-9]{3}?[A-Z]{2}$");
            return regex.IsMatch(postcode);
        }
        private void BttnAccountRemoveMedewerker_Click(object sender, EventArgs e)
        {
            if (administratie.FindGebruiker(Fullmedewerker.ID) == null)
            {
                administratie.RemoveMedewerker(Fullmedewerker);
                administratie.RefreshClass();
                vullMederwerkerList();
            }
            else
            {
                DialogResult result = MessageBox.Show("Gebruiker heeft een account, wilt u door gaan met het verwijderen?", "Question", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if(heeftaccount == true)
                    {
                        administratie.RemoveGebruiker(gebruiker);
                        administratie.RemoveMedewerker(Fullmedewerker);
                    }
                    else
                    {
                        administratie.RemoveMedewerker(Fullmedewerker);
                    }
                    administratie.RefreshClass();
                    vullMederwerkerList();
                    heeftaccount = false;
                }
            }
        }

        private void btnAccountGebrkerverw_Click(object sender, EventArgs e)
        {
            if (administratie.FindGebruiker(gebruiker.Medewerker_ID) == null)
            {
                administratie.RemoveGebruiker(gebruiker);
                administratie.RefreshClass();
                vullMederwerkerList();
            }
        }

        private void loadComboboxes()
        {
            foreach (Medewerker m in medewerkers)
            {
                cbxOnderhoudMedewerker.Items.Add(m);
            }
        }

        private void btnOnderhoudBevestig_Click(object sender, EventArgs e)
        {
            string opmerking;
            DateTime starttijd = Convert.ToDateTime(dtpOnderhoudStarttijd.Value);
            DateTime eindtijd = Convert.ToDateTime(dtpOnderhoudEindtijd.Value);
            Medewerker m = cbxOnderhoudMedewerker.SelectedItem as Medewerker;
            string soort = cbxOnderhoudSoort.SelectedItem.ToString();
            int tramnummer = Convert.ToInt32(tbxStatusbeheerOnderhoudTramnr.Text);

            opmerking = "none";
            //Opmerking werkt nog niet: designeer aanpassing nodig: geen textbox voor opmerking aanwezig;

            try
            {
                administratie.AddOnderhoudsbeurt(m, tramnummer, opmerking, soort, starttijd, eindtijd);
            }
            catch (FormatException fe)
            {
                MessageBox.Show(starttijd.ToString());
                MessageBox.Show(fe.Message);
            }

        }

        
    }
}
