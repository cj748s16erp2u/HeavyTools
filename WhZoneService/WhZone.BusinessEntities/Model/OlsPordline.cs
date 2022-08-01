using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsPordline : Base.BusinessEntity
    {
        public OlsPordline()
        {
            InverseParentpordline = new HashSet<OlsPordline>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsStlines = new HashSet<OlsStline>();
        }

        public int Pordlineid { get; set; }
        public int Pordid { get; set; }
        public int Linenum { get; set; }
        public int Def { get; set; }
        public int Itemid { get; set; }
        public DateTime Reqdate { get; set; }
        public DateTime? Confreqdate { get; set; }
        public string? Ref2 { get; set; }
        public decimal Ordqty { get; set; }
        public decimal? Confqty { get; set; }
        public decimal Movqty { get; set; }
        public decimal Purchprc { get; set; }
        public string Unitid2 { get; set; } = null!;
        public decimal Change { get; set; }
        public decimal Ordqty2 { get; set; }
        public decimal? Confqty2 { get; set; }
        public decimal Movqty2 { get; set; }
        public decimal Purchprc2 { get; set; }
        public int Pordlinestat { get; set; }
        public string? Note { get; set; }
        public int? Pglineid { get; set; }
        public int? Parentpordlineid { get; set; }
        public int Gen { get; set; }

        public virtual OlsItem Item { get; set; } = null!;
        public virtual OlsPordline? Parentpordline { get; set; }
        public virtual OlsPordhead Pord { get; set; } = null!;
        public virtual OlsUnit Unitid2Navigation { get; set; } = null!;
        public virtual ICollection<OlsPordline> InverseParentpordline { get; set; }
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
