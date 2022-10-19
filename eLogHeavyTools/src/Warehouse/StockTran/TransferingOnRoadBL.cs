using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingOnRoadBL : TransferingHeadBL3
    {
        public static new readonly string ID = typeof(TransferingOnRoadBL).FullName;

        protected override bool PreSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            bool b = base.PreSave(objects, e);

            eLog.Base.Warehouse.StockTran.StHead stHead = (eLog.Base.Warehouse.StockTran.StHead)objects.Default;

            if (e is OlcStHead)
            {
                OlcStHead olcStHead = (OlcStHead)e;
                if (olcStHead.State == eProjectWeb.Framework.Data.DataRowState.Added)
                {
                    /*if (stHead.Origstid.HasValue)
                    {
                        OlcStHead olcStHead2 = OlcStHead.Load(stHead.Origstid.Value);
                        if (olcStHead2 != null && olcStHead2.Cid.HasValue)
                        {
                            olcStHead.Cid = olcStHead2.Cid;
                            //Itt az eredeti kell
                            //olcStHead.SetupPack(stHead.Fromwhid.Value);
                            olcStHead.Packtype = olcStHead2.Packtype;
                            olcStHead.Packpdid = olcStHead2.Packpdid;
                        }
                    }*/

                    olcStHead.Stid = stHead.Stid;
                }
            }

            return b;
        }

        protected override void PostSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            base.PostSave(objects, e);

            List<eProjectWeb.Framework.Rules.ValidationError> validateErrors = new List<eProjectWeb.Framework.Rules.ValidationError>();

            if (e is StHead)
            {
                StHead stHead = (StHead)e;

                if (objects.SysParams.ListValues != null && objects.SysParams.ListValues.Count > 0)
                {
                    eProjectWeb.Framework.Data.Key origKey = new eProjectWeb.Framework.Data.Key(objects.SysParams.ListValues[0]);
                    int origStId = Convert.ToInt32(origKey["stid"]);

                    // Tetelek masolasa az eredeti raktarkozi mozgasrol
                    StLineBL lineBL = eLog.Base.Warehouse.Common.WarehouseSqlFunctions.GetLineBL(StDocType.Transfering);
                    eProjectWeb.Framework.ISearchProvider transitSearchProvider = eProjectWeb.Framework.SearchServer.Get(TransferingOnRoadSearchProvider.ID);
                    System.Data.DataTable origLines = null;
                    if (transitSearchProvider != null)
                    {
                        Dictionary<string, object> args = new Dictionary<string, object>() { { "stid", origStId } };
                        System.Data.DataSet ds = transitSearchProvider.SearchDataSet(args, false);
                        if (ds.Tables.Count > 0)
                            origLines = ds.Tables[0];
                    }

                    //StLines origLines = StLines.Load(origStId);
                    foreach (System.Data.DataRow origStLine in origLines.Rows)
                    {
                        eProjectWeb.Framework.BL.BLObjectMap map = lineBL.CreateBLObjects();

                        StLine stLine = (StLine)map.Default;

                        StLine oldStLine = StLine.Load(eProjectWeb.Framework.ConvertUtils.ToInt32(origStLine["stlineid"]));
                        if (oldStLine != null)
                        {
                            oldStLine.CopyTo(stLine);
                            stLine.Stid = null;
                            stLine.Stlineid = null;
                            stLine.Adddate = null;
                            stLine.Addusrid = null;
                            stLine.Sordlineid = null;
                        }

                        stLine.Stid = stHead.Stid;
                        //stLine.Itemid = eProjectWeb.Framework.ConvertUtils.ToInt32(origStLine["itemid"]);

                        decimal qty = eProjectWeb.Framework.ConvertUtils.ToDecimal(origStLine["movqty2"]).GetValueOrDefault(0M);

                        // szabadkészlet ellenőrzés
                        decimal freeQty = eLog.Base.Warehouse.Common.WarehouseSqlFunctions.StockFreeQty(stLine.Itemid.Value, stHead.Fromwhid.Value);

                        freeQty = Math.Max(0, freeQty);

                        if (qty > freeQty)
                        {
                            string itemCode = eLog.Base.Masters.Common.MastersSqlFunctions.ItemCode(stLine.Itemid.Value);

                            validateErrors.Add(new eProjectWeb.Framework.Rules.ValidationError(eProjectWeb.Framework.Lang.Translator.Translate("$rule_not_enought_freeqty", stHead.Fromwhid, itemCode, qty, freeQty)));
                            continue;
                        }

                        //lineBL.FillQty(stHead, stLine, qty);
                        stLine.Ordqty2 = qty;
                        stLine.Dispqty2 = qty;
                        stLine.Movqty2 = 0;
                        stLine.Costvalhome = 0;
                        stLine.Costprchome = 0;

                        stLine.Gen = (int)TransferingHeadBL3.OnRoadGenId;

                        OlcStLine olcStLine = OlcStLine.CreateNew();
                        olcStLine.Origstlineid = eProjectWeb.Framework.ConvertUtils.ToInt32(origStLine["stlineid"]);

                        OlcStHead olcStHead = OlcStHead.Load(stHead.Origstid);
                        /*if (olcStHead != null && olcStHead.Cid.HasValue)
                        {
                            olcStLine.Cid = olcStHead.Cid;
                        }*/
                        map.Add(typeof(OlcStLine).FullName, olcStLine);
                        lineBL.Save(map);
                    }

                    /* Dobozok másolása
                    var b = PackListBoxBL.New();
                    var bxes = OlcPackListBoxes.LoadNotTransfered(origStId);
                    foreach (OlcPackListBox oBox in bxes.AllRows)
                    {
                        var nb = OlcPackListBox.CreateNew();
                        nb.Stid = stHead.Stid;
                        nb.Refpbid = oBox.Pbid;
                        nb.Boxnum = oBox.Boxnum;
                        nb.Externalboxnum = oBox.Externalboxnum;
                        nb.Boxstat = 10;
                        var map = b.CreateBLObjects();
                        map.Default = nb;
                        b.Save(map);
                    } */
                }
            }

            if (validateErrors.Count != 0)
                throw new eProjectWeb.Framework.Rules.ValidateException(validateErrors);
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);
        }
    }
}
