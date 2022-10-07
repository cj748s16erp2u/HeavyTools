using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    public class OlcWhLocPrioEditTab : EditTabTemplate1<OlcWhLocPrio, OlcWhLocPrioRules, OlcWhLocPrioBL>
    {
        public static OlcWhLocPrioEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocPrioEditTab>();
            t.Initialize(nameof(OlcWhLocPrio), setup);
            return t;
        }

        protected Control ctrlRefillLimit;
        protected Control ctrlPrioType;

        protected override void CreateBase()
        {
            base.CreateBase();
            this.ctrlRefillLimit = this.EditGroup1["refilllimit"];
            this.ctrlPrioType = this.EditGroup1["whpriotype"];
            if (ctrlPrioType != null)
            {
                ctrlPrioType.SetOnChanged(new ControlEvent(ctrlPrioTypeOnChanged));
            }
        }

        private void ctrlPrioTypeOnChanged(PageUpdateArgs args)
        {
            if (ConvertUtils.ToInt32(ctrlPrioType.Value) == (int)OlcWhLocPrio_PrioType.Primary)
            {
                ctrlRefillLimit.SetMandatory(true);
            }
            else
            {
                ctrlRefillLimit.SetMandatory(false);
            }
        }

        protected override OlcWhLocPrio DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);
            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, OlcWhLocPrio e)
        {
            return base.SaveControlsToBLObjectMap(args, e);
        }

    }
}
