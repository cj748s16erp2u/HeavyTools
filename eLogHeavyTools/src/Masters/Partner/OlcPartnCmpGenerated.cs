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
        /// ? -  varchar
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
