using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFC = Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    [Table("olc_whztranhead")]
    [EFC.Index("Stid", Name = "idx_olc_whztranhead_stid")]
    public partial class OlcWhztranhead
    {
        public OlcWhztranhead()
        {
            OlcWhztranlines = new HashSet<OlcWhztranline>();
            OlcWhztranlocs = new HashSet<OlcWhztranloc>();
        }

        [Key]
        [Column("whztid")]
        public int Whztid { get; set; }
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("whzttype")]
        public int Whzttype { get; set; }
        [Column("whztdate", TypeName = "datetime")]
        public DateTime Whztdate { get; set; }
        [Column("fromwhzid")]
        public int? Fromwhzid { get; set; }
        [Column("towhzid")]
        public int? Towhzid { get; set; }
        [Column("closeusrid")]
        [StringLength(12)]
        [EFC.Unicode(false)]
        public string? Closeusrid { get; set; }
        [Column("closedate", TypeName = "datetime")]
        public DateTime? Closedate { get; set; }
        [Column("whztstat")]
        public int Whztstat { get; set; }
        [Column("note")]
        [StringLength(1000)]
        [EFC.Unicode(false)]
        public string? Note { get; set; }
        [Column("stid")]
        public int? Stid { get; set; }
        [Column("sordid")]
        public int? Sordid { get; set; }
        [Column("pordid")]
        public int? Pordid { get; set; }
        [Column("taskid")]
        public int? Taskid { get; set; }
        [Column("gen")]
        public int? Gen { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlcWhztranheadAddusrs")]
        public virtual CfwUser Addusr { get; set; } = null!;
        [ForeignKey("Closeusrid")]
        [InverseProperty("OlcWhztranheadCloseusrs")]
        public virtual CfwUser? Closeusr { get; set; }
        [ForeignKey("Cmpid")]
        [InverseProperty("OlcWhztranheads")]
        public virtual OlsCompany Cmp { get; set; } = null!;
        [ForeignKey("Fromwhzid")]
        [InverseProperty("OlcWhztranheadFromwhzs")]
        public virtual OlcWhzone? Fromwhz { get; set; }
        [ForeignKey("Stid")]
        [InverseProperty("OlcWhztranheads")]
        public virtual OlsSthead? St { get; set; }
        [ForeignKey("Towhzid")]
        [InverseProperty("OlcWhztranheadTowhzs")]
        public virtual OlcWhzone? Towhz { get; set; }
        [InverseProperty("Whzt")]
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
        [InverseProperty("Whzt")]
        public virtual ICollection<OlcWhztranloc> OlcWhztranlocs { get; set; }
    }
}
