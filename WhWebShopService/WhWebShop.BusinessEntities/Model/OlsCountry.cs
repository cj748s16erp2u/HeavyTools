using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_country")]
    public partial class OlsCountry : Entity
    {
        public OlsCountry()
        {
            OlcTaxtransext = new HashSet<OlcTaxtransext>();
            OlsCompany = new HashSet<OlsCompany>();
            OlsPartnaddr = new HashSet<OlsPartnaddr>();
        }

        [Key]
        [Column("countryid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Countryid { get; set; } = null!;
        [Column("codacode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Codacode { get; set; }
        [Column("ref1")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Ref1 { get; set; }
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("eutype")]
        public int Eutype { get; set; }
        [Column("navcode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Navcode { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsCountry")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Country")]
        public virtual ICollection<OlcTaxtransext> OlcTaxtransext { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<OlsCompany> OlsCompany { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<OlsPartnaddr> OlsPartnaddr { get; set; }
    }
}