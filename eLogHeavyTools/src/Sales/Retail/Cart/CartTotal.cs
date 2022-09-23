using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Retail.Cart
{
    internal class CartTotal
    {
        decimal originalValue;
        decimal totValue;
        decimal payvalue;
        bool isCard = false;
        string curid;
        public CartTotal(string userID, string curid)
        {
            this.curid = curid;
            var sql = $@"
select isnull(originalValue,.0) originalValue,  isnull(totValue,.0) totValue, isnull(payvalue,.0) payvalue 
  from (
	select sum(orignalGrossPrc) originalValue, sum(totVal) totValue
	  from olc_cart c
	 where c.addusrid='{userID}'
) c
outer apply (
	select sum(payvalue) payvalue from olc_cartpay cp
	 where cp.addusrid='{userID}' 
) cp";
            foreach (var row in SqlDataAdapter.Query(sql).AllRows)
            {
                originalValue = ConvertUtils.ToDecimal(row["originalValue"]).Value;
                totValue = ConvertUtils.ToDecimal(row["totValue"]).Value;
                payvalue = ConvertUtils.ToDecimal(row["payvalue"]).Value;

            }

            isCard = (int)SqlDataAdapter.ExecuteSingleValue(DB.Main, 
                $@"select isnull( sum( case when fm.paymid='KARTYA' then 1 else 0 end),0) isBK
  from olc_cartpay cp
  join fin_paymethod fm on fm.finpaymid=cp.finpaymid
	 where cp.addusrid='{userID}'") > 0;


        }

        internal decimal GetOriginalValue()
        {
            return originalValue;
        }

        internal decimal GetDiscValue()
        {
            return originalValue - totValue;
        }

        internal decimal GetTotValue()
        {
            return totValue;
        }

        internal decimal GetPayValue()
        {
            if (curid != "HUF")
            {
                return totValue;
            }

            if (isCard)
            {
                return totValue;
            }
            else
            {
                return (int)Math.Round(totValue / 5) * 5;
            }
        }

        internal decimal GetMissingValue()
        {
            var m=GetPayValue() - payvalue;
            if (m < 0)
            {
                m = 0;
            }
            return m;
        }
    }
}
