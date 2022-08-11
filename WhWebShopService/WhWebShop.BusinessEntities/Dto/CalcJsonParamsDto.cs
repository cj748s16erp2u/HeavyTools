using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
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
        public string Wid { get; set; } = null!;
        
        [JsonField(true, false)]
        public string CountryId { get; set; } = null!; 

        [JsonField(false)]
        public string LoyaltyCardNo { get; set; } = null!;

        [JsonField(false)]
        public string[] Cupons { get; set; } = null!;

        [JsonField(true)]
        public List<CartItemJson> Items { get; set; } = new List<CartItemJson>();
    }

    [JsonObjectAttributes("Items")]
    public class CartItemJson
    {
        [JsonField(true)]
        public string ItemCode { get; set; } = null!;

        [JsonField(true)]
        public int? Quantity { get; set; } = null!;
    }
}
