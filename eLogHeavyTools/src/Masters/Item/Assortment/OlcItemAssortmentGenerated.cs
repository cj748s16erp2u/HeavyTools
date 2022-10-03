using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item.Assortment
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_itemassortment")]
    public partial class OlcItemAssortment : Entity<OlcItemAssortment>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="isoid"><see cref="int?" /> isoid</param>
        public static OlcItemAssortment Load(int? isoid)
        {
            return Load(new Key(new Field[] { FieldIsoid }, new object[] { isoid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isoid")]
        public static Field FieldIsoid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isoid
        {
            get { return (int?)this[FieldIsoid]; }
            set { this[FieldIsoid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("assortmentitemid")]
        public static Field FieldAssortmentitemid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Assortmentitemid
        {
            get { return (int?)this[FieldAssortmentitemid]; }
            set { this[FieldAssortmentitemid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("itemid")]
        public static Field FieldItemid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Itemid
        {
            get { return (int?)this[FieldItemid]; }
            set { this[FieldItemid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("count")]
        public static Field FieldCount { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Count
        {
            get { return (int?)this[FieldCount]; }
            set { this[FieldCount] = value; }
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
