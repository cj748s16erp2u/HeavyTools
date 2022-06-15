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
    [Table("olc_itemmaingroup")]
    public partial class OlcItemMainGroup : Entity<OlcItemMainGroup>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="imgid"><see cref="int?" /> imgid</param>
        public static OlcItemMainGroup Load(int? imgid)
        {
            return Load(new Key(new Field[] { FieldImgid }, new object[] { imgid }));
        }

        #region Field accessors

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
        [Field("imgt1id")]
        public static Field FieldImgt1id { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Imgt1id
        {
            get { return new StringN(this[FieldImgt1id]); }
            set { this[FieldImgt1id] = value; }
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
        [Field("imgt2id")]
        public static Field FieldImgt2id { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Imgt2id
        {
            get { return new StringN(this[FieldImgt2id]); }
            set { this[FieldImgt2id] = value; }
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
        [Field("itemgrpid")]
        public static Field FieldItemgrpid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Itemgrpid
        {
            get { return new StringN(this[FieldItemgrpid]); }
            set { this[FieldItemgrpid] = value; }
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
        /// ? - int
        /// </summary>
        [Field("maingrouplastnum")]
        public static Field FieldMaingrouplastnum { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Maingrouplastnum
        {
            get { return (int?)this[FieldMaingrouplastnum]; }
            set { this[FieldMaingrouplastnum] = value; }
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
