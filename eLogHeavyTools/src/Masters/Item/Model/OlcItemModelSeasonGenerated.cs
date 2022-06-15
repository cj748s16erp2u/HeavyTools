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
    [Table("olc_itemmodelseason")]
    public partial class OlcItemModelSeason : Entity<OlcItemModelSeason>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="imsid"><see cref="int?" /> imsid</param>
        public static OlcItemModelSeason Load(int? imsid)
        {
            return Load(new Key(new Field[] { FieldImsid }, new object[] { imsid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("imsid")]
        public static Field FieldImsid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Imsid
        {
            get { return (int?)this[FieldImsid]; }
            set { this[FieldImsid] = value; }
        }

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
        [Field("icid")]
        public static Field FieldIcid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Icid
        {
            get { return new StringN(this[FieldIcid]); }
            set { this[FieldIcid] = value; }
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
