using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Reports;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Reports.Financials
{
    public class StatementOfAccount2 : SSRSDefaultPage
    {
        private static readonly string PageParameter = typeof(StatementOfAccount2).FullName;

        public StatementOfAccount2() { }

        public override void ApplyParams(string pageParameters)
        {
            XmlTypeName = PageParameter;

            Tabs.AddTab(() => StatementOfAccountParamsTab2.New(XmlTypeName));

            var labelID = eProjectWeb.Framework.Xml.XmlCache.GetLastNameElement(XmlTypeName);
            LabelID = $"$pageName_{labelID}";
        }
    }

    public class StatementOfAccountParamsTab2 : SSRSReportParamsTab
    {
        Control ctrlCmpcode;
        Control ctrlGllevel;
        Control ctrlGrouplevel;        
        Control ctrlPelementlevel;
        Control ctrlPcodelike;
        Control ctrlDatum2;
        Control ctrlDatum;

        public new static StatementOfAccountParamsTab2 New(string xmlTypeName)
        {
            var t = eProjectWeb.Framework.ObjectFactory.New<StatementOfAccountParamsTab2>();
            t.Initialize(xmlTypeName);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();

            if (EditGroup1 != null)
            {
                ctrlCmpcode = EditGroup1["cmpcode"];
                ctrlCmpcode.SetOnChangedWhenExists(new ControlEvent(cmpcodeChanged));
                ctrlGllevel = EditGroup1["gllevel"];
                ctrlGrouplevel = EditGroup1["grouplevel"];
                ctrlPelementlevel = EditGroup1["pelementlevel"];
                ctrlPcodelike = EditGroup1["pcodelike"];
                ctrlDatum2 = EditGroup1["datum2"];
                ctrlDatum2.SetOnChangedWhenExists(new ControlEvent(Datum2Changed));
                ctrlDatum = EditGroup1["datum"];
            }
        }

        private void Datum2Changed(PageUpdateArgs args)
        {
            ctrlDatum.SetValue((ctrlDatum2.GetValue<DateTime>()).Value.ToString("yyyy'/'MM'/'dd"));
        }

        private void cmpcodeChanged(PageUpdateArgs args)
        {
            var cmpCodaCode = ctrlCmpcode.GetStringValue();
            var gllevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;
            ctrlGllevel.SetValue(gllevel);
            ctrlGrouplevel.SetValue(gllevel);
            var pelementlevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
            ctrlPelementlevel.SetValue(pelementlevel);
            var pcodelike = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.PartnerPrefix)?.ValueStr;
            if (string.IsNullOrWhiteSpace(pcodelike)) pcodelike = "null";
            ctrlPcodelike.SetValue(pcodelike);
        }

        protected override string Validate(ReportParams repParams)
        {
            repParams.Params["runuser"] = Session.UserID;

            base.Validate(repParams);

            string err = "";
            foreach (var c in this.EditGroup1.ControlArray)
            {
                if (c.Mandatory && (c.Value == null || c.GetStringValue() == string.Empty))
                {
                    err += string.Concat('"', c.Label, '"', " ", eProjectWeb.Framework.Lang.Translator.Translate("$parameter_required"), "\n");
                }
            }

            var controls = repParams.Params;
            var lngid = Convert.ToString(controls["lngid"])?.Substring(0, 2).ToUpper(); // hu-HU elso 2 karakter csak
            var language = eProjectWeb.Framework.Reports.SSRSUtils.GetLanguageID(lngid);
            controls["language"] = language;

            return FormatValidateErrors(err);
        }
    }
}
