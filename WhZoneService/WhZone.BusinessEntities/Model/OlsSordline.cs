using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsSordline : Base.BusinessEntity
    {
        public OlsSordline()
        {
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsStlineBsordlines = new HashSet<OlsStline>();
            OlsStlineSordlines = new HashSet<OlsStline>();
        }

        public int Sordlineid { get; set; }
        public int Sordid { get; set; }
        public int Linenum { get; set; }
        public int Def { get; set; }
        public int Itemid { get; set; }
        public DateTime Reqdate { get; set; }
        public string? Ref2 { get; set; }
        public decimal Ordqty { get; set; }
        public decimal Movqty { get; set; }
        public decimal Selprc { get; set; }
        public decimal? Seltotprc { get; set; }
        public int? Selprctype { get; set; }
        public int? Selprcprcid { get; set; }
        public decimal Discpercnt { get; set; }
        public int? Discpercntprcid { get; set; }
        public decimal Discval { get; set; }
        public decimal? Disctotval { get; set; }
        public string Taxid { get; set; } = null!;
        public int Sordlinestat { get; set; }
        public string? Note { get; set; }
        public int? Resid { get; set; }
        public int? Ucdid { get; set; }
        public int? Pjpid { get; set; }
        public int Gen { get; set; }

        public virtual OlsItem Item { get; set; } = null!;
        public virtual OlsSordhead Sord { get; set; } = null!;
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        public virtual ICollection<OlsStline> OlsStlineBsordlines { get; set; }
        public virtual ICollection<OlsStline> OlsStlineSordlines { get; set; }
    }
}
