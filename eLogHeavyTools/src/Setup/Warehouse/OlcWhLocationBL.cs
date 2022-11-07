using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhLocationBL : DefaultBL1<OlcWhLocation, OlcWhLocationRules>
    {
        public static readonly string ID = typeof(OlcWhLocationBL).FullName;

        public static T New<T>()
            where T : OlcWhLocationBL
        {
            return ObjectFactory.New<T>();
        }

        public static OlcWhLocationBL New()
        {
            return New<OlcWhLocationBL>();
        }

        public OlcWhLocationBL() : base(DefaultBLFunctions.Basic)
        {
        }

        #region F9 | Következő
        // Engedélyezi / tíltja a helykód kereső képernyőn az F9 | Következő gomb használatát
        // F9 | Következő:
        //   a kijelölt helykód kódjából megkeresi az utolsó számot, azt helyettesíti [0-9]* maszkkal,
        //   majd az így kapott helykód maszk alapján kikeresi az adatbázisból a legnagyobb helykódot,
        //   amiből megint kiveszi az utolsó számot, majd azt 1-el megnövelve új helykódot képez a korábban meghatározott maszk alapján.
        //   A maszkban az első 3 számjegy fix, az a kiválasztott sor helykódjának első 3 számjegye marad.

        public const string GENNEXT_ACTIONID = "genNextLoc";
        public const string GENNEXT_ORIGLOCCODE = "genNextOrigLocCode";

        public static string CreateNextLoccode(string origLocCode, int amount)
        {
            return CreateNextLoccode(origLocCode, null, amount);
        }

        public static string CreateNextLoccode(string origLocCode, string pattern, int amount)
        {
            if (string.IsNullOrEmpty(origLocCode))
            {
                return origLocCode;
            }
            var typeNum = origLocCode.Substring(0, 3);
            var extractSource = origLocCode.Substring(3, origLocCode.Length - 3);
            var extractStartPos = 0;

            string number;
            int startPos, endPos;
            Utils.ExtractLastNumberFromString(extractSource, out number, out startPos, out endPos);
            startPos += extractStartPos;
            endPos += extractStartPos;

            int n;
            if (int.TryParse(number, out n))
            {
                var _m = "";
                for (var i = 0; i < number.Length; i++)
                {
                    _m += "[0-9]";
                }
                var mask = typeNum + extractSource.Substring(0, startPos + 1) + _m + extractSource.Substring(endPos + 1);
                var sql = string.Format("select max(whloccode) whloccode from olc_whlocation (nolock) where whloccode like {0}",
                    eProjectWeb.Framework.Utils.SqlToString(mask));
                object o = eProjectWeb.Framework.Data.SqlDataAdapter.ExecuteSingleValue(eProjectWeb.Framework.Data.DB.Main, sql);
                if (o != null && o != DBNull.Value)
                {
                    extractSource = origLocCode = o.ToString().Replace(".", "");
                    extractStartPos = 0;
                    Utils.ExtractLastNumberFromString(extractSource, out number, out startPos, out endPos);
                    startPos += extractStartPos;
                    endPos += extractStartPos;
                    if (int.TryParse(number, out n))
                    {
                        n += amount;
                        return AddThousandSeparator(origLocCode.Substring(0, startPos + 1) + n.ToString().PadLeft(number.Length, '0') + origLocCode.Substring(endPos + 1));
                    }
                }
            }
            return "";
        }

        //visszarakja kiszedett pontokat a helykódba
        private static string AddThousandSeparator(string loccode)
        {
            return String.Join(".", SplitToParts(loccode));
        }

        //szétválasztja a helykódot 3 számjegyenként
        private static IEnumerable<string> SplitToParts(string loccode)
        {
            for (var i = 0; i < loccode.Length; i += 3)
                yield return loccode.Substring(i, 3);
        }
        #endregion
    }
}
