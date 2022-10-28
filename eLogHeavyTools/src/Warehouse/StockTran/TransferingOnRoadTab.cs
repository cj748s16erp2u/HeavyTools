using eLog.Base.Masters.Item;
using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingOnRoadTab : EditTabTemplate1 <StHead, StHeadRules, TransferingOnRoadBL>
    {
        protected Grid stLineList;
        protected Control ctrlFromWhId;
        protected Control ctrlToWhId;
        //private Button _changeWh;

        public TransferingOnRoadTab()
            : base()
        {
        }

        public static TransferingOnRoadTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup TransitSetup)
        {
            TransferingOnRoadTab t = eProjectWeb.Framework.ObjectFactory.New(typeof(TransferingOnRoadTab)) as TransferingOnRoadTab;
            t.Initialize("Transit", TransitSetup);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();

            stLineList = (Grid)FindRenderable("StLineGrid");
            stLineList.SearchID = TransferingOnRoadSearchProvider.ID;
            stLineList.SetColumns(SearchServer.Get(TransferingOnRoadSearchProvider.ID).GetColumns());
            stLineList.MergePageData = Consts.RootEntityKey;

            if (EditGroup1 != null)
            {
                ctrlFromWhId = EditGroup1["fromwhid"];
                ctrlToWhId = EditGroup1["towhid"];
            }

            OnPageActivate += this.TransferingOnRoadTab_OnPageLoad;
            /*
            _changeWh = (Button)FindRenderable("changeWh");
            _changeWh.SetOnClick(OnChangeWhClick);
            */
            //OnPageActivate += OnOnPageActivate;

        }

        private void TransferingOnRoadTab_OnPageLoad(PageUpdateArgs args)
        {
            args.AddPostExecCommand(new eProjectWeb.Framework.UI.Script.RefreshObjectStep(stLineList.ID));
             
        }

        /*
private void OnOnPageActivate(PageUpdateArgs args)
{
   var k = new Key(args.PageData["EditedPK"]) { { "delstat", 0 } };
   var pt = OlcPendingTransaction.Load(k);
   if (pt != null)
   {
       if (pt.Approvaldate.HasValue)
       {
           _changeWh.Visible = false;
           ctrlToWhId.Disabled = false;
       }
   }
}

private void OnChangeWhClick(PageUpdateArgs args)
{
   var k = new Key(args.PageData["EditedPK"]);
   args.AddExecCommand("gPageParams.RootEntityKey = " + k.ToJSON());
   var a = new eProjectWeb.Framework.UI.Actions.EditRecordCallbackDynPageAction(args.Control.ID, TransitChangeWhLicenseRequestPage.ID, eProjectWeb.Framework.UI.Actions.eEditRecordCallbackFlags.CheckForRootEntityKey);
   args.AddPostExecSimpleAction(a);
}*/

        protected override StHead DefaultPageLoad(PageUpdateArgs args)
        {
            Key k = new Key(args.PageData[Consts.RootEntityKey]);

            args.ActionID = eProjectWeb.Framework.BL.ActionID.Modify; // Ez ahhoz kell, hogy az eredeti raktarkozi adatait betoltse a mezokbe
            args.LoadArgs = new List<object>() { k }; // Modositasnal a key-t itt keresi az EditTabTemplate1

            var entity = base.DefaultPageLoad(args);


            OlcStHead olcStHead = OlcStHead.Load(k);
            if (olcStHead != null)
            {
                if (ctrlFromWhId != null && ctrlToWhId != null)
                {
                    ctrlFromWhId.Value = StringN.ConvertToString(ctrlToWhId.Value);
                    ctrlToWhId.Value = StringN.ConvertToString(olcStHead.Onroadtowhid.Value);
                }
            }

            return entity;
        }

        protected override Key SaveBeforeClose(PageUpdateArgs args)
        {
            args.ActionID = eProjectWeb.Framework.BL.ActionID.New; // A letrejovo raktarkozi fej entity-t ujkent fogjuk lementeni
            var k = base.SaveBeforeClose(args);
            if (k != null)
            {
                var bl = TransferingOnRoadBL.New();
                string msg;
                using (DB db = DB.GetConn(DB.Main, Transaction.Use))
                {

                    if (bl.StatChange((int)k["stid"], (int)StHeadStStatList.Values.Created, null, out msg) != 0)
                    {
                        throw new MessageException(msg);
                    }

                    db.Commit();
                }
            }

            return k;
        }

        protected override eProjectWeb.Framework.BL.BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, StHead stHead)
        {
            var map = base.SaveControlsToBLObjectMap(args, stHead);
            
            stHead.Origstid = stHead.Stid;
            stHead.Stid = null; // kitoroljuk az stid-t, mert az EditTabTemplate1 az eredeti raktarkozi azonositojat teszi bele, mi pedig ujat akarunk felvinni
            stHead.Gen = (int)TransferingOnRoadBL.OnRoadGenId;
            stHead.Stdocid = TransferingOnRoadBL.OnRoadStdocIdFrom;
            stHead.Stdate = DateTime.Today;

            if (map.SysParams.ListValues == null)
                map.SysParams.ListValues = new List<object>();
            map.SysParams.ListValues.Add(args.PageData[Consts.RootEntityKey]); // A map-be bementjuk az eredeti raktarkozi azonositojat is, mert a BL-ben errol kell a teteleket lemasolni az uj raktarkozi fejhez

            var oOlcSthead = OlcStHead.Load(stHead.Origstid);
            OlcStHead olcStHead = OlcStHead.Load(stHead.PK);

            if (olcStHead == null)
            {
                olcStHead = OlcStHead.CreateNew();
                /*olcStHead.Packtype = oOlcSthead.Packtype;
                olcStHead.Packpdid = oOlcSthead.Packpdid;*/
                //Itt az eredeti kell...
                //olcStHead.SetKey(stHead.PK);
            }
            //olcStHead.SetupPack(stHead.Fromwhid.Value, stHead.Sttype);

            EditGroup1.DataBind(olcStHead, true);
            map.Add(typeof(OlcStHead).Name, olcStHead);

            return map;
        }
    }
}
