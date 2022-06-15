using System;
using System.Collections.Generic;
using System.Linq;
using CodaInt.Base.Masters.Partner;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnAddrEditTab3 : PartnAddrEditTab2, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(PartnAddrEditTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        protected virtual IEnumerable<Control> OlcControls => this.EditGroup1?.ControlArray
            .Where(c => c.CustomData == "olc");

        public new string XmlObjectName => baseType.Name;
        #endregion

        protected override PartnAddr DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID != ActionID.New)
            {
                var olc = OlcPartnAddr.Load(e.Addrid);
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

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, PartnAddr e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Addrid.HasValue ? OlcPartnAddr.Load(e.Addrid) : null) ?? OlcPartnAddr.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            map.Add(olc);

            return map;
        }
    }
}