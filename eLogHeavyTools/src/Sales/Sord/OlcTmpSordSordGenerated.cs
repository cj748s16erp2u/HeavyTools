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
    [Table("olc_tmp_sordsord")]
    public partial class OlcTmpSordSord : Entity<OlcTmpSordSord>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="sordlineid"><see cref="int?" /> sordlineid</param>
        public static OlcTmpSordSord Load(int? sordlineid)
        {
            return Load(new Key(new Field[] { FieldSordlineid }, new object[] { sordlineid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - uniqueidentifier
        /// </summary>
        [Field("ssid")]
        public static Field FieldSsid { get; protected set; }
        /// <summary>
        /// ? - uniqueidentifier
        /// </summary>
        public Guid? Ssid
        {
            get { return (Guid?)this[FieldSsid]; }
            set { this[FieldSsid] = value; }
        }

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
        [Field("linenum")]
        public static Field FieldLinenum { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Linenum
        {
            get { return (int?)this[FieldLinenum]; }
            set { this[FieldLinenum] = value; }
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
        [Field("itemcode")]
        public static Field FieldItemcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Itemcode
        {
            get { return new StringN(this[FieldItemcode]); }
            set { this[FieldItemcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("name01")]
        public static Field FieldName01 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Name01
        {
            get { return new StringN(this[FieldName01]); }
            set { this[FieldName01] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - nvarchar
        /// </summary>
        [Field("name02")]
        public static Field FieldName02 { get; protected set; }
        /// <summary>
        /// ? - nvarchar
        /// </summary>
        public StringN Name02
        {
            get { return new StringN(this[FieldName02]); }
            set { this[FieldName02] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("docnum")]
        public static Field FieldDocnum { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Docnum
        {
            get { return new StringN(this[FieldDocnum]); }
            set { this[FieldDocnum] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("qty")]
        public static Field FieldQty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Qty
        {
            get { return (Decimal?)this[FieldQty]; }
            set { this[FieldQty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("reqdate")]
        public static Field FieldReqdate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Reqdate
        {
            get { return (DateTime?)this[FieldReqdate]; }
            set { this[FieldReqdate] = value; }
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
        /// ? - varchar
        /// </summary>
        [Field("ref2")]
        public static Field FieldRef2 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Ref2
        {
            get { return new StringN(this[FieldRef2]); }
            set { this[FieldRef2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("fullordqty")]
        public static Field FieldFullordqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Fullordqty
        {
            get { return (Decimal?)this[FieldFullordqty]; }
            set { this[FieldFullordqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("fullmovqty")]
        public static Field FieldFullmovqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Fullmovqty
        {
            get { return (Decimal?)this[FieldFullmovqty]; }
            set { this[FieldFullmovqty] = value; }
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
        /// ? - numeric
        /// </summary>
        [Field("selprc")]
        public static Field FieldSelprc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Selprc
        {
            get { return (Decimal?)this[FieldSelprc]; }
            set { this[FieldSelprc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("seltotprc")]
        public static Field FieldSeltotprc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Seltotprc
        {
            get { return (Decimal?)this[FieldSeltotprc]; }
            set { this[FieldSeltotprc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("selprctype")]
        public static Field FieldSelprctype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Selprctype
        {
            get { return (int?)this[FieldSelprctype]; }
            set { this[FieldSelprctype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("selprcprcid")]
        public static Field FieldSelprcprcid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Selprcprcid
        {
            get { return (int?)this[FieldSelprcprcid]; }
            set { this[FieldSelprcprcid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("discpercnt")]
        public static Field FieldDiscpercnt { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Discpercnt
        {
            get { return (Decimal?)this[FieldDiscpercnt]; }
            set { this[FieldDiscpercnt] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discpercntprcid")]
        public static Field FieldDiscpercntprcid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discpercntprcid
        {
            get { return (int?)this[FieldDiscpercntprcid]; }
            set { this[FieldDiscpercntprcid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("discval")]
        public static Field FieldDiscval { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Discval
        {
            get { return (Decimal?)this[FieldDiscval]; }
            set { this[FieldDiscval] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("disctotval")]
        public static Field FieldDisctotval { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Disctotval
        {
            get { return (Decimal?)this[FieldDisctotval]; }
            set { this[FieldDisctotval] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("taxid")]
        public static Field FieldTaxid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Taxid
        {
            get { return new StringN(this[FieldTaxid]); }
            set { this[FieldTaxid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sordlinestat")]
        public static Field FieldSordlinestat { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sordlinestat
        {
            get { return (int?)this[FieldSordlinestat]; }
            set { this[FieldSordlinestat] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("note")]
        public static Field FieldNote { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Note
        {
            get { return new StringN(this[FieldNote]); }
            set { this[FieldNote] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("resid")]
        public static Field FieldResid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Resid
        {
            get { return (int?)this[FieldResid]; }
            set { this[FieldResid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ucdid")]
        public static Field FieldUcdid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ucdid
        {
            get { return (int?)this[FieldUcdid]; }
            set { this[FieldUcdid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("pjpid")]
        public static Field FieldPjpid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Pjpid
        {
            get { return (int?)this[FieldPjpid]; }
            set { this[FieldPjpid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("gen")]
        public static Field FieldGen { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Gen
        {
            get { return (int?)this[FieldGen]; }
            set { this[FieldGen] = value; }
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
