using System;
using System.Collections.Generic;
using System.Linq;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Masters.Partner
{
    public class EmployeeEditTab3 : EmployeeEditTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(EmployeeEditTab);

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

        public  string XmlObjectName => baseType.Name;
        #endregion

        protected override Employee DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (args.ActionID != ActionID.New)
            {
                var olc = OlcEmployee.Load(e.Empid);
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

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, Employee e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            var olc = (e.Empid.HasValue ? OlcEmployee.Load(e.Empid) : null) ?? OlcEmployee.CreateNew();
            foreach (var c in this.OlcControls)
            {
                c.DataBind(olc, true);
            }

            map.Add(olc);

            return map;
        }
    }
}