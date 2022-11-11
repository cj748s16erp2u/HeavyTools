using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_partnvattyp")]
    public partial class OlsPartnvattyp : Entity
    {
        public OlsPartnvattyp()
        {
            OlsPartner = new HashSet<OlsPartner>();
            OlsSinvhead = new HashSet<OlsSinvhead>();
        }

        [Key]
        [Column("ptvattypid")]
        [StringLength(30)]
        [Unicode(false)]
        public string Ptvattypid { get; set; } = null!;
        [Column("name")]
        [StringLength(100)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("note")]
        [StringLength(500)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("vistype")]
        public int Vistype { get; set; }
        [Column("navcateg")]
        public int? Navcateg { get; set; }
        [Column("seqno")]
        public int? Seqno { get; set; }
        [Column("xmldata", TypeName = "xml")]
        public string? Xmldata { get; set; }
        [Column("addusrid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Addusrid { get; set; } = null!;
        [Column("adddate", TypeName = "datetime")]
        public DateTime Adddate { get; set; }
        [Column("delstat")]
        public int Delstat { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsPartnvattyp")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [InverseProperty("Ptvattyp")]
        public virtual ICollection<OlsPartner> OlsPartner { get; set; }
        [InverseProperty("Ptvattyp")]
        public virtual ICollection<OlsSinvhead> OlsSinvhead { get; set; }
    }
}