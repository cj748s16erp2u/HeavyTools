using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZoneTran
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_whztranhead")]
    public partial class OlcWhZTranHead : Entity<OlcWhZTranHead>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whztid"><see cref="int?" /> whztid</param>
        public static OlcWhZTranHead Load(int? whztid)
        {
            return Load(new Key(new Field[] { FieldWhztid }, new object[] { whztid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whztid")]
        public static Field FieldWhztid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whztid
        {
            get { return (int?)this[FieldWhztid]; }
            set { this[FieldWhztid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("cmpid")]
        public static Field FieldCmpid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Cmpid
        {
            get { return (int?)this[FieldCmpid]; }
            set { this[FieldCmpid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whzttype")]
        public static Field FieldWhzttype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whzttype
        {
            get { return (int?)this[FieldWhzttype]; }
            set { this[FieldWhzttype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("whztdate")]
        public static Field FieldWhztdate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Whztdate
        {
            get { return (DateTime?)this[FieldWhztdate]; }
            set { this[FieldWhztdate] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("fromwhzid")]
        public static Field FieldFromwhzid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Fromwhzid
        {
            get { return (int?)this[FieldFromwhzid]; }
            set { this[FieldFromwhzid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("towhzid")]
        public static Field FieldTowhzid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Towhzid
        {
            get { return (int?)this[FieldTowhzid]; }
            set { this[FieldTowhzid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("closeusrid")]
        public static Field FieldCloseusrid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Closeusrid
        {
            get { return new StringN(this[FieldCloseusrid]); }
            set { this[FieldCloseusrid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("closedate")]
        public static Field FieldClosedate { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Closedate
        {
            get { return (DateTime?)this[FieldClosedate]; }
            set { this[FieldClosedate] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whztstat")]
        public static Field FieldWhztstat { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whztstat
        {
            get { return (int?)this[FieldWhztstat]; }
            set { this[FieldWhztstat] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("note")]
        public static Field FieldNote { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Note
        {
            get { return new StringN(this[FieldNote]); }
            set { this[FieldNote] = value; }
        }

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
        /// ? - int
        /// </summary>
        [Field("sordid")]
        public static Field FieldSordid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Sordid
        {
            get { return (int?)this[FieldSordid]; }
            set { this[FieldSordid] = value; }
        }

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
        [Field("taskid")]
        public static Field FieldTaskid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Taskid
        {
            get { return (int?)this[FieldTaskid]; }
            set { this[FieldTaskid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("gen")]
        public static Field FieldGen { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Gen
        {
            get { return (int?)this[FieldGen]; }
            set { this[FieldGen] = value; }
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
