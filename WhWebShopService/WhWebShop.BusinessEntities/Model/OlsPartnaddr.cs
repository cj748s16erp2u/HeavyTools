using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_partnaddr")]
    [Index("Extcode", Name = "idx_ols_partnaddr_extcode")]
    public partial class OlsPartnaddr : Entity
    {
        public OlsPartnaddr()
        {
            OlcPrctable = new HashSet<OlcPrctable>();
            OlsPartnaddrcmp = new HashSet<OlsPartnaddrcmp>();
            OlsReserve = new HashSet<OlsReserve>();
            OlsSinvheadAddr = new HashSet<OlsSinvhead>();
            OlsSinvheadDeladdr = new HashSet<OlsSinvhead>();
            OlsSordhead = new HashSet<OlsSordhead>();
        }

        [Key]
        [Column("addrid")]
        public int Addrid { get; set; }
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("def")]
        public int Def { get; set; }
        [Column("extcode")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Extcode { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("name")]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("countryid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Countryid { get; set; } = null!;
        [Column("regid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Regid { get; set; }
        [Column("postcode")]
        [StringLength(12)]
        [Unicode(false)]
        public string Postcode { get; set; } = null!;
        [Column("add01")]
        [StringLength(70)]
        [Unicode(false)]
        public string Add01 { get; set; } = null!;
        [Column("add02")]
        [StringLength(70)]
        [Unicode(false)]
        public string Add02 { get; set; } = null!;
        [Column("add03")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Add03 { get; set; }
        [Column("add04")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Add04 { get; set; }
        [Column("add05")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Add05 { get; set; }
        [Column("add06")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Add06 { get; set; }
        [Column("tel")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Tel { get; set; }
        [Column("fax")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Fax { get; set; }
        [Column("email")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("backordertype")]
        public int Backordertype { get; set; }
        [Column("deliverytime")]
        public int? Deliverytime { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("xmldata", TypeName = "xml")]
        public string? Xmldata { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsPartnaddr")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Countryid")]
        [InverseProperty("OlsPartnaddr")]
        public virtual OlsCountry Country { get; set; } = null!;
        [ForeignKey("Partnid")]
        [InverseProperty("OlsPartnaddr")]
        public virtual OlsPartner Partn { get; set; } = null!;
        [InverseProperty("Addr")]
        public virtual OlcPartnaddr OlcPartnaddr { get; set; } = null!;
        [InverseProperty("Addr")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
        [InverseProperty("Addr")]
        public virtual ICollection<OlsPartnaddrcmp> OlsPartnaddrcmp { get; set; }
        [InverseProperty("Addr")]
        public virtual ICollection<OlsReserve> OlsReserve { get; set; }
        [InverseProperty("Addr")]
        public virtual ICollection<OlsSinvhead> OlsSinvheadAddr { get; set; }
        [InverseProperty("Deladdr")]
        public virtual ICollection<OlsSinvhead> OlsSinvheadDeladdr { get; set; }
        [InverseProperty("Addr")]
        public virtual ICollection<OlsSordhead> OlsSordhead { get; set; }
    }
}