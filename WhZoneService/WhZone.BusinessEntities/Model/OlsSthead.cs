using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsSthead : Base.BusinessEntity
    {
        public OlsSthead()
        {
            InverseCorrst = new HashSet<OlsSthead>();
            InverseDispst = new HashSet<OlsSthead>();
            InverseOrigst = new HashSet<OlsSthead>();
            InverseProdrecst = new HashSet<OlsSthead>();
            InverseRetorigst = new HashSet<OlsSthead>();
            OlcWhztranheads = new HashSet<OlcWhztranhead>();
            OlsStlines = new HashSet<OlsStline>();
        }

        public int Stid { get; set; }
        public int Cmpid { get; set; }
        public string Stdocid { get; set; } = null!;
        public string Docnum { get; set; } = null!;
        public int Sttype { get; set; }
        public DateTime Stdate { get; set; }
        public int? Taxdatetype { get; set; }
        public DateTime Reldatemax { get; set; }
        public int Frompartnid { get; set; }
        public int Fromaddrid { get; set; }
        public string? Fromwhid { get; set; }
        public int Topartnid { get; set; }
        public int Toaddrid { get; set; }
        public string? Towhid { get; set; }
        public string Curid { get; set; } = null!;
        public string? Paymid { get; set; }
        public int? Paycid { get; set; }
        public decimal? Netweight { get; set; }
        public decimal? Grossweight { get; set; }
        public string? Shipparitycode { get; set; }
        public string? Shipparityplace { get; set; }
        public int? Cootype { get; set; }
        public string? Colli { get; set; }
        public string? Ref1 { get; set; }
        public string? Ref2 { get; set; }
        public string? Closeusrid { get; set; }
        public DateTime? Closedate { get; set; }
        public int Ststat { get; set; }
        public int? Manufusestat { get; set; }
        public string? Note { get; set; }
        public int? Sinvid { get; set; }
        public int Corrtype { get; set; }
        public int? Origstid { get; set; }
        public int? Corrstid { get; set; }
        public int? Transpid { get; set; }
        public string? Intransitwhid { get; set; }
        public string? Intransittowhid { get; set; }
        public int? Pinvid { get; set; }
        public int? Prodrecstid { get; set; }
        public int? Projid { get; set; }
        public int? Retorigstid { get; set; }
        public string Bustypeid { get; set; } = null!;
        public int? Dispstid { get; set; }
        public int Gen { get; set; }
        public string Langid { get; set; } = null!;
        public int Lastlinenum { get; set; }

        public virtual CfwUser? Closeusr { get; set; }
        public virtual OlsCompany Cmp { get; set; } = null!;
        public virtual OlsSthead? Corrst { get; set; }
        public virtual OlsSthead? Dispst { get; set; }
        public virtual OlsWarehouse? Fromwh { get; set; }
        public virtual OlsWarehouse? Intransittowh { get; set; }
        public virtual OlsWarehouse? Intransitwh { get; set; }
        public virtual OlsSthead? Origst { get; set; }
        public virtual OlsSthead? Prodrecst { get; set; }
        public virtual OlsSthead? Retorigst { get; set; }
        public virtual OlsWarehouse? Towh { get; set; }
        public virtual ICollection<OlsSthead> InverseCorrst { get; set; }
        public virtual ICollection<OlsSthead> InverseDispst { get; set; }
        public virtual ICollection<OlsSthead> InverseOrigst { get; set; }
        public virtual ICollection<OlsSthead> InverseProdrecst { get; set; }
        public virtual ICollection<OlsSthead> InverseRetorigst { get; set; }
        public virtual ICollection<OlcWhztranhead> OlcWhztranheads { get; set; }
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
