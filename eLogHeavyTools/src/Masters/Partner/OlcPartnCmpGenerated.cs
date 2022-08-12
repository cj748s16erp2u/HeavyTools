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
    [Table("olc_partncmp")]
    public partial class OlcPartnCmp : Entity<OlcPartnCmp>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="partnid"><see cref="int?" /> partnid</param>
        /// <param name="cmpid"><see cref="int?" /> cmpid</param>
        public static OlcPartnCmp Load(int? partnid, int? cmpid)
        {
            return Load(new Key(new Field[] { FieldPartnid, FieldCmpid }, new object[] { partnid, cmpid }));
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
        /// ? - int
        /// </summary>
        [Field("cmpid")]
        public static Field FieldCmpid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Cmpid
        {
            get { return (int?)this[FieldCmpid]; }
            set { this[FieldCmpid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("secpaymid")]
        public static Field FieldSecpaymid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Secpaymid
        {
            get { return new StringN(this[FieldSecpaymid]); }
            set { this[FieldSecpaymid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("relatedaccno")]
        public static Field FieldRelatedaccno { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Relatedaccno
        {
            get { return new StringN(this[FieldRelatedaccno]); }
            set { this[FieldRelatedaccno] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("scontoinvoice")]
        public static Field FieldScontoinvoice { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Scontoinvoice
        {
            get { return (int?)this[FieldScontoinvoice]; }
            set { this[FieldScontoinvoice] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("scontobelowaccno")]
        public static Field FieldScontobelowaccno { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Scontobelowaccno
        {
            get { return new StringN(this[FieldScontobelowaccno]); }
            set { this[FieldScontobelowaccno] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("scontoavoveaccno")]
        public static Field FieldScontoavoveaccno { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Scontoavoveaccno
        {
            get { return new StringN(this[FieldScontoavoveaccno]); }
            set { this[FieldScontoavoveaccno] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el1")]
        public static Field FieldEl1 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El1
        {
            get { return new StringN(this[FieldEl1]); }
            set { this[FieldEl1] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el2")]
        public static Field FieldEl2 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El2
        {
            get { return new StringN(this[FieldEl2]); }
            set { this[FieldEl2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el3")]
        public static Field FieldEl3 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El3
        {
            get { return new StringN(this[FieldEl3]); }
            set { this[FieldEl3] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el4")]
        public static Field FieldEl4 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El4
        {
            get { return new StringN(this[FieldEl4]); }
            set { this[FieldEl4] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el5")]
        public static Field FieldEl5 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El5
        {
            get { return new StringN(this[FieldEl5]); }
            set { this[FieldEl5] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el6")]
        public static Field FieldEl6 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El6
        {
            get { return new StringN(this[FieldEl6]); }
            set { this[FieldEl6] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el7")]
        public static Field FieldEl7 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El7
        {
            get { return new StringN(this[FieldEl7]); }
            set { this[FieldEl7] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("el8")]
        public static Field FieldEl8 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN El8
        {
            get { return new StringN(this[FieldEl8]); }
            set { this[FieldEl8] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("transactonfeeaccno")]
        public static Field FieldTransactonfeeaccno { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Transactonfeeaccno
        {
            get { return new StringN(this[FieldTransactonfeeaccno]); }
            set { this[FieldTransactonfeeaccno] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("domesticvaluerate")]
        public static Field FieldDomesticvaluerate { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Domesticvaluerate
        {
            get { return (int?)this[FieldDomesticvaluerate]; }
            set { this[FieldDomesticvaluerate] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("referencetype")]
        public static Field FieldReferencetype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Referencetype
        {
            get { return (int?)this[FieldReferencetype]; }
            set { this[FieldReferencetype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discountaccounting")]
        public static Field FieldDiscountaccounting { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discountaccounting
        {
            get { return (int?)this[FieldDiscountaccounting]; }
            set { this[FieldDiscountaccounting] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("valuecurid")]
        public static Field FieldValuecurid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Valuecurid
        {
            get { return new StringN(this[FieldValuecurid]); }
            set { this[FieldValuecurid] = value; }
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
