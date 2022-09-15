using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_currency")]
    public partial class OlsCurrency : Entity
    {
        public OlsCurrency()
        {
            OlcAction = new HashSet<OlcAction>();
            OlcPrctable = new HashSet<OlcPrctable>();
            OlsCompanyDualcur = new HashSet<OlsCompany>();
            OlsCompanyHomecur = new HashSet<OlsCompany>();
            OlsSinvhead = new HashSet<OlsSinvhead>();
            OlsSordhead = new HashSet<OlsSordhead>();
        }

        [Key]
        [Column("curid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Curid { get; set; } = null!;
        [Column("name")]
        [StringLength(40)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("decnum")]
        public int? Decnum { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsCurrency")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Cur")]
        public virtual ICollection<OlcAction> OlcAction { get; set; }
        [InverseProperty("Cur")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
        [InverseProperty("Dualcur")]
        public virtual ICollection<OlsCompany> OlsCompanyDualcur { get; set; }
        [InverseProperty("Homecur")]
        public virtual ICollection<OlsCompany> OlsCompanyHomecur { get; set; }
        [InverseProperty("Cur")]
        public virtual ICollection<OlsSinvhead> OlsSinvhead { get; set; }
        [InverseProperty("Cur")]
        public virtual ICollection<OlsSordhead> OlsSordhead { get; set; }
    }
}