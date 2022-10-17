using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_sordline")]
    public partial class OlcSordLine : Entity<OlcSordLine>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="sordlineid"><see cref="int?" /> sordlineid</param>
        public static OlcSordLine Load(int? sordlineid)
        {
            return Load(new Key(new Field[] { FieldSordlineid }, new object[] { sordlineid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sordlineid")]
        public static Field FieldSordlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sordlineid
        {
            get { return (int?)this[FieldSordlineid]; }
            set { this[FieldSordlineid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("confqty")]
        public static Field FieldConfqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Confqty
        {
            get { return (Decimal?)this[FieldConfqty]; }
            set { this[FieldConfqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("confdeldate")]
        public static Field FieldConfdeldate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Confdeldate
        {
            get { return (DateTime?)this[FieldConfdeldate]; }
            set { this[FieldConfdeldate] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - xml
        /// </summary>
        [Field("data")]
        public static Field FieldData { get; protected set; }
        /// <summary>
        /// ? - xml
        /// </summary>
        public StringN Data
        {
            get { return new StringN(this[FieldData]); }
            set { this[FieldData] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("preordersordlineid")]
        public static Field FieldPreordersordlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Preordersordlineid
        {
            get { return (int?)this[FieldPreordersordlineid]; }
            set { this[FieldPreordersordlineid] = value; }
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
