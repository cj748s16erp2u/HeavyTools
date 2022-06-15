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
    [Table("olc_itemmaingrouptype2")]
    public partial class OlcItemMainGroupType2 : Entity<OlcItemMainGroupType2>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="imgt2id"><see cref="StringN" /> imgt2id</param>
        public static OlcItemMainGroupType2 Load(StringN imgt2id)
        {
            return Load(new Key(new Field[] { FieldImgt2id }, new object[] { imgt2id }));
        }

        #region Field accessors

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
        /// ? - varchar
        /// </summary>
        [Field("groupname")]
        public static Field FieldGroupname { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Groupname
        {
            get { return new StringN(this[FieldGroupname]); }
            set { this[FieldGroupname] = value; }
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
