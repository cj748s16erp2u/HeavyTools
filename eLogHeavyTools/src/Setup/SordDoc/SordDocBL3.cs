using eLog.Base.Setup.SordDoc;
using eLog.Base.Setup.StDoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.SordDoc
{
    internal class SordDocBL3 : SordDocBL
    {
        protected override bool PreSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            bool b = base.PreSave(objects, e);

            var en = (Base.Setup.SordDoc.SordDoc)objects.Default;

            if (e is OlcSordDoc)
            {
                OlcSordDoc ce = (OlcSordDoc)e;
                if (ce.State == eProjectWeb.Framework.Data.DataRowState.Added)
                    ce.Sorddocid = en.Sorddocid;
            }
            return b;
        }

        public override void Validate(eProjectWeb.Framework.BL.BLObjectMap objects)
        {
            base.Validate(objects);

            OlcSordDoc partner2 = objects[typeof(OlcSordDoc).Name] as OlcSordDoc;
            if (partner2 != null)
                eProjectWeb.Framework.RuleServer.Validate(objects, typeof(OlcSordDocRules));
        }
    }
}
