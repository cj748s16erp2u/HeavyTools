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

                    SetTextBoxValue("SordlineWebShopEditGroup", "OrigSelVal", olc.OrigSelVal);
                    SetTextBoxValue("SordlineWebShopEditGroup", "SelPrc", olc.SelPrc);
                    SetTextBoxValue("SordlineWebShopEditGroup", "SelVal", olc.SelVal);
                    SetTextBoxValue("SordlineWebShopEditGroup", "SetTotPrc", olc.SetTotPrc);
                    SetTextBoxValue("SordlineWebShopEditGroup", "SelTotVal", olc.SelTotVal);

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

            olc.OrigSelVal = GetTextBoxValue("SordlineWebShopEditGroup", "OrigSelVal");
            olc.SelPrc = GetTextBoxValue("SordlineWebShopEditGroup", "SelPrc");
            olc.SelVal = GetTextBoxValue("SordlineWebShopEditGroup", "SelVal");
            olc.SetTotPrc = GetTextBoxValue("SordlineWebShopEditGroup", "SetTotPrc");
            olc.SelTotVal = GetTextBoxValue("SordlineWebShopEditGroup", "SelTotVal");

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