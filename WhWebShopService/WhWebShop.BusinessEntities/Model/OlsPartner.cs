using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_partner")]
    [Index("Name", Name = "idx_ols_partner_name")]
    [Index("Partncode", Name = "idx_ols_partner_partncode", IsUnique = true)]
    public partial class OlsPartner : Entity
    {
        public OlsPartner()
        {
            OlcPrctable = new HashSet<OlcPrctable>();
            OlsCompany = new HashSet<OlsCompany>();
            OlsPartnaddr = new HashSet<OlsPartnaddr>();
            OlsSordhead = new HashSet<OlsSordhead>();
        }

        [Key]
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("partncode")]
        [StringLength(12)]
        [Unicode(false)]
        public string Partncode { get; set; } = null!;
        [Column("cmpcodes")]
        [StringLength(2000)]
        [Unicode(false)]
        public string Cmpcodes { get; set; } = null!;
        [Column("extcode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Extcode { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("grp")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Grp { get; set; }
        [Column("name")]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("sname")]
        [StringLength(20)]
        [Unicode(false)]
        public string Sname { get; set; } = null!;
        [Column("vatnum")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Vatnum { get; set; }
        [Column("vatnumeu")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Vatnumeu { get; set; }
        [Column("groupvatnum")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Groupvatnum { get; set; }
        [Column("bankaccno")]
        [StringLength(40)]
        [Unicode(false)]
        public string? Bankaccno { get; set; }
        [Column("regnum")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Regnum { get; set; }
        [Column("webpage")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Webpage { get; set; }
        [Column("taxtype")]
        public int? Taxtype { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("ptvattypid")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ptvattypid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsPartner")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Partn")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
        [InverseProperty("Partn")]
        public virtual ICollection<OlsCompany> OlsCompany { get; set; }
        [InverseProperty("Partn")]
        public virtual ICollection<OlsPartnaddr> OlsPartnaddr { get; set; }
        [InverseProperty("Partn")]
        public virtual ICollection<OlsSordhead> OlsSordhead { get; set; }
    }
}