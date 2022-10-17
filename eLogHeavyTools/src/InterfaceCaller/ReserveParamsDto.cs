using eLog.Base.Warehouse.Reserve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{
    public class ReserveParamsDto
    {
        public int? Resid { get; set; }
        public int? Cmpid { get; set; }
        public int? Partnid { get; set; }
        public int? Addrid { get; set; }
        public string Whid { get; set; }
        public int? Itemid { get; set; }
        public int? Lotid { get; set; }
        public int? ResType { get; set; }
        public decimal? ResQty { get; set; }
        public DateTime? ResDate { get; set; }
        public DateTime? FreeDate { get; set; }
        public string Note { get; set; }
        public string Addusrid { get; set; }
        public DateTime? Adddate { get; set; }

        public void From(Reserve rw)
        {
            this.Resid = rw.Resid;
            this.Cmpid = rw.Cmpid;
            this.Partnid = rw.Partnid;
            this.Addrid = rw.Addrid;
            this.Whid = rw.Whid;
            this.Itemid = rw.Itemid;
            this.Lotid = rw.Lotid;
            this.ResType = rw.Restype;
            this.ResQty = rw.Resqty;
            this.ResDate = rw.Resdate;
            this.FreeDate = rw.Freedate;
            this.Note = rw.Note;
            this.Addusrid = rw.Addusrid;
            this.Adddate = rw.Adddate;
        }

    }
}
