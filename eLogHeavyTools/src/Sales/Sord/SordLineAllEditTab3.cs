using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineAllEditTab3 : SordLineAllEditTab2
    {
        public override string GetNamespaceName()
        {
            var n = typeof(SordLineAllEditTab2).Namespace;
            return n;
        }

        protected override string GetPageXmlFileName()
        {
            var n = base.GetPageXmlFileName();
            return n;
        }

        protected virtual IEnumerable<Control> OlcControls => this.EditGroup1?.ControlArray
            .Where(c => c.CustomData == "olc");

        protected override SordHead DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID != ActionID.New && e != null)
            {
                var olc = OlcSordHead.Load(e.Sordid);
                if (olc != null)
                {
                    foreach (var c in this.OlcControls)
                    {
                        c.DataBind(olc, false);
                    }

                    SetTextBoxValue("customername", olc.CustomerName);
                    SetTextBoxValue("customercountry", olc.CustomerCountry);
                    SetTextBoxValue("customerzipcode", olc.CustomerZipCode);
                    SetTextBoxValue("customercity", olc.CustomerCity);
                    SetTextBoxValue("customeraddress", olc.CustomerAddress);
                    SetTextBoxValue("couponcode", olc.CouponCode);
                }

                SordLine defSordLine = GetDefSordLine(args, true, e);

                if (defSordLine != null)
                {
                    var defOlcSordLine = OlcSordLine.Load(defSordLine.Sordlineid);

                    if (defOlcSordLine != null)
                    {
                        (m_defLine["confdeldate"] as DatePickerbox)?.SetValue(defOlcSordLine.Confdeldate);
                        (m_defLine["confqty"] as Numberbox)?.SetValue(defOlcSordLine.Confqty);
                    }
                }
            }

            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, SordHead e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Addrid.HasValue ? OlcSordHead.Load(e.Addrid) : null) ?? OlcSordHead.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            olc.CustomerName = (this.EditGroup1["customername"] as Textbox).GetStringValue();
            olc.CustomerCountry = (this.EditGroup1["customercountry"] as Textbox).GetStringValue();
            olc.CustomerZipCode = (this.EditGroup1["customerzipcode"] as Textbox).GetStringValue();
            olc.CustomerCity = (this.EditGroup1["customercity"] as Textbox).GetStringValue();
            olc.CustomerAddress = (this.EditGroup1["customeraddress"] as Textbox).GetStringValue();
            olc.CouponCode = (this.EditGroup1["couponcode"] as Textbox).GetStringValue();

            var sordLine = map.Get<SordLine>();

            OlcSordLine olcSordLine = null;

            if (sordLine != null)
            {
                olcSordLine = OlcSordLine.Load(sordLine.Sordlineid) ?? OlcSordLine.CreateNew();
            }

            if (olcSordLine == null && sordLine != null)
            {
                olcSordLine = OlcSordLine.CreateNew();
            }

            if (olcSordLine != null)
            {
                olcSordLine.Confdeldate = (m_defLine["confdeldate"] as DatePickerbox)?.GetValue<DateTime>();
                olcSordLine.Confqty = (m_defLine["confqty"] as Numberbox)?.GetValue<decimal>();
                map.Add(olcSordLine);
            }

            map.Add(olc);

            return map;
        }

        protected virtual void SetTextBoxValue(string textBox, string newValue)
        {
            (this.EditGroup1[textBox] as Textbox)?.SetValue(newValue);
        }
    }
}