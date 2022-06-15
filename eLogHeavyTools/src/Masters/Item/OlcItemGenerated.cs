using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_item")]
    public partial class OlcItem : Entity<OlcItem>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="itemid"><see cref="int?" /> itemid</param>
        public static OlcItem Load(int? itemid)
        {
            return Load(new Key(new Field[] { FieldItemid }, new object[] { itemid }));
        }

        #region Field accessors

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
        [Field("colortype1")]
        public static Field FieldColortype1 { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Colortype1
        {
            get { return (int?)this[FieldColortype1]; }
            set { this[FieldColortype1] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("colorname")]
        public static Field FieldColorname { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Colorname
        {
            get { return new StringN(this[FieldColorname]); }
            set { this[FieldColorname] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("colortype2")]
        public static Field FieldColortype2 { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Colortype2
        {
            get { return (int?)this[FieldColortype2]; }
            set { this[FieldColortype2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("colortype3")]
        public static Field FieldColortype3 { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Colortype3
        {
            get { return (int?)this[FieldColortype3]; }
            set { this[FieldColortype3] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("materialtype")]
        public static Field FieldMaterialtype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Materialtype
        {
            get { return (int?)this[FieldMaterialtype]; }
            set { this[FieldMaterialtype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("patterntype")]
        public static Field FieldPatterntype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Patterntype
        {
            get { return (int?)this[FieldPatterntype]; }
            set { this[FieldPatterntype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("patterntype2")]
        public static Field FieldPatterntype2 { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Patterntype2
        {
            get { return (int?)this[FieldPatterntype2]; }
            set { this[FieldPatterntype2] = value; }
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
