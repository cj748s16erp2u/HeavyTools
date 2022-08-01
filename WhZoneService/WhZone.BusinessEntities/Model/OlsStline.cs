using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsStline : Base.BusinessEntity
    {
        public OlsStline()
        {
            InverseDelstline = new HashSet<OlsStline>();
            InverseIcstline = new HashSet<OlsStline>();
            InverseIntransitstline = new HashSet<OlsStline>();
            InverseOrigstline = new HashSet<OlsStline>();
            InverseReccorstline = new HashSet<OlsStline>();
            InverseRetorigstline = new HashSet<OlsStline>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
        }

        public int Stlineid { get; set; }
        public int Stid { get; set; }
        public int Linenum { get; set; }
        public int Itemid { get; set; }
        public decimal Ordqty { get; set; }
        public decimal Dispqty { get; set; }
        public decimal Movqty { get; set; }
        public decimal Inqty { get; set; }
        public decimal Outqty { get; set; }
        public string Unitid2 { get; set; } = null!;
        public decimal Change { get; set; }
        public decimal Ordqty2 { get; set; }
        public decimal Dispqty2 { get; set; }
        public decimal Movqty2 { get; set; }
        public decimal Costprc { get; set; }
        public decimal Costprchome { get; set; }
        public decimal? Costprcdual { get; set; }
        public decimal Costval { get; set; }
        public decimal Costvalhome { get; set; }
        public decimal? Costvaldual { get; set; }
        public decimal? Selprc { get; set; }
        public decimal? Seltotprc { get; set; }
        public int? Selprctype { get; set; }
        public int? Selprcprcid { get; set; }
        public decimal? Selval { get; set; }
        public decimal? Seltotval { get; set; }
        public decimal? Discpercnt { get; set; }
        public int? Discpercntprcid { get; set; }
        public decimal? Discval { get; set; }
        public decimal? Disctotval { get; set; }
        public decimal? Netval { get; set; }
        public string Taxid { get; set; } = null!;
        public decimal? Invselprc { get; set; }
        public decimal? Invseltotprc { get; set; }
        public decimal? Invselval { get; set; }
        public decimal? Invseltotval { get; set; }
        public decimal? Invdiscpercnt { get; set; }
        public decimal? Invdiscval { get; set; }
        public decimal? Invdisctotval { get; set; }
        public decimal? Invnetval { get; set; }
        public decimal? Invtaxval { get; set; }
        public decimal? Invtotval { get; set; }
        public string? Note { get; set; }
        public string? Origid { get; set; }
        public int Corrtype { get; set; }
        public int? Origstlineid { get; set; }
        public int? Delstlineid { get; set; }
        public int? Sordlineid { get; set; }
        public int? Bsordlineid { get; set; }
        public int? Pordlineid { get; set; }
        public int? Dglineid { get; set; }
        public int? Retlineid { get; set; }
        public int? Philineid { get; set; }
        public int? Reqid { get; set; }
        public int? Intransitstlineid { get; set; }
        public int? Mordlineid { get; set; }
        public int? Retorigstlineid { get; set; }
        public int? Reccorstlineid { get; set; }
        public int? Prodrepid { get; set; }
        public int? Pplid { get; set; }
        public int? Costcalcfix { get; set; }
        public int? Pjpid { get; set; }
        public int? Icstlineid { get; set; }
        public int? Mediatedservices { get; set; }
        public int? Svcwsid { get; set; }
        public int Gen { get; set; }

        public virtual OlsSordline? Bsordline { get; set; }
        public virtual OlsStline? Delstline { get; set; }
        public virtual OlsStline? Icstline { get; set; }
        public virtual OlsStline? Intransitstline { get; set; }
        public virtual OlsItem Item { get; set; } = null!;
        public virtual OlsStline? Origstline { get; set; }
        public virtual OlsPordline? Pordline { get; set; }
        public virtual OlsStline? Reccorstline { get; set; }
        public virtual OlsStline? Retorigstline { get; set; }
        public virtual OlsSordline? Sordline { get; set; }
        public virtual OlsSthead St { get; set; } = null!;
        public virtual OlsUnit Unitid2Navigation { get; set; } = null!;
        public virtual ICollection<OlsStline> InverseDelstline { get; set; }
        public virtual ICollection<OlsStline> InverseIcstline { get; set; }
        public virtual ICollection<OlsStline> InverseIntransitstline { get; set; }
        public virtual ICollection<OlsStline> InverseOrigstline { get; set; }
        public virtual ICollection<OlsStline> InverseReccorstline { get; set; }
        public virtual ICollection<OlsStline> InverseRetorigstline { get; set; }
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
    }
}
