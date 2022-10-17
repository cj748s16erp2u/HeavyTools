using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_stline")]
    public partial class OlcStLine : Entity<OlcStLine>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="stlineid"><see cref="int?" /> stlineid</param>
        public static OlcStLine Load(int? stlineid)
        {
            return Load(new Key(new Field[] { FieldStlineid }, new object[] { stlineid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("stlineid")]
        public static Field FieldStlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Stlineid
        {
            get { return (int?)this[FieldStlineid]; }
            set { this[FieldStlineid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("origstlineid")]
        public static Field FieldOrigstlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Origstlineid
        {
            get { return (int?)this[FieldOrigstlineid]; }
            set { this[FieldOrigstlineid] = value; }
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
