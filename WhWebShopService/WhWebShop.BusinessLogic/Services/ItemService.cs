using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Base;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IItemService))]

public class ItemService : LogicServiceBase<ItemTmp>, IItemService
{
    private readonly IOlcPrctableService prctableService;
    private readonly IOlsItemService olsItemService;
    private readonly IOlcItemService olcItemService;
    private readonly WhWebShopDbContext whWebShopDbContext;
    private readonly IOlcPrctableCurrentService olcPrctableCurrentService;

    public ItemService(IValidator<ItemTmp> validator,
                       IRepository<ItemTmp> repository,
                       IUnitOfWork unitOfWork,
                       IEnvironmentService environmentService,
                       IOlcPrctableService prctableService,
                       IOlsItemService olsItemService,
                       IOlcItemService olcItemService,
                       WhWebShopDbContext whWebShopDbContext,
                       IOlcPrctableCurrentService olcPrctableCurrentService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.prctableService = prctableService ?? throw new ArgumentNullException(nameof(prctableService));
        this.olsItemService = olsItemService ?? throw new ArgumentNullException(nameof(olsItemService));
        this.olcItemService = olcItemService ?? throw new ArgumentNullException(nameof(olcItemService));
        this.whWebShopDbContext = whWebShopDbContext ?? throw new ArgumentNullException(nameof(whWebShopDbContext));
        this.olcPrctableCurrentService = olcPrctableCurrentService ?? throw new ArgumentNullException(nameof(olcPrctableCurrentService));
    } 

    public async Task<ItemDto> GetItems(CancellationToken cancellationToken = default)
    {
        var ps = new List<ItemItemDto>();
         
         FormattableString sql = $@"
select i.itemid ,itemcode productcode, img1.groupname cat1, img2.groupname cat2,
	i.name01 productname, 
 
	null descriptionhu, priceGrossHuf,priceSaleGrossHuf,retailPriceGrossHuf,retailPriceSaleGrossHuf,
    null descriptioen, priceGrossEurEn,priceSaleGrossEurEn,retailPriceGrossEurEn,retailPriceSaleGrossEurEn,
    null descriptinsk, priceGrossEurSK,priceSaleGrossEurSK,
    null descriptincz, priceGrossCzkCz,priceSaleGrossCzkCz,retailPriceGrossCzkCz,retailPriceSaleGrossCzkCz,
    null descriptinro, priceGrossRonRo,priceSaleGrossRonRo,retailPriceGrossRonRo,retailPriceSaleGrossRonRo,

	ean.extcode ean,
	ims.isid season, pc.date,
	
	i.addusrid, i.adddate
   from ols_item i
   join olc_item c on c.itemid=i.itemid
   join olc_itemmodelseason ims on c.imsid=ims.imsid
   join olc_itemmodel im on im.imid=ims.imid
   join olc_itemmaingroup img on img.imgid=im.imgid
   join olc_itemmaingrouptype1 img1 on img1.imgt1id=img.imgt1id
   join olc_itemmaingrouptype2 img2 on img2.imgt2id=img.imgt2id
   left join olc_prctable_current pc on pc.itemid=i.itemid
   outer apply (
	select e.extcode
	  from ols_itemext e 
	 where e.itemid=i.itemid
	   and e.type=2
	   and e.def=1
   ) ean
  where i.itemgrpid='ARU'
 
";

        var srrts = Repository.ExecuteSql<ItemTmp>(sql).ToList();

        foreach (var item in srrts)
        { 
            var p = new ItemItemDto
            {
                ProductCode = item.ProductCode!,
                Cat1 = item.Cat1!,
                Cat2 = item.Cat2!,
                ProductName = item.ProductName!,
                DescriptionHu = item.DescriptionHu!,
                PriceGrossHuf = item.PriceGrossHuf,
                PriceSaleGrossHuf = item.PriceSaleGrossHuf,
                RetailPriceGrossHuf = item.RetailPriceGrossHuf,
                RetailPriceSaleGrossHuf = item.RetailPriceSaleGrossHuf,
                DescriptioEn = item.DescriptioEn!,
                PriceGrossEurEn = item.PriceGrossEurEn,
                PriceSaleGrossEurEn = item.PriceSaleGrossEurEn,
                RetailPriceGrossEurEn = item.RetailPriceGrossEurEn,
                RetailPriceSaleGrossEurEn = item.RetailPriceSaleGrossEurEn,
                DescriptinSk = item.DescriptinSk!,
                PriceGrossEurSK = item.PriceGrossEurSK,
                PriceSaleGrossEurSK = item.PriceSaleGrossEurSK,
                DescriptinCz = item.DescriptinCz!,
                PriceGrossCzkCz = item.PriceGrossCzkCz,
                PriceSaleGrossCzkCz = item.PriceSaleGrossCzkCz,
                RetailPriceGrossCzkCz = item.RetailPriceGrossCzkCz,
                RetailPriceSaleGrossCzkCz = item.RetailPriceSaleGrossCzkCz,
                DescriptinRo = item.DescriptinRo!,
                PriceGrossRonRo = item.PriceGrossRonRo,
                PriceSaleGrossRonRo = item.PriceSaleGrossRonRo,
                RetailPriceGrossRonRo = item.RetailPriceGrossRonRo,
                RetailPriceSaleGrossRonRo = item.RetailPriceSaleGrossRonRo,
                 
                EAN = item.EAN!,
                Season = item.Season!,
            };
            ps.Add(p);
        };


        var d = new ItemDto
        {
            Items = ps.ToArray()
        };
        return d;
    }

    private static int  Ordering(int? addrid, int? partnid, int? itemid, string? isid, string? icid)
    {
        var res = 1;


        if (addrid.HasValue)
            res = 1000;

        if (partnid.HasValue)
            res += 100;

        if (itemid.HasValue)
            res += 50;

        if (!string.IsNullOrEmpty(isid))
            res += 20;

        if (!string.IsNullOrEmpty(icid))
            res += 10;


        return res;
    }

    public async Task<EmptyDto> RecalcItemPriceAsync(CancellationToken cancellationToken)
    {
        using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
        {
            whWebShopDbContext.RemoveRange(whWebShopDbContext.OlcPrctableCurrent);
           
            var d = DateTime.Now;

            var olcprctable = await this.whWebShopDbContext.OlcPrctable
           .Where(p => p.Startdate <= DateTime.Now
                    && p.Enddate >= DateTime.Now
                    && p.Delstat == 0
                    && (p.Prctype == (int)PrcType.Actual || p.Prctype == (int)PrcType.SalePrice))
           .Select(p => new PrcItem
           {
               Wid = p.Wid!,
               Prc = p.Prc,
               Curid = p.Curid!,
               Ptid = p.Ptid,
               Prctype = p.Prctype,
               Imid = p.Imid,
               Order= Ordering(p.Addrid, p.Partnid, p.Itemid, p.Isid, p.Icid),
           }).ToListAsync(cancellationToken);

            var olcitemmodelseason = await this.whWebShopDbContext
                .OlcItemmodelseason
                .Select(p => new ModelSeasonItem
                {
                    Imid = p.Imid,
                    Imsid = p.Imsid
                }).ToListAsync(cancellationToken);

            var olcitem = await this.whWebShopDbContext.OlcItem
                .Select(p => new OlcItemItem
                {
                    Imsid = p.Imsid,
                    Itemid = p.Itemid
                }).ToListAsync(cancellationToken);

            var newPrcs = new List<OlcPrctableCurrent>();

            foreach (var item in olcitem)
            {
                var ims = olcitemmodelseason.Where(m => m.Imsid == item.Imsid).FirstOrDefault();
                if (ims != null)
                {
                    var pc = new OlcPrctableCurrent()
                    {
                        Itemid = item.Itemid!.Value,
                        Date = d,

                        PriceGrossHuf = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.Actual, "HUF", "hu"),
                        PriceSaleGrossHuf = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.SalePrice, "HUF", "hu"),
                        RetailPriceGrossHuf = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.Actual, "HUF"),
                        RetailPriceSaleGrossHuf = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.SalePrice, "HUF"),
                        PriceGrossEurEn = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.Actual, "EUR", "com"),
                        PriceSaleGrossEurEn = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.SalePrice, "EUR", "com"),
                        RetailPriceGrossEurEn = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.Actual, "EUR"),
                        RetailPriceSaleGrossEurEn = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.SalePrice, "EUR"),
                        PriceGrossEurSk = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.Actual, "EUR", "sk"),
                        PriceSaleGrossEurSk = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.SalePrice, "EUR", "sk"),
                        PriceGrossCzkCz = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.Actual, "CZK", "cz"),
                        PriceSaleGrossCzkCz = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.SalePrice, "CZK", "cz"),
                        RetailPriceGrossCzkCz = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.Actual, "CZK"),
                        RetailPriceSaleGrossCzkCz = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.SalePrice, "CZK"),
                        PriceGrossRonRo = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.Actual, "RON", "ro"),
                        PriceSaleGrossRonRo = GetPrice(olcprctable, ims, PtidPrcType.Webshop, PrcType.SalePrice, "RON", "ro"),
                        RetailPriceGrossRonRo = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.Actual, "RON"),
                        RetailPriceSaleGrossRonRo = GetPrice(olcprctable, ims, PtidPrcType.Retail, PrcType.SalePrice, "RON"),
                    };
                    newPrcs.Add(pc); 
                } 
            }
            await whWebShopDbContext.AddRangeAsync(newPrcs, cancellationToken);
            tran.Commit();
        }
         
        return new EmptyDto();
    }

    private decimal? GetPrice(List<PrcItem> olcprctable,
                              ModelSeasonItem ims, 
                              PtidPrcType ptid,
                              PrcType prcType,
                              string curid,
                              string webshopWId = null!)
    { 
        var query = new SortedList<int, PrcItem>();

        foreach (var item in olcprctable)
        {
            if (webshopWId == null)
            {
                if (item.Curid == curid
                    && item.Ptid == (int)ptid
                    && item.Prctype == (int)prcType
                    && item.Imid == ims.Imid)
                {
                    query.Add(item.Order, item);
                }
            }
            else
            {
                if (item.Curid == curid
                    && item.Ptid == (int)ptid
                    && item.Prctype == (int)prcType
                    && item.Imid == ims.Imid
                    && item.Wid == webshopWId)
                {
                    query.Add(item.Order, item);
                }
            }
        }
         
        if (query.Count > 0)
        {
            var p = query.FirstOrDefault();
            return p.Value.Prc;
        }
        return null!;
    }


}
