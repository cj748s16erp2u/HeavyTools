using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_partner")]
    public partial class OlcPartner : Entity<OlcPartner>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="partnid"><see cref="int?" /> partnid</param>
        public static OlcPartner Load(int? partnid)
        {
            return Load(new Key(new Field[] { FieldPartnid }, new object[] { partnid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("partnid")]
        public static Field FieldPartnid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Partnid
        {
            get { return (int?)this[FieldPartnid]; }
            set { this[FieldPartnid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("oldcode")]
        public static Field FieldOldcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Oldcode
        {
            get { return new StringN(this[FieldOldcode]); }
            set { this[FieldOldcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("wsemail")]
        public static Field FieldWsemail { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Wsemail
        {
            get { return new StringN(this[FieldWsemail]); }
            set { this[FieldWsemail] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("inglngid")]
        public static Field FieldInvlngid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Invlngid
        {
            get { return new StringN(this[FieldInvlngid]); }
            set { this[FieldInvlngid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("loyaltycardno")]
        public static Field FieldLoyaltycardno { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Loyaltycardno
        {
            get { return new StringN(this[FieldLoyaltycardno]); }
            set { this[FieldLoyaltycardno] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("loyaltydiscpercnt")]
        public static Field FieldLoyaltydiscpercnt { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public decimal? Loyaltydiscpercnt
        {
            get { return (decimal?)this[FieldLoyaltydiscpercnt]; }
            set { this[FieldLoyaltydiscpercnt] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("debcredsumvalue")]
        public static Field FieldDebcredsumvalue { get; protected set; }
        public decimal? Debcredsumvalue
        {
            get { return (decimal?)this[FieldDebcredsumvalue]; }
            set { this[FieldDebcredsumvalue] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("regreprempid")]
        public static Field FieldRegreprempid { get; protected set; }
        public int? Regreprempid
        {
            get { return (int?)this[FieldRegreprempid]; }
            set { this[FieldRegreprempid] = value; }
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
