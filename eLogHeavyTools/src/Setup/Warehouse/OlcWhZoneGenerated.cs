using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Setup.Warehouse
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_whzone")]
    public partial class OlcWhZone : Entity<OlcWhZone>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="whzoneid"><see cref="int?" /> whzoneid</param>
        public static OlcWhZone Load(int? whzoneid)
        {
            return Load(new Key(new Field[] { FieldWhzoneid }, new object[] { whzoneid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("whzoneid")]
        public static Field FieldWhzoneid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Whzoneid
        {
            get { return (int?)this[FieldWhzoneid]; }
            set { this[FieldWhzoneid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("whid")]
        public static Field FieldWhid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Whid
        {
            get { return new StringN(this[FieldWhid]); }
            set { this[FieldWhid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("whzonecode")]
        public static Field FieldWhzonecode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Whzonecode
        {
            get { return new StringN(this[FieldWhzonecode]); }
            set { this[FieldWhzonecode] = value; }
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
        [Field("pickingtype")]
        public static Field FieldPickingtype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Pickingtype
        {
            get { return (int?)this[FieldPickingtype]; }
            set { this[FieldPickingtype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("pickingcartaccessible")]
        public static Field FieldPickingcartaccessible { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Pickingcartaccessible
        {
            get { return (int?)this[FieldPickingcartaccessible]; }
            set { this[FieldPickingcartaccessible] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isbackground")]
        public static Field FieldIsbackground { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isbackground
        {
            get { return (int?)this[FieldIsbackground]; }
            set { this[FieldIsbackground] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ispuffer")]
        public static Field FieldIspuffer { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ispuffer
        {
            get { return (int?)this[FieldIspuffer]; }
            set { this[FieldIspuffer] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("locdefvolume")]
        public static Field FieldLocdefvolume { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Locdefvolume
        {
            get { return (Decimal?)this[FieldLocdefvolume]; }
            set { this[FieldLocdefvolume] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("locdefoverfillthreshold")]
        public static Field FieldLocdefoverfillthreshold { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Locdefoverfillthreshold
        {
            get { return (Decimal?)this[FieldLocdefoverfillthreshold]; }
            set { this[FieldLocdefoverfillthreshold] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("locdefismulti")]
        public static Field FieldLocdefismulti { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Locdefismulti
        {
            get { return (int?)this[FieldLocdefismulti]; }
            set { this[FieldLocdefismulti] = value; }
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
