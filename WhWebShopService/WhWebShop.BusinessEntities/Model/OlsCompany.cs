﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_company")]
    [Index("Cmpcode", Name = "idx_ols_company_cmpcode", IsUnique = true)]
    public partial class OlsCompany : BusinessMasterEntity
    {
        [Key]
        [Column("cmpid")]
        public int Cmpid { get; set; }
        [Column("cmpcode")]
        [StringLength(12)]
        [Unicode(false)]
        public string Cmpcode { get; set; } = null!;
        [Column("sname")]
        [StringLength(20)]
        [Unicode(false)]
        public string Sname { get; set; } = null!;
        [Column("codacode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Codacode { get; set; }
        [Column("codatempcode")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Codatempcode { get; set; }
        [Column("abbr")]
        [StringLength(5)]
        [Unicode(false)]
        public string Abbr { get; set; } = null!;
        [Column("def")]
        public int Def { get; set; }
        [Column("partnid")]
        public int Partnid { get; set; }
        [Column("countryid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Countryid { get; set; } = null!;
        [Column("homecurid")]
        [StringLength(12)]
        [Unicode(false)]
        public string Homecurid { get; set; } = null!;
        [Column("dualcurid")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Dualcurid { get; set; }
        [Column("ratesrctype")]
        public int Ratesrctype { get; set; }
        [Column("selprccodetype")]
        public int Selprccodetype { get; set; }
        [Column("sordstselprcsrctype")]
        public int Sordstselprcsrctype { get; set; }
        [Column("sordstaccesstype")]
        public int? Sordstaccesstype { get; set; }
        [Column("sordselprcsrctype")]
        public int? Sordselprcsrctype { get; set; }
        [Column("note")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Note { get; set; }
        [Column("sinvdocnumpartnpropgrpid")]
        public int? Sinvdocnumpartnpropgrpid { get; set; }
        [Column("itembarcodemandtype")]
        public int? Itembarcodemandtype { get; set; }
        [Column("sinvmodtype")]
        public int? Sinvmodtype { get; set; }
        [Column("sinvusecorrtype")]
        public int? Sinvusecorrtype { get; set; }
        [Column("stcommoncmpcodes")]
        [StringLength(2000)]
        [Unicode(false)]
        public string? Stcommoncmpcodes { get; set; }
        [Column("grp")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Grp { get; set; }
        [Column("xmldata", TypeName = "xml")]
        public string? Xmldata { get; set; }

        [ForeignKey("Addusrid")]
        [InverseProperty("OlsCompanies")]
        public override CfwUser Addusr { get; set; } = null!;
    }
}
