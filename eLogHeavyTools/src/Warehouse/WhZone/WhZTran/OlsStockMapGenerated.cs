using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("ols_stockmap")]
    public partial class OlsStockMap : Entity<OlsStockMap>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whid"><see cref="StringN" /> whid</param>
        /// <param name="locid"><see cref="StringN" /> locid</param>
        /// <param name="itemid"><see cref="int?" /> itemid</param>
        public static OlsStockMap Load(StringN whid, StringN locid, int? itemid)
        {
            return Load(new Key(new Field[] { FieldWhid, FieldLocid, FieldItemid }, new object[] { whid, locid, itemid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("whid")]
        public static Field FieldWhid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Whid
        {
            get { return new StringN(this[FieldWhid]); }
            set { this[FieldWhid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("locid")]
        public static Field FieldLocid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Locid
        {
            get { return new StringN(this[FieldLocid]); }
            set { this[FieldLocid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("itemid")]
        public static Field FieldItemid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Itemid
        {
            get { return (int?)this[FieldItemid]; }
            set { this[FieldItemid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("recqty")]
        public static Field FieldRecqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Recqty
        {
            get { return (Decimal?)this[FieldRecqty]; }
            set { this[FieldRecqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("actqty")]
        public static Field FieldActqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Actqty
        {
            get { return (Decimal?)this[FieldActqty]; }
            set { this[FieldActqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("resqty")]
        public static Field FieldResqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Resqty
        {
            get { return (Decimal?)this[FieldResqty]; }
            set { this[FieldResqty] = value; }
        }

        #endregion
    }
}
