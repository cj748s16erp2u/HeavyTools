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
    [Table("olc_partnaddr")]
    public partial class OlcPartnAddr : Entity<OlcPartnAddr>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="addrid"><see cref="int?" /> addrid</param>
        public static OlcPartnAddr Load(int? addrid)
        {
            return Load(new Key(new Field[] { FieldAddrid }, new object[] { addrid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("addrid")]
        public static Field FieldAddrid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Addrid
        {
            get { return (int?)this[FieldAddrid]; }
            set { this[FieldAddrid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("oldcode")]
        public static Field FieldOldcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Oldcode
        {
            get { return new StringN(this[FieldOldcode]); }
            set { this[FieldOldcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("glnnum")]
        public static Field FieldGlnnum { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Glnnum
        {
            get { return new StringN(this[FieldGlnnum]); }
            set { this[FieldGlnnum] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("buildingname")]
        public static Field FieldBuildingname { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Buildingname
        {
            get { return new StringN(this[FieldBuildingname]); }
            set { this[FieldBuildingname] = value; }
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
