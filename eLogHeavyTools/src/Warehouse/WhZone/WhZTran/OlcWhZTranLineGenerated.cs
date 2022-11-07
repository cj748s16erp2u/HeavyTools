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
    [Table("olc_whztranline")]
    public partial class OlcWhZTranLine : Entity<OlcWhZTranLine>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whztlineid"><see cref="int?" /> whztlineid</param>
        public static OlcWhZTranLine Load(int? whztlineid)
        {
            return Load(new Key(new Field[] { FieldWhztlineid }, new object[] { whztlineid }));
        }

        #region Field accessors

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
        /// ? - numeric
        /// </summary>
        [Field("inqty")]
        public static Field FieldInqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Inqty
        {
            get { return (Decimal?)this[FieldInqty]; }
            set { this[FieldInqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("outqty")]
        public static Field FieldOutqty { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Outqty
        {
            get { return (Decimal?)this[FieldOutqty]; }
            set { this[FieldOutqty] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("unitid2")]
        public static Field FieldUnitid2 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Unitid2
        {
            get { return new StringN(this[FieldUnitid2]); }
            set { this[FieldUnitid2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("change")]
        public static Field FieldChange { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Change
        {
            get { return (Decimal?)this[FieldChange]; }
            set { this[FieldChange] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("ordqty2")]
        public static Field FieldOrdqty2 { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Ordqty2
        {
            get { return (Decimal?)this[FieldOrdqty2]; }
            set { this[FieldOrdqty2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("dispqty2")]
        public static Field FieldDispqty2 { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Dispqty2
        {
            get { return (Decimal?)this[FieldDispqty2]; }
            set { this[FieldDispqty2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("movqty2")]
        public static Field FieldMovqty2 { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Movqty2
        {
            get { return (Decimal?)this[FieldMovqty2]; }
            set { this[FieldMovqty2] = value; }
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
        [Field("stlineid")]
        public static Field FieldStlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Stlineid
        {
            get { return (int?)this[FieldStlineid]; }
            set { this[FieldStlineid] = value; }
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
        [Field("pordlineid")]
        public static Field FieldPordlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Pordlineid
        {
            get { return (int?)this[FieldPordlineid]; }
            set { this[FieldPordlineid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("taskitemid")]
        public static Field FieldTaskitemid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Taskitemid
        {
            get { return (int?)this[FieldTaskitemid]; }
            set { this[FieldTaskitemid] = value; }
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
