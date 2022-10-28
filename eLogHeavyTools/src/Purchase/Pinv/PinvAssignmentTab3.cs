using eLog.Base.Purchase.Pinv;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvAssignmentTab3 : CodaInt.Base.Purchase.Pinv.PinvAssignmentTab2
    {
        public static string[] ImportFields3 = new string[]
            {"realcostval", "costlineel3", "costlineel4", "costlineel5", "othtrlinedocid", "note"};

        protected override void SetImportDefaultText()
        {
            if (ctrlImportText != null)
                ctrlImportText.Value = string.Join("\t",
                    ImportFields3.Select(f => f.Split('|').Last())
                        .Select(f => eProjectWeb.Framework.Lang.Translator.Translate(
                            (!f.StartsWith("$") ? "$" : "") + f)));
        }

        protected override void BeforeImport(PageUpdateArgs args, CostLineBL.ImportArgs ia)
        {
            // uj import mezok
            ia.Fields = ImportFields3;
        }

    }
}
