using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhztranline : Base.BusinessEntity
    {
        public OlcWhztranline()
        {
            OlcWhzlocs = new HashSet<OlcWhzloc>();
        }

        public int Whztlineid { get; set; }
        public int Whztid { get; set; }
        public int Linenum { get; set; }
        public int Itemid { get; set; }
        public decimal Dispqty { get; set; }
        public decimal Movqty { get; set; }
        public decimal Inqty { get; set; }
        public decimal Outqty { get; set; }
        public string Unitid2 { get; set; } = null!;
        public decimal Dispqty2 { get; set; }
        public decimal Movqty2 { get; set; }
        public string? Note { get; set; }
        public int? Stlineid { get; set; }
        public int? Sordlineid { get; set; }
        public int? Pordlineid { get; set; }
        public int? Taskitemid { get; set; }
        public int? Gen { get; set; }

        public virtual OlsItem Item { get; set; } = null!;
        public virtual OlsPordline? Pordline { get; set; }
        public virtual OlsSordline? Sordline { get; set; }
        public virtual OlsStline? Stline { get; set; }
        public virtual OlsUnit Unitid2Navigation { get; set; } = null!;
        public virtual OlcWhztranhead Whzt { get; set; } = null!;
        public virtual ICollection<OlcWhzloc> OlcWhzlocs { get; set; }
    }
}
