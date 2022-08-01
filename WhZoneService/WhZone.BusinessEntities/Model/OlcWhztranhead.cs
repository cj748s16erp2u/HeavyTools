using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhztranhead : Base.BusinessEntity
    {
        public OlcWhztranhead()
        {
            OlcWhzlocs = new HashSet<OlcWhzloc>();
            OlcWhztranlines = new HashSet<OlcWhztranline>();
        }

        public int Whztid { get; set; }
        public int Cmpid { get; set; }
        public int Whzttype { get; set; }
        public DateTime Whztdate { get; set; }
        public int? Fromwhzid { get; set; }
        public int? Towhzid { get; set; }
        public string? Closeusrid { get; set; }
        public DateTime? Closedate { get; set; }
        public int Whztstat { get; set; }
        public string? Note { get; set; }
        public int? Stid { get; set; }
        public int? Sordid { get; set; }
        public int? Pordid { get; set; }
        public int? Taskid { get; set; }
        public int? Gen { get; set; }

        public virtual CfwUser? Closeusr { get; set; }
        public virtual OlsCompany Cmp { get; set; } = null!;
        public virtual OlcWhzone? Fromwhz { get; set; }
        public virtual OlsPordhead? Pord { get; set; }
        public virtual OlsSordhead? Sord { get; set; }
        public virtual OlsSthead? St { get; set; }
        public virtual OlcWhzone? Towhz { get; set; }
        public virtual ICollection<OlcWhzloc> OlcWhzlocs { get; set; }
        public virtual ICollection<OlcWhztranline> OlcWhztranlines { get; set; }
    }
}
