using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Purchase.Pord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Purchase.Pord
{
    public class PordHeadEditTab3 : PordHeadEditTab
    {
        protected static Type baseType = typeof(PordHeadEditTab);

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

        protected override PordHead DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID != ActionID.New && e != null)
            {
                var olc = OlcPordHead.Load(e.Pordid);
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

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, PordHead e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Pordid.HasValue ? OlcPordHead.Load(e.Pordid) : null) ?? OlcPordHead.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            map.Add(olc);

            return map;
        }
    }
}