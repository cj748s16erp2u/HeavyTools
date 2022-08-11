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

        public string OrigSelVal
        {
            get => this.XmlManipute.Get("OrigSelVal");
            set => this.XmlManipute.Set("OrigSelVal", value);
        }

        public string SelPrc
        {
            get => this.XmlManipute.Get("SelPrc");
            set => this.XmlManipute.Set("SelPrc", value);
        }

        public string SetTotPrc
        {
            get => this.XmlManipute.Get("SetTotPrc");
            set => this.XmlManipute.Set("SetTotPrc", value);
        }

        public string SelVal
        {
            get => this.XmlManipute.Get("SelVal");
            set => this.XmlManipute.Set("SelVal", value);
        }

        public string SelTotVal
        {
            get => this.XmlManipute.Get("SelTotVal");
            set => this.XmlManipute.Set("SelTotVal", value);
        }
    }
}