using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto
{ 
    [JsonObjectAttributes("Cart")]
    public class CalcJsonParamsDto : ICloneable
    {
        [JsonField(true, false)]
        public bool FirstPurchase { get; private set; } = false;

        /// <summary>
        /// Webhop ár
        /// </summary>
        [JsonField(true, false)]
        public string Wid { get; private set; } = null!;

        /// <summary>
        /// Ország (áfa miatt)
        /// </summary>
        [JsonField(true, false)]
        public string CountryId { get; private set; } = null!;

        [JsonField(true, false)]
        public string Curid { get; private set; } = null!;

        [JsonField(false, false)]
        public string LoyaltyCardNo { get; private set; } = null!;

        [JsonField(false, false)]
        public string[] Cupons { get; private set; } = null!;

        [JsonField(true, true)]
        public List<CartItemJson> Items { get; private set; } = new List<CartItemJson>();

        [JsonField(false, false)]
        public string? Whid { get; private set; } = null!;

        [JsonField(false, false)]
        public int? Partnid { get; private set; } = null!;

        [JsonField(false, false)]
        public int? AddrId { get; private set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    [JsonObjectAttributes("Items")]
    public class CartItemJson : ICloneable
    {
        [JsonField(null!, true, true)]
        public int? CartId { get; private set; } = null!;

        [JsonField(null!, null!, null!)]
        public int? Itemid { get; private set; } = null!;

        [JsonField(null!, true, true)]
        public string ItemCode { get; set; } = null!;

        [JsonField(null!, true, true)]
        public int? Quantity { get; private set; } = null!;

        [JsonField(null!, false, true)]
        public decimal? OrignalSelPrc { get; private set; } = null!;

        [JsonField(null!, false, true)]
        public decimal? OrignalTotprc { get; private set; } = null!;

        /// <summary>
        /// Nettó egységár
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? SelPrc { get; private set; } = null!;

        /// <summary>
        /// Bruttó egységár
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? GrossPrc { get; private set; } = null!;


        /// <summary>
        /// Nettó érték  (Nettó egységár * Mennyiség)
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? NetVal { get; private set; } = null!;

        /// <summary>
        /// Áfa érték
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? TaxVal { get; private set; } = null!;


        /// <summary>
        /// Bruttó érték
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? TotVal { get; private set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
