using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Common
{
    internal class PurchaseSqlFunctions3
    {
        // egyedi szallitoi szamla partner-e? A partner (szallito) neve es cime modosithato a szamlan
        public static bool IsPinvCustomPartner(int? partnId)
        {
            var b = false;
            if (partnId.HasValue)
            {
                var x = Base.Common.SysvalCache.GetValue("pinv:CustomPartner");
                if (x != null && !x.Missing)
                {
                    if (x.ValueObject is string)
                    {
                        if (x.ValueObject.ToString().Split(',').Contains(partnId.ToString()))
                        {
                            b = true;
                        }
                    }
                    else if (x.ValueObject is int)
                    {
                        b = Convert.ToInt32(x.ValueObject) == partnId;
                    }
                }
            }

            return b;
        }

    }
}
