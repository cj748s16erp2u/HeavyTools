using System.Collections.Generic;
using eProjectWeb.Framework.Xml;

namespace eLog.HeavyTools.Sales.Sord
{
    public partial class OlcSordHead
    {
        private static string CUSTOMERPREFIX = "customer_";
        private static string COUPONPREFIX = "coupon_";

        public override void SetDefaultValues()
        {
        }

        public OlcSordHead()
        {
            this.XmlManipute = new XmlManiputeStr(() => Data, (value) => this.Data = value, "sordhead");
        }

        /// <summary>
        /// pl.: var s = XmlData.Get("docdateatcontinuous");
        /// </summary>
        public eProjectWeb.Framework.Xml.XmlManiputeStr XmlManipute { get; private set; }

        public string CustomerName
        {
            get => this.XmlManipute.Get($"{CUSTOMERPREFIX}name");
            set => this.XmlManipute.Set($"{CUSTOMERPREFIX}name", value);
        }

        public string CustomerCountry
        {
            get => this.XmlManipute.Get($"{CUSTOMERPREFIX}country");
            set => this.XmlManipute.Set($"{CUSTOMERPREFIX}country", value);
        }

        public string CustomerZipCode
        {
            get => this.XmlManipute.Get($"{CUSTOMERPREFIX}zipcode");
            set => this.XmlManipute.Set($"{CUSTOMERPREFIX}zipcode", value);
        }

        public string CustomerCity
        {
            get => this.XmlManipute.Get($"{CUSTOMERPREFIX}city");
            set => this.XmlManipute.Set($"{CUSTOMERPREFIX}city", value);
        }

        public string CustomerAddress
        {
            get => this.XmlManipute.Get($"{CUSTOMERPREFIX}address");
            set => this.XmlManipute.Set($"{CUSTOMERPREFIX}address", value);
        }

        public string CouponCode
        {
            get => this.XmlManipute.Get<string>($"{COUPONPREFIX}code");
            set => this.XmlManipute.Set($"{COUPONPREFIX}code", value);
        }
    }
}