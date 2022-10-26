using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.Pinv
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_costline")]
    public partial class OlcCostLine : Entity<OlcCostLine>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="costlineid"><see cref="int?" /> costlineid</param>
        public static OlcCostLine Load(int? costlineid)
        {
            return Load(new Key(new Field[] { FieldCostlineid }, new object[] { costlineid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("costlineid")]
        public static Field FieldCostlineid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Costlineid
        {
            get { return (int?)this[FieldCostlineid]; }
            set { this[FieldCostlineid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("othtrlinedocid")]
        public static Field FieldOthtrlinedocid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Othtrlinedocid
        {
            get { return new StringN(this[FieldOthtrlinedocid]); }
            set { this[FieldOthtrlinedocid] = value; }
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
