using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.GiftCard
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_giftcard")]
    public partial class OlcGiftCard : Entity<OlcGiftCard>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="gcid"><see cref="int?" /> gcid</param>
        public static OlcGiftCard Load(int? gcid)
        {
            return Load(new Key(new Field[] { FieldGcid }, new object[] { gcid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("gcid")]
        public static Field FieldGcid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Gcid
        {
            get { return (int?)this[FieldGcid]; }
            set { this[FieldGcid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("barcode")]
        public static Field FieldBarcode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Barcode
        {
            get { return new StringN(this[FieldBarcode]); }
            set { this[FieldBarcode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("pincode")]
        public static Field FieldPincode { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Pincode
        {
            get { return new StringN(this[FieldPincode]); }
            set { this[FieldPincode] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("prc")]
        public static Field FieldPrc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Prc
        {
            get { return (Decimal?)this[FieldPrc]; }
            set { this[FieldPrc] = value; }
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
