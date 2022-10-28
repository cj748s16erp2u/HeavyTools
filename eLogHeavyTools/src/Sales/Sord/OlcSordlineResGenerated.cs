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
    [Table("olc_sordline_res")]
    public partial class OlcSordlineRes : Entity<OlcSordlineRes>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="sordlineidres"><see cref="int?" /> sordlineidres</param>
        public static OlcSordlineRes Load(int? sordlineidres)
        {
            return Load(new Key(new Field[] { FieldSordlineidres }, new object[] { sordlineidres }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("sordlineidres")]
        public static Field FieldSordlineidres { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sordlineidres
        {
            get { return (int?)this[FieldSordlineidres]; }
            set { this[FieldSordlineidres] = value; }
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
        [Field("preordersordlineid")]
        public static Field FieldPreordersordlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Preordersordlineid
        {
            get { return (int?)this[FieldPreordersordlineid]; }
            set { this[FieldPreordersordlineid] = value; }
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
