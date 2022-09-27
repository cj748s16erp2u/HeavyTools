using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.GiftCard
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_giftcardlog")]
    public partial class OlcGiftCardLog : Entity<OlcGiftCardLog>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="gclid"><see cref="int?" /> gclid</param>
        public static OlcGiftCardLog Load(int? gclid)
        {
            return Load(new Key(new Field[] { FieldGclid }, new object[] { gclid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("gclid")]
        public static Field FieldGclid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Gclid
        {
            get { return (int?)this[FieldGclid]; }
            set { this[FieldGclid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("gcid")]
        public static Field FieldGcid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Gcid
        {
            get { return (int?)this[FieldGcid]; }
            set { this[FieldGcid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sinvlineid")]
        public static Field FieldSinvlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sinvlineid
        {
            get { return (int?)this[FieldSinvlineid]; }
            set { this[FieldSinvlineid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sinvid")]
        public static Field FieldSinvid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sinvid
        {
            get { return (int?)this[FieldSinvid]; }
            set { this[FieldSinvid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("val")]
        public static Field FieldVal { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Val
        {
            get { return (Decimal?)this[FieldVal]; }
            set { this[FieldVal] = value; }
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

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("delstat")]
        public static Field FieldDelstat { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Delstat
        {
            get { return (int?)this[FieldDelstat]; }
            set { this[FieldDelstat] = value; }
        }

        #endregion
    }
}
