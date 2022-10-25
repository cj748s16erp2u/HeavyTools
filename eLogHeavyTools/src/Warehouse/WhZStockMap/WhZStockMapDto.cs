using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZStockMap
{
    public class WhZStockMapDto : VirtualEntity<WhZStockMapDto>
    {
        public string Whid { get; internal set; }
        public int? Whzoneid { get; internal set; }
        public string Whcode { get; internal set; }
        public string Whname { get; internal set; }
        public double Actqty { get; internal set; }
        public double Resqty { get; internal set; }
        public int Itemid { get; internal set; }
        public string Itemcode { get; internal set; }
        public string Whloccode { get; internal set; }
        public string Itemname { get; internal set; }
        public double Provqty { get; internal set; }
        public double Reqqty { get; internal set; }
        public double Recqty { get; internal set; }
        public int? Whlocid { get; internal set; }
        public string Whlocname { get; internal set; }
        public string Whzonecode { get; internal set; }
        public string Whzonename { get; internal set; }

    }
}
