using System;
using System.Collections.Generic;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhztst : Base.Entity
    {
        public int Whztid { get; set; }
        public int Whztlineid { get; set; }
        public int Stid { get; set; }
        public int Stlineid { get; set; }

        public virtual OlsSthead St { get; set; } = null!;
        public virtual OlsStline Stline { get; set; } = null!;
        public virtual OlcWhztranhead Whzt { get; set; } = null!;
        public virtual OlcWhztranline Whztline { get; set; } = null!;
    }
}
