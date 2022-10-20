using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;

[System.Diagnostics.DebuggerStepThrough]
internal class CompanyUtils
{
    private static readonly System.Text.RegularExpressions.Regex rx = new(@"([A-Z]\d{2})");
    public const string CMPCODEALL = "*";

    /// <summary>
    /// Vállalat kódok szétbontása
    /// </summary>
    /// <param name="cmpCodes">Vállalat kódok</param>
    /// <returns>Vállalat kódok listája</returns>
    public static string[] SplitCmpCodes(string cmpCodes)
    {
        if (string.IsNullOrWhiteSpace(cmpCodes))
        {
            return Array.Empty<string>();
        }

        return rx.Matches(cmpCodes).Select(m => m.Value).ToArray();
    }

    /// <summary>
    /// Ellenőrzés, hogy a vállalat kód szerepel-e az ömlesztett vállalat kódok között
    /// </summary>
    /// <param name="cmpCodes">Vállalat kódok</param>
    /// <param name="what">Keresett vállalat kód</param>
    /// <returns>Van-e találat</returns>
    public static bool CmpCodesContains(string cmpCodes, string? what)
    {
        if (string.IsNullOrWhiteSpace(cmpCodes) || string.IsNullOrWhiteSpace(what))
        {
            return false;
        }

        if (cmpCodes == CMPCODEALL)
        {
            return true;
        }

        return rx.Matches(cmpCodes).Any(m => m.Value == what);
    }
}
