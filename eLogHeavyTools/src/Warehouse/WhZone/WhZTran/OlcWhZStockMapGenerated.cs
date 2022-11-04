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
    [Table("olc_whzstockmap")]
    public partial class OlcWhZStockMap : Entity<OlcWhZStockMap>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whzstockmapid"><see cref="int?" /> whzstockmapid</param>
        public static OlcWhZStockMap Load(int? whzstockmapid)
        {
            return Load(new Key(new Field[] { FieldWhzstockmapid }, new object[] { whzstockmapid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whzstockmapid")]
        public static Field FieldWhzstockmapid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whzstockmapid
        {
            get { return (int?)this[FieldWhzstockmapid]; }
            set { this[FieldWhzstockmapid] = value; }
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
        /// ? - int
        /// </summary>
        [Field("whzoneid")]
        public static Field FieldWhzoneid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whzoneid
        {
            get { return (int?)this[FieldWhzoneid]; }
            set { this[FieldWhzoneid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whlocid")]
        public static Field FieldWhlocid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whlocid
        {
            get { return (int?)this[FieldWhlocid]; }
            set { this[FieldWhlocid] = value; }
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
        [Field("reqqty")]
        public static Field FieldReqqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Reqqty
        {
            get { return (Decimal?)this[FieldReqqty]; }
            set { this[FieldReqqty] = value; }
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

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("provqty")]
        public static Field FieldProvqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Provqty
        {
            get { return (Decimal?)this[FieldProvqty]; }
            set { this[FieldProvqty] = value; }
        }

        #endregion
    }
}
