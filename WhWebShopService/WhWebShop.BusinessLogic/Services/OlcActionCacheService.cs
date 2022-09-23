using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOlcActionCacheService))]
public class OlcActionCacheService : IOlcActionCacheService
{

    public static bool initalized = false;
    public static ConcurrentDictionary<int,ActionDataGroupDto> actions = new ConcurrentDictionary<int, ActionDataGroupDto>();

    private readonly IOlcActionextService olcActionextService;
    private readonly IOlcActionService olcActionService;
    private readonly IOlcActioncouponnumberService olcActioncouponnumberService;
    private readonly IOlcActionwebhopService olcActionwebhopService;
    private readonly IOlcActionretailService olcActionretailService;
    private readonly IOlcCartCacheService olcCartCacheService;

    public OlcActionCacheService(IOlcActionextService olcActionextService, 
        IOlcActionService olcActionService, 
        IOlcActioncouponnumberService olcActioncouponnumberService, 
        IOlcActionwebhopService olcActionwebhopService, 
        IOlcActionretailService olcActionretailService,
        IOlcCartCacheService olcCartCacheService)
    {
        this.olcActionextService = olcActionextService ?? throw new ArgumentNullException(nameof(olcActionextService));
        this.olcActionService = olcActionService ?? throw new ArgumentNullException(nameof(olcActionService));
        this.olcActioncouponnumberService = olcActioncouponnumberService ?? throw new ArgumentNullException(nameof(olcActioncouponnumberService));
        this.olcActionwebhopService = olcActionwebhopService ?? throw new ArgumentNullException(nameof(olcActionwebhopService));
        this.olcActionretailService = olcActionretailService ?? throw new ArgumentNullException(nameof(olcActionretailService));
        this.olcCartCacheService = olcCartCacheService ?? throw new ArgumentNullException(nameof(olcCartCacheService));
    }
     

    public async Task LoadFromDatabaseAsync(CancellationToken cancellationToken = default)
    {
        if (initalized)
        {
            return;
        }

        await AddItems(p => p.Validdatefrom <= DateTime.Today
           && p.Validdateto > DateTime.Today
           && p.Isactive == 1 && p.Delstat == 0, cancellationToken);

        initalized = true;
    } 

    private void CopyData(OlcAction olcActionFrom, ActionDataGroupDto adgdTo)
    {
        foreach (var pFrom in typeof(OlcAction).GetProperties())
        {
            var pTo = typeof(ActionDataGroupDto).GetProperty(pFrom.Name);
            if (pTo != null)
            {
                pTo.SetValue(adgdTo, pFrom.GetValue(olcActionFrom));
            }
        }
    }
     

    public async Task<ActionDataGroupDto> GetCuponByIdAsync(string cupon, CancellationToken cancellationToken)
    {
        await LoadFromDatabaseAsync(cancellationToken);
        foreach (var a in actions)
        {
            if (a.Value.Singlecouponnumber== cupon)
            {
                return a.Value;
            }
        }
        return null!;
    }

    public async Task UpdateAsync(ActionDataGroupDto ca, CancellationToken cancellationToken)
    {
        await LoadFromDatabaseAsync(cancellationToken);
        await olcActionService.UpdateAsync(ca, cancellationToken);
         
        ActionDataGroupDto o;
        actions.TryRemove(ca.Aid, out o!);

        await AddItems(p => p.Validdatefrom <= DateTime.Today
              && p.Validdateto > DateTime.Today
              && p.Isactive == 1 && p.Delstat == 0 && p.Aid == ca.Aid); 
    } 
     

    async Task AddItems(Expression<Func<OlcAction, bool>> predicate = null!, CancellationToken cancellationToken = default)
    {
        var aids = await this.olcActionService.QueryAsync(predicate, cancellationToken);

        var ids = (from a in aids
                   select a.Aid).ToList();
        /*
        foreach (var aid in ids)
        {
            olcCartCacheService.RemoveCartByAction(aid);
        }
        Mindet törölni kell mert lehet olyan, hogy most nem érinti egy akció a kosarat, de kéne
        */ 
        olcCartCacheService.Reset();

        var es =
           await olcActionextService.QueryAsync(p => ids.Contains(p.Aid) && p.Delstat == 0, cancellationToken);

        var ns =
            await olcActioncouponnumberService.QueryAsync(p => ids.Contains(p.Aid) && p.Delstat == 0, cancellationToken);
        var ws =
            await olcActionwebhopService.QueryAsync(p => ids.Contains(p.Aid) && p.Delstat == 0, cancellationToken);
        var rs =
            await olcActionretailService.QueryAsync(p => ids.Contains(p.Aid) && p.Delstat == 0, cancellationToken);

        foreach (var aid in aids)
        {
            var adgd = new ActionDataGroupDto();
            CopyData(aid, adgd);
            actions.TryAdd(adgd.Aid, adgd);
            FillAction(adgd, es, ns, ws, rs);
        }
    }


    private void FillAction(ActionDataGroupDto adgd, 
        IEnumerable<OlcActionext> es, 
        IEnumerable<OlcActioncouponnumber> ns, 
        IEnumerable<OlcActionwebhop> ws, 
        IEnumerable<OlcActionretail> rs)
    {
        adgd.OlcActionexts.AddRange(es.Where(p => p.Aid == adgd.Aid));
        adgd.OlcActioncouponnumbers.AddRange(ns.Where(p => p.Aid == adgd.Aid));
        adgd.OlcActionwebhops.AddRange(ws.Where(p => p.Aid == adgd.Aid));
        adgd.OlcActionretails.AddRange(rs.Where(p => p.Aid == adgd.Aid));
    }

    public async Task UpdateAsync(OlcActioncouponnumber an, CancellationToken cancellationToken)
    {
        await LoadFromDatabaseAsync(cancellationToken);

        await olcActioncouponnumberService.UpdateAsync(an, cancellationToken);

        ActionDataGroupDto o;
        actions.TryRemove(an.Aid, out o!);

        await AddItems(p => p.Validdatefrom <= DateTime.Today
              && p.Validdateto > DateTime.Today
              && p.Isactive == 1 && p.Delstat == 0 && p.Aid == an.Aid);

    }

    public async Task<ActionDataGroupDto[]> GetActiveActionsAsync(string[] cupons, CancellationToken cancellationToken)
    {
        await LoadFromDatabaseAsync(cancellationToken);

        var l = new List<ActionDataGroupDto>();

        foreach (var a in actions)
        {
            if (a.Value.ActionTypeEnum == BusinessEntities.Enums.ActionType.Cupon ||
                   a.Value.ActionTypeEnum == BusinessEntities.Enums.ActionType.VIP)
            {
                foreach (var c in cupons)
                {
                    if (a.Value.Singlecouponnumber == c)
                    {
                        l.Add(a.Value);
                        break;
                    }
                }
            } else
            {
                l.Add(a.Value);
            }
        }

        return l.ToArray();
    }

    public async Task RefreashCacheAsync(int aid, CancellationToken cancellationToken)
    {
        await LoadFromDatabaseAsync(cancellationToken);
        
        ActionDataGroupDto o;
        actions.TryRemove(aid, out o!);

        await AddItems(p => p.Validdatefrom <= DateTime.Today
              && p.Validdateto > DateTime.Today
              && p.Isactive == 1 && p.Delstat == 0 && p.Aid == aid);
    }

    public async Task<OlcActioncouponnumber> GetCoupoNumberByIdAsync(string couponnumber, CancellationToken cancellationToken)
    {
        await LoadFromDatabaseAsync(cancellationToken);

        foreach (var a in actions)
        {
            if (a.Value.ActionTypeEnum == BusinessEntities.Enums.ActionType.Cupon)
            {
                foreach (var n in a.Value.OlcActioncouponnumbers)
                {
                    if (n.Couponnumber == couponnumber)
                    {
                        return n;
                    }
                }
            }
        }
        return null!;
    }

    public async Task<bool> Reset(int? aid, CancellationToken cancellationToken = default)
    {
        if (aid.HasValue)
        {
            await RefreashCacheAsync(aid.Value!, cancellationToken);
        } else
        {
            initalized= false;
            actions = new ConcurrentDictionary<int, ActionDataGroupDto>();
            await LoadFromDatabaseAsync(cancellationToken);
        }
        return true;
    }
}