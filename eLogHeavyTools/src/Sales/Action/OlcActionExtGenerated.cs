using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Action
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_actionext")]
    public partial class OlcActionExt : Entity<OlcActionExt>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="axid"><see cref="int?" /> axid</param>
        public static OlcActionExt Load(int? axid)
        {
            return Load(new Key(new Field[] { FieldAxid }, new object[] { axid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("axid")]
        public static Field FieldAxid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Axid
        {
            get { return (int?)this[FieldAxid]; }
            set { this[FieldAxid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("aid")]
        public static Field FieldAid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Aid
        {
            get { return (int?)this[FieldAid]; }
            set { this[FieldAid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("filteritems")]
        public static Field FieldFilteritems { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Filteritems
        {
            get { return new StringN(this[FieldFilteritems]); }
            set { this[FieldFilteritems] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("filteritemsblock")]
        public static Field FieldFilteritemsblock { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Filteritemsblock
        {
            get { return new StringN(this[FieldFilteritemsblock]); }
            set { this[FieldFilteritemsblock] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("count")]
        public static Field FieldCount { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Count
        {
            get { return (int?)this[FieldCount]; }
            set { this[FieldCount] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discounttype")]
        public static Field FieldDiscounttype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discounttype
        {
            get { return (int?)this[FieldDiscounttype]; }
            set { this[FieldDiscounttype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("discountval")]
        public static Field FieldDiscountval { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Discountval
        {
            get { return (Decimal?)this[FieldDiscountval]; }
            set { this[FieldDiscountval] = value; }
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
