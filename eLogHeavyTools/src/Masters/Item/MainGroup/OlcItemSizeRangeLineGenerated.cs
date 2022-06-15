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
    [Table("olc_itemsizerangeline")]
    public partial class OlcItemSizeRangeLine : Entity<OlcItemSizeRangeLine>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="isrlid"><see cref="int?" /> isrlid</param>
        public static OlcItemSizeRangeLine Load(int? isrlid)
        {
            return Load(new Key(new Field[] { FieldIsrlid }, new object[] { isrlid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isrlid")]
        public static Field FieldIsrlid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isrlid
        {
            get { return (int?)this[FieldIsrlid]; }
            set { this[FieldIsrlid] = value; }
        }

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
        [Field("size")]
        public static Field FieldSize { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Size
        {
            get { return new StringN(this[FieldSize]); }
            set { this[FieldSize] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ordernum")]
        public static Field FieldOrdernum { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ordernum
        {
            get { return (int?)this[FieldOrdernum]; }
            set { this[FieldOrdernum] = value; }
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
