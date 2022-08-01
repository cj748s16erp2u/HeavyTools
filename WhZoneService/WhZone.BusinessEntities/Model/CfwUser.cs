using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class CfwUser : Base.Entity
    {
        public CfwUser()
        {
            CfwUsergroups = new HashSet<CfwUsergroup>();
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhzlocs = new HashSet<OlcWhzloc>();
            OlcWhzones = new HashSet<OlcWhzone>();
            OlcWhztranheadAddusrs = new HashSet<OlcWhztranhead>();
            OlcWhztranheadCloseusrs = new HashSet<OlcWhztranhead>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsCompanies = new HashSet<OlsCompany>();
            OlsItems = new HashSet<OlsItem>();
            OlsPordheads = new HashSet<OlsPordhead>();
            OlsPordlines = new HashSet<OlsPordline>();
            OlsSordheads = new HashSet<OlsSordhead>();
            OlsSordlines = new HashSet<OlsSordline>();
            OlsStheadAddusrs = new HashSet<OlsSthead>();
            OlsStheadCloseusrs = new HashSet<OlsSthead>();
            OlsStlines = new HashSet<OlsStline>();
            OlsUnits = new HashSet<OlsUnit>();
            OlsWarehouses = new HashSet<OlsWarehouse>();
        }

        public string Usrid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Password { get; set; }
        public DateTime Pwdate { get; set; }
        public int Options { get; set; }
        public string? Deflangid { get; set; }
        public string? Ldapid { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public string? Xmldata { get; set; }
        public string Cs { get; set; } = null!;

        public virtual ICollection<CfwUsergroup> CfwUsergroups { get; set; }
        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        public virtual ICollection<OlcWhzloc> OlcWhzlocs { get; set; }
        public virtual ICollection<OlcWhzone> OlcWhzones { get; set; }
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadAddusrs { get; set; }
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadCloseusrs { get; set; }
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        public virtual ICollection<OlsCompany> OlsCompanies { get; set; }
        public virtual ICollection<OlsItem> OlsItems { get; set; }
        public virtual ICollection<OlsPordhead> OlsPordheads { get; set; }
        public virtual ICollection<OlsPordline> OlsPordlines { get; set; }
        public virtual ICollection<OlsSordhead> OlsSordheads { get; set; }
        public virtual ICollection<OlsSordline> OlsSordlines { get; set; }
        public virtual ICollection<OlsSthead> OlsStheadAddusrs { get; set; }
        public virtual ICollection<OlsSthead> OlsStheadCloseusrs { get; set; }
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
        public virtual ICollection<OlsUnit> OlsUnits { get; set; }
        public virtual ICollection<OlsWarehouse> OlsWarehouses { get; set; }
    }
}
