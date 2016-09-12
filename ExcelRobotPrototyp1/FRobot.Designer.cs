namespace HiQExcelRobot
{
    partial class FRobot
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRobot));
            this.panMain = new System.Windows.Forms.Panel();
            this.spcMain = new System.Windows.Forms.SplitContainer();
            this.picLoga = new System.Windows.Forms.PictureBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.panMenu = new System.Windows.Forms.Panel();
            this.chkPrintAvtal = new System.Windows.Forms.CheckBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.erpPersonnummer = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblColumn7 = new System.Windows.Forms.Label();
            this.lblColumn6 = new System.Windows.Forms.Label();
            this.lblColumn5 = new System.Windows.Forms.Label();
            this.lblColumn4 = new System.Windows.Forms.Label();
            this.lblColumn3 = new System.Windows.Forms.Label();
            this.lblColumn2 = new System.Windows.Forms.Label();
            this.lblColumn1 = new System.Windows.Forms.Label();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcMain)).BeginInit();
            this.spcMain.Panel1.SuspendLayout();
            this.spcMain.Panel2.SuspendLayout();
            this.spcMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLoga)).BeginInit();
            this.tlpMain.SuspendLayout();
            this.panMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erpPersonnummer)).BeginInit();
            this.SuspendLayout();
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.SystemColors.Control;
            this.panMain.Controls.Add(this.spcMain);
            this.panMain.Controls.Add(this.panMenu);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 0);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1169, 322);
            this.panMain.TabIndex = 2;
            // 
            // spcMain
            // 
            this.spcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcMain.Location = new System.Drawing.Point(0, 0);
            this.spcMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spcMain.Name = "spcMain";
            this.spcMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcMain.Panel1
            // 
            this.spcMain.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(200)))), ((int)(((byte)(30)))));
            this.spcMain.Panel1.Controls.Add(this.picLoga);
            this.spcMain.Panel1MinSize = 50;
            // 
            // spcMain.Panel2
            // 
            this.spcMain.Panel2.Controls.Add(this.tlpMain);
            this.spcMain.Panel2MinSize = 184;
            this.spcMain.Size = new System.Drawing.Size(1169, 282);
            this.spcMain.SplitterWidth = 3;
            this.spcMain.TabIndex = 2;
            // 
            // picLoga
            // 
            this.picLoga.Image = ((System.Drawing.Image)(resources.GetObject("picLoga.Image")));
            this.picLoga.Location = new System.Drawing.Point(28, 8);
            this.picLoga.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.picLoga.Name = "picLoga";
            this.picLoga.Size = new System.Drawing.Size(265, 35);
            this.picLoga.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoga.TabIndex = 0;
            this.picLoga.TabStop = false;
            // 
            // tlpMain
            // 
            this.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tlpMain.ColumnCount = 7;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tlpMain.Controls.Add(this.lblColumn7, 6, 0);
            this.tlpMain.Controls.Add(this.lblColumn6, 5, 0);
            this.tlpMain.Controls.Add(this.lblColumn5, 4, 0);
            this.tlpMain.Controls.Add(this.lblColumn4, 3, 0);
            this.tlpMain.Controls.Add(this.lblColumn3, 2, 0);
            this.tlpMain.Controls.Add(this.lblColumn2, 1, 0);
            this.tlpMain.Controls.Add(this.lblColumn1, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tlpMain.Size = new System.Drawing.Size(1169, 147);
            this.tlpMain.TabIndex = 1;
            // 
            // panMenu
            // 
            this.panMenu.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panMenu.Controls.Add(this.chkPrintAvtal);
            this.panMenu.Controls.Add(this.btnNew);
            this.panMenu.Controls.Add(this.btnSave);
            this.panMenu.Controls.Add(this.btnAddRow);
            this.panMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panMenu.Location = new System.Drawing.Point(0, 282);
            this.panMenu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panMenu.Name = "panMenu";
            this.panMenu.Size = new System.Drawing.Size(1169, 40);
            this.panMenu.TabIndex = 1;
            // 
            // chkPrintAvtal
            // 
            this.chkPrintAvtal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPrintAvtal.AutoSize = true;
            this.chkPrintAvtal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPrintAvtal.Checked = true;
            this.chkPrintAvtal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrintAvtal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrintAvtal.Location = new System.Drawing.Point(813, 9);
            this.chkPrintAvtal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkPrintAvtal.Name = "chkPrintAvtal";
            this.chkPrintAvtal.Size = new System.Drawing.Size(125, 21);
            this.chkPrintAvtal.TabIndex = 5;
            this.chkPrintAvtal.Text = "Skriv ut avtal";
            this.chkPrintAvtal.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(546, 0);
            this.btnNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(77, 36);
            this.btnNew.TabIndex = 4;
            this.btnNew.Text = "Rensa";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(1089, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 36);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Spara";
            this.toolTip1.SetToolTip(this.btnSave, "Ctrl s");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddRow
            // 
            this.btnAddRow.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAddRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRow.Location = new System.Drawing.Point(0, 0);
            this.btnAddRow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(40, 36);
            this.btnAddRow.TabIndex = 2;
            this.btnAddRow.Text = "+";
            this.toolTip1.SetToolTip(this.btnAddRow, "Ctrl +");
            this.btnAddRow.UseVisualStyleBackColor = true;
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // erpPersonnummer
            // 
            this.erpPersonnummer.ContainerControl = this;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // lblColumn7
            // 
            this.lblColumn7.AutoSize = true;
            this.lblColumn7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn7.Location = new System.Drawing.Point(1002, 5);
            this.lblColumn7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn7.Name = "lblColumn7";
            this.lblColumn7.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn7.Size = new System.Drawing.Size(54, 22);
            this.lblColumn7.TabIndex = 6;
            this.lblColumn7.Tag = global::HiQExcelRobot.Konton.Default.ecVisa;
            this.lblColumn7.Text = "VISA";
            // 
            // lblColumn6
            // 
            this.lblColumn6.AutoSize = true;
            this.lblColumn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn6.Location = new System.Drawing.Point(836, 5);
            this.lblColumn6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn6.Name = "lblColumn6";
            this.lblColumn6.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn6.Size = new System.Drawing.Size(114, 22);
            this.lblColumn6.TabIndex = 5;
            this.lblColumn6.Tag = global::HiQExcelRobot.Konton.Default.ecPhonebank;
            this.lblColumn6.Text = "Telefonbank";
            // 
            // lblColumn5
            // 
            this.lblColumn5.AutoSize = true;
            this.lblColumn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn5.Location = new System.Drawing.Point(670, 5);
            this.lblColumn5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn5.Name = "lblColumn5";
            this.lblColumn5.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn5.Size = new System.Drawing.Size(116, 22);
            this.lblColumn5.TabIndex = 4;
            this.lblColumn5.Tag = global::HiQExcelRobot.Konton.Default.ecInternetbank;
            this.lblColumn5.Text = "Internetbank";
            // 
            // lblColumn4
            // 
            this.lblColumn4.AutoSize = true;
            this.lblColumn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn4.Location = new System.Drawing.Point(504, 5);
            this.lblColumn4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn4.Name = "lblColumn4";
            this.lblColumn4.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn4.Size = new System.Drawing.Size(77, 22);
            this.lblColumn4.TabIndex = 3;
            this.lblColumn4.Tag = global::HiQExcelRobot.Konton.Default.ecKonto3;
            this.lblColumn4.Text = "Konto 3";
            // 
            // lblColumn3
            // 
            this.lblColumn3.AutoSize = true;
            this.lblColumn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn3.Location = new System.Drawing.Point(338, 5);
            this.lblColumn3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn3.Name = "lblColumn3";
            this.lblColumn3.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn3.Size = new System.Drawing.Size(77, 22);
            this.lblColumn3.TabIndex = 2;
            this.lblColumn3.Tag = global::HiQExcelRobot.Konton.Default.ecKonto2;
            this.lblColumn3.Text = "Konto 2";
            // 
            // lblColumn2
            // 
            this.lblColumn2.AutoSize = true;
            this.lblColumn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn2.Location = new System.Drawing.Point(172, 5);
            this.lblColumn2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn2.Name = "lblColumn2";
            this.lblColumn2.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn2.Size = new System.Drawing.Size(77, 22);
            this.lblColumn2.TabIndex = 1;
            this.lblColumn2.Tag = global::HiQExcelRobot.Konton.Default.ecKonto1;
            this.lblColumn2.Text = "Konto 1";
            // 
            // lblColumn1
            // 
            this.lblColumn1.AutoSize = true;
            this.lblColumn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumn1.Location = new System.Drawing.Point(6, 5);
            this.lblColumn1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 0);
            this.lblColumn1.Name = "lblColumn1";
            this.lblColumn1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 0);
            this.lblColumn1.Size = new System.Drawing.Size(122, 22);
            this.lblColumn1.TabIndex = 0;
            this.lblColumn1.Tag = global::HiQExcelRobot.Konton.Default.ecPersNbr;
            this.lblColumn1.Text = "Kundnummer";
            // 
            // FRobot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 322);
            this.Controls.Add(this.panMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FRobot";
            this.Text = "Sparbanken Syd Excel robot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRobot_FormClosing);
            this.Load += new System.EventHandler(this.FRobot_Load);
            this.ResizeEnd += new System.EventHandler(this.FRobot_ResizeEnd);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.FRobot_Layout);
            this.Resize += new System.EventHandler(this.FRobot_Resize);
            this.panMain.ResumeLayout(false);
            this.spcMain.Panel1.ResumeLayout(false);
            this.spcMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcMain)).EndInit();
            this.spcMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLoga)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.panMenu.ResumeLayout(false);
            this.panMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.erpPersonnummer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.ErrorProvider erpPersonnummer;
        private System.Windows.Forms.Panel panMenu;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddRow;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.CheckBox chkPrintAvtal;
        private System.Windows.Forms.SplitContainer spcMain;
        private System.Windows.Forms.PictureBox picLoga;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lblColumn7;
        private System.Windows.Forms.Label lblColumn6;
        private System.Windows.Forms.Label lblColumn5;
        private System.Windows.Forms.Label lblColumn4;
        private System.Windows.Forms.Label lblColumn3;
        private System.Windows.Forms.Label lblColumn2;
        private System.Windows.Forms.Label lblColumn1;
    }
}