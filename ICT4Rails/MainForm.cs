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
using Oracle.ManagedDataAccess.Client;

namespace ICT4Rails
{
    public partial class MainForm : Form
    {
        private Label[][] Sporen;
        private string[][] Lijnen;

        //Medewerker        

        private Database DataMed = new Database();
        private Administratie administratie;
        private int MedID;
        private Medewerker medewerker;
        private Gebruiker gebruiker;
        private bool heeftaccount = false;
        private RFID rfid;

        public MainForm(Administratie administratie)
        {
            InitializeComponent();
            this.administratie = administratie;
            LogIn();
            VulSporen();
            Sporen = SporenArray();
            Lijnen = LijnenArray();
            VulLijnnummers();
            vullMederwerkerList();
            OpenAccountUI();
            loadListComboboxStatusbeheerOnderhoudMedewerker();

            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
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

        private void rfid_Tag(object sender, TagEventArgs e)
        {
            rfid.LED = true;
            //int id = 0;
            //if (e.Tag == "") { id = 0; } // TAG
            //else if (e.Tag == "") { id = 0; } // TAG
            //else if (e.Tag == "") { id = 0; } // TAG
            //else if (e.Tag == "") { id = 0; } // TAG
            //foreach (Tram t in administratie.Trams)
            //{
            //    if (t.Id == id)
            //    {
            //        SorteerTram(t);
            //    }
            //}
        }

        private void rfid_TagLost(object sender, TagEventArgs e)
        {
            rfid.LED = false;
        }

        private void rfid_Attach(object sender, AttachEventArgs e)
        {
            if (rfid.outputs.Count > 0)
            {
                rfid.Antenna = true;
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
            foreach (Medewerker medewerker in administratie.Medewerkers)
            {
                if (Program.loggedIn.Medewerker_ID == medewerker.ID)
                {
                    if (medewerker.Functie == "BEHEERDER")
                    {
                        tabcontrolRemise.TabPages.Insert(0, tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Insert(1, tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Insert(2, tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Insert(3, tabpageAccountBeheer);
                        tabcontrolRemise.TabPages.Insert(4, tabpageOnderhoud);
                    }
                    else if (medewerker.Functie == "REPARATEUR")
                    {
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Insert(0, tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageAccountBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageOnderhoud);
                    }
                    else if (medewerker.Functie == "SCHOONMAAK")
                    {
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageAccountBeheer);
                        tabcontrolRemise.TabPages.Insert(0, tabpageOnderhoud);
                    }
                    else if (medewerker.Functie == "WAGENPARKBEHEERDER")
                    {
                        tabcontrolRemise.TabPages.Insert(0, tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Insert(1, tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Insert(2, tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageAccountBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageOnderhoud);
                    }
                    else
                    {
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageAccountBeheer);
                        tabcontrolRemise.TabPages.Insert(0, tabpageOnderhoud);
                    }
                }


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
                        label.Text = ""; //"20" + Convert.ToString(x); // + ", " + Convert.ToString(y);
                        tableLayoutPanel1.Controls.Add(label, x, y);
                        labelCount++;
                    }
                }
            }



            // Geeft alle spoor labels de correcte text
            foreach (Label l in tableLayoutPanel1.Controls)
            {
                GetLabel(0, 0).Text = "38";
                GetLabel(1, 0).Text = "37";
                GetLabel(2, 0).Text = "36";
                GetLabel(3, 0).Text = "35";
                GetLabel(4, 0).Text = "34";
                GetLabel(5, 0).Text = "33";
                GetLabel(6, 0).Text = "32";
                GetLabel(7, 0).Text = "31";
                GetLabel(8, 0).Text = "30";
                GetLabel(10, 0).Text = "40";
                GetLabel(11, 0).Text = "41";
                GetLabel(12, 0).Text = "42";
                GetLabel(13, 0).Text = "43";
                GetLabel(14, 0).Text = "44";
                GetLabel(16, 0).Text = "45";
                GetLabel(18, 0).Text = "58";

                GetLabel(0, 13).Text = "57";
                GetLabel(1, 13).Text = "56";
                GetLabel(2, 13).Text = "55";
                GetLabel(3, 13).Text = "54";
                GetLabel(4, 13).Text = "53";
                GetLabel(5, 13).Text = "52";
                GetLabel(6, 13).Text = "51";
                GetLabel(7, 13).Text = "64";
                GetLabel(8, 13).Text = "63";
                GetLabel(9, 13).Text = "62";
                GetLabel(10, 13).Text = "61";
                GetLabel(12, 13).Text = "74";
                GetLabel(13, 13).Text = "75";
                GetLabel(14, 13).Text = "76";
                GetLabel(15, 13).Text = "77";

                GetLabel(17, 13).Text = "12";
                GetLabel(17, 14).Text = "13";
                GetLabel(17, 15).Text = "14";
                GetLabel(17, 16).Text = "15";
                GetLabel(17, 17).Text = "16";
                GetLabel(17, 18).Text = "17";
                GetLabel(17, 19).Text = "18";
                GetLabel(17, 20).Text = "19";
                GetLabel(17, 21).Text = "20";
                GetLabel(17, 22).Text = "21";
            }
        }

        private void VulLijnnummers()
        {
            for (int spoor = 0; spoor < Sporen.Length; spoor++)
            {
                for (int lijn = 0; lijn < Lijnen.Length; lijn++)
                {
                    if (Sporen[spoor] != null && Sporen[spoor][0] != null)
                    {
                        for (int lijnSpoor = 1; lijnSpoor < Lijnen[lijn].Length; lijnSpoor++)
                        {
                            if (Convert.ToString(spoor) == Lijnen[lijn][lijnSpoor])
                            {
                                Sporen[spoor][0].Text = Lijnen[lijn][0];
                            }
                        }
                    }
                }
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

                                        cbxRemiseBeheerTramType.DataSource = DataMed.GetAllTramtypes();
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

            sporenArray[12] = new Label[2] { null, GetLabel(18, 13) };
            sporenArray[13] = new Label[2] { null, GetLabel(18, 14) };
            sporenArray[14] = new Label[2] { null, GetLabel(18, 15) };
            sporenArray[15] = new Label[2] { null, GetLabel(18, 16) };
            sporenArray[16] = new Label[2] { null, GetLabel(18, 17) };
            sporenArray[17] = new Label[2] { null, GetLabel(18, 18) };
            sporenArray[18] = new Label[2] { null, GetLabel(18, 19) };
            sporenArray[19] = new Label[2] { null, GetLabel(18, 20) };
            sporenArray[20] = new Label[2] { null, GetLabel(18, 21) };
            sporenArray[21] = new Label[2] { null, GetLabel(18, 22) };
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

        private string[][] LijnenArray()
        {
            string[][] lijnenArray = new string[9][];

            lijnenArray[0] = new string[4] { "1", "36", "43", "51" };
            lijnenArray[1] = new string[5] { "2", "38", "34", "55", "63" };
            lijnenArray[2] = new string[2] { "5", "42" };
            lijnenArray[3] = new string[4] { "5", "37", "56", "54" };
            lijnenArray[4] = new string[4] { "10", "32", "41", "62" };
            lijnenArray[5] = new string[3] { "13", "44", "53" };
            lijnenArray[6] = new string[3] { "17", "52", "45" };
            lijnenArray[7] = new string[5] { "16/24", "30", "35", "33", "57" };
            lijnenArray[8] = new string[2] { "OCV", "61" };

            return lijnenArray;
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            for (int spoor = 0; spoor < Sporen.Length; spoor++)
            {
                if (Sporen[spoor] != null && Sporen[spoor][0] != null)
                {
                    Label l = Sporen[spoor][0];
                    if (e.Row == tableLayoutPanel1.GetRow(l) && e.Column == tableLayoutPanel1.GetColumn(l))
                    {
                        if (l.Text == "1") { g.FillRectangle(Brushes.LightGreen, r); }
                        else if (l.Text == "2") { g.FillRectangle(Brushes.Yellow, r); }
                        else if (l.Text == "5") { g.FillRectangle(Brushes.MediumPurple, r); }
                        else if (l.Text == "10") { g.FillRectangle(Brushes.LightGray, r); }
                        else if (l.Text == "13") { g.FillRectangle(Brushes.Blue, r); }
                        else if (l.Text == "17") { g.FillRectangle(Brushes.Red, r); }
                        else if (l.Text == "16/24") { g.FillRectangle(Brushes.DarkRed, r); }
                        else if (l.Text == "OCV" || l.Text == "") { g.FillRectangle(Brushes.Pink, r); }
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
            if (!string.IsNullOrWhiteSpace(tbxRemiseBeheerTramNummer.Text) && !string.IsNullOrWhiteSpace(tbxRemiseBeheerTramLijn.Text) && !string.IsNullOrWhiteSpace(cbxRemiseBeheerTramType.Text))
            {
                List<Tram> trams = DataMed.GetAllTrams();
                string error = "";

                if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Voeg toe")
                {
                    foreach (Tram t in trams)
                    {
                        if (t.Id == Convert.ToInt32(tbxRemiseBeheerTramNummer.Text))
                        {
                            throw new Exception("Er bestaat al een Tram met dit Tramnummer!");
                        }
                    }

                    TramType type = (TramType)cbxRemiseBeheerTramType.SelectedItem;
                    List<Status> statussen = DataMed.GetAllStatus();
                    Status status = null;

                    foreach (Status s in statussen)
                    {
                        if (s.Naam == "REMISE")
                        {
                            status = s;
                            break;
                        }
                    }

                    Tram tram = new Tram(Convert.ToInt32(tbxRemiseBeheerTramNummer.Text), type, status, tbxRemiseBeheerTramLijn.Text, true);

                    administratie.AddTram(tram);
                    MessageBox.Show("Tram is toegevoegd!");
                }
                else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Verwijder")
                {
                    error = "Deze Tram is niet gevonden!";

                    foreach (Tram t in trams)
                    {
                        if (t.Id == Convert.ToInt32(tbxRemiseBeheerTramNummer.Text))
                        {
                            administratie.TramVerwijderen(Convert.ToInt32(tbxRemiseBeheerTramNummer.Text));
                            error = "";
                            break;
                        }
                    }

                    if (error != "")
                    {
                        MessageBox.Show(error);
                    }
                }
                else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Bewerk")
                {
                    error = "Er is iets fout gegaan tijdens het bewerken van de tram, de bewerkingen zijn niet toegepast.";

                    foreach (Tram t in trams)
                    {
                        if (t.Id == Convert.ToInt32(tbxRemiseBeheerTramNummer.Text))
                        {
                            TramType type = null;

                            foreach (TramType tramtype in administratie.GetTypes())
                            {
                                if (tramtype.ToString() == cbxRemiseBeheerTramType.SelectedItem.ToString())
                                {
                                    type = tramtype;
                                    break;
                                }
                            }

                            administratie.TramBewerken(Convert.ToInt32(tbxRemiseBeheerTramNummer.Text), type.Naam, "REMISE", tbxRemiseBeheerTramLijn.Text, true);
                            error = "";
                            break;
                        }
                    }

                    if (error != "")
                    {
                        MessageBox.Show(error);
                    }
                }

                tbxRemiseBeheerTramNummer.Text = "";
                tbxRemiseBeheerTramLijn.Text = "";
                cbxRemiseBeheerTramType.SelectedItem = null;
            }
            else
            {
                MessageBox.Show("Voer eerst alle velden in.");
            }

            administratie.RefreshClass();
        }

        private void cbxRemiseBeheerTramBewerking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Voeg toe")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                tbxRemiseBeheerTramLijn.Enabled = true;
                cbxRemiseBeheerTramType.Enabled = true;
            }
            else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Verwijder")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                tbxRemiseBeheerTramLijn.Enabled = false;
                cbxRemiseBeheerTramType.Enabled = false;
            }
            else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Bewerk")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                tbxRemiseBeheerTramLijn.Enabled = true;
                cbxRemiseBeheerTramType.Enabled = true;
            }
            btnRemiseBeheerTramBeheerBevestig.Enabled = true;
        }

        private void cbxRemiseBeheerSpoorBeheerBewerking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Blokkeer")
            {
                tbxRemiseBeheerSpoorBeheerSpoorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerSectorNummer.Enabled = false;
                tbxRemiseBeheerSpoorBeheerTramNummer.Enabled = false;
            }
            else if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Reserveer")
            {
                tbxRemiseBeheerSpoorBeheerSpoorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerSectorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerTramNummer.Enabled = true;
            }
            btnRemiseBeheerSpoorBeheerBevestig.Enabled = true;
        }

        private void btnRemiseBeheerSpoorBeheerBevestig_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbxRemiseBeheerSpoorBeheerBewerking.Text) && !string.IsNullOrWhiteSpace(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text) && !string.IsNullOrWhiteSpace(tbxRemiseBeheerSpoorBeheerSectorNummer.Text) && !string.IsNullOrWhiteSpace(tbxRemiseBeheerSpoorBeheerTramNummer.Text))
            {
                List<Spoor> sporen = DataMed.GetAllSporen();
                string error = "";

                if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Blokkeer")
                {
                    error = "Dit spoor is al geblokkeerd!";

                    foreach (Spoor s in sporen)
                    {
                        if (s.Spoornummer == Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text))
                        {
                            administratie.SpoorStatusVeranderen(s.Spoorid, Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text), Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSectorNummer.Text), false);
                            error = "";
                            break;
                        }
                    }

                    if (error != "")
                    {
                        MessageBox.Show(error);
                    }
                }
                else if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Reserveer")
                {
                    error = "Dit spoor is al gereserveerd!";
                    foreach (Spoor s in sporen)
                    {
                        if (s.Spoornummer == Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text))
                        {
                            // Hier klopt niets van... je moet een reservering plaatsen niet het spoor beschikbaar maken of niet!
                            administratie.SpoorStatusVeranderen(s.Spoorid, Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text), Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSectorNummer.Text), false);
                            error = "";
                            break;
                        }
                    }

                    if (error != "")
                    {
                        MessageBox.Show(error);
                    }
                }

                tbxRemiseBeheerSpoorBeheerSpoorNummer.Text = "";
                tbxRemiseBeheerSpoorBeheerSectorNummer.Text = "";
                tbxRemiseBeheerSpoorBeheerTramNummer.Text = "";
            }
            else
            {
                MessageBox.Show("Voer eerst alle velden in.");
            }

            administratie.RefreshClass();
        }

        //AccountBeheer

        private void vullMederwerkerList()
        {
            lbAccountMedewerkers.Items.Clear();
            lbAccountGebruiker.Items.Clear();
            // Haal alle medewerkers op van database
            foreach (Medewerker medewerker in administratie.Medewerkers)
            {
                lbAccountMedewerkers.Items.Add(medewerker);
            }

            foreach (Gebruiker gebruiker in administratie.Gebruikers)
            {
                lbAccountGebruiker.Items.Add(gebruiker);
            }

        }

        private void btnAccountToevoegen_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxAccountNaam.Text) && !string.IsNullOrWhiteSpace(tbxAccountPostcode.Text)
                                                                 && !string.IsNullOrWhiteSpace(tbxAccountStrtNR.Text))
            {
                if (administratie.ValidatePostcode(tbxAccountPostcode.Text))
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
                            medewerker = new Medewerker(0, tbxAccountNaam.Text, tbxAccountEmail.Text, Cbkeuze, tbxAccountStrtNR.Text, tbxAccountPostcode.Text);
                            administratie.AddMedewerker(medewerker);
                        }
                        administratie.RefreshClass();
                        vullMederwerkerList();
                        clearTextboxes();
                    }
                    else
                    {
                        MessageBox.Show("Email is niet geldig bevonden door het systeem.");
                    }
                }
                else
                {
                    MessageBox.Show("Is geen goede postcode voor nl");
                }
            }
            else
            {
                MessageBox.Show("Vul alle gegevens in om een account aan te maken.");
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
            else if (string.IsNullOrWhiteSpace(tbxAccountUsername.Text) && string.IsNullOrWhiteSpace(tbxAccountWachtwoord.Text))
            {
                MessageBox.Show("Vul eerst alle velden in.");
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
                medewerker = lbAccountMedewerkers.SelectedItem as Medewerker;
                MedID = medewerker.ID;
                enableButtons();

                if (administratie.FindGebruiker(medewerker.ID) != null)
                {
                    gebruiker = administratie.FindGebruiker(medewerker.ID);
                    heeftaccount = true;
                }
            }
        }

        private void lbAccountGebruiker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lbAccountGebruiker.SelectedItem != null)
            {
                gebruiker = lbAccountGebruiker.SelectedItem as Gebruiker;
                btnAccountGebrkerverw.Enabled = true;
            }
        }

        private void lbAccountGebruiker_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string grbNaam = gebruiker.GebruikersNaam;
            string gbrWachtwoord = gebruiker.Wachtwoord;
            InputBoxVeranderGebruiker(gebruiker, "Verander gegevens", ref grbNaam, ref gbrWachtwoord);

            if (DialogResult.ToString() == "OK")
            {
                MessageBox.Show("Account is aangepast.");
            }
        }

        public DialogResult InputBoxVeranderGebruiker(Gebruiker gebruiker, string title, ref string valueUS, ref string valuePW)
        {
            Form form = new Form();
            Label label = new Label();
            Label label1 = new Label();
            TextBox textBox = new TextBox();
            TextBox textBox2 = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = "Gebruikersnaam:";
            label1.Text = "Wachtwoord:";
            textBox.Text = valueUS;
            textBox2.Text = valuePW;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            label1.SetBounds(12, 60, 372, 13);
            textBox2.SetBounds(12, 74, 372, 20);
            buttonOk.SetBounds(228, 105, 75, 23);
            buttonCancel.SetBounds(309, 105, 75, 23);

            label.AutoSize = true;
            label1.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            textBox2.Anchor = textBox2.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 140);
            form.Controls.AddRange(new Control[] { label, label1, textBox, textBox2, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            valueUS = textBox.Text;
            valuePW = textBox2.Text;

            try
            {
                Gebruiker UpdateGebruiker = new Gebruiker(valueUS, gebruiker.Medewerker_ID, valuePW);
                administratie.ChangeGebruiker(UpdateGebruiker);
                administratie.RefreshClass();
                vullMederwerkerList();
            }

            catch (OracleException e)
            {
                MessageBox.Show(e.Message);
            }
            return dialogResult;
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

        private void BttnAccountRemoveMedewerker_Click(object sender, EventArgs e)
        {
            if (administratie.FindGebruiker(medewerker.ID) == null)
            {
                administratie.RemoveMedewerker(medewerker);
                administratie.RefreshClass();
                vullMederwerkerList();
            }
            else
            {
                DialogResult result = MessageBox.Show("Gebruiker heeft een account, wilt u door gaan met het verwijderen?", "Question", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (heeftaccount == true)
                    {
                        administratie.RemoveGebruiker(gebruiker);
                        administratie.RemoveMedewerker(medewerker);
                    }
                    else
                    {
                        administratie.RemoveMedewerker(medewerker);
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
            foreach (Medewerker m in administratie.Medewerkers)
            {
                cbxOnderhoudMedewerker.Items.Add(m);
            }
        }



        private void btnRemiseBeheerNieuwTypeVoegToe_Click(object sender, EventArgs e)
        {
            int lengte;
            if (tbxRemiseBeheerNieuwTypeNaam.Text != "" && tbxRemiseBeheerNieuwTypeBeschrijving.Text != "" && Int32.TryParse(tbxRemiseBeheerNieuwTypeLengte.Text, out lengte))
            {
                List<TramType> types = DataMed.GetAllTramtypes();
                string error = "";
                foreach (TramType t in types)
                {
                    if (t.Naam == tbxRemiseBeheerNieuwTypeNaam.Text)
                    {
                        error = "Er bestaat al een tramtype met deze naam!";
                    }
                }

                if (error == "")
                {
                    TramType type = new TramType(tbxRemiseBeheerNieuwTypeNaam.Text, tbxRemiseBeheerNieuwTypeBeschrijving.Text, lengte);
                    administratie.AddTramType(type);
                }
            }
            else
            {
                MessageBox.Show("Voer eerst alle velden correct in!");
            }
        }

        public void loadListComboboxStatusbeheerOnderhoudMedewerker()
        {
            administratie.RefreshClass();
            if (administratie.Medewerkers.Count != 0)
            {
                cbxOnderhoudMedewerker.Items.Clear();
                foreach (Medewerker m in administratie.GetAllMedewerkers())
                {
                    cbxOnderhoudMedewerker.Items.Add(m);
                }
            }
            lB_Onderhoudslijst_historie.Items.Clear();
            foreach (Onderhoud onderhoud in administratie.Onderhoudslijst)
            {
                lB_Onderhoudslijst_historie.Items.Add(onderhoud);
            }
            lB_Onderhoud_Trams.Items.Clear();
            foreach (Tram tram in administratie.Trams)
            {
                if (tram.Status.Naam == "SCHOONMAAK" || tram.Status.Naam == "DEFECT")
                {
                    lB_Onderhoud_Trams.Items.Add(tram);
                }
            }
        }
        public void SorteerTram(Tram tram)
        {
            for (int lijn = 0; lijn < Lijnen.Length; lijn++)
            {
                if (tram.Lijn == Lijnen[lijn][0])
                {
                    for (int spoor = 1; spoor < Lijnen[lijn].Length; spoor++)
                    {
                        int spoornummer = -1;
                        if (lijn == 3 && tram.Id >= 901 && tram.Id <= 920)
                        {
                            spoornummer = Convert.ToInt32(Lijnen[3][spoor]);
                        }
                        else if (lijn == 2 && tram.Id >= 2201 && tram.Id <= 2204)
                        {
                            spoornummer = Convert.ToInt32(Lijnen[2][spoor]);
                        }
                        else if (tram.Lijn != "5")
                        {
                            spoornummer = Convert.ToInt32(Lijnen[lijn][spoor]);
                        }

                        if (spoornummer != -1)
                        {
                            for (int sector = 1; sector < Sporen[spoornummer].Length; sector++)
                            {
                                Label l = null;
                                l = Sporen[spoornummer][sector];

                                if (l.Text == "!")
                                {
                                    l.Text = Convert.ToString(tram.Id);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Check_Click_1(object sender, EventArgs e)
        {
            try
            {
                int tramnummer = Convert.ToInt32(tbxStatusbeheerTramNummer.Text);
                tbxStatusbeheerHuidigeStatus.Text = null;
                foreach (Tram t in administratie.Trams)
                {
                    if (tramnummer == t.Id)
                    {
                        tbxStatusbeheerHuidigeStatus.Text = t.Status.Naam;
                        cbxStatusbeheerTramStatus.Enabled = true;
                        btnStatusbeheerTramStatus.Enabled = true;
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Vul de gegevens correct in");
            }
        }

        private void btnStatusbeheerTramStatus_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cbxStatusbeheerTramStatus.Text))
            {
                try
                {
                    administratie.TramStatusVeranderen(Convert.ToInt32(tbxStatusbeheerTramNummer.Text), cbxStatusbeheerTramStatus.Text);
                    MessageBox.Show("De status van tram " + tbxStatusbeheerTramNummer.Text + " is veranderd in '" + cbxStatusbeheerTramStatus.Text + "'");
                }
                catch (Exception en)
                {
                    MessageBox.Show(en.ToString());
                    MessageBox.Show("Er is iets fout gegaan: Misschien een fout nummer ingevuld?");
                }
                finally
                {
                    btnStatusbeheerTramStatus.Enabled = false;
                    tbxStatusbeheerTramNummer.Text = null;
                    tbxStatusbeheerHuidigeStatus.Text = null;
                    cbxStatusbeheerTramStatus.Text = null;
                    cbxStatusbeheerTramStatus.Enabled = false;
                }
            }
        }

        private void btnOnderhoudBevestig_Click_1(object sender, EventArgs e)
        {
            DateTime starttijd = Convert.ToDateTime(dtpOnderhoudStarttijd.Value);
            DateTime eindtijd = Convert.ToDateTime(dtpOnderhoudEindtijd.Value);
            try
            {
                string opmerking = tbxStatusbeheerOnderhoudOpmerking.Text;

                Medewerker m = cbxOnderhoudMedewerker.SelectedItem as Medewerker;
                int tramnummer = Convert.ToInt32(tbxStatusbeheerOnderhoudTramnr.Text);
                //geen schoonmaak voor tram 2x toevoegen, net als reparatie;



                foreach (Tram t in administratie.Trams)
                {
                    if (t.Id == tramnummer)
                    {
                        if (t.Status.Naam == "SCHOONMAAK")
                        {
                            administratie.AddOnderhoudsbeurt(new Onderhoud(0,m,t,starttijd,eindtijd,opmerking,"SCHOONMAAK"));                        
                        }
                        if (t.Status.Naam == "DEFECT")
                        {
                            administratie.AddOnderhoudsbeurt(new Onderhoud(0, m, t, starttijd, eindtijd, opmerking, "DEFECT"));
                        }
                        administratie.TramStatusVeranderen(tramnummer, "REMISE");
                        loadListComboboxStatusbeheerOnderhoudMedewerker();
                        MessageBox.Show("De onderhoud is toegevoegd");
                    }
                }


                /*
                if (soort == "SCHOONMAAK")
                {
                    foreach (Tram t in administratie.Trams)
                    {
                        if (t.Id == tramnummer)
                        {
                            if (t.Status.Naam != soort)
                            {
                                administratie.AddOnderhoudsbeurt(m, tramnummer, opmerking, soort, starttijd, eindtijd);
                                administratie.TramStatusVeranderen(tramnummer, soort);
                                MessageBox.Show("Tram " + tramnummer.ToString() + " heeft nu de status " + soort);
                                loadListComboboxStatusbeheerOnderhoudMedewerker();
                            }
                        }
                    }
                }
                else
                {
                    foreach (Tram t in administratie.Trams)
                    {
                        if (t.Id == tramnummer)
                        {
                            if (t.Status.Naam != soort.ToUpper())
                            {
                                administratie.AddOnderhoudsbeurt(m, tramnummer, opmerking, soort, starttijd, eindtijd);
                                administratie.TramStatusVeranderen(tramnummer, soort);
                                MessageBox.Show("Tram " + tramnummer.ToString() + " heeft nu de status " + soort);
                            }
                        }
                    }
                }
                administratie.TramStatusVeranderen(tramnummer, "REMISE");*/
            }
            catch (FormatException fe)
            {
                MessageBox.Show(starttijd.ToString());
                MessageBox.Show(fe.Message);
            }
        }

        private void btn_RefreshLists_Click(object sender, EventArgs e)
        {
            administratie.RefreshClass();
            loadListComboboxStatusbeheerOnderhoudMedewerker();
        }
    }
}
