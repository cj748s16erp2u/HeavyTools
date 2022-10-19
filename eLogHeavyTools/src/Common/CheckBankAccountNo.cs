using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common
{
    public class CheckBankAccountNo
    {
        public bool ValidateBankAccountNo(string countryCode, string bankAccountNo)
        {
            bankAccountNo = bankAccountNo.ToUpper();

            if (string.IsNullOrEmpty(countryCode) || string.IsNullOrEmpty(bankAccountNo))
                return true;

            var lngid = Convert.ToString(countryCode)?.Substring(0, 2).ToUpper();

            if (lngid == "HU")
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(bankAccountNo, "^[0-9]"))
                {
                    bankAccountNo = bankAccountNo.Replace("-", string.Empty).Replace(" ", string.Empty);
                    if (bankAccountNo.Length == 16)
                        bankAccountNo = bankAccountNo.PadRight(24, '0');

                    // first
                    var bankAccountFirstPart = bankAccountNo.Substring(0, 8);
                    if (!ValidateHUBankAccountNo(bankAccountFirstPart))
                        return false;

                    // second
                    var bankAccountSecondPart = bankAccountNo.Substring(8, 16);
                    if (!ValidateHUBankAccountNo(bankAccountSecondPart))
                        return false;
                }
            }

            return true;
        }

        protected virtual bool ValidateHUBankAccountNo(string bankAccountNoPart)
        {
            int? chekNumber;

            chekNumber = GetHUAccountNoChecksum(bankAccountNoPart);

            return (Convert.ToInt32(chekNumber) % 10 == 0);
        }


        protected virtual int? GetHUAccountNoChecksum(string accNo)
        {
            int? ret = 0;

            for (int i = 0; i < accNo.Length; i++)
            {
                int accNoNumber;
                if (!int.TryParse(accNo[i].ToString(), out accNoNumber))
                    //throw new Exception();
                    return 0;

                switch (i)
                {
                    case 0:
                    case 4:
                    case 8:
                    case 12:
                        ret += accNoNumber * 9;
                        break;
                    case 1:
                    case 5:
                    case 9:
                    case 13:
                        ret += accNoNumber * 7;
                        break;
                    case 2:
                    case 6:
                    case 10:
                    case 14:
                        ret += accNoNumber * 3;
                        break;
                    case 3:
                    case 7:
                    case 11:
                    case 15:
                        ret += accNoNumber * 1;
                        break;

                    default:
                        ret = null;
                        break;
                }
            }

            return ret;

        }

    }
}
