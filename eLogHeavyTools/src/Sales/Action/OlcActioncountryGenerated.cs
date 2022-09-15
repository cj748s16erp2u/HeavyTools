using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Action
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_actioncountry")]
    public partial class OlcActioncountry : Entity<OlcActioncountry>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="acid"><see cref="int?" /> acid</param>
        public static OlcActioncountry Load(int? acid)
        {
            return Load(new Key(new Field[] { FieldAcid }, new object[] { acid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("acid")]
        public static Field FieldAcid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Acid
        {
            get { return (int?)this[FieldAcid]; }
            set { this[FieldAcid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("aid")]
        public static Field FieldAid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Aid
        {
            get { return (int?)this[FieldAid]; }
            set { this[FieldAid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("countryid")]
        public static Field FieldCountryid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Countryid
        {
            get { return new StringN(this[FieldCountryid]); }
            set { this[FieldCountryid] = value; }
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
