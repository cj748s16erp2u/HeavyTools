using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsSordhead : Base.BusinessEntity
    {
        public OlsSordhead()
        {
            OlcWhztranheads = new HashSet<OlcWhztranhead>();
            OlsSordlines = new HashSet<OlsSordline>();
        }

        public int Sordid { get; set; }
        public string Sorddocid { get; set; } = null!;
        public int Cmpid { get; set; }
        public string Docnum { get; set; } = null!;
        public string? Ref1 { get; set; }
        public int Sordtype { get; set; }
        public DateTime Sorddate { get; set; }
        public int Partnid { get; set; }
        public int Addrid { get; set; }
        public string? Whid { get; set; }
        public string Curid { get; set; } = null!;
        public string? Paymid { get; set; }
        public int? Paycid { get; set; }
        public int Sordstat { get; set; }
        public string? Note { get; set; }
        public int? Projid { get; set; }
        public string? Paritycode { get; set; }
        public string? Parityplace { get; set; }
        public int Gen { get; set; }
        public string Langid { get; set; } = null!;
        public int Lastlinenum { get; set; }

        public virtual OlsCompany Cmp { get; set; } = null!;
        public virtual OlsWarehouse? Wh { get; set; }
        public virtual ICollection<OlcWhztranhead> OlcWhztranheads { get; set; }
        public virtual ICollection<OlsSordline> OlsSordlines { get; set; }
    }
}
