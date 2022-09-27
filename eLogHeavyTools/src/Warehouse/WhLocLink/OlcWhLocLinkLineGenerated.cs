using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_whloclinkline")]
    public partial class OlcWhLocLinkLine : Entity<OlcWhLocLinkLine>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whlllineid"><see cref="int?" /> whlllineid</param>
        public static OlcWhLocLinkLine Load(int? whlllineid)
        {
            return Load(new Key(new Field[] { FieldWhlllineid }, new object[] { whlllineid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whlllineid")]
        public static Field FieldWhlllineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whlllineid
        {
            get { return (int?)this[FieldWhlllineid]; }
            set { this[FieldWhlllineid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whllid")]
        public static Field FieldWhllid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whllid
        {
            get { return (int?)this[FieldWhllid]; }
            set { this[FieldWhllid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whlocid")]
        public static Field FieldWhlocid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whlocid
        {
            get { return (int?)this[FieldWhlocid]; }
            set { this[FieldWhlocid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whllinktype")]
        public static Field FieldWhllinktype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whllinktype
        {
            get { return (int?)this[FieldWhllinktype]; }
            set { this[FieldWhllinktype] = value; }
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

        #endregion
    }
}
