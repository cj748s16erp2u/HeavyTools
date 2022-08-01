using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlsSysval : Base.Entity
    {
        public OlsSysval()
        {
        }

        public string Sysvalid { get; set; } = null!;
        public int? Valueint { get; set; }
        public decimal? Valuenum { get; set; }
        public string? Valuestr { get; set; }
        public DateTime? Valuedate { get; set; }
        public object? Valuevar { get; set; }
    }
}
