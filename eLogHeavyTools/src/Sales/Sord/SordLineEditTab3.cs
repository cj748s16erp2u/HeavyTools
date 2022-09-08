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
    public class SordLineEditTab3 : SordLineEditTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(SordLineEditTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public string XmlObjectName => baseType.Name;
        #endregion

        protected virtual IEnumerable<Control> OlcControls => this.EditGroup1?.ControlArray
            .Where(c => c.CustomData == "olc");

        protected override SordLine DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID != ActionID.New && e != null)
            {
                var olc = OlcSordLine.Load(e.Sordlineid);
                if (olc != null)
                {
                    foreach (var c in this.OlcControls)
                    {
                        c.DataBind(olc, false);
                    }

                    SetTextBoxValue("SordlineWebShopEditGroup", "OrignalSelPrc", olc.OrignalSelPrc);
                    SetTextBoxValue("SordlineWebShopEditGroup", "OrignalTotprc", olc.OrignalTotprc);
                    SetTextBoxValue("SordlineWebShopEditGroup", "SelPrc", olc.SelPrc);
                    SetTextBoxValue("SordlineWebShopEditGroup", "GrossPrc", olc.GrossPrc);
                    SetTextBoxValue("SordlineWebShopEditGroup", "NetVal", olc.NetVal);
                    SetTextBoxValue("SordlineWebShopEditGroup", "TaxVal", olc.TaxVal);
                    SetTextBoxValue("SordlineWebShopEditGroup", "TotVal", olc.TotVal);

                }
            }

            return e;
        }

        private void SetTextBoxValue(string grp, string textBox, string newValue)
        {
            ((FindRenderable<LayoutTable>(grp))[textBox] as Textbox)?.SetValue(newValue);
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, SordLine e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Sordlineid.HasValue ? OlcSordLine.Load(e.Sordlineid) : null) ?? OlcSordLine.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            olc.OrignalSelPrc = GetTextBoxValue("SordlineWebShopEditGroup", "OrignalSelPrc");
            olc.OrignalTotprc = GetTextBoxValue("SordlineWebShopEditGroup", "OrignalTotprc");
            olc.SelPrc = GetTextBoxValue("SordlineWebShopEditGroup", "SelPrc");
            olc.GrossPrc = GetTextBoxValue("SordlineWebShopEditGroup", "GrossPrc");
            olc.NetVal = GetTextBoxValue("SordlineWebShopEditGroup", "NetVal");
            olc.TaxVal = GetTextBoxValue("SordlineWebShopEditGroup", "TaxVal");
            olc.TotVal = GetTextBoxValue("SordlineWebShopEditGroup", "TotVal");

            map.Add(olc);

            return map;
        }

        private string GetTextBoxValue(string grp, string textBox)
        {
            var o= ((FindRenderable<LayoutTable>(grp))[textBox] as Textbox)?.Value;
            if (o == null)
            {
                return "";
            }
            return o.ToString();
        }

    }
}