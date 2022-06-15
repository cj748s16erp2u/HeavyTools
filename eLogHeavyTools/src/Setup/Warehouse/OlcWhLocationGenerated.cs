using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Setup.Warehouse
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_whlocation")]
    public partial class OlcWhLocation : Entity<OlcWhLocation>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whlocid"><see cref="int?" /> whlocid</param>
        public static OlcWhLocation Load(int? whlocid)
        {
            return Load(new Key(new Field[] { FieldWhlocid }, new object[] { whlocid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whlocid")]
        public static Field FieldWhlocid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whlocid
        {
            get { return (int?)this[FieldWhlocid]; }
            set { this[FieldWhlocid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("whid")]
        public static Field FieldWhid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Whid
        {
            get { return new StringN(this[FieldWhid]); }
            set { this[FieldWhid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whzoneid")]
        public static Field FieldWhzoneid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whzoneid
        {
            get { return (int?)this[FieldWhzoneid]; }
            set { this[FieldWhzoneid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("whloccode")]
        public static Field FieldWhloccode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Whloccode
        {
            get { return new StringN(this[FieldWhloccode]); }
            set { this[FieldWhloccode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("name")]
        public static Field FieldName { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Name
        {
            get { return new StringN(this[FieldName]); }
            set { this[FieldName] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("loctype")]
        public static Field FieldLoctype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Loctype
        {
            get { return (int?)this[FieldLoctype]; }
            set { this[FieldLoctype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("movloctype")]
        public static Field FieldMovloctype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Movloctype
        {
            get { return (int?)this[FieldMovloctype]; }
            set { this[FieldMovloctype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("volume")]
        public static Field FieldVolume { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Volume
        {
            get { return (Decimal?)this[FieldVolume]; }
            set { this[FieldVolume] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("overfillthreshold")]
        public static Field FieldOverfillthreshold { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Overfillthreshold
        {
            get { return (Decimal?)this[FieldOverfillthreshold]; }
            set { this[FieldOverfillthreshold] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ismulti")]
        public static Field FieldIsmulti { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ismulti
        {
            get { return (int?)this[FieldIsmulti]; }
            set { this[FieldIsmulti] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("crawlorder")]
        public static Field FieldCrawlorder { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Crawlorder
        {
            get { return (int?)this[FieldCrawlorder]; }
            set { this[FieldCrawlorder] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("capacity")]
        public static Field FieldCapacity { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Capacity
        {
            get { return (Decimal?)this[FieldCapacity]; }
            set { this[FieldCapacity] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("capunitid")]
        public static Field FieldCapunitid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Capunitid
        {
            get { return new StringN(this[FieldCapunitid]); }
            set { this[FieldCapunitid] = value; }
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
