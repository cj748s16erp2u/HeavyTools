using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhzloc : Base.BusinessEntity
    {
        public int Whztlocid { get; set; }
        public int Whztid { get; set; }
        public int Whztlineid { get; set; }
        public string Whid { get; set; } = null!;
        public int Whzoneid { get; set; }
        public int Whlocid { get; set; }
        public int Whztltype { get; set; }
        public decimal Dispqty { get; set; }
        public decimal Movqty { get; set; }

        public virtual OlsWarehouse Wh { get; set; } = null!;
        public virtual OlcWhlocation Whloc { get; set; } = null!;
        public virtual OlcWhzone Whzone { get; set; } = null!;
        public virtual OlcWhztranhead Whzt { get; set; } = null!;
        public virtual OlcWhztranline Whztline { get; set; } = null!;
    }
}
