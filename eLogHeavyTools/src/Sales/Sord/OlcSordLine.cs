using eProjectWeb.Framework.Xml;

namespace eLog.HeavyTools.Sales.Sord
{
    public partial class OlcSordLine
    {
        public override void SetDefaultValues()
        {
        }
        public OlcSordLine()
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

        public string OrignalSelPrc
        {
            get => this.XmlManipute.Get("OrignalSelPrc");
            set => this.XmlManipute.Set("OrignalSelPrc", value);
        }

        public string OrignalTotprc
        {
            get => this.XmlManipute.Get("OrignalTotprc");
            set => this.XmlManipute.Set("OrignalTotprc", value);
        }

        public string SelPrc
        {
            get => this.XmlManipute.Get("SelPrc");
            set => this.XmlManipute.Set("SelPrc", value);
        }

        public string GrossPrc
        {
            get => this.XmlManipute.Get("GrossPrc");
            set => this.XmlManipute.Set("GrossPrc", value);
        }

        public string NetVal
        {
            get => this.XmlManipute.Get("NetVal");
            set => this.XmlManipute.Set("NetVal", value);
        }
        public string TaxVal
        {
            get => this.XmlManipute.Get("TaxVal");
            set => this.XmlManipute.Set("TaxVal", value);
        }
        public string TotVal
        {
            get => this.XmlManipute.Get("TotVal");
            set => this.XmlManipute.Set("TotVal", value);
        }
    }
}