using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{
    public class CalcJsonParamsDto
    {
        public bool FirstPurchase { get; set; } = false;

        /// <summary>
        /// Webhop ár
        /// </summary>
        public string Wid { get; set; }

        /// <summary>
        /// Ország (áfa miatt)
        /// </summary>
        public string CountryId { get; set; }

         public string Curid { get; set; }

        public string LoyaltyCardNo { get; set; }

        public string[] Cupons { get; set; }

        public List<CartItemJson> Items { get; set; } = new List<CartItemJson>();

        public string Whid { get; set; }

        public int? Partnid { get; set; }

        public int? AddrId { get; set; }
    }

    public class CartItemJson
    {
        public int? CartId { get; set; }

        public int? Itemid { get; set; }

        public string ItemCode { get; set; }

        public int? Quantity { get; set; }
    }
}
