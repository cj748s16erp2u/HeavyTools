using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsUnit : Base.BusinessMasterEntity
    {
        public OlsUnit()
        {
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsItems = new HashSet<OlsItem>();
            OlsPordlines = new HashSet<OlsPordline>();
            OlsStlines = new HashSet<OlsStline>();
        }

        public string Unitid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Code2 { get; set; }
        public int? Unittype { get; set; }
        public string? Note { get; set; }
        public string? Navcode { get; set; }

        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        public virtual ICollection<OlsItem> OlsItems { get; set; }
        public virtual ICollection<OlsPordline> OlsPordlines { get; set; }
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
