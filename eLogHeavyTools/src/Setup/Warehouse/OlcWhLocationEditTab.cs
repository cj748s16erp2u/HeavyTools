using eLog.Base.Masters.Item;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
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

        protected Control ctrlWhloccode;
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

            this.ctrlWhloccode = this.EditGroup1["whloccode"];

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

        protected bool CanRenderControls = true;
        protected override OlcWhLocation DefaultPageLoad(PageUpdateArgs args)
        {
            var isGenNext = args.ActionID == OlcWhLocationBL.GENNEXT_ACTIONID;
            if (isGenNext)
            {
                args.ActionID = eProjectWeb.Framework.BL.ActionID.New;
                CanRenderControls = false;
            }
            var loc = base.DefaultPageLoad(args);

            if (loc == null)
            {
                return loc;
            }

            if (args.ActionID == ActionID.New)
            {
                var whZoneId = this.GetWhZoneId(args);
                this.ctrlWhZoneId.SetValue(whZoneId); //betölti a zónát EditTabon az előző fülről
            }

            if (isGenNext)
            {
                cmdNew.Visible = false;
                args.ActionID = OlcWhLocationBL.GENNEXT_ACTIONID;
                if (args.LoadArgs != null)
                {
                    Key origKey = null;
                    if (args.LoadArgs is List<object>)
                    {
                        var list = (List<object>)args.LoadArgs;
                        var first = list.FirstOrDefault();
                        if (first != null && first is Dictionary<string, object>)
                        {
                            origKey = new eProjectWeb.Framework.Data.Key((Dictionary<string, object>)first);
                        }
                    }
                    if (args.LoadArgs is Dictionary<string, object>)
                    {
                        origKey = new eProjectWeb.Framework.Data.Key((Dictionary<string, object>)args.LoadArgs);
                    }
                    if (origKey != null)
                    {
                        loc = eLog.HeavyTools.Setup.Warehouse.OlcWhLocation.Load(origKey);
                        args.PageData[OlcWhLocationBL.GENNEXT_ORIGLOCCODE] = loc.Whloccode;
                        loc.Whlocid = null;
                        loc.Whloccode = null;
                        loc.State = eProjectWeb.Framework.Data.DataRowState.Added;
                        EditGroup1.DataBind(loc, false);
                    }
                }
                CanRenderControls = true;
                RenderControls(args);
            }
            if (CanRenderControls)
                RenderControls(args);
            ctrlWhloccode.SetMandatory(!isGenNext); // kötelező ha új helykódot írunk vagy ha módosítunk
            return loc;
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

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, OlcWhLocation e)
        {
            var isGenNext = args.ActionID == OlcWhLocationBL.GENNEXT_ACTIONID;
            if (isGenNext)
            {
                args.ActionID = eProjectWeb.Framework.BL.ActionID.New;
            }

            BLObjectMap map = base.SaveControlsToBLObjectMap(args, e);
            OlcWhLocation loc = (OlcWhLocation)e;

            if (isGenNext)
            {
                if (eProjectWeb.Framework.Data.StringN.IsNullOrEmpty(loc.Whloccode))
                {
                    loc.Whloccode = OlcWhLocationBL.CreateNextLoccode((string)args.PageData[OlcWhLocationBL.GENNEXT_ORIGLOCCODE], 1);
                    //megnöveli a helykódot 1-el, ha az F9 | Következőre kattintunk
                }
                loc.Whlocid = null; //whlocid null-ra van állítva,
                                    //mert az a whlocid ami ebben volt tárolva már létezik az adatbázisban
                loc.State = eProjectWeb.Framework.Data.DataRowState.Added;
            }

            return map;
        }

        protected override OlcWhLocation GetCurrentEntity(PageUpdateArgs args, out OlcWhLocationBL bl)
        {
            var isGenNext = args.ActionID == OlcWhLocationBL.GENNEXT_ACTIONID;
            if (isGenNext)
            {
                args.ActionID = eProjectWeb.Framework.BL.ActionID.New;
            }
            var loc = base.GetCurrentEntity(args, out bl);
            if (isGenNext)
            {
                args.ActionID = OlcWhLocationBL.GENNEXT_ACTIONID;
            }
            return loc;
        }
    }
}
