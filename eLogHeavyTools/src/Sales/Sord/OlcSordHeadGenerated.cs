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
    [Table("olc_sordhead")]
    public partial class OlcSordHead : Entity<OlcSordHead>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="sordid"><see cref="int?" /> sordid</param>
        public static OlcSordHead Load(int? sordid)
        {
            return Load(new Key(new Field[] { FieldSordid }, new object[] { sordid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sordid")]
        public static Field FieldSordid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sordid
        {
            get { return (int?)this[FieldSordid]; }
            set { this[FieldSordid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sordapprovalstat")]
        public static Field FieldSordapprovalstat { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sordapprovalstat
        {
            get { return (int?)this[FieldSordapprovalstat]; }
            set { this[FieldSordapprovalstat] = value; }
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
        /// ? - int
        /// </summary>
        [Field("transfcond")]
        public static Field FieldTransfcond { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Transfcond
        {
            get { return (int?)this[FieldTransfcond]; }
            set { this[FieldTransfcond] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("deliverylocation")]
        public static Field FieldDeliverylocation { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Deliverylocation
        {
            get { return new StringN(this[FieldDeliverylocation]); }
            set { this[FieldDeliverylocation] = value; }
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
        /// ? - numeric
        /// </summary>
        [Field("advval")]
        public static Field FieldAdvval { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Advval
        {
            get { return (Decimal?)this[FieldAdvval]; }
            set { this[FieldAdvval] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("regreprempid")]
        public static Field FieldRegreprempid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Regreprempid
        {
            get { return (int?)this[FieldRegreprempid]; }
            set { this[FieldRegreprempid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("clerkempid")]
        public static Field FieldClerkempid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Clerkempid
        {
            get { return (int?)this[FieldClerkempid]; }
            set { this[FieldClerkempid] = value; }
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
