using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.PriceTable
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_prctable")]
    public partial class OlcPrctable : Entity<OlcPrctable>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="prcid"><see cref="int?" /> prcid</param>
        public static OlcPrctable Load(int? prcid)
        {
            return Load(new Key(new Field[] { FieldPrcid }, new object[] { prcid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("prcid")]
        public static Field FieldPrcid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Prcid
        {
            get { return (int?)this[FieldPrcid]; }
            set { this[FieldPrcid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ptid")]
        public static Field FieldPtid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ptid
        {
            get { return (int?)this[FieldPtid]; }
            set { this[FieldPtid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("prctype")]
        public static Field FieldPrctype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Prctype
        {
            get { return (int?)this[FieldPrctype]; }
            set { this[FieldPrctype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("wid")]
        public static Field FieldWid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Wid
        {
            get { return new StringN(this[FieldWid]); }
            set { this[FieldWid] = value; }
        }

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
        [Field("addrid")]
        public static Field FieldAddrid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Addrid
        {
            get { return (int?)this[FieldAddrid]; }
            set { this[FieldAddrid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("curid")]
        public static Field FieldCurid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Curid
        {
            get { return new StringN(this[FieldCurid]); }
            set { this[FieldCurid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("startdate")]
        public static Field FieldStartdate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Startdate
        {
            get { return (DateTime?)this[FieldStartdate]; }
            set { this[FieldStartdate] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("enddate")]
        public static Field FieldEnddate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Enddate
        {
            get { return (DateTime?)this[FieldEnddate]; }
            set { this[FieldEnddate] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("prc")]
        public static Field FieldPrc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Prc
        {
            get { return (Decimal?)this[FieldPrc]; }
            set { this[FieldPrc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("imid")]
        public static Field FieldImid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Imid
        {
            get { return (int?)this[FieldImid]; }
            set { this[FieldImid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("isid")]
        public static Field FieldIsid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Isid
        {
            get { return new StringN(this[FieldIsid]); }
            set { this[FieldIsid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("icid")]
        public static Field FieldIcid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Icid
        {
            get { return new StringN(this[FieldIcid]); }
            set { this[FieldIcid] = value; }
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
