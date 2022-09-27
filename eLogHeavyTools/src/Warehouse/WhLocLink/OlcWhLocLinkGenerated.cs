using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_whloclink")]
    public partial class OlcWhLocLink : Entity<OlcWhLocLink>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whllid"><see cref="int?" /> whllid</param>
        public static OlcWhLocLink Load(int? whllid)
        {
            return Load(new Key(new Field[] { FieldWhllid }, new object[] { whllid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whllid")]
        public static Field FieldWhllid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whllid
        {
            get { return (int?)this[FieldWhllid]; }
            set { this[FieldWhllid] = value; }
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
