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
    public class CompensationDraft : SSRSDefaultPage
    {
        private static readonly string PageParameter = typeof(CompensationDraft).FullName;

        public CompensationDraft() { }

        public override void ApplyParams(string pageParameters)
        {
            XmlTypeName = PageParameter;

            Tabs.AddTab(() => CompensationDraftParamsTab.New(XmlTypeName));

            var labelID = eProjectWeb.Framework.Xml.XmlCache.GetLastNameElement(XmlTypeName);
            LabelID = $"$pageName_{labelID}";
        }
    }

    public class CompensationDraftParamsTab : SSRSReportParamsTab
    {
        Control ctrlCmpcode;
        Control ctrlPelementlevel;
        //Control ctrlPcodelike;

        public new static CompensationDraftParamsTab New(string xmlTypeName)
        {
            var t = eProjectWeb.Framework.ObjectFactory.New<CompensationDraftParamsTab>();
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
                ctrlPelementlevel = EditGroup1["pelementlevel"];
                //ctrlPcodelike = EditGroup1["pcodelike"];
            }
        }

        private void cmpcodeChanged(PageUpdateArgs args)
        {
            var cmpCodaCode = ctrlCmpcode.GetStringValue();
            var pelementlevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
            ctrlPelementlevel.SetValue(pelementlevel);
            //var pcodelike =CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.PartnerPrefix)?.ValueStr;
            //if (string.IsNullOrWhiteSpace(pcodelike)) pcodelike = "null";
            //ctrlPcodelike.SetValue(pcodelike);
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
