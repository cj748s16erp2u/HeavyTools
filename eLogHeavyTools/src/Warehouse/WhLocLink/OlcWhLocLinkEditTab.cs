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

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkEditTab : EditTabTemplate1<OlcWhLocLink, OlcWhLocLinkRules, OlcWhLocLinkBL>
    {
        public static OlcWhLocLinkEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<OlcWhLocLinkEditTab>();
            t.Initialize("OlcWhLocLink", setup);
            return t;
        }

        protected Control WhIdCtrl;
        protected Control WhZoneIdCtrl;
        protected Control WhLocIdCtrl;
        protected Control WhOverfillthreshold;

        protected OlcWhLocLinkEditTab() { }

        protected override void CreateBase()
        {
            base.CreateBase();

            this.WhIdCtrl = this.EditGroup1["whid"];
            this.WhZoneIdCtrl = this.EditGroup1["whzoneid"];
            this.WhLocIdCtrl = this.EditGroup1["whlocid"];
            this.WhOverfillthreshold = this.EditGroup1["overfillthreshold"];

            this.WhLocIdCtrl.SetOnChangedWhenExists(WhLocIdCtrl_Onchanged);
        }

        /// <summary>
        /// Javítás felületen egyes lehetőségek legyenek nem megváltoztathatóak.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>OlcWhLocLink</returns>
        protected override OlcWhLocLink DefaultPageLoad(PageUpdateArgs args)
        {
            var loclinkDis = base.DefaultPageLoad(args);

            if (args.ActionID == ActionID.Modify)
            {    
                WhIdCtrl.SetDisabled(true);       
                WhZoneIdCtrl.SetDisabled(true);
                WhLocIdCtrl.SetDisabled(true);
            }

            return loclinkDis;
        }

        /// <summary>
        /// Túltöltődési határ öröklődése új Helykód kapcsolás létrehozásakor. 
        /// Kapja meg az eredeti értéket, ha a felhasználó nem ad meg semmit.
        /// </summary>
        /// <param name="args"></param>
        private void WhLocIdCtrl_Onchanged(PageUpdateArgs args)
        {
            if(args.ActionID == ActionID.New)
            {
                var locid = WhLocIdCtrl.GetValue<int>();

                decimal? overfillthreshold = null;

                if (locid != null)
                {
                    var bl = OlcWhLocLinkBL.New();

                    overfillthreshold = bl.GetLocationThreshold(locid);
                }

                WhOverfillthreshold.SetValue(overfillthreshold);
            }          
        }
    }
}
