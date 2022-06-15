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
    public class OlcWhLocationEditTab : EditTabTemplate1<OlcWhLocation, OlcWhLocationRules, OlcWhLocationBL>
    {
        public static OlcWhLocationEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocationEditTab>();
            t.Initialize(nameof(OlcWhLocation), setup);
            return t;
        }

        protected OlcWhLocationEditTab() { }

        protected Control ctrlWhZoneId;

        protected Control ctrlName;

        protected Control ctrlLocType;
        protected Control ctrlMovLocType;

        protected Control ctrlVolume;
        protected Control ctrlOverfillThreshold;
        protected Control ctrlIsMulti;
        protected Control ctrlCrawlOrder;

        protected Control ctrlCapacity;
        protected Control ctrlCapUnitId;

        protected override void CreateBase()
        {
            this.ParentEntityKey = Consts.DetailEntityKey;

            base.CreateBase();

            this.ctrlWhZoneId = this.EditGroup1["whzoneid"];

            this.ctrlName = this.EditGroup1["name"];

            this.ctrlLocType = this.EditGroup1["loctype"];
            this.ctrlLocType.SetOnChangedWhenExists(this.LocType_OnChanged);
            this.ctrlMovLocType = this.EditGroup1["movloctype"];

            this.ctrlVolume = this.EditGroup1["volume"];
            this.ctrlOverfillThreshold = this.EditGroup1["overfillthreshold"];
            this.ctrlIsMulti = this.EditGroup1["ismulti"];
            this.ctrlCrawlOrder = this.EditGroup1["crawlorder"];

            this.ctrlCapacity = this.EditGroup1["capacity"];
            this.ctrlCapUnitId = this.EditGroup1["capunitid"];
        }

        protected override OlcWhLocation DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (e == null)
            {
                return e;
            }

            if (args.ActionID == ActionID.New)
            {
                var whZoneId = this.GetWhZoneId(args);
                this.ctrlWhZoneId.SetValue(whZoneId);
            }

            return e;
        }

        private int? GetWhZoneId(PageUpdateArgs args)
        {
            object o;
            if (args.PageData.TryGetValue(Consts.RootEntityKey, out o) && o is IDictionary<string, object>)
            {
                var dict = (IDictionary<string, object>)o;
                if (dict.TryGetValue(OlcWhLocation.FieldWhzoneid.Name, out o) && (o is int? || o is long?))
                {
                    return ConvertUtils.ToInt32(o);
                }
            }

            return null;
        }

        private void LocType_OnChanged(PageUpdateArgs args)
        {
            var locType = (OlcWhLocation_LocType?)this.ctrlLocType.GetValue<int>();
            switch (locType)
            {
                case OlcWhLocation_LocType.Normal:
                    this.SetupNormalLocation(args);
                    break;
                case OlcWhLocation_LocType.Moving:
                    this.SetupMovingLocation(args);
                    break;
                case OlcWhLocation_LocType.TransferArea:
                    this.SetupTransferAreaLocation(args);
                    break;
            }
        }

        private void SetupNormalLocation(PageUpdateArgs args)
        {
            var whZoneId = this.GetWhZoneId(args);
            var whZone = OlcWhZone.Load(whZoneId);

            this.SetupControl(this.ctrlName, false);
            this.SetupControl(this.ctrlMovLocType, true);
            this.SetupControl(this.ctrlVolume, false, whZone?.Locdefvolume == null);
            this.SetupControl(this.ctrlOverfillThreshold, false, whZone?.Locdefoverfillthreshold == null);
            this.SetupControl(this.ctrlIsMulti, false, whZone?.Locdefismulti == null);
            this.SetupControl(this.ctrlCrawlOrder, false, true);
            this.SetupControl(this.ctrlCapacity, true);
            this.SetupControl(this.ctrlCapUnitId, true);
        }

        private void SetupMovingLocation(PageUpdateArgs args)
        {
            this.SetupControl(this.ctrlName, false, true);
            this.SetupControl(this.ctrlMovLocType, false, true);
            this.SetupControl(this.ctrlVolume, true);
            this.SetupControl(this.ctrlOverfillThreshold, false, true);
            this.SetupControl(this.ctrlIsMulti, true);
            this.SetupControl(this.ctrlCrawlOrder, true);
            this.SetupControl(this.ctrlCapacity, false, true);
            this.SetupControl(this.ctrlCapUnitId, false, true);
        }

        private void SetupTransferAreaLocation(PageUpdateArgs args)
        {
            this.SetupControl(this.ctrlName, false, true);
            this.SetupControl(this.ctrlMovLocType, true);
            this.SetupControl(this.ctrlVolume, true);
            this.SetupControl(this.ctrlOverfillThreshold, true);
            this.SetupControl(this.ctrlIsMulti, true);
            this.SetupControl(this.ctrlCrawlOrder, true);
            this.SetupControl(this.ctrlCapacity, true);
            this.SetupControl(this.ctrlCapUnitId, true);
        }

        private void SetupControl(Control control, bool disabled, bool? mandatory = null)
        {
            if (disabled)
            {
                control.SetValue(null);
                control.SetDisabled(true);
                control.SetMandatory(false);
            }
            else
            {
                control.SetDisabled(false);
                control.SetMandatory(mandatory.GetValueOrDefault(false));
            }
        }
    }
}
