using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;

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
                var olc = OlcSordLine.Load(e.Sordid);
                if (olc != null)
                {
                    foreach (var c in this.OlcControls)
                    {
                        c.DataBind(olc, false);
                    }
                }
            }

            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, SordLine e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Sordlineid.HasValue ? OlcSordLine.Load(e.Sordlineid) : null) ?? OlcSordLine.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            map.Add(olc);

            return map;
        }
    }
}