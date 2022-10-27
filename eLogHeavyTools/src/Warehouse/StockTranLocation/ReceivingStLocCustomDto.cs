using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomDto : VirtualEntity<ReceivingStLocCustomDto>
    {
        [Field("Whztlocid", DataType = FieldType.Integer, IsPrimaryKey = true)]
        public static Field FieldWhztlocid { get; protected set; }
        public int? Whztlocid
        {
            get => (int?)this[FieldWhztlocid];
            set => this[FieldWhztlocid] = value;
        }

        [Field("Whztid", DataType = FieldType.Integer)]
        public static Field FieldWhztid { get; protected set; }
        public int? Whztid
        {
            get => (int?)this[FieldWhztid];
            set => this[FieldWhztid] = value;
        }

        [Field("Whztlineid", DataType = FieldType.Integer)]
        public static Field FieldWhztlineid { get; protected set; }
        public int? Whztlineid
        {
            get => (int?)this[FieldWhztlineid];
            set => this[FieldWhztlineid] = value;
        }

        [Field("Whid", DataType = FieldType.String)]
        public static Field FieldWhid { get; protected set; }
        public StringN Whid
        {
            get => new StringN(this[FieldWhid]);
            set => this[FieldWhid] = value;
        }

        [Field("Whname", DataType = FieldType.String)]
        public static Field FieldWhname { get; protected set; }
        public StringN Whname
        {
            get => new StringN(this[FieldWhname]);
            set => this[FieldWhname] = value;
        }

        [Field("Whzoneid", DataType = FieldType.Integer)]
        public static Field FieldWhzoneid { get; protected set; }
        public int? Whzoneid
        {
            get => (int?)this[FieldWhzoneid];
            set => this[FieldWhzoneid] = value;
        }

        [Field("Whzonecode", DataType = FieldType.String)]
        public static Field FieldWhzonecode { get; protected set; }
        public StringN Whzonecode
        {
            get => new StringN(this[FieldWhzonecode]);
            set => this[FieldWhzonecode] = value;
        }

        [Field("Whzonename", DataType = FieldType.String)]
        public static Field FieldWhzonename { get; protected set; }
        public StringN Whzonename
        {
            get => new StringN(this[FieldWhzonename]);
            set => this[FieldWhzonename] = value;
        }

        [Field("Whlocid", DataType = FieldType.Integer)]
        public static Field FieldWhlocid { get; protected set; }
        public int? Whlocid
        {
            get => (int?)this[FieldWhlocid];
            set => this[FieldWhlocid] = value;
        }

        [Field("Whloccode", DataType = FieldType.String)]
        public static Field FieldWhloccode { get; protected set; }
        public StringN Whloccode
        {
            get => new StringN(this[FieldWhloccode]);
            set => this[FieldWhloccode] = value;
        }

        [Field("Whlocname", DataType = FieldType.String)]
        public static Field FieldWhlocname { get; protected set; }
        public StringN Whlocname
        {
            get => new StringN(this[FieldWhlocname]);
            set => this[FieldWhlocname] = value;
        }

        [Field("itemid", DataType = FieldType.Integer)]
        public static Field FieldItemid { get; protected set; }
        public int? Itemid
        {
            get => (int?)this[FieldItemid];
            set => this[FieldItemid] = value;
        }

        [Field("Itemcode", DataType = FieldType.String)]
        public static Field FieldItemcode { get; protected set; }
        public StringN Itemcode
        {
            get => new StringN(this[FieldItemcode]);
            set => this[FieldItemcode] = value;
        }

        [Field("Itemname01", DataType = FieldType.String)]
        public static Field FieldItemname01 { get; protected set; }
        public StringN Itemname01
        {
            get => new StringN(this[FieldItemname01]);
            set => this[FieldItemname01] = value;
        }

        [Field("Whztltype", DataType = FieldType.Integer)]
        public static Field FieldWhztltype { get; protected set; }
        public int? Whztltype
        {
            get => (int?)this[FieldWhztltype];
            set => this[FieldWhztltype] = value;
        }

        [Field("Ordqty", DataType = FieldType.Decimal)]
        public static Field FieldOrdqty { get; protected set; }
        public decimal? Ordqty
        {
            get => (decimal?)this[FieldOrdqty];
            set => this[FieldOrdqty] = value;
        }

        [Field("Dispqty", DataType = FieldType.Decimal)]
        public static Field FieldDispqty { get; protected set; }
        public decimal? Dispqty
        {
            get => (decimal?)this[FieldDispqty];
            set => this[FieldDispqty] = value;
        }

        [Field("Movqty", DataType = FieldType.Decimal)]
        public static Field FieldMovqty { get; protected set; }
        public decimal? Movqty
        {
            get => (decimal?)this[FieldMovqty];
            set => this[FieldMovqty] = value;
        }

        [Field("Addusrid", DataType = FieldType.String)]
        public static Field FieldAddusrid { get; protected set; }
        public StringN Addusrid
        {
            get => new StringN(this[FieldAddusrid]);
            set => this[FieldAddusrid] = value;
        }

        [Field("Adddate", DataType = FieldType.DateTime)]
        public static Field FieldAdddate { get; protected set; }
        public DateTime? Adddate
        {
            get => (DateTime?)this[FieldAdddate];
            set => this[FieldAdddate] = value;
        }
    }
}
