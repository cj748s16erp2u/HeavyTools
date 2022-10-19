using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Setup.SordDoc
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_sorddoc")]
    public partial class OlcSordDoc : Entity<OlcSordDoc>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="sorddocid"><see cref="StringN" /> sorddocid</param>
        public static OlcSordDoc Load(StringN sorddocid)
        {
            return Load(new Key(new Field[] { FieldSorddocid }, new object[] { sorddocid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("sorddocid")]
        public static Field FieldSorddocid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Sorddocid
        {
            get { return new StringN(this[FieldSorddocid]); }
            set { this[FieldSorddocid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("frameordersorddocid")]
        public static Field FieldFrameordersorddocid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Frameordersorddocid
        {
            get { return new StringN(this[FieldFrameordersorddocid]); }
            set { this[FieldFrameordersorddocid] = value; }
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
