using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Sinv
{
    public class SinvPrintBL3 : Base.Sales.Sinv.SinvPrintBL
    {
        public override void Prepare(eProjectWeb.Framework.UI.Templates.PrintArgs param)
        {
            base.Prepare(param);

            // A Financials adatbazis nevet is atadjuk a tartolt eljarasnak, mert az egyenleghez szukseg van ra
            var dbLinkERP = eProjectWeb.Framework.Data.DBConfig.GetDatabaseLink(eProjectWeb.Framework.Session.Catalog, CodaInt.Base.Module.eLogDBConnID);
            var dbLinkFin = eProjectWeb.Framework.Data.DBConfig.GetDatabaseLink(eProjectWeb.Framework.Session.Catalog, CodaInt.Base.Module.CodaDBConnID);

            var finDB = dbLinkFin.Database;
            if (dbLinkERP.Server != dbLinkFin.Server)
                finDB = null;
            param.Param["findb"] = finDB;
        }

        // Eloszor megnezzuk, hogy a szamlarol van-e tartolt PDF. Ha van, akkor azt adjuk vissza. Ha nincs, akkor generalunk.
        public string GetOrGeneratePDF(int? sinvId = null, Base.Sales.Sinv.SinvHead sinvHead = null, GeneratePDF_LanguageSource langSource = GeneratePDF_LanguageSource.Default, string langId = null)
        {
            if (!sinvId.HasValue && sinvHead == null)
                return null;

            if (sinvHead != null && sinvId == null)
                sinvId = sinvHead.Sinvid;

            var pdfContent = (Base.Sales.Sinv.SinvHeadBL.New() as SinvHeadBL3).GetPrintedPdf(sinvId.Value);
            if (pdfContent != null)
            {
                var fileName = System.IO.Path.Combine(eProjectWeb.Framework.Globals.ReportsTempFolder, eProjectWeb.Framework.Utils.GetTempFilenameWithoutExtension() + ".pdf");
                System.IO.File.WriteAllBytes(fileName, pdfContent);
                return fileName;
            }

            return GeneratePDF(sinvId, sinvHead, langSource, langId);
        }
    }
}
