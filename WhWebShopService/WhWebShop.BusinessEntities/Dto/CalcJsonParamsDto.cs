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
        [JsonField(MandotaryType.Yes, MandotaryType.No)]
        public bool FirstPurchase { get; private set; } = false;

        /// <summary>
        /// B2B partner
        /// </summary>
        [JsonField("Wid", MandotaryType.Yes, MandotaryType.No)]
        public string B2B { get; private set; } = null!;

        /// <summary>
        /// Webhop ár
        /// </summary>
        [JsonField("B2B", MandotaryType.Yes, MandotaryType.No)]
        public string Wid { get; private set; } = null!;

        /// <summary>
        /// Ország (áfa miatt)
        /// </summary>
        [JsonField(MandotaryType.Yes, MandotaryType.No)]
        public string CountryId { get; private set; } = null!;

        [JsonField(MandotaryType.Yes, MandotaryType.No)]
        public string Curid { get; private set; } = null!;

        [JsonField(MandotaryType.No, MandotaryType.No)]
        public string LoyaltyCardNo { get; private set; } = null!;

        [JsonField(MandotaryType.No, MandotaryType.No)]
        public string[] Cupons { get; private set; } = null!;

        [JsonField(MandotaryType.Yes, MandotaryType.Yes)]
        public List<CartItemJson> Items { get; private set; } = new List<CartItemJson>();

        [JsonField(MandotaryType.No, MandotaryType.No)]
        public string? Whid { get; private set; } = null!;

        [JsonField(MandotaryType.No, MandotaryType.No)]
        public int? Partnid { get; private set; } = null!;

        [JsonField(MandotaryType.No, MandotaryType.No)]
        public int? AddrId { get; private set; } = null!;
    }

    [JsonObjectAttributes("Items")]
    public class CartItemJson : ICloneable
    {
        [JsonField(MandotaryType.Pointless, MandotaryType.Yes, MandotaryType.Yes)]
        public int? CartId { get; private set; } = null!;

        [JsonField("ItemCode", MandotaryType.Pointless, MandotaryType.Yes, MandotaryType.Yes)]
        public int? Itemid { get; private set; } = null!;

        [JsonField("Itemid", MandotaryType.Pointless, MandotaryType.Yes, MandotaryType.Yes)]
        public string ItemCode { get; set; } = null!;

        [JsonField(MandotaryType.Pointless, MandotaryType.Yes, MandotaryType.Yes)]
        public int? Quantity { get; private set; } = null!;

        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? OrignalSelPrc { get; private set; } = null!;

        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? OrignalTotprc { get; private set; } = null!;

        /// <summary>
        /// Nettó egységár
        /// </summary>
        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? SelPrc { get; private set; } = null!;

        /// <summary>
        /// Bruttó egységár
        /// </summary>
        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? GrossPrc { get; private set; } = null!;


        /// <summary>
        /// Nettó érték  (Nettó egységár * Mennyiség)
        /// </summary>
        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? NetVal { get; private set; } = null!;

        /// <summary>
        /// Áfa érték
        /// </summary>
        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? TaxVal { get; private set; } = null!;


        /// <summary>
        /// Bruttó érték
        /// </summary>
        [JsonField(MandotaryType.Pointless, MandotaryType.No, MandotaryType.Yes)]
        public decimal? TotVal { get; private set; } = null!;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
