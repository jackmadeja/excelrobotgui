using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiQExcelRobot
{
    class ControlCreator
    {
        public event EventHandler EnterHandler;
        public event EventHandler LeaveHandler;
        public event EventHandler Validated;
        public event CancelEventHandler Validating;
        public event EventHandler SelectedIndexChanged;
        public event KeyPressEventHandler ComboKeyPress;
        public event EventHandler SwitchChanged;
        public event EventHandler OmyndigDispRättChanged;
        public event EventHandler ComboTextChanged;
        public event TableLayoutCellPaintEventHandler OnSplitterPaint;

        private Dictionary<int, StringCollection> _chosenAccounts;
        private Konton K = Konton.Default;

        private int _rowHeight = 120;

        public ControlCreator()
        {
            _chosenAccounts = new Dictionary<int, StringCollection>();
        }

        public CheckBox CreateCheckbox(ExcelPosition ep, string name, int tabIndex, string label = "")
        {
            var chk = new CheckBox();
            chk.AutoSize = true;
            chk.Location = new Point(20, 5);
            chk.Name = name;
            chk.Text = label;
            chk.Size = new Size(22, 21);
            chk.TabIndex = tabIndex;
            chk.UseVisualStyleBackColor = true;
            chk.Tag = ep;
            chk.Enter += EnterHandler;
            chk.Leave += LeaveHandler;
            return chk;
        }


        public MaskedTextBox CreateTextbox(ExcelPosition ep, string name, int tabIndex, DockStyle dock, bool enabled)
        {
            var textBox = new MaskedTextBox();
            textBox.Dock = dock;
            textBox.Name = name;
            textBox.Size = new Size(153, 26);
            textBox.TabIndex = tabIndex;
            textBox.Tag = ep;
            textBox.Enter += EnterHandler;
            textBox.Leave += LeaveHandler;
            textBox.Validating += Validating;
            textBox.Validated += Validated;
            textBox.Enabled = enabled;
            return textBox;
        }

        public ComboBox CreateComboBox(ExcelPosition ep, string name, int tabIndex, StringCollection items, bool enabled, bool editable = false)
        {
            var combo = new ComboBox();
            combo.Dock = DockStyle.Top;
            combo.FormattingEnabled = true;
            combo.AutoCompleteSource = AutoCompleteSource.ListItems;
            combo.AutoCompleteMode = AutoCompleteMode.Suggest;
            var konton = items.Cast<string>().ToArray<string>();
            combo.Items.AddRange(konton);
            combo.Name = name;
            combo.Size = new Size(204, 28);
            combo.TabIndex = tabIndex;
            combo.Tag = ep;
            combo.Enabled = enabled;
            combo.Enter += EnterHandler;
            combo.Leave += LeaveHandler;
            combo.SelectedIndexChanged += SelectedIndexChanged;
            combo.TextChanged += ComboTextChanged;
            if (!editable) combo.KeyPress += ComboKeyPress;
            return combo;
        }

        private void AddToSplitContainer(ExcelPosition ep, SplitContainer spcPersonnummer, Control ctrl1, Control ctrl2, string labelText1, string labelText2, AccountType accountType, int row)
        {
            spcPersonnummer.SplitterDistance = 25;

            var spcA = new SplitContainer()
            {
                Name = "spcFörmynd_A",
                Orientation = Orientation.Horizontal,
                Panel1MinSize = 13,
                SplitterDistance = 13,
                Dock = DockStyle.Top,
                TabStop = false
            };
            if (labelText1 != string.Empty)
            {
                var lbl = new Label();
                lbl.Dock = DockStyle.Fill;
                lbl.Text = labelText1;
                spcA.Panel1.Controls.Add(lbl);
            }
            if (ctrl1.GetType() == typeof(MaskedTextBox)) spcA.Panel2.Padding = new Padding(0, 0, 15, 0);
            ctrl1.Dock = DockStyle.Top;
            spcA.Panel2.Controls.Add(ctrl1);  //adding textBox1

            var spcB = new SplitContainer()
            {
                Name = "spcFörmynd_B",
                Orientation = Orientation.Horizontal,
                Panel1MinSize = 13,
                SplitterDistance = 13,
                Dock = DockStyle.Top,
                TabStop = false
            };
            if (labelText2 != string.Empty)
            {
                var lbl = new Label();
                lbl.Dock = DockStyle.Fill;
                lbl.Text = labelText2;
                spcB.Panel1.Controls.Add(lbl);
            }
            if (ctrl2.GetType() == typeof(MaskedTextBox)) spcB.Panel2.Padding = new Padding(0, 0, 15, 0);    
            ctrl2.Dock = DockStyle.Top;
            spcB.Panel2.Controls.Add(ctrl2); //adding textBox2

            var spcAB = new SplitContainer()
            {
                Name = "spcFörmynd_AB",
                Orientation = Orientation.Horizontal,
                SplitterDistance = 39,
                Location = new Point(0, 0),
                TabStop = false
            };
            spcAB.Panel1.Controls.Add(spcA);
            spcAB.Panel2.Controls.Add(spcB);

            SetToUnderage(spcPersonnummer, false, row, true, accountType);

            //spcAB.BorderStyle = BorderStyle.FixedSingle;
            var verticalSplit = spcPersonnummer.Panel2.Controls.OfType<SplitContainer>().FirstOrDefault(); // ToList<SplitContainer>();
            verticalSplit.Panel2.Controls.Add(spcAB);

            SetFiktivLabel(ep, spcPersonnummer.Panel1, accountType);

        }

        private void AddToSplitContainer(ExcelPosition ep, SplitContainer spcKonto, Control ctrl1, Control ctrl2, string labelText1, string labelText2, int row)
        {
            spcKonto.SplitterDistance = 25;

            var spcA = new SplitContainer()
            {
                Name = "spcCtrl_1",
                Orientation = Orientation.Horizontal,
                Panel1MinSize = 13,
                SplitterDistance = 13,
                Dock = DockStyle.Top,
                TabStop = false
            };
            if (labelText1 != string.Empty)
            {
                var lbl = new Label();
                lbl.Dock = DockStyle.Fill;
                lbl.Text = labelText1;
                spcA.Panel1.Controls.Add(lbl);
            }
            if (ctrl1.GetType() == typeof(MaskedTextBox)) spcA.Panel2.Padding = new Padding(0, 0, 0, 0);
            ctrl1.Dock = DockStyle.Top;
            spcA.Panel2.Controls.Add(ctrl1);  //adding textBox1

            var spcB = new SplitContainer()
            {
                Name = "spcCtrl_2",
                Orientation = Orientation.Horizontal,
                Panel1MinSize = 13,
                SplitterDistance = 13,
                Dock = DockStyle.Top,
                TabStop = false
            };
            if (labelText2 != string.Empty)
            {
                var lbl = new Label();
                lbl.Dock = DockStyle.Fill;
                lbl.Text = labelText2;
                spcB.Panel1.Controls.Add(lbl);
            }
            if (ctrl2.GetType() == typeof(MaskedTextBox)) spcB.Panel2.Padding = new Padding(0, 0, 0, 0);
            ctrl2.Dock = DockStyle.Top;
            spcB.Panel2.Controls.Add(ctrl2); //adding textBox2

            var spcAB = new SplitContainer()
            {
                Name = "spcKonto",
                Orientation = Orientation.Horizontal,
                SplitterDistance = 39,
                Location = new Point(0, 0),
                TabStop = false
            };
            spcAB.Panel1.Controls.Add(spcA);
            spcAB.Panel2.Controls.Add(spcB);

            spcKonto.Panel2.Controls.Add(spcAB);

        }

        private void SetFiktivLabel(ExcelPosition ep, SplitterPanel panel, AccountType accountType)
        {
            var lblFiktiv = panel.Controls.Find("lblFiktiv", true).FirstOrDefault();
            if (accountType == AccountType.NormalFict || accountType == AccountType.UnderageFict)
            {
                if (lblFiktiv != null) lblFiktiv.Text = "Fiktiv";
            }
            else
            {
                if (lblFiktiv != null) lblFiktiv.Text = String.Empty;
            }
            lblFiktiv.Tag = ep;
        }

        private SplitContainer CreateSplitContainer(string name, Control ctrl1, Control ctrl2, Control ctrl3)
        {
            var spc = new SplitContainer();
            spc.Dock = DockStyle.Fill;
            spc.Name = name;
            spc.Orientation = Orientation.Horizontal;
            spc.Size = new Size(204, 84);
            spc.SplitterDistance = 20;
            spc.TabStop = false;

            spc.Panel1.Controls.Add(ctrl1);

            ctrl2.Dock = DockStyle.Top;
            spc.Panel2.Controls.Add(ctrl2);

            ctrl3.Dock = DockStyle.Bottom;
            spc.Panel2.Controls.Add(ctrl3);
            spc.Panel2.Enabled = false;
            return spc;
        }

        private SplitContainer CreateSplitContainer(string name, Control ctrl1, Control ctrl2, Control ctrl3, string labelText)
        {
            var spc = new SplitContainer();
            spc.Dock = DockStyle.Fill;
            spc.Name = name;
            spc.Orientation = Orientation.Horizontal;
            spc.Size = new Size(204, 84);
            spc.SplitterDistance = 20;
            spc.TabStop = false;

            if (ctrl1.GetType() == typeof(MaskedTextBox))
            {
                spc.Panel1.Padding = new Padding(3, 0, 25, 0);
            }
            spc.Panel1.Controls.Add(ctrl1);

            ctrl2.Location = new Point(20, 0);
            spc.Panel2.Controls.Add(ctrl2);

            if (labelText != string.Empty)
            {
                var lbl = new Label();
                lbl.AutoSize = true;
                lbl.Dock = DockStyle.None;
                lbl.Location = new Point(0, 32);
                lbl.Size = new Size(128, 20);
                lbl.Text = labelText;
                spc.Panel2.Controls.Add(lbl);
            }

            if (ctrl3 != null)
            {
                ctrl3.Dock = DockStyle.Bottom;
                spc.Panel2.Controls.Add(ctrl3);
            }

            return spc;
        }

        private SplitContainer CreateSplitContainer(string name, Control ctrl1, Control ctrl2, Control ctrl3, string labelText2, string labelText3)
        {
            var spc = new SplitContainer();
            spc.Dock = DockStyle.Fill;
            spc.Name = name;
            spc.Orientation = Orientation.Horizontal;
            spc.Size = new Size(204, 84);
            spc.SplitterDistance = 20;
            spc.TabStop = false;

            if (ctrl1.GetType() == typeof(MaskedTextBox))
            {
                spc.Panel1.Padding = new Padding(3, 0, 25, 0);
            }
            spc.Panel1.Controls.Add(ctrl1);

            ctrl2.Location = new Point(20, 0);
            spc.Panel2.Controls.Add(ctrl2);

            if (labelText2 != string.Empty)
            {
                var lbl = new Label();
                lbl.AutoSize = true;
                lbl.Dock = DockStyle.None;
                lbl.Location = new Point(0, 32);
                lbl.Size = new Size(128, 20);
                lbl.Text = labelText2;
                spc.Panel2.Controls.Add(lbl);
            }

            if (ctrl3 != null)
            {
                ctrl3.Dock = DockStyle.Bottom;
                spc.Panel2.Controls.Add(ctrl3);

                if (labelText3 != string.Empty)
                {
                    var lbl = new Label();
                    lbl.AutoSize = true;
                    lbl.Dock = DockStyle.None;
                    lbl.Location = new Point(0, 32);
                    lbl.Size = new Size(128, 20);
                    lbl.Text = labelText3;
                    spc.Panel2.Controls.Add(lbl);
                }
            }

            return spc;
        }

        private SplitContainer CreateSplitContainer(string name, Control ctrl1, Control ctrl2, string labelText)
        {
            var spc = new SplitContainer();
            spc.Dock = DockStyle.Fill;
            spc.Name = name;
            spc.Orientation = Orientation.Horizontal;
            spc.Size = new Size(204, 84);
            spc.SplitterDistance = 30;
            spc.TabStop = false;

            if (ctrl1.GetType() == typeof(MaskedTextBox))
            {
                spc.Panel1.Padding = new Padding(3, 0, 15, 0);
            }
            spc.Panel1.Controls.Add(ctrl1);

            if (labelText != string.Empty)
            {
                var lbl = new Label();
                lbl.AutoSize = true;
                lbl.Dock = DockStyle.None;
                lbl.Location = new Point(0, 25);
                lbl.Size = new Size(128, 20);
                lbl.Text = labelText;
                spc.Panel2.Controls.Add(lbl);
            }

            if (ctrl2 != null)
            {
                ctrl2.Dock = DockStyle.Bottom;
                if (ctrl2.GetType() == typeof(MaskedTextBox))
                {
                    spc.Panel2.Padding = new Padding(3, 0, 25, 0);
                }
                spc.Panel2.Controls.Add(ctrl2);
            }

            return spc;
        }

        private SplitContainer CreateSplitContainer(string name, Control ctrl1)
        {
            var spc = new SplitContainer();
            spc.Dock = DockStyle.Fill;
            spc.Name = name;
            spc.Orientation = Orientation.Horizontal;
            spc.Size = new Size(204, 84);
            spc.SplitterDistance = 30;
            spc.TabStop = false;

            if (ctrl1.GetType() == typeof(MaskedTextBox))
            {
                spc.Panel1.Padding = new Padding(3, 0, 15, 0);
            }
            spc.Panel1.Controls.Add(ctrl1);

            return spc;
        }

        private void SetToUnderage(SplitContainer spcPersonnummer, bool switchState, int row, bool value, AccountType accountType)
        {
            SplitContainer spcSwitch = null;  
            if (accountType == AccountType.NormalFict || accountType == AccountType.UnderageFict)
            {
                var chkBox = new CheckBox();
                chkBox.Name = "chkUnderage" + row;
                chkBox.Checked = value;
                chkBox.CheckAlign = ContentAlignment.MiddleRight;
                chkBox.Padding = new Padding(0, 12, 0, 0);
                chkBox.Height = 52;
                if (value)
                {
                    chkBox.Text = "--";
                    chkBox.Width = 20;
                }
                else
                {
                    chkBox.Text = "Omyndig kund";
                    chkBox.Width = 110;
                }
                chkBox.CheckedChanged += SwitchChanged;
                spcSwitch = CreateVerticalSplitContainer(spcPersonnummer.Name + "Vertical", chkBox.Width, chkBox, null);
                spcSwitch.Panel1.Controls.Add(chkBox);
            } else {
                spcSwitch = CreateVerticalSplitContainer(spcPersonnummer.Name + "Vertical", 1, null, null);
            }
            spcPersonnummer.Panel2.Controls.Add(spcSwitch);
        }

        internal void RemoveAllEditCtrls(TableLayoutPanel tlpMain)
        {
            try
            {
                tlpMain.SuspendLayout();

                var allSplits = (from Item in tlpMain.Controls.OfType<SplitContainer>() select Item).ToList();

                int totalCount = allSplits.Count;
                for (int i = 0; i < totalCount; i++)
                {
                    tlpMain.Controls.Remove(allSplits[i]);
                }

                Enumerable.Range(0, tlpMain.RowCount)
                    .Except(tlpMain.Controls.OfType<Control>()
                    .Select(c => tlpMain.GetRow(c)))
                    .Reverse()
                    .ToList()
                    .ForEach(rowIndex =>
                    {
                        tlpMain.RowStyles.RemoveAt(rowIndex - 1);
                        tlpMain.RowCount--;
                    });

                tlpMain.ResumeLayout();
                tlpMain.PerformLayout();
            } catch (Exception)
            {
                throw;
            }
        }

        public int AddTableRow(TableLayoutPanel tlpMain)
        {
            int index = tlpMain.RowCount++;
            if (index > 7)
            {
                MessageBox.Show("Du kan ha max 7 rader i denna form.");
                return 0;
            }
            RowStyle style = new RowStyle(SizeType.Absolute, _rowHeight);
            tlpMain.RowStyles.Add(style);
            tlpMain.Height = 45 + (index * _rowHeight) + (index * 2);
            CreateEmptyAccountRow(index - 1, tlpMain); //zero-based row nbr.

            return index - 1;
        }

        public void SetFocusOnFirstPersnrCtrl(TableLayoutPanel tlpMain, int currentRow)
        {
            var spcPersNbrs = tlpMain.Controls.OfType<SplitContainer>().Where(c => c.Name.StartsWith("spcPersonnummer"));
            foreach (SplitContainer ctrl in spcPersNbrs)
            {
                var split = ctrl.Panel1.Controls.OfType<SplitContainer>().FirstOrDefault();
                if (split != null)
                {
                    var textBoxPersNr = split.Panel1.Controls.OfType<MaskedTextBox>().FirstOrDefault();
                    if (textBoxPersNr != null)
                    {
                        var NbrInName = textBoxPersNr.Name.Substring(textBoxPersNr.Name.Length - 1, 1);
                        int rowNbr = 0;
                        var succ = Int32.TryParse(NbrInName, out rowNbr);
                        if (succ && rowNbr == currentRow)
                        {
                            textBoxPersNr.Focus();
                            break;
                        }
                    }
                }
            }

        }

        private void CreateEmptyAccountRow(int row, TableLayoutPanel tlpMain)
        {
                tlpMain.SuspendLayout();

                int tabIndex = row * 14;

                tlpMain.Visible = false;
                var c = new CtrlMapp(AccountType.Normal);

                var mtbPersnrCtrl = CreateTextbox(ep(row, K.ecPersNbr), c.GetByCol(Col.Zero)[0] + row, 0 + tabIndex, DockStyle.Top, true);
                var lastCtrl = c.GetByCol(Col.Zero)[c.GetByCol(Col.Zero).Count - 1];
                Debug.WriteLineIf(lastCtrl == string.Empty, "lastCtrl is empty!");
                var spcAccountMain = CreateVerticalSplitContainer(lastCtrl + row + "F", 110,
                    mtbPersnrCtrl, new Label()
                    {
                        Name = "lblFiktiv",
                        ForeColor = Color.Black,
                        Font = new Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular)
                    });
                var spcNewAccount = CreateSplitContainer(lastCtrl + row, spcAccountMain);
                tlpMain.Controls.Add(spcNewAccount, 0, row + 1);
                tlpMain.CellPaint += OnSplitterPaint;  //Ritar en linje mellan raderna
                tlpMain.ResumeLayout();
                tlpMain.Visible = true;

        }

        private SplitContainer CreateVerticalSplitContainer(string name, int distance, Control ctrl1, Control ctrl2)
        {
            var spc = new SplitContainer();
            spc.Dock = DockStyle.Fill;
            spc.Name = name;
            spc.Orientation = Orientation.Vertical;
            spc.SplitterDistance = distance;
            spc.SplitterWidth = 1;
            spc.Panel1MinSize = 20;
            spc.TabStop = false;

            if (ctrl1 != null)
            {
                if (ctrl1.GetType() == typeof(MaskedTextBox))
                {
                    spc.Panel1.Padding = new Padding(3, 0, 15, 0);
                }
                spc.Panel1.Controls.Add(ctrl1);
            }
            if (ctrl2 != null) spc.Panel2.Controls.Add(ctrl2);
            return spc;
        }

        internal Tuple<int, Control> CreateUnderageAccountRow(TableLayoutPanel tlpMain, string ctrlInFocus, AccountType accountType)
        {
            tlpMain.SuspendLayout();

            int row = GetRowNbr(ctrlInFocus);
            int tabIndex = row * 14;
            var c = new CtrlMapp(AccountType.Underage);
            string lastCtrl = string.Empty;

            //Find splitcontainer for Kundnummer.
            SplitContainer spcPersonnummer = FindSplitContainer(tlpMain, row);

            //Col 0
            var mtbFörmyndare1 = CreateTextbox(ep(row, K.ecPersNbrCareT1), c.GetByCol(Col.Zero)[1] + row, 2 + tabIndex, DockStyle.Top, true);
            var mtbFörmyndare2 = CreateTextbox(ep(row, K.ecPersNbrCareT2), c.GetByCol(Col.Zero)[2] + row, 3 + tabIndex, DockStyle.Bottom, true);
            AddToSplitContainer(ep(row, K.ecFiktiv), spcPersonnummer, mtbFörmyndare1, mtbFörmyndare2, K.LblCustodian1, K.LblCustodian2, accountType, row);

            //Col 1
            var cmbKontoCtrl1 = CreateComboBox(ep(row, K.ecKonto1), c.GetByCol(Col.One)[0] + row, 4 + tabIndex, K.KontoOmyndig, true);
            var chkGemDisp1 = CreateCheckbox(ep(row, K.ecKonto1ComDisp), c.GetByCol(Col.One)[1] + row, 5 + tabIndex, K.ChkGemDisp);
            var chkOmyndigDisp1 = CreateCheckbox(ep(row, K.ecKonto1Minor), c.GetByCol(Col.One)[2] + row, 6 + tabIndex, K.ChkOmyndigDisp);
            chkOmyndigDisp1.CheckedChanged += OmyndigDispRättChanged;
            lastCtrl = c.GetByCol(Col.One)[c.GetByCol(Col.One).Count - 1];
            var spcKonto1 = CreateSplitContainer(lastCtrl + row, cmbKontoCtrl1, chkGemDisp1, chkOmyndigDisp1);
            tlpMain.Controls.Add(spcKonto1, Col.One, row + 1);

            //Col 2
            var cmbKontoCtrl2 = CreateComboBox(ep(row, K.ecKonto2), c.GetByCol(Col.Two)[0] + row, 7 + tabIndex, K.KontoOmyndig, true);
            var chkGemDisp2 = CreateCheckbox(ep(row, K.ecKonto2ComDisp), c.GetByCol(Col.Two)[1] + row, 8 + tabIndex, K.ChkGemDisp);
            var chkOmyndigDisp2 = CreateCheckbox(ep(row, K.ecKonto2Minor), c.GetByCol(Col.Two)[2] + row, 9 + tabIndex, K.ChkOmyndigDisp);
            chkOmyndigDisp2.CheckedChanged += OmyndigDispRättChanged;
            lastCtrl = c.GetByCol(Col.Two)[c.GetByCol(Col.Two).Count - 1];
            var spcKonto2 = CreateSplitContainer(lastCtrl + row, cmbKontoCtrl2, chkGemDisp2, chkOmyndigDisp2);
            tlpMain.Controls.Add(spcKonto2, Col.Two, row + 1);

            //Col 3
            var cmbKontoCtrl3 = CreateComboBox(ep(row, K.ecKonto3), c.GetByCol(Col.Three)[0] + row, 10 + tabIndex, K.KontoOmyndig, true);
            var chkGemDisp3 = CreateCheckbox(ep(row, K.ecKonto3ComDisp), c.GetByCol(Col.Three)[1] + row, 11 + tabIndex, K.ChkGemDisp);
            var chkOmyndigDisp3 = CreateCheckbox(ep(row, K.ecKonto3Minor), c.GetByCol(Col.Three)[2] + row, 12 + tabIndex, K.ChkOmyndigDisp);
            chkOmyndigDisp3.CheckedChanged += OmyndigDispRättChanged;
            lastCtrl = c.GetByCol(Col.Three)[c.GetByCol(Col.Three).Count - 1];
            var spcKonto3 = CreateSplitContainer(lastCtrl + row, cmbKontoCtrl3, chkGemDisp3, chkOmyndigDisp3);
            tlpMain.Controls.Add(spcKonto3, Col.Three, row + 1);

            //Col 4
            //var items = K.IntBankMyndig;
            var items = K.IntBankOmyndig;
            if (accountType == AccountType.UnderageFict) items = K.IntBankOmyndigFikt;
            var cmbIntBankCtrl = CreateComboBox(ep(row, K.ecInternetbank), c.GetByCol(Col.Four)[0] + row, 13 + tabIndex, items, true);
            var mtbUserNumberCtrl = CreateTextbox(ep(row, K.ecUserNumberCtrl), c.GetByCol(Col.Four)[1] + row, 14 + tabIndex, DockStyle.Bottom, true); // false);
            var chkKarnkontoCtrl = CreateCheckbox(ep(row, K.ecInternetbankCore), c.GetByCol(Col.Four)[2] + row, 15 + tabIndex, K.ChkCoreAccount);
            chkKarnkontoCtrl.Visible = false;
            lastCtrl = c.GetByCol(Col.Four)[c.GetByCol(Col.Four).Count - 1];
            var spcIntBank = CreateSplitContainer(lastCtrl + row, cmbIntBankCtrl); //, chkKarnkontoCtrl, string.Empty);
            AddToSplitContainer(ep(row, K.ecInternetbank), spcIntBank, mtbUserNumberCtrl, chkKarnkontoCtrl, K.LblUserNumberCtrl, string.Empty, row);
            spcIntBank.Panel2.Enabled = false;
            tlpMain.Controls.Add(spcIntBank, Col.Four, row + 1);

            //Col 5
            var chkTfnBankCtrl = CreateCheckbox(ep(row, K.ecPhonebank), c.GetByCol(Col.Five)[0] + row, 15 + tabIndex, K.ChkOrderTfnBankUng);
            if (accountType == AccountType.UnderageFict) chkTfnBankCtrl.Enabled = false;
            //var cmbTfnBankKoppladKontoCtrl = CreateComboBox(ep(row, K.ecPhonebankConn), c.GetByCol(Col.Five)[1] + row, 16 + tabIndex, new StringCollection(), false);
            lastCtrl = c.GetByCol(Col.Five)[c.GetByCol(Col.Five).Count - 1];
            //var spcTfnBank = CreateSplitContainer(lastCtrl + row, chkTfnBankCtrl, cmbTfnBankKoppladKontoCtrl, K.LblConnectedAccount);
            var spcTfnBank = CreateSplitContainer(lastCtrl + row, chkTfnBankCtrl);
            tlpMain.Controls.Add(spcTfnBank, Col.Five, row + 1);

            //Col 6
            var chkVisaCtrl = CreateCheckbox(ep(row, K.ecVisa), c.GetByCol(Col.Six)[0] + row, 17 + tabIndex, K.ChkOrderVisaOnline);
            var cmbVisaKoppladKontoCtrl = CreateComboBox(ep(row, K.ecVisaConn), c.GetByCol(Col.Six)[1] + row, 18 + tabIndex, new StringCollection(), false, true);
            lastCtrl = c.GetByCol(Col.Six)[c.GetByCol(Col.Six).Count - 1];
            var spcVisa = CreateSplitContainer(lastCtrl + row, chkVisaCtrl, cmbVisaKoppladKontoCtrl, K.LblConnectedAccount);
            tlpMain.Controls.Add(spcVisa, Col.Six, row + 1);

            tlpMain.ResumeLayout();
            return new Tuple<int,Control>(row, mtbFörmyndare1);
        }

        internal Tuple<int, Control> CreateNormalAccountRow(TableLayoutPanel tlpMain, string ctrlInFocus, AccountType accountType)
        {
            tlpMain.SuspendLayout();

            int row = GetRowNbr(ctrlInFocus);
            int tabIndex = row * 14;
            var c = new CtrlMapp(accountType);
            string lastCtrl = string.Empty;

            //Col 0
            SplitContainer spcPersonnummer = FindSplitContainer(tlpMain, row);
            SetFiktivLabel(ep(row, K.ecFiktiv), spcPersonnummer.Panel1, accountType);
            SetToUnderage(spcPersonnummer, false, row, false, accountType);

            //Col 1
            var cmbKontoCtrl1 = CreateComboBox(ep(row, K.ecKonto1), c.GetByCol(Col.One)[0] + row, 1 + tabIndex, K.KontoMyndig, true);
            var mtbMedKontoHavCtrl1 = CreateTextbox(ep(row, K.ecKonto1Coown), c.GetByCol(Col.One)[1] + row, 8 + tabIndex, DockStyle.Fill, true); // false);
            var mtbDispRattCtrl1 = CreateTextbox(ep(row, K.ecDispRattCtrl1), c.GetByCol(Col.One)[2] + row, 9 + tabIndex, DockStyle.Bottom, true); // false);
            lastCtrl = c.GetByCol(Col.One)[c.GetByCol(Col.One).Count - 1];
            var spcKonto1 = CreateSplitContainer(lastCtrl + row, cmbKontoCtrl1); //, mtbDispRattCtrl1, KLblDispRattCtrl);
            AddToSplitContainer(ep(row, K.ecKonto1), spcKonto1, mtbMedKontoHavCtrl1, mtbDispRattCtrl1, K.lblAccountCoowner, K.LblDispRattCtrl, row);
            tlpMain.Controls.Add(spcKonto1, Col.One, row + 1);

            //Col 2
            var cmbKontoCtrl2 = CreateComboBox(ep(row, K.ecKonto2), c.GetByCol(Col.Two)[0] + row, 2 + tabIndex, K.KontoMyndig, true);
            var mtbMedKontoHavCtrl2 = CreateTextbox(ep(row, K.ecKonto2Coown), c.GetByCol(Col.Two)[1] + row, 10 + tabIndex, DockStyle.Fill, true); // false);
            var mtbDispRattCtrl2 = CreateTextbox(ep(row, K.ecDispRattCtrl2), c.GetByCol(Col.Two)[2] + row, 11 + tabIndex, DockStyle.Bottom, true); // false);
            lastCtrl = c.GetByCol(Col.Two)[c.GetByCol(Col.Two).Count - 1];
            var spcKonto2 = CreateSplitContainer(lastCtrl + row, cmbKontoCtrl2); //, mtbMedKontoHavCtrl2, K.lblAccountCoowner);
            AddToSplitContainer(ep(row, K.ecKonto2), spcKonto2, mtbMedKontoHavCtrl2, mtbDispRattCtrl2, K.lblAccountCoowner, K.LblDispRattCtrl, row);
            tlpMain.Controls.Add(spcKonto2, Col.Two, row + 1);

            //Col 3
            var cmbKontoCtrl3 = CreateComboBox(ep(row, K.ecKonto3), c.GetByCol(Col.Three)[0] + row, 3 + tabIndex, K.KontoMyndig, true);
            var mtbMedKontoHavCtrl3 = CreateTextbox(ep(row, K.ecKonto3Coown), c.GetByCol(Col.Three)[1] + row, 12 + tabIndex, DockStyle.Fill, true); // false);
            var mtbDispRattCtrl3 = CreateTextbox(ep(row, K.ecDispRattCtrl3), c.GetByCol(Col.Three)[2] + row, 13 + tabIndex, DockStyle.Bottom, true); // false);
            lastCtrl = c.GetByCol(Col.Three)[c.GetByCol(Col.Three).Count - 1];
            var spcKonto3 = CreateSplitContainer(lastCtrl + row, cmbKontoCtrl3); //, mtbMedKontoHavCtrl3, K.lblAccountCoowner);
            AddToSplitContainer(ep(row, K.ecKonto3), spcKonto3, mtbMedKontoHavCtrl3, mtbDispRattCtrl3, K.lblAccountCoowner, K.LblDispRattCtrl, row);
            tlpMain.Controls.Add(spcKonto3, Col.Three, row + 1);

            //Col 4
            var items = K.IntBankMyndig;
            if (accountType == AccountType.NormalFict) items = K.IntBankMyndigFikt;
            var cmbIntBankCtrl = CreateComboBox(ep(row, K.ecInternetbank), c.GetByCol(Col.Four)[0] + row, 4 + tabIndex, items, true);
            var mtbUserNumberCtrl = CreateTextbox(ep(row, K.ecUserNumberCtrl), c.GetByCol(Col.Four)[1] + row, 14 + tabIndex, DockStyle.Fill, true); // false);
            var chkKarnkontoCtrl = CreateCheckbox(ep(row, K.ecInternetbankCore), c.GetByCol(Col.Four)[2] + row, 15 + tabIndex, K.ChkCoreAccount);
            chkKarnkontoCtrl.Checked = true;
            lastCtrl = c.GetByCol(Col.Four)[c.GetByCol(Col.Four).Count - 1];
            var spcIntBank = CreateSplitContainer(lastCtrl + row, cmbIntBankCtrl); //, chkKarnkontoCtrl, string.Empty);
            AddToSplitContainer(ep(row, K.ecInternetbank), spcIntBank, mtbUserNumberCtrl, chkKarnkontoCtrl, K.LblUserNumberCtrl, string.Empty, row);
            spcIntBank.Panel2.Enabled = false;
            tlpMain.Controls.Add(spcIntBank, Col.Four, row + 1);

            //Col 5
            var chkTfnBankCtrl = CreateCheckbox(ep(row, K.ecPhonebank), c.GetByCol(Col.Five)[0] + row, 6 + tabIndex, K.ChkOrderTfnBank);
            if (accountType == AccountType.NormalFict) chkTfnBankCtrl.Enabled = false;
            //var cmbTfnBankKoppladKontoCtrl = CreateComboBox(ep(row, K.ecPhonebankConn), c.GetByCol(Col.Five)[1] + row, 16 + tabIndex, new StringCollection(), false);
            lastCtrl = c.GetByCol(Col.Five)[c.GetByCol(Col.Five).Count - 1];
            var spcTfnBank = CreateSplitContainer(lastCtrl + row, chkTfnBankCtrl, null, string.Empty);
            tlpMain.Controls.Add(spcTfnBank, Col.Five, row + 1);

            //Col 6
            string orderVisa = K.ChkOrderVisa;
            if (accountType == AccountType.NormalFict) orderVisa = K.ChkOrderVisaOnline;
            var chkVisaCtrl = CreateCheckbox(ep(row, K.ecVisa), c.GetByCol(Col.Six)[0] + row, 7 + tabIndex, orderVisa);
            //chkVisaCtrl.Enabled = false;
            var chkExtraVisaCtrl = CreateCheckbox(ep(row, K.ecExtraVisa), c.GetByCol(Col.Six)[0] + row, 8 + tabIndex, K.ChkExtraVisaKort);
            chkExtraVisaCtrl.Enabled = false;
            var cmbVisaKoppladKontoCtrl = CreateComboBox(ep(row, K.ecVisaConn), c.GetByCol(Col.Six)[1] + row, 17 + tabIndex, new StringCollection(), false, true);
            lastCtrl = c.GetByCol(Col.Six)[c.GetByCol(Col.Six).Count - 1];
            var spcVisa = CreateSplitContainer(lastCtrl + row, chkVisaCtrl, chkExtraVisaCtrl, cmbVisaKoppladKontoCtrl, K.LblConnectedAccount);
            tlpMain.Controls.Add(spcVisa, Col.Six, row + 1);

            tlpMain.ResumeLayout();
            return new Tuple<int,Control>(row, cmbKontoCtrl1);
        }

        internal void CtrlLogic(TableLayoutPanel tlpMain, string ctrlInFocus, AccountType accType)
        {
            var cm = new CtrlMapp(accType);
            var dc = cm.GetCtrl(ctrlInFocus);

            if (dc.Count > 0 && dc[1] != string.Empty)
            {
                foreach (Control ctrl in tlpMain.Controls)
                {
                    var lastCtrl = dc[dc.Count - 1];
                    if ((ctrl.GetType() == typeof(SplitContainer)) && (ctrl.Name.StartsWith(lastCtrl)))
                    {
                        var spltContControls = ((SplitContainer)ctrl).Panel2.Controls;
                        foreach (Control c in spltContControls)
                        {
                            //from the second control to the second last.
                            for (int col = 1; col < dc.Count - 1; col++)
                            {
                                if (c.Name.StartsWith(dc[col]))
                                {
                                    c.Enabled = true;
                                    if (c.GetType() == typeof(ComboBox))
                                    {
                                        //Fill combobox from choisen in previous account comboboxes.
                                        ((ComboBox)c).Items.Clear();
                                        if (_chosenAccounts.Count > 0)
                                        {
                                            int row = GetRowNbr(ctrlInFocus);
                                            if (_chosenAccounts.Count > row)
                                            {
                                                var konton = _chosenAccounts[row].Cast<string>().ToArray<string>();
                                                ((ComboBox)c).Items.AddRange(konton);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

        }

        internal void ChangeActiveAccounts(TableLayoutPanel tlpMain, string ctrlInFocus, AccountType accountType)
        {
            if (ctrlInFocus.StartsWith("cmbKonto") || ctrlInFocus.StartsWith("chkOmyndigDisp"))
            {
                int row = GetRowNbr(ctrlInFocus);
                ClearAccounts(row);
                var accounts = new StringCollection();

                tlpMain.Controls.OfType<SplitContainer>()
                    .Where(s => s.Name.StartsWith("spcKonto")).OfType<SplitContainer>().ToList()
                    .ForEach(ctrl =>
                    {
                        if (row == GetRowNbr(ctrl.Name)) {
                            var comboBox = ctrl.Panel1.Controls.OfType<ComboBox>().First<ComboBox>();
                            var addKontoOk = false;
                            var removeKontoOk = false;
                            if (comboBox.SelectedIndex >= 0) 
                            {
                                if (comboBox.Text != "E-sparkonto" & comboBox.Text != "Österlenkonto") addKontoOk = true;

                                if (comboBox.Text == "Kompiskonto")
                                {
                                    var chekBoxDisprätt = ctrl.Panel2.Controls.OfType<CheckBox>()
                                        .Where(c => c.Name.StartsWith("chkOmyndigDisp")).FirstOrDefault();
                                    addKontoOk = (chekBoxDisprätt != null && chekBoxDisprätt.Checked) ? true : false;
                                }
                                ctrl.Panel2.Enabled = true;
                                ctrl.Panel2.Show();
                            }
                            else  //Inget är valt i comboboxen. Antangligen för att kunden just raderat ett tidigare val.
                            {
                                //Först disablar vi panel2
                                var id = ctrl.Name.Substring(8, 2);
                                var spcKontoPanel = tlpMain.Controls.OfType<SplitContainer>()
                                    .First<SplitContainer>(s => s.Name.Equals("spcKonto" + id)).Panel2;
                                spcKontoPanel.Enabled = false;
                                ctrl.Panel2.Enabled = false;
                                removeKontoOk = true;
                                //Sen söker upp VISA sektionen för att kontrollera så att det konto som nyss tagits bort inte är valt som kopplat konto i VISA.

                            }
                            var strColumnHead = GetColumnHead(tlpMain, ((ExcelPosition)comboBox.Tag).Column);
                            if (addKontoOk) accounts.Add(strColumnHead);
                            if (removeKontoOk) accounts.Remove(strColumnHead);
                        }
                    });

                    var chkVisa = (from Item in tlpMain.Controls.OfType<SplitContainer>() select Item).ToList()
                        .Find(s => s.Name.EndsWith(row.ToString()) && s.Name.StartsWith("spcVisa"))
                        .Panel1.Controls.OfType<CheckBox>().FirstOrDefault();
                    var cmbKoppladeKonton = (from Item in tlpMain.Controls.OfType<SplitContainer>() select Item).ToList()
                        .Find(s => s.Name.EndsWith(row.ToString()) && s.Name.StartsWith("spcVisa"))
                        .Panel2.Controls.OfType<ComboBox>().FirstOrDefault();

                    SetAccounts(row, accounts, chkVisa, cmbKoppladeKonton);
                    ActivateCheckBoxes(tlpMain, row, accountType);

            }
            else if (ctrlInFocus.StartsWith("cmbInternetbank"))
            {
                
                tlpMain.Controls.OfType<SplitContainer>().ToList()
                    .ForEach(ctrl =>
                    {
                        if (ctrl.Name.StartsWith("spcIntBank") && (GetRowNbr(ctrlInFocus) == GetRowNbr(ctrl.Name)))
                        {
                            var internetBas = ExcludeInternetBas(tlpMain, GetRowNbr(ctrl.Name));
                            ctrl.Panel2.Enabled = true; //Changed, panel2 also covers user number so we can't diasable the panel for internetBas;
                            var chkBox = ctrl.Panel2.Controls.OfType<CheckBox>().FirstOrDefault();
                            if (chkBox != null) chkBox.Checked = internetBas;
                        }

                    });
            }
        }

        private bool ExcludeInternetBas(TableLayoutPanel tlpMain, int row)
        {
            var returValue = true;
            var ctrl = tlpMain.Controls.OfType<SplitContainer>().Where(c => c.Name.Equals("spcIntBank" + row)).FirstOrDefault()
                .Panel1.Controls.OfType<ComboBox>().FirstOrDefault<ComboBox>();
            if (ctrl != null) returValue = ! ctrl.Text.StartsWith("Internetbank Bas");
            return returValue;
        }

        private void ActivateCheckBoxes(TableLayoutPanel tlpMain, int row, AccountType accountType)
        {
            var spcTfnBankCheck = (from Item in tlpMain.Controls.OfType<SplitContainer>() select Item).ToList()
                .Find(s => s.Name.EndsWith(row.ToString()) && s.Name.StartsWith("spcTfnBank"))
                .Panel1.Controls.OfType<CheckBox>().FirstOrDefault();
            if (spcTfnBankCheck != null)
            {
                if (accountType != AccountType.NormalFict && accountType != AccountType.UnderageFict) spcTfnBankCheck.Enabled = true;
            }

            var spcVisakCheck = (from Item in tlpMain.Controls.OfType<SplitContainer>() select Item).ToList()
                .Find(s => s.Name.EndsWith(row.ToString()) && s.Name.StartsWith("spcVisa"))
                .Panel1.Controls.OfType<CheckBox>().FirstOrDefault();
            if (spcVisakCheck != null) spcVisakCheck.Enabled = true;

            Debug.WriteLine("ActCheckBoxes");
        }

        internal int GetRowNbr(string ctrlName)
        {
            string lastChar = ctrlName.Substring(ctrlName.Length - 1, 1);
            int ret = 0;
            var success = int.TryParse(lastChar, out ret);
            return ret;
        }

        private void ClearAccounts(int row)
        {
            var currentAccounts = _chosenAccounts.Where(p => p.Key == row).ToDictionary(p => p.Key, p => p.Value);
            if (currentAccounts.Count > 0)
            {
                _chosenAccounts[row].Clear();
            }
        }

        internal void RemoveCtrlsOnRow(TableLayoutPanel tlpMain, int row)
        {
            var allSplits = (from Item in tlpMain.Controls.OfType<SplitContainer>() select Item).ToList();
            int totalCount = allSplits.Count;
            for (int i = 0; i < totalCount; i++)
            {
                var rowNbr = GetRowNbr(allSplits[i].Name);
                if (rowNbr == row)
                {
                    if (allSplits[i].Name.StartsWith("spcPersonnummer"))
                    {
                        ((SplitContainer)allSplits[i]).Panel2.Controls.Clear();
                    }
                    else
                    {
                        tlpMain.Controls.Remove(allSplits[i]);
                    }
                }
            }
        }

        private void SetAccounts(int row, StringCollection accounts, CheckBox spcVisakCheck, ComboBox cmbKoppladeKonton)
        {
            var currentAccounts = _chosenAccounts.Where(p => p.Key == row).ToDictionary(p => p.Key, p => p.Value);

            if (currentAccounts.Count > 0)
            {
                _chosenAccounts[row] = accounts;
            }
            else
            {
                _chosenAccounts.Add(row, accounts);
            }

            cmbKoppladeKonton.Items.Clear();
            var konton = _chosenAccounts[row].Cast<string>().ToArray<string>();
            cmbKoppladeKonton.Items.AddRange(konton);
            if (cmbKoppladeKonton.Text.Trim() != string.Empty) //kontrollera om comboboxen har en item valt
            {
                if (!konton.Contains(cmbKoppladeKonton.Text.Trim()))
                {
                    cmbKoppladeKonton.Text = string.Empty;
                    cmbKoppladeKonton.Focus();
                }
            }

        }

        private string GetColumnHead(TableLayoutPanel tlpMain, string columnHead)
        {
            foreach (Control ctrl in tlpMain.Controls)
            {
                if (ctrl.Name.StartsWith("lblColumn"))
                {
                    string tag = ctrl.Tag.ToString();
                    int semC = tag.IndexOf(";");
                    string column = tag.Substring(semC + 1, tag.Length - (semC + 1));
                    if (column == columnHead)
                    {
                        return ctrl.Text;
                    }
                }
            }
            return string.Empty;
        }

        private ExcelPosition ep(int row, string column)
        {
            var retValue = new ExcelPosition();
            retValue.RowNbr = row;

            int semC = column.IndexOf(";");
            retValue.SortOrder = int.Parse(column.Substring(0, semC));
            retValue.Column = column.Substring(semC + 1, column.Length - (semC + 1));

            return retValue;
        }


        internal void ModifyHeadLabels(TableLayoutPanel tlpMain)
        {
            foreach (Control ctrl in tlpMain.Controls)
            {
                if (ctrl.Name.StartsWith("lblColumn"))
                {
                    string tag = ctrl.Tag.ToString();
                    int semC = tag.IndexOf(";");
                    string column = tag.Substring(semC + 1, tag.Length - (semC + 1));
                    if (column != string.Empty) ctrl.Text = column;
                }
            }
        }

        internal SplitContainer FindSplitContainer(TableLayoutPanel tlpMain, int row)
        {
            SplitContainer spcPersonnummer = null;
            string lastCtrl = string.Empty;
            var c = new CtrlMapp(AccountType.Underage);

            foreach (Control ctrl in tlpMain.Controls)
            {
                lastCtrl = c.GetByCol(Col.Zero)[c.GetByCol(Col.Zero).Count - 1];
                if ((ctrl.GetType() == typeof(SplitContainer))
                    && (ctrl.Name.StartsWith(lastCtrl)))
                {
                    int accrow = GetRowNbr(ctrl.Name);
                    if (accrow == row)
                    {
                        spcPersonnummer = (SplitContainer)ctrl;
                        spcPersonnummer.Tag = row + 1;
                        break;
                    }
                }
            }
            return spcPersonnummer;
        }



    }

}
