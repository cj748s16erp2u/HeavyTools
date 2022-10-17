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
    [Table("olc_sthead")]
    public partial class OlcStHead : Entity<OlcStHead>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="stid"><see cref="int?" /> stid</param>
        public static OlcStHead Load(int? stid)
        {
            return Load(new Key(new Field[] { FieldStid }, new object[] { stid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("stid")]
        public static Field FieldStid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Stid
        {
            get { return (int?)this[FieldStid]; }
            set { this[FieldStid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("onroadtowhid")]
        public static Field FieldOnroadtowhid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Onroadtowhid
        {
            get { return new StringN(this[FieldOnroadtowhid]); }
            set { this[FieldOnroadtowhid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("onroadfromstid")]
        public static Field FieldOnroadfromstid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Onroadfromstid
        {
            get { return (int?)this[FieldOnroadfromstid]; }
            set { this[FieldOnroadfromstid] = value; }
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
