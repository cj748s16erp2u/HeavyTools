using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using CodaInt.Base.Common.DocHeadLineElement;
using eLog.Base.Masters.Partner;
using CodaInt.Base.Setup.OthTrLineDoc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class CostLineBL3 : CodaInt.Base.Purchase.Pinv.CostLineBL2
    {
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var b = base.PreSave(objects, e);

            if (b)
            {
                var costLine = objects.Default as Base.Purchase.Pinv.CostLine;
                var defaultCostType = CustomSettings.GetString("PINVASSIGNOTHERDefaultCostType");

                if (e is Base.Purchase.Pinv.CostLine)
                {
                    if (!string.IsNullOrEmpty(defaultCostType))
                    {
                        if (eProjectWeb.Framework.ConvertUtils.ToInt32(costLine?.Pinvlineid).HasValue)
                        {
                            if (string.IsNullOrEmpty(costLine.Costtypeid))
                                costLine.Costtypeid = defaultCostType.ToString();
                        }
                    }
                }

                if (e is OlcCostLine)
                {
                    var olcCostLine = (OlcCostLine)e;
                    if (olcCostLine.State == eProjectWeb.Framework.Data.DataRowState.Added)
                        olcCostLine.Costlineid = costLine.Costlineid;
                }
            }

            return b;
        }

        public override void Validate(eProjectWeb.Framework.BL.BLObjectMap objects)
        {
            base.Validate(objects);

            var olcCostLine = objects.Get<OlcCostLine>();
            if (olcCostLine != null)
                eProjectWeb.Framework.RuleServer.Validate(objects, typeof(OlcCostLineRules));
        }

        protected override void PreDelete(PreAndPostDeleteArgs args)
        {
            base.PreDelete(args);

            var olcCostLine = OlcCostLine.Load(args.Costline.Costlineid);
            if (olcCostLine != null)
            {
                olcCostLine.State = eProjectWeb.Framework.Data.DataRowState.Deleted;
                olcCostLine.Save();
            }
        }

        protected override void ImportValues(ImportValueArgs args)
        {
            // default costtype toltese, mert nincs az import mezok kozott
            var defaultCostType = CustomSettings.GetString("PINVASSIGNOTHERDefaultCostType");
            if (string.IsNullOrEmpty(args.Costline.Costtypeid))
                args.Costline.Costtypeid = defaultCostType;

            base.ImportValues(args);

            var ctn = typeof(OfcDocHLElm).Name;
            var ofc = args.Map.Get(ctn) as OfcDocHLElm;
            if (ofc == null)
            {
                ofc = OfcDocHLElm.CreateNew();
                ofc.Dhltype = (int)OfcDocHLElmDhltype.Costline;
                args.Map.Add(ctn, ofc);
            }

            ImportOfcDocHLElm(args);
            ImportOlcCostline(args);
        }

        protected void ImportOfcDocHLElm(ImportValueArgs args)
        {
            var ctn = typeof(OfcDocHLElm).Name;
            var ofc = args.Map.Get(ctn) as OfcDocHLElm;
            if (ofc == null)
            {
                ofc = OfcDocHLElm.CreateNew();
                ofc.Dhltype = (int)OfcDocHLElmDhltype.Costline;
                args.Map.Add(ctn, ofc);
            }

            switch (args.Field)
            {
                case "costlineel3":
                    ofc.El3 = args.Value;
                    break;
                case "costlineel4":
                    ofc.El4 = args.Value;
                    break;
                case "costlineel5":
                    ofc.El5 = args.Value;
                    break;
            }
        }

        protected void ImportOlcCostline(ImportValueArgs args)
        {
            var tn = typeof(OlcCostLine).Name;
            var ccl = args.Map.Get(tn) as OlcCostLine;
            if (ccl == null)
            {
                ccl = OlcCostLine.CreateNew();
                args.Map.Add(tn, ccl);
            }

            switch (args.Field)
            {
                case "othtrlinedocid":
                    try
                    {
                        var o = OthTrLineDoc.Load(new Key { { OthTrLineDoc.FieldOthtrlinedocid.Name, args.Value.ToString() } });
                        if (o == null)
                        {
                            throw new MessageException("$othtrlinedoc_not_found");
                        }
                        ccl.Othtrlinedocid = o.Othtrlinedocid;
                    }
                    catch
                    {
                        throw new MessageException("$othtrlinedoc_not_found");
                    }

                    break;
            }
        }

    }
}
