using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Retail.Cart
{
    /// <summary>
    /// <para>Generált Entity</para>
    /// ?
    /// </summary>
    [Table("olc_cart")]
    public partial class OlcCart : Entity<OlcCart>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="cartid"><see cref="int?" /> cartid</param>
        public static OlcCart Load(int? cartid)
        {
            return Load(new Key(new Field[] { FieldCartid }, new object[] { cartid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("cartid")]
        public static Field FieldCartid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Cartid
        {
            get { return (int?)this[FieldCartid]; }
            set { this[FieldCartid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("itemid")]
        public static Field FieldItemid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Itemid
        {
            get { return (int?)this[FieldItemid]; }
            set { this[FieldItemid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("loyaltyCardNo")]
        public static Field FieldLoyaltyCardNo { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN LoyaltyCardNo
        {
            get { return new StringN(this[FieldLoyaltyCardNo]); }
            set { this[FieldLoyaltyCardNo] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("cupon")]
        public static Field FieldCupon { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Cupon
        {
            get { return new StringN(this[FieldCupon]); }
            set { this[FieldCupon] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isHandPrice")]
        public static Field FieldIsHandPrice { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? IsHandPrice
        {
            get { return (int?)this[FieldIsHandPrice]; }
            set { this[FieldIsHandPrice] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("orignalSelPrc")]
        public static Field FieldOrignalSelPrc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? OrignalSelPrc
        {
            get { return (Decimal?)this[FieldOrignalSelPrc]; }
            set { this[FieldOrignalSelPrc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("orignalGrossPrc")]
        public static Field FieldOrignalGrossPrc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? OrignalGrossPrc
        {
            get { return (Decimal?)this[FieldOrignalGrossPrc]; }
            set { this[FieldOrignalGrossPrc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("orignalTotVal")]
        public static Field FieldOrignalTotVal { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? OrignalTotVal
        {
            get { return (Decimal?)this[FieldOrignalTotVal]; }
            set { this[FieldOrignalTotVal] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("selPrc")]
        public static Field FieldSelPrc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? SelPrc
        {
            get { return (Decimal?)this[FieldSelPrc]; }
            set { this[FieldSelPrc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("grossPrc")]
        public static Field FieldGrossPrc { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? GrossPrc
        {
            get { return (Decimal?)this[FieldGrossPrc]; }
            set { this[FieldGrossPrc] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("netVal")]
        public static Field FieldNetVal { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? NetVal
        {
            get { return (Decimal?)this[FieldNetVal]; }
            set { this[FieldNetVal] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("taxVal")]
        public static Field FieldTaxVal { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? TaxVal
        {
            get { return (Decimal?)this[FieldTaxVal]; }
            set { this[FieldTaxVal] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("totVal")]
        public static Field FieldTotVal { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? TotVal
        {
            get { return (Decimal?)this[FieldTotVal]; }
            set { this[FieldTotVal] = value; }
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
