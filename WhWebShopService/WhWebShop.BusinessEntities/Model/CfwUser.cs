using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("cfw_user")]
    public partial class CfwUser : Entity
    {
        public CfwUser()
        {
            CfwUsergroup = new HashSet<CfwUsergroup>();
            OlcAction = new HashSet<OlcAction>();
            OlcActioncountry = new HashSet<OlcActioncountry>();
            OlcActioncouponnumber = new HashSet<OlcActioncouponnumber>();
            OlcActionext = new HashSet<OlcActionext>();
            OlcActionretail = new HashSet<OlcActionretail>();
            OlcActionwebhop = new HashSet<OlcActionwebhop>();
            OlcGiftcard = new HashSet<OlcGiftcard>();
            OlcGiftcardlog = new HashSet<OlcGiftcardlog>();
            OlcItem = new HashSet<OlcItem>();
            OlcPartner = new HashSet<OlcPartner>();
            OlcPrctable = new HashSet<OlcPrctable>();
            OlcPrctype = new HashSet<OlcPrctype>();
            OlcSordhead = new HashSet<OlcSordhead>();
            OlcSordline = new HashSet<OlcSordline>();
            OlcTaxtransext = new HashSet<OlcTaxtransext>();
            OlsCompany = new HashSet<OlsCompany>();
            OlsCountry = new HashSet<OlsCountry>();
            OlsCurrency = new HashSet<OlsCurrency>();
            OlsItem = new HashSet<OlsItem>();
            OlsPartnaddr = new HashSet<OlsPartnaddr>();
            OlsPartner = new HashSet<OlsPartner>();
            OlsSinvheadAddusr = new HashSet<OlsSinvhead>();
            OlsSinvheadCloseusr = new HashSet<OlsSinvhead>();
            OlsSordhead = new HashSet<OlsSordhead>();
            OlsSordline = new HashSet<OlsSordline>();
            OlsTax = new HashSet<OlsTax>();
            OlsTaxtrans = new HashSet<OlsTaxtrans>();
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
        public virtual ICollection<CfwUsergroup> CfwUsergroup { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcAction> OlcAction { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcActioncountry> OlcActioncountry { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcActioncouponnumber> OlcActioncouponnumber { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcActionext> OlcActionext { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcActionretail> OlcActionretail { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcActionwebhop> OlcActionwebhop { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcGiftcard> OlcGiftcard { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcGiftcardlog> OlcGiftcardlog { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcItem> OlcItem { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcPartner> OlcPartner { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcPrctype> OlcPrctype { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcSordhead> OlcSordhead { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcSordline> OlcSordline { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlcTaxtransext> OlcTaxtransext { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsCompany> OlsCompany { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsCountry> OlsCountry { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsCurrency> OlsCurrency { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsItem> OlsItem { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsPartnaddr> OlsPartnaddr { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsPartner> OlsPartner { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsSinvhead> OlsSinvheadAddusr { get; set; }
        [InverseProperty("Closeusr")]
        public virtual ICollection<OlsSinvhead> OlsSinvheadCloseusr { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsSordhead> OlsSordhead { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsSordline> OlsSordline { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsTax> OlsTax { get; set; }
        [InverseProperty("Addusr")]
        public virtual ICollection<OlsTaxtrans> OlsTaxtrans { get; set; }
    }
}