namespace ICT4Rails
{
    partial class ConducteurForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCheckStatus = new System.Windows.Forms.Button();
            this.lblTramNummer = new System.Windows.Forms.Label();
            this.lblStatusHuidig = new System.Windows.Forms.Label();
            this.tbxTramnummer = new System.Windows.Forms.TextBox();
            this.tbxStatusHuidig = new System.Windows.Forms.TextBox();
            this.cbxStatusNieuw = new System.Windows.Forms.ComboBox();
            this.lblNieuweStatus = new System.Windows.Forms.Label();
            this.btnBevestigStatus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCheckStatus
            // 
            this.btnCheckStatus.Location = new System.Drawing.Point(332, 44);
            this.btnCheckStatus.Name = "btnCheckStatus";
            this.btnCheckStatus.Size = new System.Drawing.Size(75, 31);
            this.btnCheckStatus.TabIndex = 0;
            this.btnCheckStatus.Text = "Check";
            this.btnCheckStatus.UseVisualStyleBackColor = true;
            this.btnCheckStatus.Click += new System.EventHandler(this.btnCheckStatus_Click);
            // 
            // lblTramNummer
            // 
            this.lblTramNummer.AutoSize = true;
            this.lblTramNummer.Location = new System.Drawing.Point(13, 15);
            this.lblTramNummer.Name = "lblTramNummer";
            this.lblTramNummer.Size = new System.Drawing.Size(96, 17);
            this.lblTramNummer.TabIndex = 1;
            this.lblTramNummer.Text = "Tramnummer:";
            // 
            // lblStatusHuidig
            // 
            this.lblStatusHuidig.AutoSize = true;
            this.lblStatusHuidig.Location = new System.Drawing.Point(13, 51);
            this.lblStatusHuidig.Name = "lblStatusHuidig";
            this.lblStatusHuidig.Size = new System.Drawing.Size(98, 17);
            this.lblStatusHuidig.TabIndex = 2;
            this.lblStatusHuidig.Text = "Huidige status";
            // 
            // tbxTramnummer
            // 
            this.tbxTramnummer.Location = new System.Drawing.Point(130, 12);
            this.tbxTramnummer.Name = "tbxTramnummer";
            this.tbxTramnummer.Size = new System.Drawing.Size(185, 22);
            this.tbxTramnummer.TabIndex = 4;
            // 
            // tbxStatusHuidig
            // 
            this.tbxStatusHuidig.Location = new System.Drawing.Point(130, 48);
            this.tbxStatusHuidig.Name = "tbxStatusHuidig";
            this.tbxStatusHuidig.ReadOnly = true;
            this.tbxStatusHuidig.Size = new System.Drawing.Size(185, 22);
            this.tbxStatusHuidig.TabIndex = 5;
            // 
            // cbxStatusNieuw
            // 
            this.cbxStatusNieuw.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStatusNieuw.Enabled = false;
            this.cbxStatusNieuw.FormattingEnabled = true;
            this.cbxStatusNieuw.Items.AddRange(new object[] {
            "Remise",
            "Dienst",
            "Schoonmaak",
            "Defect"});
            this.cbxStatusNieuw.Location = new System.Drawing.Point(130, 85);
            this.cbxStatusNieuw.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxStatusNieuw.Name = "cbxStatusNieuw";
            this.cbxStatusNieuw.Size = new System.Drawing.Size(185, 24);
            this.cbxStatusNieuw.TabIndex = 6;
            // 
            // lblNieuweStatus
            // 
            this.lblNieuweStatus.AutoSize = true;
            this.lblNieuweStatus.Location = new System.Drawing.Point(13, 88);
            this.lblNieuweStatus.Name = "lblNieuweStatus";
            this.lblNieuweStatus.Size = new System.Drawing.Size(96, 17);
            this.lblNieuweStatus.TabIndex = 7;
            this.lblNieuweStatus.Text = "Nieuwe status";
            // 
            // btnBevestigStatus
            // 
            this.btnBevestigStatus.Location = new System.Drawing.Point(332, 81);
            this.btnBevestigStatus.Name = "btnBevestigStatus";
            this.btnBevestigStatus.Size = new System.Drawing.Size(75, 31);
            this.btnBevestigStatus.TabIndex = 8;
            this.btnBevestigStatus.Text = "Bevestig";
            this.btnBevestigStatus.UseVisualStyleBackColor = true;
            this.btnBevestigStatus.Click += new System.EventHandler(this.btnBevestigStatus_Click);
            // 
            // ConducteurForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 148);
            this.Controls.Add(this.btnBevestigStatus);
            this.Controls.Add(this.lblNieuweStatus);
            this.Controls.Add(this.cbxStatusNieuw);
            this.Controls.Add(this.tbxStatusHuidig);
            this.Controls.Add(this.tbxTramnummer);
            this.Controls.Add(this.lblStatusHuidig);
            this.Controls.Add(this.lblTramNummer);
            this.Controls.Add(this.btnCheckStatus);
            this.Name = "ConducteurForm";
            this.Text = "ConducteurForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCheckStatus;
        private System.Windows.Forms.Label lblTramNummer;
        private System.Windows.Forms.Label lblStatusHuidig;
        private System.Windows.Forms.TextBox tbxTramnummer;
        private System.Windows.Forms.TextBox tbxStatusHuidig;
        private System.Windows.Forms.ComboBox cbxStatusNieuw;
        private System.Windows.Forms.Label lblNieuweStatus;
        private System.Windows.Forms.Button btnBevestigStatus;
    }
}