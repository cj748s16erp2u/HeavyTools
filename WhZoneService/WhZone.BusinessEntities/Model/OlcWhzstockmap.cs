using System;
using System.Collections.Generic;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Comparers;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model
{
    public partial class OlcWhzstockmap : Base.Entity, IWhZStockMapKey
    {
        public static IEqualityComparer<IWhZStockMapKey> Comparer { get; } = WhZStockMapKeyComparer.Instance;

        public int Whzstockmapid { get; set; }
        public int Itemid { get; set; }
        public string Whid { get; set; } = null!;
        public int? Whzoneid { get; set; }
        public int Whlocid { get; set; }
        public decimal Recqty { get; set; }
        public decimal Reqqty { get; set; }
        public decimal Actqty { get; set; }
        public decimal Resqty { get; set; }
        public decimal? Provqty { get; set; }

        public virtual OlsItem Item { get; set; } = null!;
        public virtual OlsWarehouse Wh { get; set; } = null!;
        public virtual OlcWhlocation Whloc { get; set; } = null!;
        public virtual OlcWhzone? Whzone { get; set; }

        public IWhZStockMapKey CreateKey()
        {
            return new WhZStockMapKey
            {
                Itemid = this.Itemid,
                Whid = this.Whid,
                Whzoneid = this.Whzoneid,
                Whlocid = this.Whlocid,
            };
        }

        public string KeyString()
        {
            return $"ItemId: {this.Itemid}, Whid: {this.Whid}, Whzoneid: {this.Whzoneid}, Whlocid: {this.Whlocid}";
        }

        bool IEquatable<IWhZStockMapKey>.Equals(IWhZStockMapKey? other) => Comparer.Equals(this, other);

        IWhZStockKey IWhZStockKey.CreateKey()
        {
            return new WhZStockKey
            {
                Itemid = this.Itemid,
                Whid = this.Whid,
                Whzoneid = this.Whzoneid,
            };
        }

        bool IEquatable<IWhZStockKey>.Equals(IWhZStockKey? other) => WhZStockKeyComparer.Instance.Equals(this, other);
    }
}
