using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HiQExcelRobot
{
    internal class Validator
    {
        internal bool ValidatePersnr(string persnr)
        {
            bool formatOk = false;

            var rgx1 = new Regex(@"\d{6}-\d{4}");
            var rgx2 = new Regex(@"\d{6}\d{4}");
            var rgx3 = new Regex(@"\d{8}-\d{4}");
            var rgx4 = new Regex(@"\d{8}\d{4}");
            var rgx5 = new Regex(@"\d{2}-\d{2}-\d{2} \d{4}");
            var rgx6 = new Regex(@"\d{4}-\d{2}-\d{2} \d{4}");
            var rgx7 = new Regex(@"9570\d{7}");
            if (persnr == String.Empty) formatOk = true;
            if (rgx1.IsMatch(persnr)) formatOk = true;
            if (rgx2.IsMatch(persnr)) formatOk = true;
            if (rgx3.IsMatch(persnr)) formatOk = true;
            if (rgx4.IsMatch(persnr)) formatOk = true;
            if (rgx5.IsMatch(persnr)) formatOk = true;
            if (rgx6.IsMatch(persnr)) formatOk = true;
            if (rgx7.IsMatch(persnr)) formatOk = true;

            if (formatOk) formatOk = Correctnr(persnr);
            return formatOk;
        }


        internal AccountType PersonnummerKontroll(ref string personNummer)
        {
            AccountType returnType = AccountType.Undefined;
            bool shortNbr = false;
            try
            {
                //Normalisera föddelsedatum genom att ta bort onödiga bindesträck och ge den rätt decenium
                var persnr = personNummer.Replace("-", string.Empty);
                persnr = persnr.Replace(" ", string.Empty);
                string birthDate = string.Empty;
                string löpnummer = string.Empty;
                if (persnr.Length == 12)
                {
                    birthDate = persnr.Substring(0, 8);
                    löpnummer = persnr.Substring(8, 4);
                }
                else if (persnr.Length == 10)
                {
                    birthDate = persnr.Substring(0, 6);
                    löpnummer = persnr.Substring(6, 4);
                    var year = int.Parse((birthDate.Substring(0, 2)));
                    birthDate = (year > int.Parse(DateTime.Now.Year.ToString().Substring(0, 2))) ? "19" + birthDate : "20" + birthDate;
                    shortNbr = true;
                }
                else
                {
                    return returnType;
                }                

                //Kontrollera så att det är ett giltigt datum
                DateTime persBirthDate;
                DateTime.TryParse(birthDate.Insert(4, "-").Insert(7, "-"), out persBirthDate);
                returnType = persBirthDate == DateTime.MinValue ? AccountType.Undefined : AccountType.Normal;

                if (shortNbr && (returnType == AccountType.Undefined))
                {
                    //Vi har här ett tiosiffrig datum och ett ogilltig datum. Tolkar det som normal fiktiv nummer
                    returnType = AccountType.NormalFict;
                }
                else
                {
                    //Kontroll om personen är mindreårig
                    var ageYears = GetAge(persBirthDate);
                    if (ageYears < 18)
                    {
                        if (returnType == AccountType.Normal) returnType = AccountType.Underage;
                    }
                    personNummer = birthDate + löpnummer; //ref retur för århundrande korrigering,.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return returnType;
        }

        private bool Correctnr(string persnr)
        {
            persnr = persnr.Replace("-", string.Empty);
            persnr = persnr.Replace(" ", string.Empty);

            // Check if it's a usernumber
            if (IsUserNumber(ref persnr))   
            {
                return true;
            }

            // Check if it's a correct personal identnumber
            if (persnr.Length == 12) persnr = persnr.Substring(2, 10);

            int value = 0;
            for (int i = 0; i < persnr.Length; i++)
            {
                int t = (persnr[i] - 48) << (1 - (i & 1));
                if (t > 9) t = t - 9;
                value += t;
            }
            var restvalue = (value % 10);
            return restvalue == 0 ? true : false;
        }

        internal bool IsUserNumber(ref string persnr)
        {
            // Check if it's a usernumber
            if (persnr.Length == 11)
            {
                int number;
                if (Int32.TryParse(persnr.Substring(0, 4), out number))
                {
                    if (number == 9570)
                    {
                        if (Int32.TryParse(persnr.Substring(4, 7), out number))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private int GetAge(DateTime birthDate)
        {
            DateTime n = DateTime.Now.Date;
            int age = n.Year - birthDate.Year;
            if (n.Month < birthDate.Month || (n.Month == birthDate.Month && n.Day < birthDate.Day)) age--;
            return age;
        }

    }
}
