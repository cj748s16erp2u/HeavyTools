using System;
using System.Collections.Generic;
using System.Text;
using CodaInt.Base;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Common.xcval
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [CODATable("ofc_xcval")]
    public partial class OfcXcval : Entity<OfcXcval>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="xcvid"><see cref="int?" /> xcvid</param>
        public static OfcXcval Load(int? xcvid)
        {
            return Load(new Key(new Field[] { FieldXcvid }, new object[] { xcvid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("xcvid")]
        public static Field FieldXcvid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Xcvid
        {
            get { return (int?)this[FieldXcvid]; }
            set { this[FieldXcvid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("xcvcode")]
        public static Field FieldXcvcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Xcvcode
        {
            get { return new StringN(this[FieldXcvcode]); }
            set { this[FieldXcvcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("xcvrid")]
        public static Field FieldXcvrid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Xcvrid
        {
            get { return (int?)this[FieldXcvrid]; }
            set { this[FieldXcvrid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("xcvextcode")]
        public static Field FieldXcvextcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Xcvextcode
        {
            get { return new StringN(this[FieldXcvextcode]); }
            set { this[FieldXcvextcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - xml
        /// </summary>
        [Field("xmldata")]
        public static Field FieldXmldata { get; protected set; }
        /// <summary>
        /// ? - xml
        /// </summary>
        public StringN Xmldata
        {
            get { return new StringN(this[FieldXmldata]); }
            set { this[FieldXmldata] = value; }
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

        #endregion
    }
}
