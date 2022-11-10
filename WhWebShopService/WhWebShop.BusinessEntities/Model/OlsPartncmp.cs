using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_partncmp")]
    public partial class OlsPartncmp : Entity
    {
        [Key]
        [Column("partnid")]
        public int Partnid { get; set; }
        [Key]
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("codacode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Codacode { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("paymid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Paymid { get; set; }
        [Column("paycid")]
        public int? Paycid { get; set; }
        [Column("credlimit", TypeName = "numeric(19, 6)")]
        public decimal Credlimit { get; set; }
        [Column("prcgrpidprc")]
        public int? Prcgrpidprc { get; set; }
        [Column("prcgrpiddiscnt")]
        public int? Prcgrpiddiscnt { get; set; }
        [Column("selprcincdiscnttype")]
        public int Selprcincdiscnttype { get; set; }
        [Column("pcstat")]
        public int? Pcstat { get; set; }
        [Column("posttype")]
        public int Posttype { get; set; }
        [Column("codapostdate", TypeName = "datetime")]
        public DateTime? Codapostdate { get; set; }
        [Column("curid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Curid { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsPartncmp")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Cmpid")]
        [InverseProperty("OlsPartncmp")]
        public virtual OlsCompany Cmp { get; set; } = null!;
        [ForeignKey("Curid")]
        [InverseProperty("OlsPartncmp")]
        public virtual OlsCurrency? Cur { get; set; }
        [ForeignKey("Partnid")]
        [InverseProperty("OlsPartncmp")]
        public virtual OlsPartner Partn { get; set; } = null!;
    }
}