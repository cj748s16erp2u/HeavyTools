using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhlocation : Base.BusinessMasterEntity
    {
        public OlcWhlocation()
        {
            OlcWhzlocs = new HashSet<OlcWhzloc>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
        }

        public int Whlocid { get; set; }
        public string Whid { get; set; } = null!;
        public int? Whzoneid { get; set; }
        public string Whloccode { get; set; } = null!;
        public string? Name { get; set; }
        public int Loctype { get; set; }
        public int? Movloctype { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Overfillthreshold { get; set; }
        public int? Ismulti { get; set; }
        public int? Crawlorder { get; set; }
        public decimal? Capacity { get; set; }
        public string? Capunitid { get; set; }
        public string? Note { get; set; }

        public virtual OlsUnit? Capunit { get; set; }
        public virtual OlsWarehouse Wh { get; set; } = null!;
        public virtual OlcWhzone? Whzone { get; set; }
        public virtual ICollection<OlcWhzloc> OlcWhzlocs { get; set; }
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
    }
}
