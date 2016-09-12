using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiQExcelRobot
{
    internal class TblKey
    {
        public int Row;
        public string Column;
        public int? ColumnNbr;
    }

    internal class ExcelPosition
    {
        public int RowNbr { get; set; }
        public string Column { get; set; }
        public int SortOrder { get; set; }
    }

    internal enum AccountType
    {
        Undefined,
        Underage,
        Normal, 
        UnderageFict,
        NormalFict
    }

    internal struct Col
    {
        internal const int Zero = 0;
        internal const int One = 1;
        internal const int Two = 2;
        internal const int Three = 3;
        internal const int Four = 4;
        internal const int Five = 5;
        internal const int Six = 6;
    }

    internal class CtrlMapp
    {
        private List<List<string>> _lstCtrlN = new List<List<string>>()
        {
            new List<string>{"mtbPersonnummer", "spcPersonnummer"},
            new List<string>{"cmbKonto1", "mtbMedkonto1", "mtbDispRatt1", "spcKonto1" }, 
            new List<string>{"cmbKonto2", "mtbMedkonto2", "mtbDispRatt2", "spcKonto2"}, 
            new List<string>{"cmbKonto3", "mtbMedkonto3", "mtbDispRatt3", "spcKonto3"}, 
            new List<string>{"cmbInternetbank", "mtbUserNumber", "chkKarnkonto", "spcIntBank"},
            new List<string>{"chkTfnBank", "cmbTfnBankKopplad", "spcTfnBank"},
            new List<string>{"chkVisa", "cmbVisaKopplad", "spcVisa"}
        };

        private List<List<string>> _lstCtrlU = new List<List<string>>()
        {
            new List<string>{"mtbPersonnummer", "mtbFörmyndare1", "mtbFörmyndare2", "spcPersonnummer"},
            new List<string>{"cmbKonto1", "chkGemDisp1", "chkOmyndigDisp1", "spcKonto1"}, 
            new List<string>{"cmbKonto2", "chkGemDisp2", "chkOmyndigDisp2", "spcKonto2"}, 
            new List<string>{"cmbKonto3", "chkGemDisp3", "chkOmyndigDisp3", "spcKonto3"},  
            new List<string>{"cmbInternetbank", "mtbUserNumber", "chkKarnkonto", "spcIntBank"},
            new List<string>{"chkTfnBank", "cmbTfnBankKopplad", "spcTfnBank"},
            new List<string>{"chkVisa", "cmbVisaKopplad", "spcVisa"}
        };


        private List<List<string>> _activeCtrl;

        internal CtrlMapp(AccountType account)
        {
            if (account == AccountType.Normal || account == AccountType.NormalFict)
            {
                _activeCtrl = _lstCtrlN;
            } else if (account == AccountType.Underage || account == AccountType.UnderageFict)
            {
                _activeCtrl = _lstCtrlU;
            }
            else //default
            {
                _activeCtrl = _lstCtrlN;
            }
        }

        public List<string> GetByCol(int col)
        {
            return _activeCtrl[col];
        }

        public List<string> GetCtrl(string key)
        {
            List<string> result = new List<string>();
            for (int col = 0; col < _activeCtrl.Count; col++)
            {
                if (key.StartsWith(_activeCtrl[col][0])) result = _activeCtrl[col];
            }
            return result;
        }
    }

}
