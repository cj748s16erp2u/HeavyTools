using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Reserve;
using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordSordBL : DefaultBL1<OlcTmpSordSord, TypedBaseRuleSet<OlcTmpSordSord>>
    {
        public static readonly string ID = typeof(SordSordBL).FullName;

        public static SordSordBL New()
        {
            return (SordSordBL)ObjectFactory.New(typeof(SordSordBL));
        }

        internal Guid CreateOlcTmpSordSord(int sordid)
        {
            Guid guid = Guid.NewGuid();

            using (DB dB = DB.GetConn(DB.Main, Transaction.Use))
            {
                var sql = $@"declare @t table (
	sordlineid int
)
insert into @t
select slt.sordlineid
  from ols_sordhead shf
  join ols_sordhead sht on sht.curid=shf.curid and sht.partnid=shf.partnid and sht.addrid=shf.addrid and isnull(sht.whid,'')=isnull(shf.whid,'') and sht.curid=shf.curid
  join ols_sorddoc sdt on sdt.sorddocid=sht.sorddocid and sdt.type=1
  join ols_sorddoc sdf on sdf.sorddocid=shf.sorddocid and sdf.type=2
  join ols_sordline slt on slt.sordid=sht.sordid
  left join olc_sordline clt on clt.sordlineid=slt.sordlineid
  outer apply (
		select count(0)  c
		  from ols_sordline sl2 
		 where sl2.itemid=slt.itemid and sl2.sordid=shf.sordid
  ) af
  where shf.sordid={sordid}
    and shf.sordstat=40
	and af.c=0



delete tt 
 from olc_tmp_sordsord tt
 join @t t on t.sordlineid=tt.sordlineid


insert into olc_tmp_sordsord
select '{guid}' ssid, slt.sordlineid, {sordid}, slt.linenum, slt.itemid, itemcode, name01, name02 ,sht.docnum, null qty, slt.reqdate, clt.confqty, clt.confdeldate, slt.ref2, 

isnull(orderedqty,0)-isnull(slt.movqty,0),
slt.ordqty, slt.movqty,

    slt.ordqty-isnull(orderedqty,0) ordqty, 0 movqty, slt.selprc, slt.seltotprc, slt.selprctype, slt.selprcprcid, slt.discpercnt, slt.discpercntprcid, slt.discval, slt.disctotval, slt.taxid, slt.sordlinestat, slt.note, null, slt.ucdid, slt.pjpid, slt.gen,'{Session.UserID}', getdate()
  from  ols_sordhead sht   
  join ols_sordline slt on slt.sordid=sht.sordid
  join ols_item i on i.itemid=slt.itemid
  left join olc_sordline clt on clt.sordlineid=slt.sordlineid
  join @t t on t.sordlineid=slt.sordlineid
outer apply(
	select sum(case when l2.movqty>0 then l2.movqty else l2.ordqty end) orderedqty
	  from olc_sordline c2
	  join ols_sordline l2 on l2.sordlineid=c2.sordlineid
	  left join ols_stline stl on slt.sordlineid=l2.sordlineid
	  left join ols_sthead sth on sth.stid=stl.stid
	  where c2.preordersordlineid=slt.sordlineid and isnull(sth.ststat,10)<100
  ) x
";
                SqlDataAdapter.ExecuteNonQuery(DB.Main, sql);

                dB.Commit();
            }

            return guid;
        }

        internal List<Key> CommitSordSord(Key k)
        {
            List<Key> list = new List<Key>();
            try
            {
                using (DB dB = DB.GetConn(DB.Main, Transaction.Use))
                {
                    OlcTmpSordSords tmpSordSords = OlcTmpSordSords.Load(k);
                    if (tmpSordSords.AllRows.Count == 0)
                    {
                        return list;
                    }

                    var sordLineBL = SordLineBL.New();
                  
                    foreach (OlcTmpSordSord row in tmpSordSords.Rows)
                    {
                        if (row.Qty.HasValue)
                        if (!(row.Qty.Value <= 0m))
                        { 
                            var sordLine = CreateSordLine(sordLineBL, row);
                            Key pK = sordLine.PK;
                            list.Add(pK);
                        }
                    }

                    dB.Commit();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new MessageException(Translator.Translate("$commitsordsord_err", ex.Message));
            }
        }

        private SordLine CreateSordLine(SordLineBL bl, OlcTmpSordSord row)
        {
            if (row.Qty > (row.Ordqty-row.Movqty))
            {
                var i = Item.Load(row.Itemid);
                throw new MessageException("$qtyerror", row.Qty, row.Ordqty, i.Itemcode, row.Movqty);
            }
            int? newResid = null;

            var osl = SordLine.Load(row.Sordlineid);
            if (osl.Resid.HasValue)
            {
                var rBl = ReserveBL.New();

                var ores = Reserve.Load(osl.Resid);

                var q = row.Qty;
                if (ores.Resqty < q)
                {
                    q = ores.Resqty;
                }
                ores.Resqty = ores.Resqty - q;
                SaveReserve(rBl, ores);

                var nres = Reserve.CreateNew();
                nres.Restype = 1;
                nres.Resqty = q;
                nres.Resdate = DateTime.Today;
                nres.Cmpid = ores.Cmpid;
                nres.Partnid = ores.Partnid;
                nres.Addrid = ores.Addrid;
                nres.Whid = ores.Whid;
                nres.Itemid = ores.Itemid;


                SaveReserve(rBl, nres);
                newResid = nres.Resid;
            }


            var sl = SordLine.CreateNew();
            var csl = OlcSordLine.CreateNew();

            csl.Preordersordlineid = row.Sordlineid;

            sl.Sordid = row.Sordid;
            sl.Def = 0;
            sl.Itemid = row.Itemid;
            sl.Reqdate = row.Reqdate;
            sl.Ref2 = row.Ref2;
            sl.Ordqty = row.Qty;
            sl.Movqty = 0;
            sl.Selprc = row.Selprc; 
            sl.Seltotprc = row.Seltotprc;
            sl.Selprctype = row.Selprctype;
            sl.Selprcprcid = row.Selprcprcid;
            sl.Discpercnt = row.Discpercnt;
            sl.Discpercntprcid = row.Discpercntprcid;
            sl.Discval = row.Discval;
            sl.Disctotval = row.Disctotval;
            sl.Taxid = row.Taxid;
            sl.Sordlinestat = 10;
            sl.Note = row.Note;
            sl.Resid = newResid;
            sl.Ucdid = row.Ucdid;
            sl.Pjpid = row.Pjpid;
            sl.Gen = row.Gen;

            csl.Confqty = row.Confqty;
            csl.Confdeldate = row.Confdeldate;


            var map = bl.CreateBLObjects();
            map.Default = sl;
            map.Add(csl);
            bl.Save(map);
             
            return sl;
        }

        private void SaveReserve(ReserveBL rBl, Reserve res)
        {
            var map = rBl.CreateBLObjects();
            map.Default = res;
            rBl.Save(map);
        }

        internal void ClearOlcTmpSordSord(Key k, List<Key> selectedRowPKs)
        {
            using (DB dB = DB.GetConn(DB.Main, Transaction.Use))
            {
                OlcTmpSordSords tmpSordSords = OlcTmpSordSords.Load(k);
                foreach (OlcTmpSordSord row in tmpSordSords.Rows)
                {
                    Key pK = row.PK;
                    foreach (Key selectedRowPK in selectedRowPKs)
                    {
                        if (selectedRowPK.Equals(pK))
                        {
                            row.Qty = default(decimal);
                            break;
                        }
                    }
                }

                tmpSordSords.Save();
                dB.Commit(); 
            }

        }

        internal void FillOlcTmpSordSord(Key k, List<Key> selectedRowPKs)
        {
            using (DB dB = DB.GetConn(DB.Main, Transaction.Use))
            {
                OlcTmpSordSords tmpSordSords = OlcTmpSordSords.Load(k);
                foreach (OlcTmpSordSord row in tmpSordSords.Rows)
                {
                    Key pK = row.PK;
                    foreach (Key selectedRowPK in selectedRowPKs)
                    {
                        if (selectedRowPK.Equals(pK))
                        {
                            row.Qty = row.Ordqty.GetValueOrDefault(0) - row.Movqty.GetValueOrDefault(0);
                            break;
                        }
                    }
                }

                tmpSordSords.Save();
                dB.Commit();
            }
        }
    }
}
