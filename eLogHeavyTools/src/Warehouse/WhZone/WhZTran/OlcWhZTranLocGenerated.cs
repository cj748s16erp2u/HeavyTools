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
    [Table("olc_whztranloc")]
    public partial class OlcWhZTranLoc : Entity<OlcWhZTranLoc>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whztlocid"><see cref="int?" /> whztlocid</param>
        public static OlcWhZTranLoc Load(int? whztlocid)
        {
            return Load(new Key(new Field[] { FieldWhztlocid }, new object[] { whztlocid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whztlocid")]
        public static Field FieldWhztlocid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whztlocid
        {
            get { return (int?)this[FieldWhztlocid]; }
            set { this[FieldWhztlocid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whztid")]
        public static Field FieldWhztid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whztid
        {
            get { return (int?)this[FieldWhztid]; }
            set { this[FieldWhztid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whztlineid")]
        public static Field FieldWhztlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whztlineid
        {
            get { return (int?)this[FieldWhztlineid]; }
            set { this[FieldWhztlineid] = value; }
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
        /// ? - int
        /// </summary>
        [Field("whztltype")]
        public static Field FieldWhztltype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whztltype
        {
            get { return (int?)this[FieldWhztltype]; }
            set { this[FieldWhztltype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("ordqty")]
        public static Field FieldOrdqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Ordqty
        {
            get { return (Decimal?)this[FieldOrdqty]; }
            set { this[FieldOrdqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("dispqty")]
        public static Field FieldDispqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Dispqty
        {
            get { return (Decimal?)this[FieldDispqty]; }
            set { this[FieldDispqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("movqty")]
        public static Field FieldMovqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Movqty
        {
            get { return (Decimal?)this[FieldMovqty]; }
            set { this[FieldMovqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("addusrid")]
        public static Field FieldAddusrid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Addusrid
        {
            get { return new StringN(this[FieldAddusrid]); }
            set { this[FieldAddusrid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("adddate")]
        public static Field FieldAdddate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Adddate
        {
            get { return (DateTime?)this[FieldAdddate]; }
            set { this[FieldAdddate] = value; }
        }

        #endregion
    }
}
