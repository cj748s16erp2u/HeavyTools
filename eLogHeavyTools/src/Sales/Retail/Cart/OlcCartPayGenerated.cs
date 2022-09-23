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
    [Table("olc_cartpay")]
    public partial class OlcCartPay : Entity<OlcCartPay>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="cartpayid"><see cref="int?" /> cartpayid</param>
        public static OlcCartPay Load(int? cartpayid)
        {
            return Load(new Key(new Field[] { FieldCartpayid }, new object[] { cartpayid }));
        }

        #region Field accessors

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("cartpayid")]
        public static Field FieldCartpayid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Cartpayid
        {
            get { return (int?)this[FieldCartpayid]; }
            set { this[FieldCartpayid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("payvalue")]
        public static Field FieldPayvalue { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Payvalue
        {
            get { return (Decimal?)this[FieldPayvalue]; }
            set { this[FieldPayvalue] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("finpaymid")]
        public static Field FieldFinpaymid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Finpaymid
        {
            get { return new StringN(this[FieldFinpaymid]); }
            set { this[FieldFinpaymid] = value; }
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
