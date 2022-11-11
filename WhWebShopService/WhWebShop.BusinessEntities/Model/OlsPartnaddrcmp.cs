using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_partnaddrcmp")]
    public partial class OlsPartnaddrcmp : Entity
    {
        [Key]
        [Column("addrid")]
        public int Addrid { get; set; }
        [Key]
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("codatag")]
        public int? Codatag { get; set; }
        [Column("codareferred")]
        public int? Codareferred { get; set; }
        [Column("salesagent")]
        public int? Salesagent { get; set; }
        [Column("prcgrpidprc")]
        public int? Prcgrpidprc { get; set; }
        [Column("prcgrpiddiscnt")]
        public int? Prcgrpiddiscnt { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addrid")]
        [InverseProperty("OlsPartnaddrcmp")]
        public virtual OlsPartnaddr Addr { get; set; } = null!;
        [ForeignKey("Addusrid")]
        [InverseProperty("OlsPartnaddrcmp")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Cmpid")]
        [InverseProperty("OlsPartnaddrcmp")]
        public virtual OlsCompany Cmp { get; set; } = null!;
    }
}