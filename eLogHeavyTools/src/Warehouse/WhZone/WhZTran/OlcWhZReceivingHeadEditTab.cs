using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingHeadEditTab : OlcWhZTranHeadEditTab<OlcWhZReceivingHeadRules, OlcWhZReceivingHeadBL>
    {
        protected OlcWhZReceivingHeadEditTab()
        {

        }

        public static OlcWhZReceivingHeadEditTab New(DefaultPageSetup setup)
        {
            return New<OlcWhZReceivingHeadEditTab>(setup);
        }

        protected Control ctrFromWhZId;
        protected Control ctrWhId;
        protected Control ctrlDocNum;
        protected Control ctrlSordId;
        protected Control ctrlPordId;
        protected Control ctrlTaskId;
        protected Control ctrlGen;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrFromWhZId = this.EditGroup1["fromwhzid"];
            this.ctrWhId = this.EditGroup1["whid"];
            this.ctrlDocNum = this.EditGroup1["docnum"];
            this.ctrlSordId = this.EditGroup1["sordid"];
            this.ctrlPordId = this.EditGroup1["pordid"];
            this.ctrlTaskId = this.EditGroup1["taskid"];
            this.ctrlGen = this.EditGroup1["gen"];
        }

        protected override OlcWhZTranHead DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (e == null)
            {
                return null;
            }

            if (args.ActionID == ActionID.New)
            {
                ctrFromWhZId.SetDisabled(true);
                ctrWhId.SetDisabled(true);
                ctrlDocNum.SetDisabled(true);
                ctrlSordId.SetDisabled(true);
                ctrlPordId.SetDisabled(true);
                ctrlTaskId.SetDisabled(true);
                ctrlGen.SetDisabled(true);
            }
            if (args.ActionID == ActionID.Modify || args.ActionID == ActionID.View)
            {
                OlcWhZTranHead result = DefaultPageLoad_LoadEntity(args);
                if (args.ActionID == ActionID.View)
                {
                    DisableControls();
                }

                return result;
            }

            return e;
        }

        protected override OlcWhZTranHead DefaultPageLoad_LoadEntity(PageUpdateArgs args)
        {
            return this.DefaultPageLoad_LoadEntity(args);
        }
    }
}
