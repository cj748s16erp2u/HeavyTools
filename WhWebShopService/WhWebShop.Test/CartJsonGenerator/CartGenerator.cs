using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.Test.CartJsonGenerator
{

    internal class CartGenerator
    {

        public static string GetRandomCart()
        {
            var rnd = new Random();
            int itemnum = rnd.Next(2, 10); 

            var str = @"
{ 
    ""Cart"": {
        ""Wid"": ""com"",
        ""Curid"": ""EUR"",
        ""FirstPurchase"": ""false"",
		""LoyaltyCardNo"": ""ABC12345678"",
        ""CountryId"": ""HU"",
		""Cupons"": [
			""XXX""],
        ""Items"": [
                
 ";

            for (int i = 0; i < itemnum; i++)
            {
                int qty = rnd.Next(1, 3);

                str += $@"{{
				""CartId"": ""{i}"",
				""ItemCode"": ""{ItemCodes.Itemcodes[i]}"",
				""Quantity"": {qty}
			}},";
            }

            str += @"]
	}
}";
            return str;
        } 
    }
}
