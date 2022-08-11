using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.PriceCalc
{
    public class PriceCalcBL
    {
        public static CalcJsonResultDto DoCalc(CalcJsonParamsDto? calcitem)
        {
            var o = new CalcJsonResultDto
            {
                Curid = "HUF"
            };
            var l = new List<CalcItemJsonResultDto>();

            foreach (var i in calcitem!.Items)
            {
                l.Add(new CalcItemJsonResultDto
                {
                    ItemCode = i.ItemCode,
                    Quantity = i.Quantity,
                    OrigSelVal = 200.2M,
                    SelPrc = 100.5M,
                    SetTotPrc = Math.Round(127.63M, 2),

                    SelVal = Math.Round(100.5M * i.Quantity!.Value, 2),
                    SelTotVal = Math.Round(127.635M * i.Quantity!.Value, 2),

                });
            }
            o.Items = l.ToArray();
            o.Success = true;
            return o;
        }
    }
}
