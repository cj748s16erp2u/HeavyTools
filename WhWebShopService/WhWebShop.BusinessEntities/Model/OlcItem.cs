using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_item")]
    public partial class OlcItem : Entity
    {
        [Key]
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("imsid")]
        public int? Imsid { get; set; }
        [Column("isrlid")]
        public int? Isrlid { get; set; }
        [Column("colortype1")]
        public int? Colortype1 { get; set; }
        [Column("colorname")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Colorname { get; set; }
        [Column("colortype2")]
        public int? Colortype2 { get; set; }
        [Column("colortype3")]
        public int? Colortype3 { get; set; }
        [Column("materialtype")]
        public int? Materialtype { get; set; }
        [Column("patterntype")]
        public int? Patterntype { get; set; }
        [Column("patterntype2")]
        public int? Patterntype2 { get; set; }
        [Column("catalogpagenumber")]
        public int? Catalogpagenumber { get; set; }
        [Column("iscollectionarticlenumber")]
        public int? Iscollectionarticlenumber { get; set; }
        [Column("webcategory")]
        [StringLength(6)]
        [Unicode(false)]
        public string? Webcategory { get; set; }
        [Column("note")]
        [StringLength(2000)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Addusrid { get; set; }
        [Column("adddate", TypeName = "datetime")]
        public DateTime? Adddate { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcItem")]
        public virtual CfwUser? Addusr { get; set; }
        [ForeignKey("Imsid")]
        [InverseProperty("OlcItem")]
        public virtual OlcItemmodelseason? Ims { get; set; }
    }
}