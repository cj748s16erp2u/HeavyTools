using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhzone : Base.BusinessMasterEntity
    {
        public OlcWhzone()
        {
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhzlocs = new HashSet<OlcWhzloc>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhztranheadFromwhzs = new HashSet<OlcWhztranhead>();
            OlcWhztranheadTowhzs = new HashSet<OlcWhztranhead>();
        }

        public int Whzoneid { get; set; }
        public string Whid { get; set; } = null!;
        public string Whzonecode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Pickingtype { get; set; }
        public int Pickingcartaccessible { get; set; }
        public int Isbackground { get; set; }
        public int Ispuffer { get; set; }
        public decimal? Locdefvolume { get; set; }
        public decimal? Locdefoverfillthreshold { get; set; }
        public int? Locdefismulti { get; set; }
        public string? Note { get; set; }

        public virtual OlsWarehouse Wh { get; set; } = null!;
        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        public virtual ICollection<OlcWhzloc> OlcWhzlocs { get; set; }
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadFromwhzs { get; set; }
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadTowhzs { get; set; }
    }
}
