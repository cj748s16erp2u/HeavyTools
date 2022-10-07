using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("cfw_user")]
    public partial class CfwUser
    {
        public CfwUser()
        {
            CfwUsergroups = new HashSet<CfwUsergroup>();
            OlcWhlocations = new HashSet<OlcWhlocation>();
            OlcWhzones = new HashSet<OlcWhzone>();
            OlcWhztranheadAddusrs = new HashSet<OlcWhztranhead>();
            OlcWhztranheadCloseusrs = new HashSet<OlcWhztranhead>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlcWhztranlocs = new HashSet<OlcWhztranloc>();
            OlsCompanies = new HashSet<OlsCompany>();
            OlsItems = new HashSet<OlsItem>();
            OlsStheadAddusrs = new HashSet<OlsSthead>();
            OlsStheadCloseusrs = new HashSet<OlsSthead>();
            OlsStlines = new HashSet<OlsStline>();
            OlsUnits = new HashSet<OlsUnit>();
            OlsWarehouses = new HashSet<OlsWarehouse>();
        }

        [Key]
        [Column("usrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Usrid { get; set; } = null!;
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("password")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Password { get; set; }
        [Column("pwdate", TypeName = "datetime")]
        public DateTime Pwdate { get; set; }
        [Column("options")]
        public int Options { get; set; }
        [Column("deflangid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Deflangid { get; set; }
        [Column("ldapid")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Ldapid { get; set; }
        [Column("email")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("xmldata", TypeName = "xml")]
        public string? Xmldata { get; set; }
        [Column("cs")]
        [StringLength(50)]
        [Unicode(false)]
        public string Cs { get; set; } = null!;

        [InverseProperty("Usr")]
        public virtual ICollection<CfwUsergroup> CfwUsergroups { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcWhlocation> OlcWhlocations { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcWhzone> OlcWhzones { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadAddusrs { get; set; }
        [InverseProperty("Closeusr")]
        public virtual ICollection<OlcWhztranhead> OlcWhztranheadCloseusrs { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcWhztranloc> OlcWhztranlocs { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsCompany> OlsCompanies { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsItem> OlsItems { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsSthead> OlsStheadAddusrs { get; set; }
        [InverseProperty("Closeusr")]
        public virtual ICollection<OlsSthead> OlsStheadCloseusrs { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsUnit> OlsUnits { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsWarehouse> OlsWarehouses { get; set; }
    }
}
