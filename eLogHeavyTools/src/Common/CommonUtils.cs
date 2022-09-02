using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Common
{
    public static class CommonUtils
    {
        public enum PayerIdGenType
        {
            /// <summary>
            /// Vevoi szamla (10)
            /// </summary>
            Sinv = 10,
        }

        public static String PayerIdCDVGen(String payerId)
        {
            //Luhn character check digit
            string cdv = null;

            if (string.IsNullOrEmpty(payerId))
                return cdv;

            var s = 0;
            var a = true;
            var d = payerId.ToCharArray();
            for (int i = d.Length - 1; i >= 0; i--)
            {
                var n = (d[i] - 48);
                if (a)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }
                s += n;
                a = !a;
            }
            if ((s % 10) == 0)
                cdv = "0";
            else
                cdv = (10 - (s % 10)).ToString();

            return cdv;
        }
    }
}
