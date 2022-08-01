using System;
using System.Collections.Generic;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhzstock : Base.Entity, IWhZStockKey
    {
        public static IEqualityComparer<IWhZStockKey> Comparer { get; } = WhZStockKeyComparer.Instance;

        public int Whzstockid { get; set; }
        public int Itemid { get; set; }
        public string Whid { get; set; } = null!;
        public int? Whzoneid { get; set; }
        public decimal Recqty { get; set; }
        public decimal Reqqty { get; set; }
        public decimal Actqty { get; set; }
        public decimal Resqty { get; set; }
        public decimal? Provqty { get; set; }

        public virtual OlsItem Item { get; set; } = null!;
        public virtual OlsWarehouse Wh { get; set; } = null!;
        public virtual OlcWhzone? Whzone { get; set; }

        public IWhZStockKey CreateKey()
        {
            return new WhZStockKey
            {
                Itemid = this.Itemid,
                Whid = this.Whid,
                Whzoneid = this.Whzoneid,
            };
        }

        public string KeyString()
        {
            return $"Itemid: {this.Itemid}, Whid: {this.Whid}, Whzoneid: {this.Whzoneid}";
        }

        bool IEquatable<IWhZStockKey>.Equals(IWhZStockKey? other) => Comparer.Equals(this, other);
    }
}
