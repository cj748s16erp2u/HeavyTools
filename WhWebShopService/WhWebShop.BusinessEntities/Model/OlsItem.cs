using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_item")]
    [Index("Itemcode", Name = "idx_ols_item_itemcode", IsUnique = true)]
    [Index("Name01", Name = "idx_ols_item_name01")]
    public partial class OlsItem : Entity
    {
        public OlsItem()
        {
            InverseRootitem = new HashSet<OlsItem>();
            OlcPrctable = new HashSet<OlcPrctable>();
            OlcPrctableCurrent = new HashSet<OlcPrctableCurrent>();
            OlsReserve = new HashSet<OlsReserve>();
            OlsSordline = new HashSet<OlsSordline>();
            OlsTmpSordst = new HashSet<OlsTmpSordst>();
        }

        [Key]
        [Column("itemid")]
        public int Itemid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("itemcode")]
        [StringLength(25)]
        [Unicode(false)]
        public string Itemcode { get; set; } = null!;
        [Column("cmpcodes")]
        [StringLength(2000)]
        [Unicode(false)]
        public string Cmpcodes { get; set; } = null!;
        [Column("type")]
        public int Type { get; set; }
        [Column("itemgrpid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Itemgrpid { get; set; } = null!;
        [Column("name01")]
        [StringLength(100)]
        [Unicode(false)]
        public string Name01 { get; set; } = null!;
        [Column("name02")]
        [StringLength(100)]
        public string? Name02 { get; set; }
        [Column("name03")]
        [StringLength(100)]
        public string? Name03 { get; set; }
        [Column("sname")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Sname { get; set; }
        [Column("custtarid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Custtarid { get; set; }
        [Column("unitid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Unitid { get; set; } = null!;
        [Column("releasedate", TypeName = "datetime")]
        public DateTime Releasedate { get; set; }
        [Column("rootitemid")]
        public int? Rootitemid { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("uqcardtype")]
        public int? Uqcardtype { get; set; }
        [Column("netweight", TypeName = "numeric(19, 6)")]
        public decimal? Netweight { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsItem")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Rootitemid")]
        [InverseProperty("InverseRootitem")]
        public virtual OlsItem? Rootitem { get; set; }
        [InverseProperty("Rootitem")]
        public virtual ICollection<OlsItem> InverseRootitem { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlcPrctable> OlcPrctable { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlcPrctableCurrent> OlcPrctableCurrent { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlsReserve> OlsReserve { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlsSordline> OlsSordline { get; set; }
        [InverseProperty("Item")]
        public virtual ICollection<OlsTmpSordst> OlsTmpSordst { get; set; }
    }
}