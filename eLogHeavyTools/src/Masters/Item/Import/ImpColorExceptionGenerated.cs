using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Item.Import
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("imp_colorexception")]
    public partial class ImpColorException : Entity<ImpColorException>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="ice"><see cref="int?" /> ice</param>
        public static ImpColorException Load(int? ice)
        {
            return Load(new Key(new Field[] { FieldIce }, new object[] { ice }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("ice")]
        public static Field FieldIce { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Ice
        {
            get { return (int?)this[FieldIce]; }
            set { this[FieldIce] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("modelnumber")]
        public static Field FieldModelnumber { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Modelnumber
        {
            get { return new StringN(this[FieldModelnumber]); }
            set { this[FieldModelnumber] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("colourcode")]
        public static Field FieldColourcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Colourcode
        {
            get { return new StringN(this[FieldColourcode]); }
            set { this[FieldColourcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("colorbalance")]
        public static Field FieldColorbalance { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Colorbalance
        {
            get { return new StringN(this[FieldColorbalance]); }
            set { this[FieldColorbalance] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("color1")]
        public static Field FieldColor1 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Color1
        {
            get { return new StringN(this[FieldColor1]); }
            set { this[FieldColor1] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("color2")]
        public static Field FieldColor2 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Color2
        {
            get { return new StringN(this[FieldColor2]); }
            set { this[FieldColor2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("color3")]
        public static Field FieldColor3 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Color3
        {
            get { return new StringN(this[FieldColor3]); }
            set { this[FieldColor3] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("sample1")]
        public static Field FieldSample1 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Sample1
        {
            get { return new StringN(this[FieldSample1]); }
            set { this[FieldSample1] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("sample2")]
        public static Field FieldSample2 { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Sample2
        {
            get { return new StringN(this[FieldSample2]); }
            set { this[FieldSample2] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("season")]
        public static Field FieldSeason { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Season
        {
            get { return new StringN(this[FieldSeason]); }
            set { this[FieldSeason] = value; }
        }

        #endregion
    }
}
