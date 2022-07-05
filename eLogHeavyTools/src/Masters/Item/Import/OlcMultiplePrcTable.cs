using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Import
{
    class OlcMultiplePrcTable : VirtualEntity<OlcMultiplePrcTable>
    {
        public const string SESSIONCNTID = "OlcMultiplePrcTable";

        [Field("itemcode", DataType = FieldType.String, IsPrimaryKey = true)]
        public static Field FieldItemCode { get; protected set; }
        public string Itemid
        {
            get { return eProjectWeb.Framework.ConvertUtils.ToString(this[FieldItemCode]); }
            set { this[FieldItemCode] = value; }
        }

        [Field("Ar1", DataType = FieldType.Decimal)]
        public static Field FieldAr1 { get; protected set; }
        [Field("Ar2", DataType = FieldType.Decimal)]
        public static Field FieldAr2 { get; protected set; }
        [Field("Ar3", DataType = FieldType.Decimal)]
        public static Field FieldAr3 { get; protected set; }
        [Field("Ar4", DataType = FieldType.Decimal)]
        public static Field FieldAr4 { get; protected set; }
        [Field("Ar5", DataType = FieldType.Decimal)]
        public static Field FieldAr5 { get; protected set; }
        [Field("Ar6", DataType = FieldType.Decimal)]
        public static Field FieldAr6 { get; protected set; }
        [Field("Ar7", DataType = FieldType.Decimal)]
        public static Field FieldAr7 { get; protected set; }
        [Field("Ar8", DataType = FieldType.Decimal)]
        public static Field FieldAr8 { get; protected set; }
        [Field("Ar9", DataType = FieldType.Decimal)]
        public static Field FieldAr9 { get; protected set; }
        [Field("Ar10", DataType = FieldType.Decimal)]
        public static Field FieldAr10 { get; protected set; }
        [Field("Ar11", DataType = FieldType.Decimal)]
        public static Field FieldAr11 { get; protected set; }
        [Field("Ar12", DataType = FieldType.Decimal)]
        public static Field FieldAr12 { get; protected set; }
        [Field("Ar13", DataType = FieldType.Decimal)]
        public static Field FieldAr13 { get; protected set; }
        [Field("Ar14", DataType = FieldType.Decimal)]
        public static Field FieldAr14 { get; protected set; }
        [Field("Ar15", DataType = FieldType.Decimal)]
        public static Field FieldAr15 { get; protected set; }
        [Field("Ar16", DataType = FieldType.Decimal)]
        public static Field FieldAr16 { get; protected set; }
        [Field("Ar17", DataType = FieldType.Decimal)]
        public static Field FieldAr17 { get; protected set; }
        [Field("Ar18", DataType = FieldType.Decimal)]
        public static Field FieldAr18 { get; protected set; }



        public decimal? Ar1 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr1]); } set { this[FieldAr1] = value; } }
        public decimal? Ar2 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr2]); } set { this[FieldAr2] = value; } }
        public decimal? Ar3 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr3]); } set { this[FieldAr3] = value; } }
        public decimal? Ar4 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr4]); } set { this[FieldAr4] = value; } }
        public decimal? Ar5 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr5]); } set { this[FieldAr5] = value; } }
        public decimal? Ar6 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr6]); } set { this[FieldAr6] = value; } }
        public decimal? Ar7 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr7]); } set { this[FieldAr7] = value; } }
        public decimal? Ar8 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr8]); } set { this[FieldAr8] = value; } }
        public decimal? Ar9 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr9]); } set { this[FieldAr9] = value; } }
        public decimal? Ar10 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr10]); } set { this[FieldAr10] = value; } }
        public decimal? Ar11 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr11]); } set { this[FieldAr11] = value; } }
        public decimal? Ar12 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr12]); } set { this[FieldAr12] = value; } }
        public decimal? Ar13 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr13]); } set { this[FieldAr13] = value; } }
        public decimal? Ar14 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr14]); } set { this[FieldAr14] = value; } }
        public decimal? Ar15 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr15]); } set { this[FieldAr15] = value; } }
        public decimal? Ar16 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr16]); } set { this[FieldAr16] = value; } }
        public decimal? Ar17 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr17]); } set { this[FieldAr17] = value; } }
        public decimal? Ar18 { get { return eProjectWeb.Framework.ConvertUtils.ToDecimal(this[FieldAr18]); } set { this[FieldAr18] = value; } }

    }
}
