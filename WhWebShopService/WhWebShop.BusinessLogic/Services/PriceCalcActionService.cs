using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IPriceCalcActionService))]
public class PriceCalcActionService : IPriceCalcActionService
{ 
    private readonly IOlcActionCacheService olcActionCacheService;
    private readonly ILoyaltycardService loyaltycardService;

    public PriceCalcActionService(
        IOlcActionCacheService olcActionCacheService, ILoyaltycardService loyaltycardService)
    {
        this.olcActionCacheService = olcActionCacheService ?? throw new ArgumentNullException(nameof(olcActionCacheService));
        this.loyaltycardService = loyaltycardService ?? throw new ArgumentNullException(nameof(loyaltycardService));
    }

    private void CheckWebshop(CalcJsonParamsDto cart, ActionDataGroupDto olcAction)
    { 
        //ha nincs szűrés mindenre érvényes
        if (olcAction.OlcActionwebhops.Count() > 0)
        {
            var found = false;
            foreach (var item in olcAction.OlcActionwebhops)
            {
                if (item.Wid == cart.Wid)
                {
                    found = true;
                }
            }
            if (!found)
            {
                throw new Exception("Nem engedélyezett ebben a webshopban");
            }
        }
    }

    public async Task CalculateActionPriceAsync(CalcJsonParamsDto cart, CalcJsonResultDto cartRes, PriceCalcActionResultDto pricecalcactionresult, CancellationToken cancellationToken)
    { 
        if (cart.Curid == null)
        {
            throw new Exception("Nincs Curid");
        }
        decimal loyaltyTotalPurchaseAmount = 0;

        if (!string.IsNullOrEmpty(cart.LoyaltyCardNo))
        {
            loyaltyTotalPurchaseAmount = await loyaltycardService.GetTotalPurchaseAmount(cart.LoyaltyCardNo, cart.Curid, cancellationToken);
        }




        var actionList = new List<ActionDataGroupDto>();

        var assB = await olcActionCacheService.GetActiveActionsAsync(cart.Cupons, cancellationToken);


        var ass = assB.OrderBy(p => p.Priority ?? 0);

        foreach (var action in ass)
        {
            try
            {
                // Ha webes ár meg kell nézni, hogy engedélyezett-e
                if (!string.IsNullOrEmpty(cart.Wid))
                {
                    CheckWebshop(cart, action);
                }

                // Ha kiser ár meg kell nézni, hogy engedélyezett-e
                if (!string.IsNullOrEmpty(cart.Whid))
                {
                    // TODO CheckWarehouse
                    CheckWarehouse(cart, action);
                }

                // Ha partner ár meg kell nézni, hogy engedélyezett-e
                if (cart.AddrId.HasValue)
                {   // TODO CheckAddress
                    //await CheckAddress(cart, olcAction);
                }
                if (cart.Partnid.HasValue)
                {   // TODO CheckPartner
                    //await CheckPartner(cart, olcAction);
                } 
                actionList.Add(action);
            }
            catch (Exception e)
            {
                pricecalcactionresult.ActionResults.Add(new PriceCalcActionResultItemDto(action.Aid, e.Message));
            }
        }


        foreach (var action in actionList)
        {
            string whyNotMessage;
            int cnt;
            try
            {
                var co=CalculeteMultiple(action, cart, cartRes, loyaltyTotalPurchaseAmount);

                whyNotMessage = co.WhyNotMessage;
                cnt = co.Cnt;
                if (cnt > 0)
                {
                    pricecalcactionresult.ActionResults.Add(new PriceCalcActionResultItemDto(action.Aid));
                }
                else
                {
                    pricecalcactionresult.ActionResults.Add(new PriceCalcActionResultItemDto(action.Aid, whyNotMessage));
                }
            }
            catch (Exception e)
            {
                pricecalcactionresult.ActionResults.Add(new PriceCalcActionResultItemDto(action.Aid, e.Message));
            }
        }

        foreach (var ci in cartRes.Items)
        {
            if (ci.CartActionType == CartActionType.None)
            {
                ci.RawSelPrc = ci.RawOrigSelPrc!.Value;
                ci.RawSelVal = ci.RawSelPrc; // 1 db van itt
            }
        }
    }

    private void CheckWarehouse(CalcJsonParamsDto cart, ActionDataGroupDto olcAction)
    {
        //ha nincs szűrés mindenre érvényes
        if (olcAction.OlcActionretails.Count() > 0)
        {
            var found = false;
            foreach (var item in olcAction.OlcActionretails)
            {
                if (item.Whid == cart.Whid)
                {
                    found = true;
                }
            }
            if (!found)
            {
                throw new Exception("Nem engedélyezett ebben a boltban");
            }
        }
    }

    private MultiCalculeteOut CalculeteMultiple(ActionDataGroupDto action, CalcJsonParamsDto cart, CalcJsonResultDto cartRes, decimal loyaltyTotalPurchaseAmount)
    {
        var co = new MultiCalculeteOut
        {
            Cnt = 0
        };
        var used = false;

        for (; ; )
        {

            var c = Calculete(action, cart, cartRes, loyaltyTotalPurchaseAmount);

            if (c.Used)
            {
                used = true;
                co.Cnt++;

                if (cartRes.TempFreeFee)
                {
                    cartRes.FreeFee = true;
                }
                if (cartRes.TempFreeShipping)
                {
                    cartRes.FreeShipping = true;
                }

            }

            if (!c.Used)
            {
                co.WhyNotMessage = c.WhyNotMessage;
                break;
            }
        }

        if (used)
        {
            co.WhyNotMessage = null!;
        }
        return co;
    }

    private CalculeteOut Calculete(ActionDataGroupDto action, CalcJsonParamsDto cart, CalcJsonResultDto cartRes, decimal loyaltyTotalPurchaseAmount)
    { 
        foreach (var item in cartRes.Items)
        {
            if (item.CartActionType == CartActionType.TempCalculated)
            {
                item.CartActionType = CartActionType.None;
            }
        }

        if (action.Curid != null && !action.Curid.Equals(cart.Curid))
        { 
            return new CalculeteOut()
            {
                WhyNotMessage = "Eltérő pénznem",
                Used = false
            }; 
        }

        // Összetett és egyszerű érték feltétel
        var cartTotval = CalcCartTotVal(cartRes);
        if (action.Validtotvalfrom > cartTotval)
        {
            return new CalculeteOut()
            {
                WhyNotMessage = "Kevés érték van a kosárban",
                Used = false
            }; 
        }
        // Összetett és egyszerű érték feltétel
        if (action.Validtotvalto < cartTotval)
        {
            return new CalculeteOut()
            {
                WhyNotMessage = "Több érték van a kosárban",
                Used = false
            }; 
        }
        
        // Összetett és egyszerű vevő feltétel
        if (action.Filtercustomerstype.HasValue)
        {
          
            switch (action.FilterCustomersTypeEnum)
            {
                case FilterCustomersType.All:
                    break;
                case FilterCustomersType.OnlyLoyalty:
                    if (string.IsNullOrEmpty(cart.LoyaltyCardNo))
                    {
                        return new CalculeteOut()
                        {
                            WhyNotMessage = "Nem törzsvásárló",
                            Used = false
                        };
                    }
                    break;
                case FilterCustomersType.NotForLoyalty:
                    if (!string.IsNullOrEmpty(cart.LoyaltyCardNo))
                    {
                        return new CalculeteOut()
                        {
                            WhyNotMessage = "Törzsvásárlóra nem érvényes",
                            Used = false
                        };
                    }
                    break;
                case FilterCustomersType.NotForResale:
                    return new CalculeteOut()
                    {
                        //TODO: b2b értékesítés-e
                        WhyNotMessage = "NotImplementedException:Filtercustomers: NotForResale",
                        Used = false
                    };
                default:
                    return new CalculeteOut()
                    {
                        WhyNotMessage = "Filtercustomers: Ismeretlen",
                        Used = false
                    };
            }
        }

        // Összetett és egyszerű kosár tartalom db feltétel
        if (action.Count.HasValue && action.Count.Value > cart.Items.Count)
        {
            return new CalculeteOut()
            {
                WhyNotMessage = "Kevés db cikk van a kosárban",
                Used = false
            };
        }

        // Összetett és egyszerű cupon feltétel
        if (action.ActionTypeEnum == BusinessEntities.Enums.ActionType.Cupon)
        {
            var found = false;
            
            foreach (var c in action.OlcActioncouponnumbers)
            {
                foreach (var cupon in cart.Cupons)
                {
                    if (c.Couponnumber == cupon)
                    {
                        found = true;
                    }
                }
            }

            foreach (var cupon in cart.Cupons)
            {
                if (action.Singlecouponnumber == cupon)
                {
                    found = true;
                }
            }
            if (!found)
            {
                return new CalculeteOut()
                {
                    WhyNotMessage = "Nincs hozzá kupon",
                    Used = false
                };
            }
        }
        
        if (action.ActionTypeEnum == BusinessEntities.Enums.ActionType.Loyaltycardno)
        {
            if (!string.IsNullOrEmpty(cart.LoyaltyCardNo))
            {
                if (action.Validtotvalfrom > loyaltyTotalPurchaseAmount)
                {
                    return new CalculeteOut()
                    {
                        WhyNotMessage = $"Törzsvásárló: nem vásárolt még eleget {loyaltyTotalPurchaseAmount}<{action.Validtotvalfrom}",
                        Used = false
                    };
                }
            }
        }
        
        var cartToUse = new List<CalcItemJsonResultDto>();
        var conditionCarts = new List<CalcItemJsonResultDto>();

        //Össztett feltétel
        if (action.Isextcondition == 1)
        {
            foreach (var ae in action.OlcActionexts)
            {
                if (ae.Isdiscount == 0)
                {
                    var found = 0;
                    foreach (var item in cartRes.Items)
                    {
                        var canAdd = BaseCanAdd(action, item, ae.Filteritems, ae.Filteritemsblock!);

                        if (canAdd)
                        {
                            found++;

                            item.CartActionType = CartActionType.TempCalculated;
                            cartToUse.Add(item);
                            conditionCarts.Add(item);

                            if (found == ae.Count)
                            {
                                break;
                            }
                        }
                    }
                    if (found < ae.Count)
                    {
                        return new CalculeteOut()
                        {
                            WhyNotMessage = "Össztett  feltétel, hiányzó feltétel tétel",
                            Used = false
                        };
                    }
                }
            }  
        } else
        { 
            foreach (var item in cartRes.Items)
            {
                var add = BaseCanAdd(action, item, action.Filteritems!, action.Filteritemsblock!);

                if (add)
                {
                    item.CartActionType = CartActionType.TempCalculated;
                    cartToUse.Add(item);
                }
            }

            if (cartToUse.Count == 0)
            {
                return new CalculeteOut()
                {
                    WhyNotMessage = "Nincs cikk a kosárban, amire érvényesíteni lehetne",
                    Used = false
                };
            }
        }

        if (action.PurchasetypeEnum == BusinessEntities.Enums.Purchasetype.First && !cart.FirstPurchase)
        {
            return new CalculeteOut()
            {
                WhyNotMessage = "Nem az első vásárlás",
                Used = false
            };
        }

        //Volt-e bármilyen kedvezmény számolva
        var singleDiscountCount = 0;

        //Ingyen fizetés 
        if (action.Discountforfree.HasValue && action.Discountforfree.Value == 1)
        {
            if (!cartRes.TempFreeFee && !cartRes.FreeFee)
            {
                cartRes.TempFreeFee = true;
                singleDiscountCount++;
            }
        }        
        
        //Ingyen szállítás
        if (action.Discountfreetransportation.HasValue && action.Discountfreetransportation.Value==1)
        {
            if (!cartRes.TempFreeShipping && !cartRes.FreeShipping)
            {
                cartRes.TempFreeShipping = true;
                singleDiscountCount++;
            }
        }

        if (action.Discountaid.HasValue)
        {
            //TODO: blokk kupon 
            return new CalculeteOut()
            {
                WhyNotMessage = "NotImplementedException:Discountaid ",
                Used = false
            };
        }

        //Egyszerű kedvezmény
        if (action.Isextdiscount == 0)
        { 
           

            //A kedvezményt kupon formájában adjuk
            if (action.DiscounttypeEnum == BusinessEntities.Enums.Discounttype.Cupon)
            {
                //TODO blokk kupon
                return new CalculeteOut()
                {
                    WhyNotMessage = "NotImplementedException:Discounttype ",
                    Used = false
                };
            }
            else
            //A kedvezményt érték formájában adjuk
            if (action.DiscounttypeEnum == BusinessEntities.Enums.Discounttype.Val)
            {
                if (!action.Discountval.HasValue)
                {
                    return new CalculeteOut()
                    {
                        WhyNotMessage = "Nincs kedvezmény érték megadva",
                        Used = false
                    };
                }
                if (action.Discountval.Value>0)
                switch (action.DiscountcalculationtypeEnum)
                {
                    case BusinessEntities.Enums.Discountcalculationtype.Division:
                        switch (action.PurchasetypeEnum)
                        {
                            case BusinessEntities.Enums.Purchasetype.First:
                            case BusinessEntities.Enums.Purchasetype.All:

                                DivisionDiscountValue(cartToUse, action.Discountval!.Value);

                                singleDiscountCount++;
                                break;
                            case BusinessEntities.Enums.Purchasetype.Cheapest:
                                // meg kell keresni a legolcsóbat
                                CalcItemJsonResultDto cc = null!;
                                foreach (var ci in cartToUse)
                                {
                                    cc.CartActionType = CartActionType.None;
                                    if (cc == null || cc.RawOrigSelPrc > ci.RawOrigSelPrc)
                                    {
                                        cc = ci;
                                    }
                                }
                                cc.RawSelPrc = cc.RawOrigSelPrc!.Value - action.Discountval;
                                if (cc.RawSelPrc < 0)
                                {
                                    cc.RawOrigSelPrc = 0;
                                }
                                cc.RawSelVal = cc.RawSelPrc; // 1 db van itt
                                cc.Aid = action.Aid;
                                cc.AidName = action.Name;
                                cc.CartActionType = CartActionType.Calculated;
                                singleDiscountCount++;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(action.Discountcalculationtype));
                        }
                        break;
                    //TODO: 1 termék
                    case BusinessEntities.Enums.Discountcalculationtype.OneProduct:
                        throw new NotImplementedException();
                    default:
                        throw new ArgumentOutOfRangeException(nameof(action.Discountcalculationtype));
                }
            }
            else
            //A kedvezményt % formájában adjuk
            if (action.DiscounttypeEnum == BusinessEntities.Enums.Discounttype.Percent)
            {
                if (!action.Discountval.HasValue)
                {
                    return new CalculeteOut()
                    {
                        WhyNotMessage = "Nincs kedvezmény érték megadva",
                        Used = false
                    };
                }
                if (action.Discountval.Value>0)
                    switch (action.DiscountcalculationtypeEnum)
                    {
                        //TODO: 1 termék
                        case BusinessEntities.Enums.Discountcalculationtype.OneProduct:
                            throw new NotImplementedException();


                        default: 
                            switch (action.PurchasetypeEnum)
                            {
                                case BusinessEntities.Enums.Purchasetype.First:
                                case BusinessEntities.Enums.Purchasetype.All:
                                    foreach (var ci in cartToUse)
                                    {
                                        ci.RawSelPrc = 
                                            ci.RawOrigSelPrc!.Value / 100 * (100-action.Discountval.Value);
                                        ci.RawSelVal = ci.RawSelPrc; // 1 db van itt
                                        ci.CartActionType = CartActionType.Calculated;
                                        ci.Aid = action.Aid;
                                        ci.AidName = action.Name;
                                    }
                                    singleDiscountCount++;
                                    break;
                                case BusinessEntities.Enums.Purchasetype.Cheapest:
                                    // meg kell keresni a legolcsóbat
                                    CalcItemJsonResultDto cc = null!;
                                    foreach (var ci in cartToUse)
                                    {
                                        cc.CartActionType = CartActionType.None;
                                        if (cc == null || cc.RawOrigSelPrc > ci.RawOrigSelPrc)
                                        {
                                            cc = ci;
                                        }
                                    }
                                    cc.RawSelPrc = cc.RawOrigSelPrc!.Value / 100 * (100-action.Discountval.Value);
                                    cc.RawSelVal = cc.RawSelPrc; // 1 db van itt
                                    cc.Aid = action.Aid;
                                    cc.AidName = action.Name;
                                    cc.CartActionType = CartActionType.Calculated;
                                    singleDiscountCount++;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(action.Discountcalculationtype));
                            }
                            break;
                    }
            }
       

            if (singleDiscountCount > 0)
            {
                return new CalculeteOut()
                {
                    Used = true
                };
            }
        }
        else
        {
            //Összetett kedvezmény

            var discountCount = 0;

            var allDiscountCount = 0;

            foreach (var ae in action.OlcActionexts)
            {
             
                decimal allPrice = 0;
                decimal allCount = 0;

                var discountCarts = new List<CalcItemJsonResultDto>();

                if (ae.Isdiscount == 1)
                {
                    allDiscountCount++;
                    var found = 0;
                    foreach (var item in cartRes.Items)
                    {
                        var canAdd = BaseCanAdd(action, item, ae.Filteritems, ae.Filteritemsblock!);

                        if (canAdd)
                        {
                            found++;
                            allPrice += item.RawOrigSelPrc!.Value;
                            allCount++;

                            item.CartActionType = CartActionType.TempCalculated;

                            discountCarts.Add(item);
                            cartToUse.Add(item);
                            if (found == ae.Count)
                            {
                                break;
                            }
                        }
                    }
                    if (found < ae.Count)
                    {
                        return new CalculeteOut()
                        {
                            WhyNotMessage = "Össztett  feltétel, hiányzó kedvezmény tétel",
                            Used = false
                        };
                    }

                    decimal discountValue = 0;

                    //A kedvezményt érték formájában adjuk
                    if (ae.DiscounttypeEnum== BusinessEntities.Enums.Discounttype.Val)
                    {
                        if (!ae.Discountval.HasValue)
                        {
                            return new CalculeteOut()
                            {
                                WhyNotMessage = "Nincs kedvezmény érték megadva",
                                Used = false
                            };
                        }

                        discountValue = ae.Discountval.Value;
                    }
                    else
                    //A kedvezményt % formájában adjuk
                    if (ae.DiscounttypeEnum == BusinessEntities.Enums.Discounttype.Percent)
                    {
                        if (!ae.Discountval.HasValue)
                        {
                            return new CalculeteOut()
                            {
                                WhyNotMessage = "Nincs kedvezmény érték megadva",
                                Used = false
                            };
                        }

                        foreach (var c in discountCarts)
                        {
                            discountValue += c.RawOrigSelPrc!.Value / 100 *             ae.Discountval!.Value;
                        }
                    }

                    foreach (var ci in cartToUse)
                    {
                        //Minden cikket megjelölünk akcióban résztvevőnek
                        ci.CartActionType = CartActionType.Calculated;
                        ci.Aid = action.Aid;
                        ci.AidName = action.Name;
                    }
                    if (discountValue > 0)
                        switch (ae.DiscountcalculationtypeEnum)
                        {
                            case BusinessEntities.Enums.Discountcalculationtype.Division:
                                DivisionDiscountValue(cartToUse, discountValue);
                                discountCount++;
                                break;
                            case BusinessEntities.Enums.Discountcalculationtype.ExtDiscountOnly:
                                DivisionDiscountValue(discountCarts, discountValue);
                                discountCount++;
                                break;
                            case BusinessEntities.Enums.Discountcalculationtype.ExtConditionOnly:
                                DivisionDiscountValue(conditionCarts, discountValue);
                                discountCount++;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(ae.Discountcalculationtype));
                        }
                }
            }
              
            if (discountCount== allDiscountCount)
            {
                return new CalculeteOut()
                {
                    Used = true
                };
            }

            return new CalculeteOut()
            {
                WhyNotMessage = "Összetett kedvezmény: Nincs minden kedvezmény érvényesítve",
                Used = false
            };

        }
        throw new NotImplementedException();
    }

    private void DivisionDiscountValue(List<CalcItemJsonResultDto> cart, decimal discountValue)
    {
        decimal fullprc = 0;
        foreach (var ci in cart)
        {
            fullprc += ci.RawOrigSelPrc!.Value;
        }
        foreach (var ci in cart)
        {
            ci.RawSelPrc =
                ci.RawOrigSelPrc!.Value - (discountValue / 100 * (ci.RawOrigSelPrc!.Value / fullprc * 100));
            ci.RawSelVal = ci.RawSelPrc; // 1 db van itt
        }
    }

    private bool BaseCanAdd(ActionDataGroupDto action, CalcItemJsonResultDto item, string filteritems, string filteritemsblock)
    {
        bool add = false;
        if (item.CartActionType == CartActionType.None)
        {
            if (ItemCodeAllowed(item.ItemCode, filteritems!, filteritemsblock!))
            {
                add = true;
            }
        }

        if (item.CartActionType == CartActionType.PrcTable)
        {
            if (action.Validforsaleproducts.HasValue && action.Validforsaleproducts == 1)
            {
                add = true;
            }
        }

        return add;
    }

    private bool ItemCodeAllowed(string itemCode, string filteritems, string filteritemsblock)
    {
        if (!string.IsNullOrEmpty(filteritems))
        {
            var items = filteritems.Split(',');
            foreach (var item in items)
            {
                if (item.Contains("*"))
                { 
                    if (new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(item, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(itemCode))
                    {
                        return true;
                    }
                 
                }
                else
                {
                    if (itemCode == item)
                    {
                        return true;
                    } 
                }
            }
            return false;
        }

        if (!string.IsNullOrEmpty(filteritemsblock))
        { 
            var items = filteritemsblock.Split(',');
            foreach (var item in items)
            {
                if (item.Contains('*'))
                {
                    if (new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(item, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(itemCode))
                    {
                        return false;
                    }
            
                }
                else
                {
                    if (itemCode == item)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private decimal CalcCartTotVal(CalcJsonResultDto cartRes)
    {
        decimal summ = 0;
        foreach (var item in cartRes.Items)
        {
            if (item.RawOrigSelPrc.HasValue)
            {
                summ += item.RawOrigSelPrc.Value;
            }
        }
        return summ;
    }
}
public class MultiCalculeteOut
{
    public int Cnt { get; set; } = 0;
    public string WhyNotMessage { get; set; } = null!;
}

public class CalculeteOut
{
    public string WhyNotMessage { get; set; } = null!;
    public bool Used { get; set; } = false;
}
