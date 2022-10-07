using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("ols_item")]
    [EFC.Index("Itemcode", Name = "idx_ols_item_itemcode", IsUnique = true)]
    [EFC.Index("Name01", Name = "idx_ols_item_name01")]
    public partial class OlsItem
    {
        public OlsItem()
        {
            InverseRootitem = new HashSet<OlsItem>();
            OlcWhzstockmaps = new HashSet<OlcWhzstockmap>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlsStlines = new HashSet<OlsStline>();
        }

        [Key]
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("itemcode")]
        [StringLength(25)]
        [EFC.Unicode(false)]
        public string Itemcode { get; set; } = null!;
        [Column("cmpcodes")]
        [StringLength(2000)]
        [EFC.Unicode(false)]
        public string Cmpcodes { get; set; } = null!;
        [Column("type")]
        public int Type { get; set; }
        [Column("itemgrpid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Itemgrpid { get; set; } = null!;
        [Column("name01")]
        [StringLength(100)]
        [EFC.Unicode(false)]
        public string Name01 { get; set; } = null!;
        [Column("name02")]
        [StringLength(100)]
        public string? Name02 { get; set; }
        [Column("name03")]
        [StringLength(100)]
        public string? Name03 { get; set; }
        [Column("sname")]
        [StringLength(20)]
        [EFC.Unicode(false)]
        public string? Sname { get; set; }
        [Column("custtarid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Custtarid { get; set; }
        [Column("unitid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string Unitid { get; set; } = null!;
        [Column("releasedate", TypeName = "datetime")]
        public DateTime Releasedate { get; set; }
        [Column("rootitemid")]
        public int? Rootitemid { get; set; }
        [Column("note")]
        [StringLength(200)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }
        [Column("uqcardtype")]
        public int? Uqcardtype { get; set; }
        [Column("netweight", TypeName = "numeric(19, 6)")]
        public decimal? Netweight { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsItems")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Rootitemid")]
        [InverseProperty("InverseRootitem")]
        public virtual OlsItem? Rootitem { get; set; }
        [ForeignKey("Unitid")]
        [InverseProperty("OlsItems")]
        public virtual OlsUnit Unit { get; set; } = null!;
        [InverseProperty("Rootitem")]
        public virtual ICollection<OlsItem> InverseRootitem { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlcWhzstockmap> OlcWhzstockmaps { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlsStline> OlsStlines { get; set; }
    }
}
