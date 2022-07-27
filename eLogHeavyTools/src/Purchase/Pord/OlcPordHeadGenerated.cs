using System;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.Pord
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_pordhead")]
    public partial class OlcPordHead : Entity<OlcPordHead>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="pordid"><see cref="int?" /> pordid</param>
        public static OlcPordHead Load(int? pordid)
        {
            return Load(new Key(new Field[] { FieldPordid }, new object[] { pordid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("pordid")]
        public static Field FieldPordid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Pordid
        {
            get { return (int?)this[FieldPordid]; }
            set { this[FieldPordid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("paritytypeid")]
        public static Field FieldParitytypeid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Paritytypeid
        {
            get { return (int?)this[FieldParitytypeid]; }
            set { this[FieldParitytypeid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("advance")]
        public static Field FieldAdvance { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Advance
        {
            get { return (int?)this[FieldAdvance]; }
            set { this[FieldAdvance] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("clerkempid")]
        public static Field FieldClerkempid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Clerkempid
        {
            get { return (int?)this[FieldClerkempid]; }
            set { this[FieldClerkempid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("locality")]
        public static Field FieldLocality { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Locality
        {
            get { return new StringN(this[FieldLocality]); }
            set { this[FieldLocality] = value; }
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
