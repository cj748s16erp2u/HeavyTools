using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Sales.Sinv;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordHeadEditTab3 : SordHeadEditTab2
    {
        protected Control m_duedate;

        SordHeadWebShopEditCommon m_common;

        private LayoutTable m_webshopLayoutTable;

        public override string GetNamespaceName()
        {
            var n = typeof(SordHeadEditTab2).Namespace;
            return n;
        }

        protected override string GetPageXmlFileName()
        {
            var n = base.GetPageXmlFileName();
            return n;
        }

        protected virtual IEnumerable<Control> OlcControls => this.EditGroup1?.ControlArray
            .Where(c => c.CustomData == "olc");

        protected virtual IEnumerable<Control> OlcControlsWebShop => this.m_common.EditGroup?.ControlArray
            .Where(c => c.CustomData == "olc");

        protected override void CreateBase()
        {
            base.CreateBase();

            this.m_webshopLayoutTable = (LayoutTable)this["WebShopEditGroup"];

            this.m_duedate = this.EditGroup1[SinvHead.FieldDuedate.Name];

            this.m_common = new SordHeadWebShopEditCommon(this.m_webshopLayoutTable);
        }

        protected override SordHead DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID == ActionID.New)
            {
                if (Session.CompanyIds.Length == 1)
                {
                    int? cmpid = Session.CompanyIds[0];
                    if (this.m_cmpid != null)
                    {
                        this.m_cmpid.Value = cmpid;
                        this.m_cmpid.FireEvent(Control.Event_OnChanged, args, false);
                        this.m_cmpid.Disabled = true;
                    }
                }

                if (this.m_duedate != null)
                {
                    this.m_duedate.Disabled = true;
                }
            }

            if (args.ActionID != ActionID.New && e != null)
            {
                var olc = OlcSordHead.Load(e.Sordid);
                if (olc != null)
                {
                    foreach (var c in this.OlcControls)
                    {
                        c.DataBind(olc, false);
                    }

                    foreach (var c in this.OlcControlsWebShop)
                    {
                        c.DataBind(olc, false);
                    }

                    this.SetTextBoxValue("customername", olc.CustomerName);
                    this.SetTextBoxValue("customercountry", olc.CustomerCountry);
                    this.SetTextBoxValue("customerzipcode", olc.CustomerZipCode);
                    this.SetTextBoxValue("customercity", olc.CustomerCity);
                    this.SetTextBoxValue("customeraddress", olc.CustomerAddress);
                    this.SetTextBoxValue("couponcode", olc.CouponCode);
                }
                
                SordLine defSordLine = this.GetDefSordLine(args, true, e);

                if (defSordLine != null)
                {
                    var defOlcSordLine = OlcSordLine.Load(defSordLine.Sordlineid);

                    if (defOlcSordLine != null)
                    {
                        (this.m_defLine["confdeldate"] as DatePickerbox)?.SetValue(defOlcSordLine.Confdeldate);
                        (this.m_defLine["confqty"] as Numberbox)?.SetValue(defOlcSordLine.Confqty);
                    }
                }
            }

            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, SordHead e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Sordid.HasValue ? OlcSordHead.Load(e.Sordid) : null) ?? OlcSordHead.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            foreach (var c in this.OlcControlsWebShop)
            {
                c.DataBind(olc, true);
            }
           
            olc.CustomerName = (this.m_common.EditGroup["customername"] as Textbox).GetStringValue();
            olc.CustomerCountry = (this.m_common.EditGroup["customercountry"] as Textbox).GetStringValue();
            olc.CustomerZipCode = (this.m_common.EditGroup["customerzipcode"] as Textbox).GetStringValue();
            olc.CustomerCity = (this.m_common.EditGroup["customercity"] as Textbox).GetStringValue();
            olc.CustomerAddress = (this.m_common.EditGroup["customeraddress"] as Textbox).GetStringValue();
            olc.CouponCode = (this.m_common.EditGroup["couponcode"] as Textbox).GetStringValue();

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
                olcSordLine.Confdeldate = (this.m_defLine["confdeldate"] as DatePickerbox)?.GetValue<DateTime>();
                olcSordLine.Confqty = (this.m_defLine["confqty"] as Numberbox)?.GetValue<decimal>();
                map.Add(olcSordLine);
            }

            map.Add(olc);

            return map;
        }

        protected virtual void SetTextBoxValue(string textBox, string newValue)
        {
            (m_common.EditGroup[textBox] as Textbox)?.SetValue(newValue);
        }
    }
}