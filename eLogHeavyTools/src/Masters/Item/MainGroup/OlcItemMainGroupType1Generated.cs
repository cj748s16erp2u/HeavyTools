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
    [Table("olc_itemmaingrouptype1")]
    public partial class OlcItemMainGroupType1 : Entity<OlcItemMainGroupType1>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="imgt1id"><see cref="StringN" /> imgt1id</param>
        public static OlcItemMainGroupType1 Load(StringN imgt1id)
        {
            return Load(new Key(new Field[] { FieldImgt1id }, new object[] { imgt1id }));
        }

        #region Field accessors

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
        /// ? - int
        /// </summary>
        [Field("grouplastnum")]
        public static Field FieldGrouplastnum { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Grouplastnum
        {
            get { return (int?)this[FieldGrouplastnum]; }
            set { this[FieldGrouplastnum] = value; }
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
