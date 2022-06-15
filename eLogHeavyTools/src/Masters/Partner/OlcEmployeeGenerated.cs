using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_employee")]
    public partial class OlcEmployee : Entity<OlcEmployee>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="empid"><see cref="int?" /> empid</param>
        public static OlcEmployee Load(int? empid)
        {
            return Load(new Key(new Field[] { FieldEmpid }, new object[] { empid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("empid")]
        public static Field FieldEmpid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Empid
        {
            get { return (int?)this[FieldEmpid]; }
            set { this[FieldEmpid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("ptel")]
        public static Field FieldPtel { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Ptel
        {
            get { return new StringN(this[FieldPtel]); }
            set { this[FieldPtel] = value; }
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
