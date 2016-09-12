using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ex = Microsoft.Office.Interop.Excel;

namespace HiQExcelRobot
{
    internal class ExcelManager
    {
        Dictionary<TblKey, string> _excelData;  //rowNbr, column Key, column Value
        Missing _misValue = Missing.Value;
        private Konton K = Konton.Default;

        internal bool SaveToExcelFile(TableLayoutPanel tlpMain, bool skrivUtStatus)
        {
            _excelData = new Dictionary<TblKey, string>();
            var columnCells = new List<SplitContainer>();

            //_excelData.Add(new TblKey { Row = 0, Column = K.ecAccountType, ColumnNbr = 1 }, K.ecAccountType);

            foreach (SettingsProperty currentProperty in K.Properties)
            {
                if (currentProperty.Name.StartsWith("ec"))
                {
                    string tag = currentProperty.DefaultValue.ToString();
                    int semC = tag.IndexOf(";");
                    string column = tag.Substring(semC + 1, tag.Length - (semC + 1));
                    int pos = int.Parse(tag.Substring(0, semC));
                    _excelData.Add(new TblKey { Row = 0, Column = column, ColumnNbr = pos }, column);
                }
            }

            columnCells = tlpMain.Controls.OfType<SplitContainer>().ToList<SplitContainer>();

            //Run thru all controls in SplitContainer and move their values and keys to excelData list.
            foreach (SplitContainer spc in columnCells)
            {
                UpdateDictionary(spc.Panel1);
                UpdateDictionary(spc.Panel2);
            }

            DecideAccountType();

            PositionExcelColumns();

            AddSkrivUtColumn(skrivUtStatus);

            return Save();
        }


        private void AddSkrivUtColumn(bool skrivUtStatus)
        {
            //Get one list with all the rows for just one column that has row number in the name.
            var rows = _excelData.Where(c => K.ecPersNbr.EndsWith(c.Key.Column))
                .ToDictionary(c => c.Key, c => c.Value)
                .OrderBy(x => x.Key.Column);
            int highestRowNbr = 0;
            foreach(KeyValuePair<TblKey, string> r in rows)
            {
                if (r.Key.Row > highestRowNbr) highestRowNbr = r.Key.Row;
            }

            var headerRowList = _excelData.Where(c => c.Key.Row == 0)
                .ToDictionary(c => c.Key, c => c.Value);
            int lastColumnNbr = headerRowList.Count + 1;

            _excelData.Add(new TblKey { Row = 0, Column = "ecSkrivUt0", ColumnNbr = lastColumnNbr }, "Skriv ut");
            for (int i = 1; i <= highestRowNbr; i++)
            {
                _excelData.Add(new TblKey { Row = i, Column = "ecSkrivUt", ColumnNbr = lastColumnNbr }, skrivUtStatus ? "JA" : "NEJ");
            }

        }

        private void DecideAccountType()
        {
            _excelData.Where(c => K.ecKonto1Minor.EndsWith(c.Key.Column) && c.Key.Row != 0).ToList()
                .ForEach(cell =>
                {
                    _excelData.Add(new TblKey { Row = cell.Key.Row, Column = "Kontotyp", ColumnNbr = 1 }, "Omyndig");
                });

            _excelData.Where(c => K.ecKonto1Coown.EndsWith(c.Key.Column) && c.Key.Row != 0).ToList()
                .ForEach(cell =>
                {
                    _excelData.Add(new TblKey { Row = cell.Key.Row, Column = "Kontotyp", ColumnNbr = 1 }, "Myndig");
                });


        }


        private void PositionExcelColumns()
        {
            //get all cells in row 0 and sort them by the value in the cell.
            var rowZero = _excelData.Where(c => c.Key.Row == 0 && c.Key.Column != K.ecAccountType)
                .ToDictionary(c => c.Key, c => c.Value)
                .OrderBy(x => x.Key.ColumnNbr);

            foreach (var cell in rowZero)
            {
                var colx = _excelData.Where(c => c.Key.Column == cell.Key.Column && c.Key.Row != 0).FirstOrDefault();
                if (colx.Key != null)
                {
                    var nbr = colx.Key.ColumnNbr;
                    cell.Key.ColumnNbr = nbr;
                }
            }
        }

        private void UpdateDictionary(SplitterPanel panel)
        {
            foreach (Control spctrl in panel.Controls)
            {
                if (spctrl.GetType() == typeof(SplitContainer))
                {
                    UpdateDictionary(((SplitContainer)spctrl).Panel1);
                    UpdateDictionary(((SplitContainer)spctrl).Panel2);
                }
                else if (spctrl.GetType() != typeof(Label))
                {
                    string value = string.Empty;

                    if (spctrl.GetType() == typeof(CheckBox))
                    {
                        if (spctrl.Name.StartsWith("chkVisa"))
                        {
                            value = ((CheckBox)spctrl).Checked ? spctrl.Text.EndsWith("Online") ? "VISA Online" : "VISA" : string.Empty;
                        }
                        else if (spctrl.Name.StartsWith("chkTfnBank"))
                        {
                            value = ((CheckBox)spctrl).Checked ? spctrl.Text.EndsWith("Ung") ? "Telefonbank Ung" : "Telefonbank" : string.Empty;
                        }
                        else if (spctrl.Name.StartsWith("chkGemDisp"))
                        {
                            value = ((CheckBox)spctrl).Checked ? spctrl.Text : string.Empty;
                        }
                        else if (spctrl.Name.StartsWith("chkOmyndigDisp"))
                        {
                            value = ((CheckBox)spctrl).Checked ? spctrl.Text : string.Empty;
                        }
                        else
                        {
                            value = spctrl.Text;
                        }
                    }
                    else
                    {
                        value = spctrl.Text;
                    }

                    var ec = (ExcelPosition)spctrl.Tag;
                    if (ec != null)
                    {
                        _excelData.Add(new TblKey { Row = ec.RowNbr + 1, Column = ec.Column, ColumnNbr = ec.SortOrder }, value);
                    }
                }
                else //Om det är en label kolla om vi hittar label med bokstaven "F" som "Fiktiv" och sätt detta konto till fiktivt.
                {
                    var ec = (ExcelPosition)spctrl.Tag;
                    if (ec != null)
                    {
                        var value = string.Empty;
                        if (spctrl.Text == "Fiktiv") value = "FIKTIV";
                        _excelData.Add(new TblKey { Row = ec.RowNbr + 1, Column = ec.Column, ColumnNbr = ec.SortOrder }, value);
                    }
                }
            }
        }

            
        internal bool ValidateData(TableLayoutPanel tlpMain)
        {
            return true;
        }

        private Ex.Worksheet WriteFormContent2ExcelFile(Ex.Workbook xlWorkBook)
        {
            Ex.Worksheet xlWorkSheet;
            try
            {
                xlWorkSheet = (Ex.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                //Color and font
                xlWorkSheet.Cells[1, 1].EntireRow.Font.Bold = true;
                xlWorkSheet.get_Range("A1:X1", _misValue).Interior.Color = ColorTranslator.ToOle(Color.Cornsilk);

                foreach (var cell in _excelData)
                {
                    xlWorkSheet.Cells[cell.Key.Row + 1, cell.Key.ColumnNbr] = cell.Value;
                }
            } catch (Exception ex)
            {
                throw;
            }
            return xlWorkSheet;
        }

        private bool Save()
        {
            try
            {
                var xlApp = new Ex.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("Excel is not properly installed!!");
                    return false;
                }
                else
                {
                    if (!Directory.Exists(Konton.Default.ExcelFilePath))
                    {
                        MessageBox.Show("Sökväg: '" + Konton.Default.ExcelFilePath + "' finns inte. Kan inte spara till Excel-fil.");
                        return false;
                    }

                    Ex.Workbook xlWorkBook = xlApp.Workbooks.Add(_misValue);
                    xlWorkBook.Application.DisplayAlerts = false;

                    Ex.Worksheet xlWorkSheet = WriteFormContent2ExcelFile(xlWorkBook);
                    //var documentFilePath = Konton.Default.ExcelFilePath + "K" + DateTime.Now.ToString().Replace("-", "").Replace(":", "").Replace(" ", "_") + ".xlsx";
                    var documentFilePath = Konton.Default.ExcelFilePath + "Kunddata.xlsx";
                    
                    var robotFilePath = Konton.Default.RobotFilePath;

                    xlWorkBook.SaveAs(documentFilePath, Ex.XlFileFormat.xlOpenXMLWorkbook, _misValue, _misValue, _misValue, _misValue, Ex.XlSaveAsAccessMode.xlExclusive, Ex.XlSaveConflictResolution.xlLocalSessionChanges,
                      _misValue, _misValue, _misValue, _misValue);
                    xlWorkBook.Close(true, _misValue, _misValue);
                    xlApp.Quit();

                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);

                    if (K.ShowExcel) Process.Start(documentFilePath);
                    if (K.StartRobot)
                    {
                        try
                        {
                            Process.Start(robotFilePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ett fel inträffade. Process '" + robotFilePath + "' går inte att starta.", "Varning");
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            return true;
        }

        private void releaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
