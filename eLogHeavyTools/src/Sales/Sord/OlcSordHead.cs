using System.Collections.Generic;
using eProjectWeb.Framework.Xml;

namespace eLog.HeavyTools.Sales.Sord
{
    public partial class OlcSordHead
    {
        public override void SetDefaultValues() { }

        public OlcSordHead()
        {
            this.XmlManipute = new XmlManiputeStr(() => Data, (value) => this.Data = value, "sordhead");
        }

        /// <summary>
        /// pl.: var s = XmlData.Get("docdateatcontinuous");
        /// </summary>
        public eProjectWeb.Framework.Xml.XmlManiputeStr XmlManipute
        {
            get;
            private set;
        }

        public string Sinv_Name
        {
            get => this.XmlManipute.Get("Sinv_Name");
            set => this.XmlManipute.Set("Sinv_Name", value);
        }
        public string Sinv_countryid
        {
            get => this.XmlManipute.Get("Sinv_countryid");
            set => this.XmlManipute.Set("Sinv_countryid", value);
        }
        public string Sinv_postcode
        {
            get => this.XmlManipute.Get("Sinv_postcode");
            set => this.XmlManipute.Set("Sinv_postcode", value);
        }
        public string Sinv_city
        {
            get => this.XmlManipute.Get("Sinv_city");
            set => this.XmlManipute.Set("Sinv_city", value);
        }
        public string Sinv_building
        {
            get => this.XmlManipute.Get("Sinv_building");
            set => this.XmlManipute.Set("Sinv_building", value);
        }
        public string Sinv_district
        {
            get => this.XmlManipute.Get("Sinv_district");
            set => this.XmlManipute.Set("Sinv_district", value);
        }
        public string Sinv_door
        {
            get => this.XmlManipute.Get("Sinv_door");
            set => this.XmlManipute.Set("Sinv_door", value);
        }
        public string Sinv_hnum
        {
            get => this.XmlManipute.Get("Sinv_hnum");
            set => this.XmlManipute.Set("Sinv_hnum", value);
        }
        public string Sinv_floor
        {
            get => this.XmlManipute.Get("Sinv_floor");
            set => this.XmlManipute.Set("Sinv_floor", value);
        }
        public string Sinv_place
        {
            get => this.XmlManipute.Get("Sinv_place");
            set => this.XmlManipute.Set("Sinv_place", value);
        }
        public string Sinv_placetype
        {
            get => this.XmlManipute.Get("Sinv_placetype");
            set => this.XmlManipute.Set("Sinv_placetype", value);
        }
        public string Sinv_stairway
        {
            get => this.XmlManipute.Get("Sinv_stairway");
            set => this.XmlManipute.Set("Sinv_stairway", value);
        }
        public string Shipping_name
        {
            get => this.XmlManipute.Get("Shipping_name");
            set => this.XmlManipute.Set("Shipping_name", value);
        }
        public string Shipping_countryid
        {
            get => this.XmlManipute.Get("Shipping_countryid");
            set => this.XmlManipute.Set("Shipping_countryid", value);
        }
        public string Shipping_postcode
        {
            get => this.XmlManipute.Get("Shipping_postcode");
            set => this.XmlManipute.Set("Shipping_postcode", value);
        }
        public string Shipping_city
        {
            get => this.XmlManipute.Get("Shipping_city");
            set => this.XmlManipute.Set("Shipping_city", value);
        }
        public string Shipping_building
        {
            get => this.XmlManipute.Get("Shipping_building");
            set => this.XmlManipute.Set("Shipping_building", value);
        }
        public string Shipping_district
        {
            get => this.XmlManipute.Get("Shipping_district");
            set => this.XmlManipute.Set("Shipping_district", value);
        }
        public string Shipping_door
        {
            get => this.XmlManipute.Get("Shipping_door");
            set => this.XmlManipute.Set("Shipping_door", value);
        }
        public string Shipping_hnum
        {
            get => this.XmlManipute.Get("Shipping_hnum");
            set => this.XmlManipute.Set("Shipping_hnum", value);
        }
        public string Shipping_floor
        {
            get => this.XmlManipute.Get("Shipping_floor");
            set => this.XmlManipute.Set("Shipping_floor", value);
        }
        public string Shipping_place
        {
            get => this.XmlManipute.Get("Shipping_place");
            set => this.XmlManipute.Set("Shipping_place", value);
        }
        public string Shipping_placetype
        {
            get => this.XmlManipute.Get("Shipping_placetype");
            set => this.XmlManipute.Set("Shipping_placetype", value);
        }
        public string Shipping_stairway
        {
            get => this.XmlManipute.Get("Shipping_stairway");
            set => this.XmlManipute.Set("Shipping_stairway", value);
        }
        public string Phone
        {
            get => this.XmlManipute.Get("Phone");
            set => this.XmlManipute.Set("Phone", value);
        }
        public string Email
        {
            get => this.XmlManipute.Get("Email");
            set => this.XmlManipute.Set("Email", value);
        }
        public string LoyaltyCardNo
        {
            get => this.XmlManipute.Get("LoyaltyCardNo");
            set => this.XmlManipute.Set("LoyaltyCardNo", value);
        }
        public string ShippinPrc
        {
            get => this.XmlManipute.Get("ShippinPrc");
            set => this.XmlManipute.Set("ShippinPrc", value);
        }
        public string Paymenttransaciondata
        {
            get => this.XmlManipute.Get("Paymenttransaciondata");
            set => this.XmlManipute.Set("Paymenttransaciondata", value);
        }
        public string Netgopartnid
        {
            get => this.XmlManipute.Get("Netgopartnid");
            set => this.XmlManipute.Set("Netgopartnid", value);
        }
        public string Pppid
        {
            get => this.XmlManipute.Get("Pppid");
            set => this.XmlManipute.Set("Pppid", value);
        }
        public string Glsid
        {
            get => this.XmlManipute.Get("Glsid");
            set => this.XmlManipute.Set("Glsid", value);
        }
        public string Foxpostid
        {
            get => this.XmlManipute.Get("Foxpostid");
            set => this.XmlManipute.Set("Foxpostid", value);
        }
        public string CentralRetailType
        {
            get => this.XmlManipute.Get("CentralRetailType");
            set => this.XmlManipute.Set("CentralRetailType", value);
        }
        public string Exchangepackagesnumber
        {
            get => this.XmlManipute.Get("Exchangepackagesnumber");
            set => this.XmlManipute.Set("Exchangepackagesnumber", value);
        }
        public string ShippingId
        {
            get => this.XmlManipute.Get("ShippingId");
            set => this.XmlManipute.Set("ShippingId", value);
        }
        public string PaymentId
        {
            get => this.XmlManipute.Get("PaymentId");
            set => this.XmlManipute.Set("PaymentId", value);
        }
        public string Coupons
        {
            get => this.XmlManipute.Get("Coupons");
            set => this.XmlManipute.Set("Coupons", value);
        }
        public string GiftCardLogId
        {
            get => this.XmlManipute.Get("GiftCardLogId");
            set => this.XmlManipute.Set("GiftCardLogId", value);
        }

    }
}