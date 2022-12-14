using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Setup.PriceTable
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_prctype")]
    public partial class OlcPrcType : Entity<OlcPrcType>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="ptid"><see cref="int?" /> ptid</param>
        public static OlcPrcType Load(int? ptid)
        {
            return Load(new Key(new Field[] { FieldPtid }, new object[] { ptid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ptid")]
        public static Field FieldPtid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ptid
        {
            get { return (int?)this[FieldPtid]; }
            set { this[FieldPtid] = value; }
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
        /// ? - int
        /// </summary>
        [Field("isnet")]
        public static Field FieldIsnet { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isnet
        {
            get { return (int?)this[FieldIsnet]; }
            set { this[FieldIsnet] = value; }
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
