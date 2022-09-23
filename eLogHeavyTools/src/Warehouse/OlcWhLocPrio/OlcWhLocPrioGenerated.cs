using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.WhLocPrio
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_whlocprio")]
    public partial class OlcWhLocPrio : Entity<OlcWhLocPrio>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whlpid"><see cref="int?" /> whlpid</param>
        public static OlcWhLocPrio Load(int? whlpid)
        {
            return Load(new Key(new Field[] { FieldWhlpid }, new object[] { whlpid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whlpid")]
        public static Field FieldWhlpid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whlpid
        {
            get { return (int?)this[FieldWhlpid]; }
            set { this[FieldWhlpid] = value; }
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
        /// ? - int
        /// </summary>
        [Field("whpriotype")]
        public static Field FieldWhpriotype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whpriotype
        {
            get { return (int?)this[FieldWhpriotype]; }
            set { this[FieldWhpriotype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("refilllimit")]
        public static Field FieldRefilllimit { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Refilllimit
        {
            get { return (Decimal?)this[FieldRefilllimit]; }
            set { this[FieldRefilllimit] = value; }
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
