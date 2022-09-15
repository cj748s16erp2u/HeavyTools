using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_actioncountry")]
    public partial class OlcActioncountry : Entity
    {
        [Key]
        [Column("acid")]
        public int Acid { get; set; }
        [Column("aid")]
        public int Aid { get; set; }
        [Column("countryid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Countryid { get; set; } = null!;
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcActioncountry")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Countryid")]
        [InverseProperty("OlcActioncountry")]
        public virtual OlsCountry Country { get; set; } = null!;
    }
}