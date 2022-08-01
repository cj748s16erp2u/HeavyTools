using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("cfw_user")]
    public partial class CfwUser : Entity
    {
        public CfwUser()
        {
            CfwUsergroups = new HashSet<CfwUsergroup>();
            OlsCompanies = new HashSet<OlsCompany>();
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
        public virtual ICollection<OlsCompany> OlsCompanies { get; set; }
    }
}
