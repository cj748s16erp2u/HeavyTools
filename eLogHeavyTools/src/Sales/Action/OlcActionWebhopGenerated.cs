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
    [Table("olc_actionwebhop")]
    public partial class OlcActionWebhop : Entity<OlcActionWebhop>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="awid"><see cref="int?" /> awid</param>
        public static OlcActionWebhop Load(int? awid)
        {
            return Load(new Key(new Field[] { FieldAwid }, new object[] { awid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("awid")]
        public static Field FieldAwid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Awid
        {
            get { return (int?)this[FieldAwid]; }
            set { this[FieldAwid] = value; }
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
        [Field("wid")]
        public static Field FieldWid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Wid
        {
            get { return new StringN(this[FieldWid]); }
            set { this[FieldWid] = value; }
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
