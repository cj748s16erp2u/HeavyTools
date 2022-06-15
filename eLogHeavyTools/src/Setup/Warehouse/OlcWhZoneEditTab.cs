using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.Warehouse
{
    public class OlcWhZoneEditTab : EditTabTemplate1<OlcWhZone, OlcWhZoneRules, OlcWhZoneBL>
    {
        public static OlcWhZoneEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhZoneEditTab>();
            t.Initialize(nameof(OlcWhZone), setup);
            return t;
        }

        protected OlcWhZoneEditTab() { }

        protected Control ctrlWhZoneCode;
        protected Control ctrlLocDefVolume;
        protected Control ctrlLocDefOverfillThreshold;
        protected Control ctrlLocDefIsMulti;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrlWhZoneCode = this.EditGroup1["whzonecode"];
            this.ctrlLocDefVolume = this.EditGroup1["locdefvolume"];
            this.ctrlLocDefOverfillThreshold = this.EditGroup1["locdefoverfillthreshold"];
            this.ctrlLocDefIsMulti = this.EditGroup1["locdefismulti"];
        }

        protected override OlcWhZone DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (e == null)
            {
                return e;
            }

            if (args.ActionID != ActionID.New)
            {
                this.ctrlWhZoneCode.SetDisabled(true);

                var bl = OlcWhZoneBL.New();
                if (bl.CheckHasLocationWOSpecValue(e.Whzoneid, OlcWhLocation.FieldVolume, OlcWhLocation_LocType.Normal))
                {
                    this.ctrlLocDefVolume.SetMandatory(true);
                }
                if (bl.CheckHasLocationWOSpecValue(e.Whzoneid, OlcWhLocation.FieldOverfillthreshold, OlcWhLocation_LocType.Normal))
                {
                    this.ctrlLocDefOverfillThreshold.SetMandatory(true);
                }
                if (bl.CheckHasLocationWOSpecValue(e.Whzoneid, OlcWhLocation.FieldIsmulti, OlcWhLocation_LocType.Normal))
                {
                    this.ctrlLocDefIsMulti.SetMandatory(true);
                }
            }

            return e;
        }
    }
}
