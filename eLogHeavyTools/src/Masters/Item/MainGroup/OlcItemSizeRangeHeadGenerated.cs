using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item.MainGroup
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_itemsizerangehead")]
    public partial class OlcItemSizeRangeHead : Entity<OlcItemSizeRangeHead>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="isrhid"><see cref="int?" /> isrhid</param>
        public static OlcItemSizeRangeHead Load(int? isrhid)
        {
            return Load(new Key(new Field[] { FieldIsrhid }, new object[] { isrhid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isrhid")]
        public static Field FieldIsrhid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isrhid
        {
            get { return (int?)this[FieldIsrhid]; }
            set { this[FieldIsrhid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("code")]
        public static Field FieldCode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Code
        {
            get { return new StringN(this[FieldCode]); }
            set { this[FieldCode] = value; }
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
