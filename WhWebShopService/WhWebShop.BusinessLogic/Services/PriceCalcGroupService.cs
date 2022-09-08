using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IPriceCalcGroupService))]
internal class PriceCalcGroupService : IPriceCalcGroupService
{
    public void GroupCart(CalcJsonResultDto res)
    {
        var newCarts = new List<CalcItemJsonResultDto>();

        foreach (var i in res.Items)
        {
            CalcItemJsonResultDto foundCalcItemJsonResultDto = null!; 
            foreach (var ni in newCarts)
            {
                if (IsEqualCart(i, ni))
                {
                    foundCalcItemJsonResultDto = ni;
                    break;
                }
            }
            if (foundCalcItemJsonResultDto!=null)
            {
                AddQuantity(foundCalcItemJsonResultDto);
            }
            else
            {
                newCarts.Add(i);
            }

        }
        res.Items = newCarts.ToArray();
    }
      

    private void AddQuantity(CalcItemJsonResultDto foundCalcItemJsonResultDto)
    {
        foundCalcItemJsonResultDto.Quantity++;
        foundCalcItemJsonResultDto.RawSelVal = foundCalcItemJsonResultDto.RawSelPrc * foundCalcItemJsonResultDto.Quantity;
    }
  
    private bool IsEqualCart(CalcItemJsonResultDto i, CalcItemJsonResultDto ni)
    {
        if (i.ItemCode == ni.ItemCode &&
              i.RawOrigSelPrc == ni.RawOrigSelPrc &&
              i.RawSelPrc == ni.RawSelPrc)
        {
            return true;
        }
        return false;
    }
}
