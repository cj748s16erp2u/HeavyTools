using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsItem : Base.BusinessMasterEntity
    {
        public OlsItem()
        {
            InverseRootitem = new HashSet<OlsItem>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhzstocks = new HashSet<OlcWhzstock>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsPordlines = new HashSet<OlsPordline>();
            OlsSordlines = new HashSet<OlsSordline>();
            OlsStlines = new HashSet<OlsStline>();
        }

        public int Itemid { get; set; }
        public int Cmpid { get; set; }
        public string Itemcode { get; set; } = null!;
        public string Cmpcodes { get; set; } = null!;
        public int Type { get; set; }
        public string Itemgrpid { get; set; } = null!;
        public string Name01 { get; set; } = null!;
        public string? Name02 { get; set; }
        public string? Name03 { get; set; }
        public string? Sname { get; set; }
        public string? Custtarid { get; set; }
        public string Unitid { get; set; } = null!;
        public DateTime Releasedate { get; set; }
        public int? Rootitemid { get; set; }
        public string? Note { get; set; }
        public int? Uqcardtype { get; set; }
        public decimal? Netweight { get; set; }

        public virtual OlsItem? Rootitem { get; set; }
        public virtual OlsUnit Unit { get; set; } = null!;
        public virtual ICollection<OlsItem> InverseRootitem { get; set; }
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        public virtual ICollection<OlcWhzstock> OlcWhzstocks { get; set; }
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        public virtual ICollection<OlsPordline> OlsPordlines { get; set; }
        public virtual ICollection<OlsSordline> OlsSordlines { get; set; }
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
