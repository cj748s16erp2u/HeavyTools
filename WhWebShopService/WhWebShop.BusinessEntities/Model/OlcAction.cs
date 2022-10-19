using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_action")]
    public partial class OlcAction : Entity
    {
        public OlcAction()
        {
            InverseDiscounta = new HashSet<OlcAction>();
        }

        [Key]
        [Column("aid")]
        public int Aid { get; set; }
        [Column("actiontype")]
        public int Actiontype { get; set; }
        [Column("isactive")]
        public int Isactive { get; set; }
        [Column("isextcondition")]
        public int Isextcondition { get; set; }
        [Column("isextdiscount")]
        public int? Isextdiscount { get; set; }
        [Column("name")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Name { get; set; }
        [Column("priority")]
        public int? Priority { get; set; }
        [Column("curid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Curid { get; set; }
        [Column("singlecouponnumber")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Singlecouponnumber { get; set; }
        [Column("couponunlimiteduse")]
        public int? Couponunlimiteduse { get; set; }
        [Column("discounttype")]
        public int? Discounttype { get; set; }
        [Column("discountval", TypeName = "numeric(19, 6)")]
        public decimal? Discountval { get; set; }
        [Column("discountforfree")]
        public int? Discountforfree { get; set; }
        [Column("discountfreetransportation")]
        public int? Discountfreetransportation { get; set; }
        [Column("discountcalculationtype")]
        public int? Discountcalculationtype { get; set; }
        [Column("discountaid")]
        public int? Discountaid { get; set; }
        [Column("validdatefrom", TypeName = "datetime")]
        public DateTime? Validdatefrom { get; set; }
        [Column("validforsaleproducts")]
        public int? Validforsaleproducts { get; set; }
        [Column("validdateto", TypeName = "datetime")]
        public DateTime? Validdateto { get; set; }
        [Column("validtotvalfrom", TypeName = "numeric(19, 6)")]
        public decimal? Validtotvalfrom { get; set; }
        [Column("validtotvalto", TypeName = "numeric(19, 6)")]
        public decimal? Validtotvalto { get; set; }
        [Column("purchasetype")]
        public int Purchasetype { get; set; }
        [Column("filtercustomerstype")]
        public int? Filtercustomerstype { get; set; }
        [Column("filteritems")]
        [Unicode(false)]
        public string? Filteritems { get; set; }
        [Column("filteritemsblock")]
        [Unicode(false)]
        public string? Filteritemsblock { get; set; }
        [Column("count")]
        public int? Count { get; set; }
        [Column("note")]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("netgoid")]
        public int? Netgoid { get; set; }
        [Column("blockmessage")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Blockmessage { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcAction")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Curid")]
        [InverseProperty("OlcAction")]
        public virtual OlsCurrency? Cur { get; set; }
        [ForeignKey("Discountaid")]
        [InverseProperty("InverseDiscounta")]
        public virtual OlcAction? Discounta { get; set; }
        [InverseProperty("Discounta")]
        public virtual ICollection<OlcAction> InverseDiscounta { get; set; }
    }
}