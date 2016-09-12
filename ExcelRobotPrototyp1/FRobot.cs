using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiQExcelRobot
{

    public partial class FRobot : Form
    {
        private ControlCreator _creator;
        private Validator _val;
        private bool thisLock;
        private object onLock = new Object();
        private bool _formIsClosing;
        private bool _inEdit;
        private Control _ctrlInFocus;
        private Dictionary<int, AccountType> _accounts = new Dictionary<int,AccountType>();
        private delegate void ChangeFocusDelegate(Control ctl);
        private Konton K = Konton.Default;

        public FRobot()
        {
            InitializeComponent();
        }

        private void FRobot_Load(object sender, EventArgs e)
        {
            try
            {
                _creator = new ControlCreator();
                _creator.EnterHandler += CtrlEnter;
                _creator.LeaveHandler += CtrlLeave;
                _creator.Validating += PersnrValidating;
                _creator.Validated += PersnrValidated;
                _creator.SelectedIndexChanged += comboBoxSelectedIndexChanged;
                _creator.ComboTextChanged += comboTextChanged;
                _creator.ComboKeyPress += comboBoxKeyPress;
                _creator.SwitchChanged += OnAccountTypeCheckedChanged;
                _creator.OmyndigDispRättChanged += OnOmyndigDisprättChanged;
                _creator.OnSplitterPaint += OnSplitterchange;

                _val = new Validator();

                InitSetup();

                var winState = K.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
                this.WindowState = winState;

                CheckPosition();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i FRobot_Load: - " + ex.Message);
            }
        }

        private void OnSplitterchange(object sender, TableLayoutCellPaintEventArgs e)
        {
            try
            {
                var startLoc = new Point(e.CellBounds.Location.X, e.CellBounds.Location.Y - 3);
                var endLoc = new Point(e.CellBounds.Right, e.CellBounds.Top - 3);
                var rect = new Rectangle(e.CellBounds.Location.X, e.CellBounds.Location.Y - 3,
                    e.CellBounds.Right - e.CellBounds.Location.X, 5);
                var linjeColor1 = Color.FromArgb(245, 200, 30);
                var linjeColor2 = Color.FromArgb(210, 181, 64); //Samm färg som kring loggan, men med lägre Saturation
                Brush aGradientBrush = new LinearGradientBrush(rect, linjeColor1, linjeColor2, LinearGradientMode.Vertical);

                //var pen = new Pen(linjeColor1); //Om jag vill skippa gradering
                var pen = new Pen(aGradientBrush);

                pen.Width = 5;
                e.Graphics.DrawLine(pen, startLoc, endLoc);
            } catch (Exception ex)
            {
                Debug.WriteLine("Exception at OnSplitterchange - ex: " + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void OnOmyndigDisprättChanged(object sender, EventArgs e)
        {
            try
            {
                string ctrlName = ((Control)sender).Name;
                var accountType = GetAccount(_creator.GetRowNbr(ctrlName));
                _creator.ChangeActiveAccounts(tlpMain, ctrlName, accountType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i OnOmyndigDisprättChanged: - " + ex.Message);
            }
        }

        private void InitSetup()
        {
            tlpMain.RowStyles.Clear(); //Somehow the control has 2 RowStyles from start.
            RowStyle style = new RowStyle(SizeType.Absolute, 30); //Header row
            tlpMain.RowStyles.Add(style);

            _creator.ModifyHeadLabels(tlpMain);
            var currentRow = _creator.AddTableRow(tlpMain);
            
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                var currentRow = _creator.AddTableRow(tlpMain);
                _creator.SetFocusOnFirstPersnrCtrl(tlpMain, currentRow);
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i btnAddRow: - " + ex.Message);
            }
        }


        private void CtrlEnter(object sender, EventArgs e)
        {
            try
            {
                if (thisLock) return;

                thisLock = true;
                ((Control)sender).BackColor = Color.LightYellow;
                string ctrlName = ((Control)sender).Name;
                var accType = AccountType.Undefined;
                var row = int.Parse(ctrlName.Substring(ctrlName.Length - 1, 1));
                if (_accounts.Count > row)
                {
                    accType = _accounts[row];
                }
                _creator.CtrlLogic(tlpMain, ctrlName, accType);
                _ctrlInFocus = (Control)sender;
                thisLock = false;
            } catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i CtrlEnter: - " + ex.Message);
            }
        }

        private void CtrlLeave(object sender, EventArgs e)
        {
            try
            {
                Control ctrl = (Control)sender;
                ctrl.BackColor = SystemColors.Control;
                if (ctrl.Name.StartsWith("mtbMedkonto") && ctrl.Text.Trim() == String.Empty)
                {
                    bool hasFoundText = false;

                    tlpMain.Controls.OfType<SplitContainer>()
                        .Where<SplitContainer>(s => s.Name.StartsWith("spcKonto"))
                        .ToList<SplitContainer>().ForEach(s =>
                        {
                            var mtb = s.Panel2.Controls.OfType<MaskedTextBox>();
                            if (mtb != null)
                            {
                                var txtBox = mtb.FirstOrDefault<MaskedTextBox>();
                                if (txtBox != null && txtBox.Text.Trim() != string.Empty) hasFoundText = true;
                            }
                        });

                    int row = _creator.GetRowNbr(ctrl.Name);
                    SetExtraKortCheckBox(row, hasFoundText);
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i CtrlLeave: " + ex.Message);
            }
        }

        private void changeFocus(Control ctl)
        {
            ctl.Focus();
        }

        private AccountType ValidateAndModifyPersNbrCtrl(Control control, ErrorProvider erpPersonnummer)
        {
            var personNummer = control.Text;

            if (_val.IsUserNumber(ref personNummer))
            {
                return AccountType.Undefined;
            }
            var accountType = _val.PersonnummerKontroll(ref personNummer);
            if (accountType == AccountType.Undefined)
            {
                erpPersonnummer.SetError(control, K.wrongPersnbr);
            }
            else
            {
                control.Text = personNummer;
            }
            return accountType;
        }

        private void PersnrValidated(object sender, EventArgs e)
        {
            try
            {
                if (_formIsClosing) return;
                if ((((Control)sender).Text.Trim()) == string.Empty) return;

                erpPersonnummer.SetError((Control)sender, String.Empty);

                string ctrlName = ((Control)sender).Name;
                int row = _creator.GetRowNbr(ctrlName);
                var accountType = ValidateAndModifyPersNbrCtrl((Control)sender, erpPersonnummer);
                if (accountType == AccountType.Undefined) return;

                if (ctrlName.StartsWith("mtbPersonnummer"))
                {
                    bool chkUnderageChecked = GetValueOfUnderageChk(tlpMain, row);

                    if ((accountType == AccountType.Normal || accountType == AccountType.Underage) && chkUnderageChecked)
                    {
                        UncheckUnderageFiktCheckBox(row);
                        chkUnderageChecked = false;
                    }

                    if (!chkUnderageChecked)
                    {
                        var currentRowAccountType = GetAccount(row);

                        if (accountType == AccountType.Underage || accountType == AccountType.UnderageFict)
                        {
                            if (currentRowAccountType != AccountType.Underage && currentRowAccountType != AccountType.UnderageFict)
                            {
                                //Then we already have an account on that row
                                if (GetAccount(row) != AccountType.Undefined) _creator.RemoveCtrlsOnRow(tlpMain, row);

                                var rowNbrAndCtrl = _creator.CreateUnderageAccountRow(tlpMain, ctrlName, accountType);
                                SetAccount(rowNbrAndCtrl.Item1, AccountType.Underage);
                                this.BeginInvoke(new ChangeFocusDelegate(changeFocus), rowNbrAndCtrl.Item2);
                            }
                        }
                        else
                        {
                            if ((currentRowAccountType != AccountType.Normal && currentRowAccountType != AccountType.NormalFict) ||
                                (accountType != currentRowAccountType))
                            {
                                //Then we already have an account on that row
                                if (GetAccount(row) != AccountType.Undefined) _creator.RemoveCtrlsOnRow(tlpMain, row);
                                var rowNbrAndCtrl = _creator.CreateNormalAccountRow(tlpMain, ctrlName, accountType);
                                SetAccount(rowNbrAndCtrl.Item1, accountType);
                                this.BeginInvoke(new ChangeFocusDelegate(changeFocus), rowNbrAndCtrl.Item2);
                            }
                        }
                    }
                }
                else if (ctrlName.StartsWith("mtbMedkonto"))
                {
                    bool connectedChosenAccountExist = CheckIfAccountIsChoosen(ctrlName);
                    if (connectedChosenAccountExist)
                    {
                        SetExtraKortCheckBox(row, true);
                    }
                    else
                    {
                        MessageBox.Show(K.mustChooseAccount);
                    }
                }
                else if (ctrlName.StartsWith("mtbDispRatt"))
                {
                    erpPersonnummer.SetError((Control)sender, "Disp#");
                }
                else if (ctrlName.StartsWith("mtbUserNum"))
                {
                    string strUserNum = ((Control)sender).Text;
                    if (!_val.IsUserNumber(ref strUserNum))
                    {
                        erpPersonnummer.SetError((Control)sender, "User#");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i PersnrValidated: - " + ex.Message);
            }
        }

        private void SetExtraKortCheckBox(int row, bool value)
        {
            var checkBox = tlpMain.Controls.OfType<SplitContainer>()
                .First<SplitContainer>(s => s.Name.Equals("spcVisa" + row))
                .Panel2.Controls.OfType<CheckBox>().FirstOrDefault();

            if (checkBox != null) checkBox.Enabled = value;
        }

        private bool CheckIfAccountIsChoosen(string ctrlName)
        {
            var id = ctrlName.Substring(11, 2);
            var accountComboBoxName = "cmbKonto" + id;
            var comboBox = tlpMain.Controls.OfType<SplitContainer>().First<SplitContainer>(s => s.Name.Equals("spcKonto" + id))
                .Panel1.Controls.OfType<ComboBox>().FirstOrDefault();
            return (comboBox.SelectedIndex >= 0);
        }

        private void UncheckUnderageFiktCheckBox(int row)
        {
            var chkBox = tlpMain.Controls.OfType<SplitContainer>().First<SplitContainer>(s => s.Name.EndsWith(row.ToString()))
                .Panel2.Controls.OfType<SplitContainer>().FirstOrDefault()
                .Panel1.Controls.OfType<CheckBox>().FirstOrDefault();

            if (chkBox != null) chkBox.Checked = false;
        }

        protected void OnAccountTypeCheckedChanged(object sender, EventArgs e)
        {
            string ctrlName = ((Control)sender).Name;
            int row = _creator.GetRowNbr(ctrlName);
            if (GetAccount(row) != AccountType.Undefined) _creator.RemoveCtrlsOnRow(tlpMain, row);
            if (((CheckBox)(sender)).Checked == true)
            {
                var rowNbrAndCtrl = _creator.CreateUnderageAccountRow(tlpMain, ctrlName, AccountType.UnderageFict);
                SetAccount(rowNbrAndCtrl.Item1, AccountType.UnderageFict);
                this.BeginInvoke(new ChangeFocusDelegate(changeFocus), rowNbrAndCtrl.Item2);
            }
            else
            {
                var rowNbrAndCtrl = _creator.CreateNormalAccountRow(tlpMain, ctrlName, AccountType.NormalFict);
                SetAccount(rowNbrAndCtrl.Item1, AccountType.NormalFict);
                this.BeginInvoke(new ChangeFocusDelegate(changeFocus), rowNbrAndCtrl.Item2);
            }
        }

        private AccountType GetAccount(int row)
        {
            var currentAccount = _accounts.FirstOrDefault(p => p.Key == row);
            return currentAccount.Value;
        }

        private void SetAccount(int row, AccountType type)
        {
            _accounts[row] = type;
        }


        private void PersnrValidating(object sender, CancelEventArgs e)
        {
            if (!_val.ValidatePersnr(((Control)sender).Text))
            {
                e.Cancel = true;
                erpPersonnummer.SetError((Control)sender, "Felaktig personnummer");
            }
            else
            {
                if ((((Control)sender).Text.Trim()) != String.Empty)
                {
                    _inEdit = true;
                }
            }
        }


        private void FRobot_Layout(object sender, LayoutEventArgs e)
        {
            try
            {
                _creator.SetFocusOnFirstPersnrCtrl(tlpMain, 0);
            } catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i FRobot_Layout: - " + ex.Message);
            }
        }

        private void FRobot_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_formIsClosing) return;

                if (!_inEdit || IsFormInEdit())
                {
                    _formIsClosing = true;
                    while (Controls.Count > 0)
                    {
                        Controls[0].Dispose();
                    }
                    this.Close();
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i FRobot_FormClosing: - " + ex.Message);
            }
        }

        private bool IsFormInEdit()
        {
            bool returnValue = false;
            DialogResult result = MessageBox.Show(K.unsavedData, "Varning", MessageBoxButtons.YesNo);
            returnValue = result == DialogResult.Yes ? true : false;
            return returnValue;
        }

        private void comboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveToExcel();
            } catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i btnSave_Click: - " + ex.Message);
            }
        }

        private void SaveToExcel()
        {
            var excelManager = new ExcelManager();
            if (excelManager.ValidateData(tlpMain))
            {
                try
                {
                    DialogResult result = MessageBox.Show(K.saveMsg, "Info", MessageBoxButtons.OKCancel);
                    switch (result)
                    {
                        case DialogResult.OK:
                            {
                                var success = excelManager.SaveToExcelFile(tlpMain, chkPrintAvtal.Checked);
                                if (success)
                                {
                                    _creator.RemoveAllEditCtrls(tlpMain);
                                    _accounts.Clear();
                                    _inEdit = false;
                                    InitSetup();
                                }
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ett fel uppstod i SaveToExcel: - " + ex.Message);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keys)
        {
            switch (keys)
            {
                case Keys.Oemplus | Keys.Control:
                    btnAddRow_Click(this, new EventArgs());
                    return true; 
                case Keys.S | Keys.Control:
                    SaveToExcel();
                    return true;
                case Keys.N | Keys.Control:
                    btnNew_Click(this, new EventArgs());
                    return true;
                case Keys.Enter:
                    SendKeys.Send("{TAB}");
                    return true;
            }

            return base.ProcessCmdKey(ref message, keys);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                bool returnValue = false;
                if (_inEdit)
                {
                    DialogResult result = MessageBox.Show(K.unsavedDataNewPage, "Varning", MessageBoxButtons.YesNo);
                    returnValue = result == DialogResult.Yes ? true : false;
                }
                if (returnValue)
                {
                    _creator.RemoveAllEditCtrls(tlpMain);
                    _accounts.Clear();
                    InitSetup();
                    _inEdit = false;
                    tlpMain.Refresh();
                }
                _ctrlInFocus.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i btnNew_Click: - " + ex.Message);
            }
        }

        private void FRobot_ResizeEnd(object sender, EventArgs e)
        {
            CheckPosition();
        }

        private void CheckPosition()
        {
            int xLoc = btnNew.Location.X + (int)((panMenu.Width - btnNew.Location.X) / 2);
            chkPrintAvtal.Location = new Point(xLoc, 8);
        }

        private void FRobot_Resize(object sender, EventArgs e)
        {
            CheckPosition();
        }

        private bool GetValueOfUnderageChk(TableLayoutPanel tlpMain, int row)
        {
            bool retValue = false;
            var c = new CtrlMapp(AccountType.Underage);
            var lastCtrl = c.GetByCol(Col.Zero)[c.GetByCol(Col.Zero).Count - 1];
            var chkUnderage = ((SplitContainer)tlpMain.Controls.Find(lastCtrl + row, true).FirstOrDefault())
                .Panel2.Controls.Find("chkUnderage" + row, true).FirstOrDefault();

            if (chkUnderage != null) retValue = ((CheckBox)chkUnderage).Checked;
            return retValue;
        }

        #region comboBoxEvents
        private void comboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string ctrlName = ((Control)sender).Name;
                var accountType = GetAccount(_creator.GetRowNbr(ctrlName));
                _creator.ChangeActiveAccounts(tlpMain, ctrlName, accountType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ett fel uppstod i comboBoxSelectedIndexChanged: - " + ex.Message);
            }
        }

        private void comboTextChanged(object sender, EventArgs e)
        {
            var ctrlText = ((Control)sender).Text;
            if (ctrlText.Trim() == string.Empty)
            {
                string ctrlName = ((Control)sender).Name;
                var accountType = GetAccount(_creator.GetRowNbr(ctrlName));
                _creator.ChangeActiveAccounts(tlpMain, ctrlName, accountType);
            }
        }
        #endregion

    }
}
