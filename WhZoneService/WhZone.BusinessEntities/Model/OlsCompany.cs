using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsCompany : Base.BusinessMasterEntity
    {
        public OlsCompany()
        {
            OlcWhztranheads = new HashSet<OlcWhztranhead>();
            OlsPordheads = new HashSet<OlsPordhead>();
            OlsSordheads = new HashSet<OlsSordhead>();
            OlsStheads = new HashSet<OlsSthead>();
        }

        public int Cmpid { get; set; }
        public string Cmpcode { get; set; } = null!;
        public string Sname { get; set; } = null!;
        public string? Codacode { get; set; }
        public string? Codatempcode { get; set; }
        public string Abbr { get; set; } = null!;
        public int Def { get; set; }
        public int Partnid { get; set; }
        public string Countryid { get; set; } = null!;
        public string Homecurid { get; set; } = null!;
        public string? Dualcurid { get; set; }
        public int Ratesrctype { get; set; }
        public int Selprccodetype { get; set; }
        public int Sordstselprcsrctype { get; set; }
        public int? Sordstaccesstype { get; set; }
        public int? Sordselprcsrctype { get; set; }
        public string? Note { get; set; }
        public int? Sinvdocnumpartnpropgrpid { get; set; }
        public int? Itembarcodemandtype { get; set; }
        public int? Sinvmodtype { get; set; }
        public int? Sinvusecorrtype { get; set; }
        public string? Stcommoncmpcodes { get; set; }
        public string? Grp { get; set; }
        public string? Xmldata { get; set; }

        public virtual ICollection<OlcWhztranhead> OlcWhztranheads { get; set; }
        public virtual ICollection<OlsPordhead> OlsPordheads { get; set; }
        public virtual ICollection<OlsSordhead> OlsSordheads { get; set; }
        public virtual ICollection<OlsSthead> OlsStheads { get; set; }
    }
}
