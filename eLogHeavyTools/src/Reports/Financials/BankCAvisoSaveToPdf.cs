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
    public class BankCAvisoSaveToPdf : SSRSDefaultPage
    {
        private static readonly string PageParameter = typeof(BankCAvisoSaveToPdf).FullName;

        public BankCAvisoSaveToPdf() { }

        public override void ApplyParams(string pageParameters)
        {
            XmlTypeName = PageParameter;

            Tabs.AddTab(() => BankCAvisoSaveToPdfParamsTab.New(XmlTypeName));

            var labelID = eProjectWeb.Framework.Xml.XmlCache.GetLastNameElement(XmlTypeName);
            LabelID = $"$pageName_{labelID}";
        }
    }

    public class BankCAvisoSaveToPdfParamsTab : SSRSReportParamsTab
    {
        Control ctrlPaymentList;
        CheckedCombo ctrlPaymentList2;
        Control ctrlDescr;
        Control ctrlSendMail;

        public new static BankCAvisoSaveToPdfParamsTab New(string xmlTypeName)
        {
            var t = eProjectWeb.Framework.ObjectFactory.New<BankCAvisoSaveToPdfParamsTab>();
            t.Initialize(xmlTypeName);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();

            if (EditGroup1 != null)
            {
                ctrlPaymentList = EditGroup1["payment_list"];
                ctrlPaymentList2 = (CheckedCombo)EditGroup1["payment_list2"];
                ctrlPaymentList2.SetOnChangedWhenExists(PaymentList2OnChanged);
                ctrlDescr = EditGroup1["descr"];
                ctrlSendMail = EditGroup1["sendmail"];
            }

            OnPageLoad += BankCAvisoSaveToPdfParamsTab_OnPageLoad;
        }

        private void BankCAvisoSaveToPdfParamsTab_OnPageLoad(PageUpdateArgs args)
        {
            ctrlSendMail.SetValue(true);
        }

        private void PaymentList2OnChanged(PageUpdateArgs args)
        {
            var payments = ctrlPaymentList2.GetValues();

            if (payments == null)
            {
                ctrlDescr.SetValue(null);
                ctrlPaymentList.SetValue(null);
                return;
            }

            var packagecodelist = new List<string>();
            var packagenamelist = new List<string>();

            IListProvider prov = ListServer.TryGet(ctrlPaymentList2.ListID);
            if (prov != null)
            {
                var packages = (eProjectWeb.Framework.Data.DataSet)prov.GetValues(
                    new Dictionary<string, object>()
                    {
                            { "psqid", "cpayment_list" },
                    });

                foreach (var package in packages.AllRows)
                {
                    var packagecode = ConvertUtils.ToString(package["code"]);
                    var packagename = ConvertUtils.ToString(package["name"]);
                    if (payments.Contains(packagecode))
                    {
                        packagecodelist.Add(packagecode);
                        packagenamelist.Add(packagename);

                        ctrlDescr.SetValue(string.Join("\n", packagenamelist));
                        ctrlPaymentList.SetValue(string.Join("|", packagecodelist));
                    }
                }
            }
        }

        protected override string Validate(ReportParams repParams)
        {
            repParams.Params["user"] = Session.UserID;

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
