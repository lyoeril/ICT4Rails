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
        private Administratie administratie;
        private InputBox inputbox;
        private int MedID;
        private Medewerker medewerker;
        private Gebruiker gebruiker;
        private bool heeftaccount = false;
        private RFID rfid;
        private Tram sleepTram;

        public MainForm(Administratie administratie)
        {
            InitializeComponent();
            this.administratie = administratie;
            LogIn();
            VulSporen();
            Sporen = SporenArray();
            Lijnen = LijnenArray();
            VulLijnnummers();

            OpenAccountUI();

            // lijsten en comboboxes
            loadComboboxes();
            VerversLijsten();


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

            //if (e.Tag == "2200c77481") // Gele cirkel
            //{
            //    foreach (Spoor s in administratie.Sporen)
            //    {
            //        if (s.Spoornummer == 52 && s.Sectornummer == 4)
            //        {
            //            if (s.Beschikbaar)
            //            {
            //                s.Beschikbaar = false;
            //            }
            //            else
            //            {
            //                s.Beschikbaar = true;
            //            }
            //            tableLayoutPanel1.Refresh();
            //        }
            //    }
            //}
            int id = 0;
            if (e.Tag == "2300fb7939") { id = 2201; } // TAG
            else if (e.Tag == "2800a79650") { id = 2202; } // TAG
            //else if (e.Tag == "") { id = 0; } // TAG
            //else if (e.Tag == "") { id = 0; } // TAG
            foreach (Tram t in administratie.Trams)
            {
                if (t.Id == id)
                {
                    SorteerTram(t);
                }
            }
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
                        tabpageOnderhoud.Text = "Reparatie";
                    }
                    else if (medewerker.Functie == "SCHOONMAKER")
                    {
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Remove(tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Remove(tabpageAccountBeheer);
                        tabcontrolRemise.TabPages.Insert(0, tabpageOnderhoud);
                        tabpageOnderhoud.Text = "Schoonmaak";
                    }
                    else if (medewerker.Functie == "WAGENPARKBEHEERDER")
                    {
                        tabcontrolRemise.TabPages.Insert(0, tabpageRemiseOverzicht);
                        tabcontrolRemise.TabPages.Insert(1, tabpageRemiseBeheer);
                        tabcontrolRemise.TabPages.Insert(2, tabpageStatusBeheer);
                        tabcontrolRemise.TabPages.Insert(3, tabpageAccountBeheer);
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
                        //label.Click += new EventHandler(label_Click);
                        label.MouseDown += new MouseEventHandler(label_MouseDown);
                        label.MouseUp += new MouseEventHandler(label_MouseUp);
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

        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            Label startLabel = (Label)sender;
            foreach (Tram t in administratie.Trams)
            {
                if (Convert.ToString(t.Id) == startLabel.Text)
                {
                    sleepTram = t;
                }
            }
            //if (labelText == "" || labelText == null)
            //{
            //    Label startLabel = (Label)sender;
            //    labelText = startLabel.Text;
            //}
        }

        private void label_MouseUp(object sender, MouseEventArgs e)
        {
            Point tlpMousePos = tableLayoutPanel1.PointToClient(MousePosition);
            Point? cr = administratie.GetRowColIndex(tableLayoutPanel1, tlpMousePos);
            Trampositie oldPos = null;
            bool beschikbaar = true;
            if (cr != null)
            {
                Label endLabel = GetLabel(cr.Value.X, cr.Value.Y);
                for (int spoor = 0; spoor < Sporen.Length; spoor++)
                {
                    if (Sporen[spoor] != null)
                    {
                        for (int sector = 0; sector < Sporen[spoor].Length; sector++)
                        {
                            if (Sporen[spoor][sector] != null)
                            {
                                if (Sporen[spoor][sector] == endLabel)
                                {
                                    foreach (Spoor s in administratie.Sporen)
                                    {
                                        if (s.Spoorid == spoor)
                                        {
                                            if (s.Sectornummer == sector)
                                            {
                                                foreach (Trampositie t in administratie.Posities)
                                                {
                                                    if (t.Vertrektijd == null)
                                                    {
                                                        if (t.Spoor == s)
                                                        {
                                                            beschikbaar = false;
                                                            break;
                                                        }
                                                        else if (t.Tram == sleepTram)
                                                        {
                                                            oldPos = t;
                                                        }
                                                    }
                                                }
                                                if (beschikbaar)
                                                {
                                                    administratie.UpdateSpoor(spoor, sector, false);
                                                    administratie.UpdateTramPositie(oldPos.Id, oldPos.Spoor, oldPos.Tram, oldPos.Aankomstijd, DateTime.Now);
                                                    administratie.AddTramPositie(administratie.Posities.Count + 1, s, sleepTram, DateTime.Now);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Sector is al bezet");
                                                }
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //if (labelText != "" && labelText != " " && cr != null)
            //{
            //    Label endLabel = GetLabel(cr.Value.X, cr.Value.Y);
            //    for (int spoor = 0; spoor < Sporen.Length; spoor++)
            //    {
            //        if (Sporen[spoor] != null)
            //        {
            //            for (int sector = 1; sector < Sporen[spoor].Length; sector++)
            //            {
            //                if (Sporen[spoor][sector] != null)
            //                {
            //                    if (endLabel == Sporen[spoor][sector])
            //                    {
            //                        Trampositie tp = null;
            //                        foreach (Trampositie t in administratie.Posities)
            //                        {
            //                            if (t.Vertrektijd == null)
            //                            {
            //                                if (t.Tram.Id == Convert.ToInt32(labelText))
            //                                {
            //                                    tp = t;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                        Spoor positieSpoor = null;
            //                        foreach (Spoor s in administratie.Sporen)
            //                        {
            //                            if (s.Spoornummer == spoor)
            //                            {
            //                                if (s.Sectornummer == sector)
            //                                {
            //                                    positieSpoor = s;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                        administratie.UpdateTramPositie(tp.Id, tp.Spoor, tp.Tram, tp.Aankomstijd, DateTime.Now);
            //                        administratie.AddTramPositie(administratie.Posities.Count + 1, positieSpoor, tp.Tram, DateTime.Now);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void VulLijnnummers()
        {
            for (int spoor = 0; spoor < Sporen.Length; spoor++)
            {
                for (int lijn = 0; lijn < Lijnen.Length - 1; lijn++)
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
        //private void label_Click(object sender, EventArgs e)
        //{
        //    Label label = (Label)sender;
        //    if (label.Text != "")
        //    {
        //        foreach (Tram t in administratie.Trams)
        //        {
        //            if (Convert.ToString(t.Id) == label.Text)
        //            {
        //                for (int spoor = 0; spoor < Sporen.Length; spoor++)
        //                {
        //                    if (Sporen[spoor] != null)
        //                    {
        //                        for (int sector = 1; sector < Sporen[spoor].Length; sector++)
        //                        {
        //                            if (Sporen[spoor][sector] == label)
        //                            {
        //                                this.tabcontrolRemise.SelectedTab = tabpageRemiseBeheer;
        //                                tbxRemiseBeheerTramNummer.Text = Convert.ToString(t.Id);
        //                                cbxRemisebeheerTrambeheerLijn.Text = Convert.ToString(t.Lijn);
        //                                //tbxRemiseBeheerTramType.SelectedItem = 
        //                                //moet nog een methode toegevoegd worden om alle bestaande types uit de db te halen
        //                                tbxRemiseBeheerSpoorBeheerSpoorNummer.Text = Convert.ToString(spoor);
        //                                tbxRemiseBeheerSpoorBeheerSectorNummer.Text = Convert.ToString(sector);
        //                                tbxRemiseBeheerSpoorBeheerTramNummer.Text = Convert.ToString(t.Id);
        //                                tbxStatusbeheerTramNummer.Text = Convert.ToString(t.Id);

        //                                cbxRemiseBeheerTramType.DataSource = administratie.GetTypes();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

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
            string[][] lijnenArray = new string[10][];

            lijnenArray[0] = new string[4] { "1", "36", "43", "51" };
            lijnenArray[1] = new string[5] { "2", "38", "34", "55", "63" };
            lijnenArray[2] = new string[2] { "5", "42" };
            lijnenArray[3] = new string[4] { "5", "37", "56", "54" };
            lijnenArray[4] = new string[4] { "10", "32", "41", "62" };
            lijnenArray[5] = new string[3] { "13", "44", "53" };
            lijnenArray[6] = new string[3] { "17", "52", "45" };
            lijnenArray[7] = new string[5] { "16/24", "30", "35", "33", "57" };
            lijnenArray[8] = new string[2] { "OCV", "61" };
            lijnenArray[9] = new string[17] { "RES", "64", "62", "74", "75", "76", "77", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21" };

            return lijnenArray;
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            for (int spoor = 0; spoor < Sporen.Length; spoor++)
            {
                if (Sporen[spoor] != null)
                {
                    for (int sector = 0; sector < Sporen[spoor].Length; sector++)
                    {
                        if (Sporen[spoor][sector] != null)
                        {
                            Label l = Sporen[spoor][sector];
                            if (e.Row == tableLayoutPanel1.GetRow(l) && e.Column == tableLayoutPanel1.GetColumn(l))
                            {
                                if (sector == 0)
                                {
                                    if (l.Text == "1") { g.FillRectangle(Brushes.LightGreen, r); }
                                    else if (l.Text == "2") { g.FillRectangle(Brushes.Yellow, r); }
                                    else if (l.Text == "5") { g.FillRectangle(Brushes.MediumPurple, r); }
                                    else if (l.Text == "10") { g.FillRectangle(Brushes.Gray, r); }
                                    else if (l.Text == "13") { g.FillRectangle(Brushes.Blue, r); }
                                    else if (l.Text == "17") { g.FillRectangle(Brushes.Red, r); }
                                    else if (l.Text == "16/24") { g.FillRectangle(Brushes.DarkRed, r); }
                                    else if (l.Text == "OCV") { g.FillRectangle(Brushes.Pink, r); }
                                    else if (l.Text == "") { g.FillRectangle(Brushes.Lavender, r); }
                                }
                                else
                                {
                                    g.FillRectangle(Brushes.LightGray, r);

                                    foreach (Spoor s in administratie.Sporen)
                                    {
                                        if (s.Spoorid == spoor)
                                        {
                                            if (s.Sectornummer == sector)
                                            {
                                                if (s.Beschikbaar == false)
                                                {
                                                    g.DrawLine(Pens.Black, new Point(e.CellBounds.X, e.CellBounds.Y), new Point(e.CellBounds.X + e.CellBounds.Width, e.CellBounds.Y + e.CellBounds.Height));
                                                    g.DrawLine(Pens.Black, new Point(e.CellBounds.X, e.CellBounds.Y + e.CellBounds.Height), new Point(e.CellBounds.X + e.CellBounds.Width, e.CellBounds.Y));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //foreach (Spoor s in administratie.Sporen)
                                    //{
                                    //    if (s.Spoornummer == spoor && s.Sectornummer == sector && s.Beschikbaar == false)
                                    //    {
                                    //        l.Text = " ";
                                    //        g.DrawLine(Pens.Black, new Point(e.CellBounds.X, e.CellBounds.Y), new Point(e.CellBounds.X + e.CellBounds.Width, e.CellBounds.Y + e.CellBounds.Height));
                                    //        g.DrawLine(Pens.Black, new Point(e.CellBounds.X, e.CellBounds.Y + e.CellBounds.Height), new Point(e.CellBounds.X + e.CellBounds.Width, e.CellBounds.Y));
                                    //        break;
                                    //    }
                                    //    else if (s.Spoornummer == spoor && s.Sectornummer == sector && s.Beschikbaar == true && l.Text == " ")
                                    //    {
                                    //        l.Text = "";
                                    //        break;
                                    //    }
                                    //}
                                }
                            }
                        }
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
            if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Voeg toe")
            {
                try
                {
                    foreach (Tram t in administratie.Trams)
                    {
                        if (t.Id == Convert.ToInt32(tbxRemiseBeheerTramNummer.Text))
                        {
                            MessageBox.Show("De tram met dit nummer bestaat al");
                            return;
                        }
                    }
                    TramType type = cbxRemiseBeheerTramType.SelectedItem as TramType;
                    Status status = null;

                    foreach (Status s in administratie.Statuslijst)
                    {
                        if (s.Naam == "REMISE")
                        {
                            status = s;
                            break;
                        }
                    }
                    administratie.AddTram(new Tram(Convert.ToInt32(tbxRemiseBeheerTramNummer.Text), type, status, cbxRemisebeheerTrambeheerLijn.SelectedItem.ToString()));
                    MessageBox.Show("Tram is toegevoegd!");
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets fout gegaan, tram is niet toegevoegd");
                }

            }
            else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Verwijder")
            {
                try
                {
                    foreach (Tram t in administratie.Trams)
                    {
                        if (t.Id == Convert.ToInt32(tbxRemiseBeheerTramNummer.Text))
                        {
                            administratie.TramVerwijderen(Convert.ToInt32(tbxRemiseBeheerTramNummer.Text));
                            MessageBox.Show("De tram is succesvol verwijderd");
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Het ingevulde tramnummer is onjuist");
                }

            }
            else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Bewerk")
            {
                TramType type = cbxRemiseBeheerTramType.SelectedItem as TramType;
                try
                {
                    foreach (Tram t in administratie.Trams)
                    {
                        if (t.Id == Convert.ToInt32(tbxRemiseBeheerTramNummer.Text))
                        {
                            administratie.TramBewerken(Convert.ToInt32(tbxRemiseBeheerTramNummer.Text), type.Naam, "REMISE", cbxRemisebeheerTrambeheerLijn.SelectedItem.ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets fout gegaan tijdens het bewerken van de tram, de bewerkingen zijn niet toegepast.");
                }
            }
            tbxRemiseBeheerTramNummer.Text = "";
            cbxRemisebeheerTrambeheerLijn.Text = "";
            cbxRemiseBeheerTramType.SelectedItem = null;


            VerversLijsten();
        }

        private void cbxRemiseBeheerTramBewerking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Voeg toe")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                cbxRemisebeheerTrambeheerLijn.Enabled = true;
                cbxRemiseBeheerTramType.Enabled = true;
            }
            else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Verwijder")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                cbxRemisebeheerTrambeheerLijn.Enabled = false;
                cbxRemiseBeheerTramType.Enabled = false;
            }
            else if (cbxRemiseBeheerTramBewerking.SelectedItem.ToString() == "Bewerk")
            {
                tbxRemiseBeheerTramNummer.Enabled = true;
                cbxRemisebeheerTrambeheerLijn.Enabled = true;
                cbxRemiseBeheerTramType.Enabled = true;
            }
            btnRemiseBeheerTramBeheerBevestig.Enabled = true;
        }

        private void cbxRemiseBeheerSpoorBeheerBewerking_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Blokkeer")
            {
                tbxRemiseBeheerSpoorBeheerSpoorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerSectorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerTramNummer.Enabled = false;
                dtp_datum_reservering.Enabled = false;
            }
            else if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Reserveer")
            {
                tbxRemiseBeheerSpoorBeheerSpoorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerSectorNummer.Enabled = true;
                tbxRemiseBeheerSpoorBeheerTramNummer.Enabled = true;
                dtp_datum_reservering.Enabled = true;
            }
            btnRemiseBeheerSpoorBeheerBevestig.Enabled = true;
        }

        private void btnRemiseBeheerSpoorBeheerBevestig_Click(object sender, EventArgs e)
        {
            if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Blokkeer")
            {
                foreach (Spoor s in administratie.Sporen)
                {
                    if (s.Spoornummer == Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text))
                    {
                        administratie.SpoorStatusVeranderen(s.Spoorid, Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text), Convert.ToInt32(tbxRemiseBeheerSpoorBeheerSectorNummer.Text), false);
                        break;
                    }
                }

            }
            else if (cbxRemiseBeheerSpoorBeheerBewerking.SelectedItem.ToString() == "Reserveer")
            {
                try
                {
                    int spoornummer;
                    if (!int.TryParse(tbxRemiseBeheerSpoorBeheerSpoorNummer.Text, out spoornummer))
                    {
                        MessageBox.Show("Het spoornummer is niet correct ingevuld!");
                        return;
                    }
                    int spoorsector;
                    if (!int.TryParse(tbxRemiseBeheerSpoorBeheerSectorNummer.Text, out spoorsector))
                    {
                        MessageBox.Show("De spoorsector in niet correct ingevuld!");
                        return;
                    }
                    int tramnummer;
                    if (!int.TryParse(tbxRemiseBeheerSpoorBeheerTramNummer.Text, out tramnummer))
                    {
                        MessageBox.Show("Het tramnummer is niet correct ingevuld!");
                        return;
                    }
                    DateTime date = new DateTime(dtp_datum_reservering.Value.Year, dtp_datum_reservering.Value.Month, dtp_datum_reservering.Value.Day);
                    administratie.SpoorReserveren(spoornummer, spoorsector, tramnummer, date);
                    MessageBox.Show("De reservering is succesvol toegevoegd.");
                }
                catch (Exception en)
                {
                    MessageBox.Show(en.Message);
                }

            }

            tbxRemiseBeheerSpoorBeheerSpoorNummer.Text = "";
            tbxRemiseBeheerSpoorBeheerSectorNummer.Text = "";
            tbxRemiseBeheerSpoorBeheerTramNummer.Text = "";


            VerversLijsten();

        }
        //AccountBeheer

        private void VerversLijsten()
        {
            lbAccountMedewerkers.Items.Clear();
            lbAccountGebruiker.Items.Clear();
            lB_RemisebeheerTramlijst.Items.Clear();
            lB_RemisebeheerSpoorlijst.Items.Clear();
            // Haal alle gegevens op van database
            foreach (Medewerker medewerker in administratie.Medewerkers)
            {
                lbAccountMedewerkers.Items.Add(medewerker);
            }

            foreach (Gebruiker gebruiker in administratie.Gebruikers)
            {
                lbAccountGebruiker.Items.Add(gebruiker);
            }

            foreach (Tram tram in administratie.Trams)
            {
                lB_RemisebeheerTramlijst.Items.Add(tram);
            }

            foreach (Spoor spoor in administratie.Sporen)
            {
                lB_RemisebeheerSpoorlijst.Items.Add(spoor);
            }

            // gemaakt door Yoeri

            foreach (Onderhoud onderhoud in administratie.Onderhoudslijst)
            {
                if (onderhoud.Medewerker != null)
                {
                    lB_Onderhoudslijst_historie.Items.Add(onderhoud);
                }
                else
                {
                    lB_Onderhoudslijst_TODO.Items.Add(onderhoud);
                }
            }


            lB_Onderhoud_Trams.Items.Clear();
            foreach (Tram tram in administratie.Trams)
            {
                if (tram.Status.Naam == "SCHOONMAAK" || tram.Status.Naam == "DEFECT")
                {
                    lB_Onderhoud_Trams.Items.Add(tram);
                }
            }

            lB_Onderhoudslijst_historie.Items.Clear();
            lB_Onderhoudslijst_TODO.Items.Clear();
            cbxOnderhoudMedewerker.Items.Clear();
            if (tabpageOnderhoud.Text == "Onderhoud")
            {
                foreach (Onderhoud onderhoud in administratie.Onderhoudslijst)
                {
                    if (onderhoud.Medewerker != null)
                    {
                        lB_Onderhoudslijst_historie.Items.Add(onderhoud);
                    }
                    else
                    {
                        lB_Onderhoudslijst_TODO.Items.Add(onderhoud);
                    }
                }
                foreach (Medewerker m in administratie.Medewerkers)
                {
                    cbxOnderhoudMedewerker.Items.Add(m.Naam);
                }
            }

            if (tabpageOnderhoud.Text == "Schoonmaak")
            {
                foreach (Onderhoud onderhoud in administratie.Onderhoudslijst)
                {
                    if (onderhoud.Soort == "SCHOONMAAK")
                    {
                        if (onderhoud.Medewerker != null)
                        {
                            lB_Onderhoudslijst_historie.Items.Add(onderhoud);
                        }
                        else
                        {
                            lB_Onderhoudslijst_TODO.Items.Add(onderhoud);
                        }
                    }
                }
                foreach (Medewerker m in administratie.Medewerkers)
                {
                    if (m.Functie == "SCHOONMAKER")
                    {
                        cbxOnderhoudMedewerker.Items.Add(m.Naam);
                    }
                }
            }

            if (tabpageOnderhoud.Text == "Reparatie")
            {
                foreach (Onderhoud onderhoud in administratie.Onderhoudslijst)
                {
                    if (onderhoud.Soort == "DEFECT")
                    {
                        if (onderhoud.Medewerker != null)
                        {
                            lB_Onderhoudslijst_historie.Items.Add(onderhoud);
                        }
                        else
                        {
                            lB_Onderhoudslijst_TODO.Items.Add(onderhoud);
                        }
                    }
                }
                foreach (Medewerker m in administratie.Medewerkers)
                {
                    if (m.Functie == "REPARATEUR")
                    {
                        cbxOnderhoudMedewerker.Items.Add(m.Naam);
                    }
                }
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

                        VerversLijsten();
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


                VerversLijsten();
                clearTextboxes();
            }
        }

        private void lbAccountMedewerkers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbAccountMedewerkers.SelectedItem != null)
            {
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
            if (lbAccountGebruiker.SelectedItem != null)
            {
                inputbox = new InputBox();
                inputbox.InputBoxVeranderGebruiker(administratie, (Gebruiker)lbAccountGebruiker.SelectedItem, "Verander gegevens");
            }
            VerversLijsten();
        }

        private void lbAccountMedewerkers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbAccountMedewerkers.SelectedItem != null)
            {
                inputbox = new InputBox();
                inputbox.InputBoxVeranderGebruiker(administratie, (Medewerker)lbAccountMedewerkers.SelectedItem, "Verander gegevens");
            }
            VerversLijsten();

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
                VerversLijsten();
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

                    VerversLijsten();
                    heeftaccount = false;
                }
            }
        }

        private void btnAccountGebrkerverw_Click(object sender, EventArgs e)
        {
            if (administratie.FindGebruiker(gebruiker.Medewerker_ID) != null)
            {
                administratie.RemoveGebruiker(gebruiker);
                VerversLijsten();
            }
        }

        private void loadComboboxes()
        {


            cbxRemiseBeheerTramType.Items.Clear();
            foreach (TramType t in administratie.Tramtypes)
            {
                cbxRemiseBeheerTramType.Items.Add(t);
            }
        }



        private void btnRemiseBeheerNieuwTypeVoegToe_Click(object sender, EventArgs e)
        {
            int lengte;
            if (tbxRemiseBeheerNieuwTypeNaam.Text != "" && tbxRemiseBeheerNieuwTypeBeschrijving.Text != "" && Int32.TryParse(tbxRemiseBeheerNieuwTypeLengte.Text, out lengte))
            {
                List<TramType> types = administratie.Tramtypes;
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



        public void SorteerTram(Tram tram)
        {
            foreach (Trampositie t in administratie.Posities)
            {
                if (t.Vertrektijd == null)
                {
                    if (t.Tram == tram)
                    {
                        foreach (Status s in administratie.Statuslijst)
                        {
                            if (s.Naam == "DIENST")
                            {
                                tram.Status = s;
                                break;
                            }
                        }
                        administratie.UpdateTram(tram);

                        foreach (Spoor s in administratie.Sporen)
                        {
                            if (s == t.Spoor)
                            {
                                administratie.UpdateSpoor(s.Spoorid, s.Sectornummer, true);
                                break;
                            }
                        }

                        administratie.UpdateTramPositie(t.Id, t.Spoor, t.Tram, t.Aankomstijd, DateTime.Now);
                        return;
                    }
                }
            }

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
                                foreach (Spoor sp in administratie.Sporen)
                                {
                                    if (sp.Spoorid == spoornummer)
                                    {
                                        if (sp.Sectornummer == sector)
                                        {
                                            if (sp.Beschikbaar)
                                            {
                                                if (tram.Status.Naam == "DIENST")
                                                {
                                                    foreach (Status st in administratie.Statuslijst)
                                                    {
                                                        if (st.Naam == "REMISE")
                                                        {
                                                            tram.Status = st;
                                                            break;
                                                        }
                                                    }
                                                }
                                                administratie.UpdateTram(tram);
                                                administratie.UpdateSpoor(sp.Spoorid, sp.Sectornummer, false);
                                                administratie.AddTramPositie(administratie.Posities.Count + 1, sp, tram, DateTime.Now);
                                                return;
                                            }
                                        }
                                    }
                                }
                                if (lijn != 9)
                                {
                                    if (Convert.ToString(spoornummer) == Lijnen[lijn][Lijnen[lijn].Length - 1])
                                    {
                                        if (sector == Sporen[spoornummer].Length - 1)
                                        {
                                            lijn = 9;
                                            spoor = 1;
                                            sector = 0;
                                            spoornummer = Convert.ToInt32(Lijnen[lijn][spoor]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckTrampositieDB()
        {
            for (int spoor = 0; spoor < Sporen.Length; spoor++)
            {
                if (Sporen[spoor] != null)
                {
                    for (int sector = 1; sector < Sporen[spoor].Length; sector++)
                    {
                        if (Sporen[spoor][sector] != null)
                        {
                            Sporen[spoor][sector].Text = "";
                        }
                    }
                }
            }

            foreach (Trampositie tp in administratie.Posities)
            {
                if (tp.Vertrektijd == null)
                {
                    Sporen[tp.Spoor.Spoorid][tp.Spoor.Sectornummer].Text = Convert.ToString(tp.Tram.Id);
                }
            }
        }

        private void Check_Click_1(object sender, EventArgs e)
        {
            bool gevonden = false;
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
                        gevonden = true;
                    }
                }

                if (!gevonden)
                {
                    MessageBox.Show("De tram is niet gevonden");
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
                    VerversLijsten();
                    btnStatusbeheerTramStatus.Enabled = false;
                    tbxStatusbeheerTramNummer.Text = null;
                    tbxStatusbeheerHuidigeStatus.Text = null;
                    cbxStatusbeheerTramStatus.Text = null;
                    cbxStatusbeheerTramStatus.Enabled = false;
                }
                catch (Exception en)
                {
                    MessageBox.Show(en.ToString());
                    MessageBox.Show("Er is iets fout gegaan: Misschien een fout nummer ingevuld?");
                }
            }
        }

        private void btnOnderhoudBevestig_Click_1(object sender, EventArgs e)
        {
            try
            {
                int onderhoudsid = Convert.ToInt32(nUd_Onderhoud_Onderhoudsid.Value);
                DateTime starttijd = Convert.ToDateTime(dtpOnderhoudStarttijd.Value);
                DateTime eindtijd = Convert.ToDateTime(dtpOnderhoudEindtijd.Value);
                string medewerker = cbxOnderhoudMedewerker.SelectedItem.ToString();
                Medewerker m = null;
                //geen schoonmaak voor tram 2x toevoegen, net als reparatie;
                foreach (Medewerker mede in administratie.Medewerkers)
                {
                    if (medewerker == mede.Naam)
                    {
                        m = mede;
                        break;
                    }
                }
                foreach (Onderhoud o in administratie.Onderhoudslijst)
                {
                    if (o.ID == onderhoudsid)
                    {
                        if (o.Soort == "SCHOONMAAK")
                        {
                            administratie.UpdateOnderhoudsbeurt(new Onderhoud(onderhoudsid, m, o.Tram, starttijd, eindtijd, "", "SCHOONMAAK"));
                        }
                        if (o.Soort == "DEFECT")
                        {
                            administratie.UpdateOnderhoudsbeurt(new Onderhoud(onderhoudsid, m, o.Tram, starttijd, eindtijd, "", "DEFECT"));
                        }
                        administratie.TramStatusVeranderen(o.Tram.Id, "REMISE");
                        VerversLijsten();
                        MessageBox.Show("De onderhoud is toegevoegd");
                    }
                }
            }
            catch (Exception fe)
            {
                MessageBox.Show(fe.Message);
            }
        }

        private void btn_RefreshLists_Click(object sender, EventArgs e)
        {
            VerversLijsten();
        }


        private void btn_RefreshList2_Click(object sender, EventArgs e)
        {
            VerversLijsten();
        }

        private void btnOnderhoudBevestiging_Click(object sender, EventArgs e)
        {
            try
            {
                string opmerking = tB_Statusbeheer_Opmerking.Text;
                foreach (Tram t in administratie.Trams)
                {
                    if (t.Id == Convert.ToInt32(nUd_StatusbeheerTramnummer.Value))
                    {
                        if (opmerking == "" || opmerking == null)
                        {
                            MessageBox.Show("Vul een opmerking in!");
                        }
                        else
                        {
                            administratie.AddOnderhoudsbeurt(new Onderhoud(1, t, t.Status.Naam, opmerking));
                            MessageBox.Show("De onderhoud is toegevoegd.");
                            VerversLijsten();
                            nUd_StatusbeheerTramnummer.ResetText();
                            tB_Statusbeheer_Opmerking.ResetText();
                        }

                    }
                }
            }
            catch (Exception en)
            {
                MessageBox.Show(en.Message);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            VerversLijsten();
        }

        private void lB_RemisebeheerTramlijst_SelectedIndexChanged(object sender, EventArgs e)
        {
            Tram tram = lB_RemisebeheerTramlijst.SelectedItem as Tram;
            tbxRemiseBeheerTramNummer.Text = tram.Id.ToString();
        }

        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point? labelPoint = administratie.GetRowColIndex(tableLayoutPanel1, (new Point(e.Location.X, e.Location.Y)));
            if (labelPoint == null) { return; }
            Label label = GetLabel(labelPoint.Value.X, labelPoint.Value.Y);
            if (label == null) { MessageBox.Show("Geen geldige sector"); return; }
            for (int spoor = 0; spoor < Sporen.Length; spoor++)
            {
                if (Sporen[spoor] != null)
                {
                    for (int sector = 1; sector < Sporen[spoor].Length; sector++)
                    {
                        if (Sporen[spoor][sector] != null)
                        {
                            if (Sporen[spoor][sector] == label)
                            {
                                foreach (Spoor s in administratie.Sporen)
                                {
                                    if (s.Spoornummer == spoor && s.Sectornummer == sector)
                                    {
                                        // TODO Schrijf naar DB
                                        if (s.Beschikbaar)
                                        {
                                            s.Beschikbaar = false;
                                        }
                                        else
                                        {
                                            s.Beschikbaar = true;
                                        }
                                        tableLayoutPanel1.Refresh();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void lB_RemisebeheerSpoorlijst_SelectedIndexChanged(object sender, EventArgs e)
        {
            Spoor spoor = lB_RemisebeheerSpoorlijst.SelectedItem as Spoor;
            tbxRemiseBeheerSpoorBeheerSpoorNummer.Text = spoor.Spoorid.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (Tram t in administratie.Trams)
            {
                if (t.Lijn == "OCV")
                {
                    SorteerTram(t);
                }
            }
            CheckTrampositieDB();
        }
    }
}