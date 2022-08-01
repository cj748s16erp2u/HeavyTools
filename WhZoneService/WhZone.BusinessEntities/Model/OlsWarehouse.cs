using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsWarehouse : Base.BusinessMasterEntity
    {
        public OlsWarehouse()
        {
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhzlocs = new HashSet<OlcWhzloc>();
            OlcWhzones = new HashSet<OlcWhzone>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhzstocks = new HashSet<OlcWhzstock>();
            OlsPordheads = new HashSet<OlsPordhead>();
            OlsSordheads = new HashSet<OlsSordhead>();
            OlsStheadFromwhs = new HashSet<OlsSthead>();
            OlsStheadIntransittowhs = new HashSet<OlsSthead>();
            OlsStheadIntransitwhs = new HashSet<OlsSthead>();
            OlsStheadTowhs = new HashSet<OlsSthead>();
        }

        public string Whid { get; set; } = null!;
        public int Cmpid { get; set; }
        public string Cmpcodes { get; set; } = null!;
        public int Partnid { get; set; }
        public int Addrid { get; set; }
        public string Name { get; set; } = null!;
        public int? Whtype { get; set; }
        public int Loctype { get; set; }
        public int Storemtype { get; set; }
        public int Pickmtype { get; set; }
        public int Backordertype { get; set; }
        public int? Projid { get; set; }
        public string? Note { get; set; }
        public string? Codacode { get; set; }

        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        public virtual ICollection<OlcWhzloc> OlcWhzlocs { get; set; }
        public virtual ICollection<OlcWhzone> OlcWhzones { get; set; }
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        public virtual ICollection<OlcWhzstock> OlcWhzstocks { get; set; }
        public virtual ICollection<OlsPordhead> OlsPordheads { get; set; }
        public virtual ICollection<OlsSordhead> OlsSordheads { get; set; }
        public virtual ICollection<OlsSthead> OlsStheadFromwhs { get; set; }
        public virtual ICollection<OlsSthead> OlsStheadIntransittowhs { get; set; }
        public virtual ICollection<OlsSthead> OlsStheadIntransitwhs { get; set; }
        public virtual ICollection<OlsSthead> OlsStheadTowhs { get; set; }
    }
}
