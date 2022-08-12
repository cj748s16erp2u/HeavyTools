using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

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

                    SetTextBoxValue("Sinv_Name", olc.Sinv_Name);
                    SetTextBoxValue("Sinv_countryid", olc.Sinv_countryid);
                    SetTextBoxValue("Sinv_postcode", olc.Sinv_postcode);
                    SetTextBoxValue("Sinv_city", olc.Sinv_city);
                    SetTextBoxValue("Sinv_building", olc.Sinv_building);
                    SetTextBoxValue("Sinv_district", olc.Sinv_district);
                    SetTextBoxValue("Sinv_door", olc.Sinv_door);
                    SetTextBoxValue("Sinv_hnum", olc.Sinv_hnum);
                    SetTextBoxValue("Sinv_floor", olc.Sinv_floor);
                    SetTextBoxValue("Sinv_place", olc.Sinv_place);
                    SetTextBoxValue("Sinv_placetype", olc.Sinv_placetype);
                    SetTextBoxValue("Sinv_stairway", olc.Sinv_stairway);
                    SetTextBoxValue("Shipping_name", olc.Shipping_name);
                    SetTextBoxValue("Shipping_countryid", olc.Shipping_countryid);
                    SetTextBoxValue("Shipping_postcode", olc.Shipping_postcode);
                    SetTextBoxValue("Shipping_city", olc.Shipping_city);
                    SetTextBoxValue("Shipping_building", olc.Shipping_building);
                    SetTextBoxValue("Shipping_district", olc.Shipping_district);
                    SetTextBoxValue("Shipping_door", olc.Shipping_door);
                    SetTextBoxValue("Shipping_hnum", olc.Shipping_hnum);
                    SetTextBoxValue("Shipping_floor", olc.Shipping_floor);
                    SetTextBoxValue("Shipping_place", olc.Shipping_place);
                    SetTextBoxValue("Shipping_placetype", olc.Shipping_placetype);
                    SetTextBoxValue("Shipping_stairway", olc.Shipping_stairway);
                    SetTextBoxValue("Phone", olc.Phone);
                    SetTextBoxValue("Email", olc.Email);
                    SetTextBoxValue("LoyaltyCardNo", olc.LoyaltyCardNo);
                    SetTextBoxValue("ShippinPrc", olc.ShippinPrc);
                    SetTextBoxValue("Paymenttransaciondata", olc.Paymenttransaciondata);
                    SetTextBoxValue("Netgopartnid", olc.Netgopartnid);
                    SetTextBoxValue("Pppid", olc.Pppid);
                    SetTextBoxValue("Glsid", olc.Glsid);
                    SetTextBoxValue("Foxpostid", olc.Foxpostid);
                    SetTextBoxValue("CentralRetailType", olc.CentralRetailType);
                    SetTextBoxValue("Exchangepackagesnumber", olc.Exchangepackagesnumber);
                    SetTextBoxValue("ShippingId", olc.ShippingId);
                    SetTextBoxValue("PaymentId", olc.PaymentId);
                    SetTextBoxValue("Coupons", olc.Coupons);
                    SetTextBoxValue("GiftCardLogId", olc.GiftCardLogId);

                }

                SordLine defSordLine = GetDefSordLine(args, true, e);

                if (defSordLine != null)
                {
                    var defOlcSordLine = OlcSordLine.Load(defSordLine.Sordlineid);

                    if (defOlcSordLine != null)
                    {
                        (m_defLine["confdeldate"] as DatePickerbox)?.SetValue(defOlcSordLine.Confdeldate);
                        (m_defLine["confqty"] as Numberbox)?.SetValue(defOlcSordLine.Confqty);

                        SetTextBoxValue("SordlineWebShopEditGroup", "OrigSelVal", defOlcSordLine.OrigSelVal);
                        SetTextBoxValue("SordlineWebShopEditGroup", "SelPrc", defOlcSordLine.SelPrc);
                        SetTextBoxValue("SordlineWebShopEditGroup", "SelVal", defOlcSordLine.SelVal);
                        SetTextBoxValue("SordlineWebShopEditGroup", "SetTotPrc", defOlcSordLine.SetTotPrc);
                        SetTextBoxValue("SordlineWebShopEditGroup", "SelTotVal", defOlcSordLine.SelTotVal);


                    }
                }
            }

            return e;
        }

        private void SetTextBoxValue(string grp, string textBox, string newValue)
        {
            ((FindRenderable<LayoutTable>(grp))[textBox] as Textbox)?.SetValue(newValue);
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, SordHead e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Addrid.HasValue ? OlcSordHead.Load(e.Addrid) : null) ?? OlcSordHead.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            olc.Sinv_Name = (this.EditGroup1["Sinv_Name"] as Textbox).GetStringValue();
            olc.Sinv_countryid = (this.EditGroup1["Sinv_countryid"] as Textbox).GetStringValue();
            olc.Sinv_postcode = (this.EditGroup1["Sinv_postcode"] as Textbox).GetStringValue();
            olc.Sinv_city = (this.EditGroup1["Sinv_city"] as Textbox).GetStringValue();
            olc.Sinv_building = (this.EditGroup1["Sinv_building"] as Textbox).GetStringValue();
            olc.Sinv_district = (this.EditGroup1["Sinv_district"] as Textbox).GetStringValue();
            olc.Sinv_door = (this.EditGroup1["Sinv_door"] as Textbox).GetStringValue();
            olc.Sinv_hnum = (this.EditGroup1["Sinv_hnum"] as Textbox).GetStringValue();
            olc.Sinv_floor = (this.EditGroup1["Sinv_floor"] as Textbox).GetStringValue();
            olc.Sinv_place = (this.EditGroup1["Sinv_place"] as Textbox).GetStringValue();
            olc.Sinv_placetype = (this.EditGroup1["Sinv_placetype"] as Textbox).GetStringValue();
            olc.Sinv_stairway = (this.EditGroup1["Sinv_stairway"] as Textbox).GetStringValue();
            olc.Shipping_name = (this.EditGroup1["Shipping_name"] as Textbox).GetStringValue();
            olc.Shipping_countryid = (this.EditGroup1["Shipping_countryid"] as Textbox).GetStringValue();
            olc.Shipping_postcode = (this.EditGroup1["Shipping_postcode"] as Textbox).GetStringValue();
            olc.Shipping_city = (this.EditGroup1["Shipping_city"] as Textbox).GetStringValue();
            olc.Shipping_building = (this.EditGroup1["Shipping_building"] as Textbox).GetStringValue();
            olc.Shipping_district = (this.EditGroup1["Shipping_district"] as Textbox).GetStringValue();
            olc.Shipping_door = (this.EditGroup1["Shipping_door"] as Textbox).GetStringValue();
            olc.Shipping_hnum = (this.EditGroup1["Shipping_hnum"] as Textbox).GetStringValue();
            olc.Shipping_floor = (this.EditGroup1["Shipping_floor"] as Textbox).GetStringValue();
            olc.Shipping_place = (this.EditGroup1["Shipping_place"] as Textbox).GetStringValue();
            olc.Shipping_placetype = (this.EditGroup1["Shipping_placetype"] as Textbox).GetStringValue();
            olc.Shipping_stairway = (this.EditGroup1["Shipping_stairway"] as Textbox).GetStringValue();
            olc.Phone = (this.EditGroup1["Phone"] as Textbox).GetStringValue();
            olc.Email = (this.EditGroup1["Email"] as Textbox).GetStringValue();
            olc.LoyaltyCardNo = (this.EditGroup1["LoyaltyCardNo"] as Textbox).GetStringValue();
            olc.ShippinPrc = (this.EditGroup1["ShippinPrc"] as Textbox).GetStringValue();
            olc.Paymenttransaciondata = (this.EditGroup1["Paymenttransaciondata"] as Textbox).GetStringValue();
            olc.Netgopartnid = (this.EditGroup1["Netgopartnid"] as Textbox).GetStringValue();
            olc.Pppid = (this.EditGroup1["Pppid"] as Textbox).GetStringValue();
            olc.Glsid = (this.EditGroup1["Glsid"] as Textbox).GetStringValue();
            olc.Foxpostid = (this.EditGroup1["Foxpostid"] as Textbox).GetStringValue();
            olc.CentralRetailType = (this.EditGroup1["CentralRetailType"] as Textbox).GetStringValue();
            olc.Exchangepackagesnumber = (this.EditGroup1["Exchangepackagesnumber"] as Textbox).GetStringValue();
            olc.ShippingId = (this.EditGroup1["ShippingId"] as Textbox).GetStringValue();
            olc.PaymentId = (this.EditGroup1["PaymentId"] as Textbox).GetStringValue();
            olc.Coupons = (this.EditGroup1["Coupons"] as Textbox).GetStringValue();
            olc.GiftCardLogId = (this.EditGroup1["GiftCardLogId"] as Textbox).GetStringValue();
 
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

                olcSordLine.OrigSelVal = GetTextBoxValue("SordlineWebShopEditGroup", "OrigSelVal");
                olcSordLine.SelPrc = GetTextBoxValue("SordlineWebShopEditGroup", "SelPrc");
                olcSordLine.SelVal = GetTextBoxValue("SordlineWebShopEditGroup", "SelVal");
                olcSordLine.SetTotPrc = GetTextBoxValue("SordlineWebShopEditGroup", "SetTotPrc");
                olcSordLine.SelTotVal = GetTextBoxValue("SordlineWebShopEditGroup", "SelTotVal");

                map.Add(olcSordLine);
            }


            map.Add(olc);

            return map;
        }

        protected virtual void SetTextBoxValue(string textBox, string newValue)
        {
            (this.EditGroup1[textBox] as Textbox)?.SetValue(newValue);
        }

        private string GetTextBoxValue(string grp, string textBox)
        {
            var o = ((FindRenderable<LayoutTable>(grp))[textBox] as Textbox)?.Value;
            if (o == null)
            {
                return "";
            }
            return o.ToString();
        }
    }
}