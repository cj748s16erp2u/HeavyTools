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
    [Table("olc_actioncouponnumber")]
    public partial class OlcActionCouponNumber : Entity<OlcActionCouponNumber>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="acnid"><see cref="int?" /> acnid</param>
        public static OlcActionCouponNumber Load(int? acnid)
        {
            return Load(new Key(new Field[] { FieldAcnid }, new object[] { acnid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("acnid")]
        public static Field FieldAcnid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Acnid
        {
            get { return (int?)this[FieldAcnid]; }
            set { this[FieldAcnid] = value; }
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
        [Field("couponnumber")]
        public static Field FieldCouponnumber { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Couponnumber
        {
            get { return new StringN(this[FieldCouponnumber]); }
            set { this[FieldCouponnumber] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("used")]
        public static Field FieldUsed { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Used
        {
            get { return (int?)this[FieldUsed]; }
            set { this[FieldUsed] = value; }
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
