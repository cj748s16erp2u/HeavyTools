using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Reserve;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Sord
{
    public class OlcReserveLayoutTable
    {
        TabPage2 Tab;
        private LayoutTable layoutTable;

        public OlcReserveLayoutTable(TabPage2 tabPage2, LayoutTable layoutTable)
        {
            Tab = tabPage2;
            this.layoutTable = layoutTable;

            for (int i = 0; i < 10; i++)
            {
                var w = new Combo("whid" + i, eLog.Base.Setup.Warehouse.WarehouseList.ID);

                w.LabelId = "$whid";

                var d = new Datebox("date" + i)
                {
                    LabelId = "$resdate"
                };

                var q = new Numberbox("resqty" + i)
                {
                    LabelId = "$stock_resqty"
                };

                layoutTable.AddControl(w);
                layoutTable.AddControl(d);
                layoutTable.AddControl(q); 
            } 
        }

        internal void SetSordlineId(int? sordlineid)
        {
            var ds = new Dictionary<DateTime, Reserve>();

            if (sordlineid.HasValue)
            {
                foreach (OlcSordlineRes slres in OlcSordlineReses.Load(new Key() { { SordLine.FieldSordlineid.Name, sordlineid } }).AllRows)
                {
                    if (slres.Resid.HasValue)
                    {
                        var r = Reserve.Load(slres.Resid);
                        if (ds.ContainsKey(r.Resdate.Value))
                        {
                            ds[r.Resdate.Value].Resqty += r.Resqty;
                        }
                        else
                        {
                            ds.Add(r.Resdate.Value, r);
                        }
                    }
                }
            }
             
            var ii = 0;
            foreach (KeyValuePair<DateTime, Reserve> item in ds)
            {
                ii++;

                var w = layoutTable.FindRenderable<Combo>("whid" + ii);
                w.Value = item.Value.Whid;

                var d = layoutTable.FindRenderable<Datebox>("date" + ii);
                d.Value = item.Key;

                var q = layoutTable.FindRenderable<Numberbox>("resqty" + ii);
                q.Value = item.Value.Resqty;
            }
            
            for (int i = 0; i < 10; i++)
            {
                var w = layoutTable.FindRenderable<Combo>("whid" + i);
                var d = layoutTable.FindRenderable<Datebox>("date" + i); 
                var q = layoutTable.FindRenderable<Numberbox>("resqty" + i);

                if (d.Value == null)
                {
                    w.Visible = false;
                    d.Visible = false;
                    q.Visible = false;
                }
            }
        }
    }
}
