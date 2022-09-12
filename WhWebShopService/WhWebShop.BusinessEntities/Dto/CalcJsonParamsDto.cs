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
    public class CalcJsonParamsDto
    {
        [JsonField(true, false)]
        public bool FirstPurchase { get; set; } = false;

        /// <summary>
        /// Webhop ár
        /// </summary>
        [JsonField(true, false)]
        public string Wid { get; set; } = null!;

        /// <summary>
        /// Ország (áfa miatt)
        /// </summary>
        [JsonField(true, false)]
        public string CountryId { get; set; } = null!;

        [JsonField(true, false)]
        public string Curid { get; set; } = null!;

        [JsonField(false, false)]
        public string LoyaltyCardNo { get; set; } = null!;

        [JsonField(false, false)]
        public string[] Cupons { get; set; } = null!;

        [JsonField(true, true)]
        public List<CartItemJson> Items { get; set; } = new List<CartItemJson>();

        [JsonField(false, false)]
        public string? Whid { get; set; } = null!;

        [JsonField(false, false)]
        public int? Partnid { get; set; } = null!;

        [JsonField(false, false)]
        public int? AddrId { get; set; } = null!;
    }

    [JsonObjectAttributes("Items")]
    public class CartItemJson
    {
        [JsonField(null!, true, true)]
        public int? CartId { get; set; } = null!;

        [JsonField(null!, null!, null!)]
        public int? Itemid { get; set; } = null!;

        [JsonField(null!, true, true)]
        public string ItemCode { get; set; } = null!;

        [JsonField(null!, true, true)]
        public int? Quantity { get; set; } = null!;

        [JsonField(null!, false, true)]
        public decimal? OrignalSelPrc { get; set; } = null!;

        [JsonField(null!, false, true)]
        public decimal? OrignalTotprc { get; set; } = null!;

        /// <summary>
        /// Nettó egységár
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? SelPrc { get; set; } = null!;

        /// <summary>
        /// Bruttó egységár
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? GrossPrc { get; set; } = null!;


        /// <summary>
        /// Nettó érték  (Nettó egységár * Mennyiség)
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? NetVal { get; set; } = null!;

        /// <summary>
        /// Áfa érték
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? TaxVal { get; set; } = null!;


        /// <summary>
        /// Bruttó érték
        /// </summary>
        [JsonField(null!, false, true)]
        public decimal? TotVal { get; set; } = null!;

    }
}
