using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item.Season
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_itemseason")]
    public partial class OlcItemSeason : Entity<OlcItemSeason>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="isid"><see cref="StringN" /> isid</param>
        public static OlcItemSeason Load(StringN isid)
        {
            return Load(new Key(new Field[] { FieldIsid }, new object[] { isid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("isid")]
        public static Field FieldIsid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Isid
        {
            get { return new StringN(this[FieldIsid]); }
            set { this[FieldIsid] = value; }
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
        [Field("oldcode")]
        public static Field FieldOldcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Oldcode
        {
            get { return new StringN(this[FieldOldcode]); }
            set { this[FieldOldcode] = value; }
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
