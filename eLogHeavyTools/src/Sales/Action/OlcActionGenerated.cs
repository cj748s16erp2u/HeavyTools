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
    [Table("olc_action")]
    public partial class OlcAction : Entity<OlcAction>
    {
        /// <summary>
        /// <para>Load by primary keys</para>
        /// </summary>
        /// <param name="aid"><see cref="int?" /> aid</param>
        public static OlcAction Load(int? aid)
        {
            return Load(new Key(new Field[] { FieldAid }, new object[] { aid }));
        }

        #region Field accessors

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
        /// ? - int
        /// </summary>
        [Field("actiontype")]
        public static Field FieldActiontype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Actiontype
        {
            get { return (int?)this[FieldActiontype]; }
            set { this[FieldActiontype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isactive")]
        public static Field FieldIsactive { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isactive
        {
            get { return (int?)this[FieldIsactive]; }
            set { this[FieldIsactive] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isextcondition")]
        public static Field FieldIsextcondition { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isextcondition
        {
            get { return (int?)this[FieldIsextcondition]; }
            set { this[FieldIsextcondition] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("isextdiscount")]
        public static Field FieldIsextdiscount { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Isextdiscount
        {
            get { return (int?)this[FieldIsextdiscount]; }
            set { this[FieldIsextdiscount] = value; }
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
        [Field("priority")]
        public static Field FieldPriority { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Priority
        {
            get { return (int?)this[FieldPriority]; }
            set { this[FieldPriority] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("curid")]
        public static Field FieldCurid { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Curid
        {
            get { return new StringN(this[FieldCurid]); }
            set { this[FieldCurid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("singlecouponnumber")]
        public static Field FieldSinglecouponnumber { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Singlecouponnumber
        {
            get { return new StringN(this[FieldSinglecouponnumber]); }
            set { this[FieldSinglecouponnumber] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("couponunlimiteduse")]
        public static Field FieldCouponunlimiteduse { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Couponunlimiteduse
        {
            get { return (int?)this[FieldCouponunlimiteduse]; }
            set { this[FieldCouponunlimiteduse] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discounttype")]
        public static Field FieldDiscounttype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discounttype
        {
            get { return (int?)this[FieldDiscounttype]; }
            set { this[FieldDiscounttype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("discountval")]
        public static Field FieldDiscountval { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Discountval
        {
            get { return (Decimal?)this[FieldDiscountval]; }
            set { this[FieldDiscountval] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discountforfree")]
        public static Field FieldDiscountforfree { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discountforfree
        {
            get { return (int?)this[FieldDiscountforfree]; }
            set { this[FieldDiscountforfree] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discountfreetransportation")]
        public static Field FieldDiscountfreetransportation { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discountfreetransportation
        {
            get { return (int?)this[FieldDiscountfreetransportation]; }
            set { this[FieldDiscountfreetransportation] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discountcalculationtype")]
        public static Field FieldDiscountcalculationtype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discountcalculationtype
        {
            get { return (int?)this[FieldDiscountcalculationtype]; }
            set { this[FieldDiscountcalculationtype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("discountaid")]
        public static Field FieldDiscountaid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Discountaid
        {
            get { return (int?)this[FieldDiscountaid]; }
            set { this[FieldDiscountaid] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("validdatefrom")]
        public static Field FieldValiddatefrom { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Validdatefrom
        {
            get { return (DateTime?)this[FieldValiddatefrom]; }
            set { this[FieldValiddatefrom] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("validforsaleproducts")]
        public static Field FieldValidforsaleproducts { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Validforsaleproducts
        {
            get { return (int?)this[FieldValidforsaleproducts]; }
            set { this[FieldValidforsaleproducts] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - datetime
        /// </summary>
        [Field("validdateto")]
        public static Field FieldValiddateto { get; protected set; }
        /// <summary>
        /// ? - datetime
        /// </summary>
        public DateTime? Validdateto
        {
            get { return (DateTime?)this[FieldValiddateto]; }
            set { this[FieldValiddateto] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("validtotvalfrom")]
        public static Field FieldValidtotvalfrom { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Validtotvalfrom
        {
            get { return (Decimal?)this[FieldValidtotvalfrom]; }
            set { this[FieldValidtotvalfrom] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - numeric
        /// </summary>
        [Field("validtotvalto")]
        public static Field FieldValidtotvalto { get; protected set; }
        /// <summary>
        /// ? - numeric
        /// </summary>
        public Decimal? Validtotvalto
        {
            get { return (Decimal?)this[FieldValidtotvalto]; }
            set { this[FieldValidtotvalto] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("purchasetype")]
        public static Field FieldPurchasetype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Purchasetype
        {
            get { return (int?)this[FieldPurchasetype]; }
            set { this[FieldPurchasetype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("filtercustomerstype")]
        public static Field FieldFiltercustomerstype { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Filtercustomerstype
        {
            get { return (int?)this[FieldFiltercustomerstype]; }
            set { this[FieldFiltercustomerstype] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("filteritems")]
        public static Field FieldFilteritems { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Filteritems
        {
            get { return new StringN(this[FieldFilteritems]); }
            set { this[FieldFilteritems] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - varchar
        /// </summary>
        [Field("filteritemsblock")]
        public static Field FieldFilteritemsblock { get; protected set; }
        /// <summary>
        /// ? - varchar
        /// </summary>
        public StringN Filteritemsblock
        {
            get { return new StringN(this[FieldFilteritemsblock]); }
            set { this[FieldFilteritemsblock] = value; }
        }

        /// <summary>
        /// <para>Field</para>
        /// ? - int
        /// </summary>
        [Field("count")]
        public static Field FieldCount { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Count
        {
            get { return (int?)this[FieldCount]; }
            set { this[FieldCount] = value; }
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
        [Field("netgoid")]
        public static Field FieldNetgoid { get; protected set; }
        /// <summary>
        /// ? - int
        /// </summary>
        public int? Netgoid
        {
            get { return (int?)this[FieldNetgoid]; }
            set { this[FieldNetgoid] = value; }
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
