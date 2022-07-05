using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item.Model
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_itemmodel")]
    public partial class OlcItemModel : Entity<OlcItemModel>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="imid"><see cref="int?" /> imid</param>
        public static OlcItemModel Load(int? imid)
        {
            return Load(new Key(new Field[] { FieldImid }, new object[] { imid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("imid")]
        public static Field FieldImid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Imid
        {
            get { return (int?)this[FieldImid]; }
            set { this[FieldImid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("imgid")]
        public static Field FieldImgid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Imgid
        {
            get { return (int?)this[FieldImgid]; }
            set { this[FieldImgid] = value; }
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
        [Field("unitid")]
        public static Field FieldUnitid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Unitid
        {
            get { return new StringN(this[FieldUnitid]); }
            set { this[FieldUnitid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("exclusivetype")]
        public static Field FieldExclusivetype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Exclusivetype
        {
            get { return (int?)this[FieldExclusivetype]; }
            set { this[FieldExclusivetype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("netweight")]
        public static Field FieldNetweight { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Netweight
        {
            get { return (Decimal?)this[FieldNetweight]; }
            set { this[FieldNetweight] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("grossweight")]
        public static Field FieldGrossweight { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Grossweight
        {
            get { return (Decimal?)this[FieldGrossweight]; }
            set { this[FieldGrossweight] = value; }
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
        /// ? - int
        /// </summary>
        [Field("isimported")]
        public static Field FieldIsimported { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isimported
        {
            get { return (int?)this[FieldIsimported]; }
            set { this[FieldIsimported] = value; }
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
