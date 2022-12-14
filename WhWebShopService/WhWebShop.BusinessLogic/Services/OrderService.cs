using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers.CSVParser;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces; 
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Sord;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOrderService))]
public class OrderService : LogicServiceBase<OlsSordhead>, IOrderService
{
    private readonly IOlsSordheadService sordHeadService;
    private readonly IOlsSordlineService sordLineService;
    private readonly IOlsRecidService recIdService;
    private readonly IItemCacheService itemCache;
    private readonly IOptions<Options.SordOptions> sordoptions;
    private readonly IOSSService oSSService;
    private readonly IOlsCountryService countryService;
    private readonly IOlcSordheadService olcSordHeadService;
    private readonly IOlcSordlineService olcSordLineService;
    private readonly IPriceCalcService priceCalcService;
    private readonly IPriceCalcCuponUsageService priceCalcCuponUsageService;
    private readonly IReserveService reserveService;
    private readonly IOlsSorddocCacheService olsSorddocCacheService;
    private readonly IOlcSpOlsGetnewsordnumService olcSpOlsGetnewsordnumService;

    public OrderService(IValidator<OlsSordhead> validator,
                        IRepository<OlsSordhead> repository,
                        IUnitOfWork unitOfWork,
                        IEnvironmentService environmentService,
                        IOlsSordheadService sordHeadService,
                        IOlsSordlineService sordLineService,
                        IOlsRecidService recIdService,
                        IItemCacheService itemCache,
                        IOptions<Options.SordOptions> sordoptions,
                        IOSSService oSSService,
                        IOlsCountryService countryService,
                        IOlcSordheadService olcSordHeadService,
                        IOlcSordlineService olcSordLineService,
                        IPriceCalcService priceCalcService,
                        IPriceCalcCuponUsageService priceCalcCuponUsageService,
                        IReserveService reserveService,
                        IOlsSorddocCacheService olsSorddocCacheService,
                        IOlcSpOlsGetnewsordnumService olcSpOlsGetnewsordnumService) : base(validator, repository, unitOfWork, environmentService)
    {
        this.sordHeadService = sordHeadService ?? throw new ArgumentNullException(nameof(sordHeadService));
        this.sordLineService = sordLineService ?? throw new ArgumentNullException(nameof(sordLineService));
        this.recIdService = recIdService ?? throw new ArgumentNullException(nameof(recIdService));
        this.itemCache = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
        this.sordoptions = sordoptions ?? throw new ArgumentNullException(nameof(sordoptions));
        this.oSSService = oSSService ?? throw new ArgumentNullException(nameof(oSSService));
        this.countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
        this.olcSordHeadService = olcSordHeadService ?? throw new ArgumentNullException(nameof(olcSordHeadService));
        this.olcSordLineService = olcSordLineService ?? throw new ArgumentNullException(nameof(olcSordLineService));
        this.priceCalcService = priceCalcService ?? throw new ArgumentNullException(nameof(priceCalcService));
        this.priceCalcCuponUsageService = priceCalcCuponUsageService ?? throw new ArgumentNullException(nameof(priceCalcCuponUsageService));
        this.reserveService = reserveService ?? throw new ArgumentNullException(nameof(reserveService));
        this.olsSorddocCacheService = olsSorddocCacheService ?? throw new ArgumentNullException(nameof(olsSorddocCacheService));
        this.olcSpOlsGetnewsordnumService = olcSpOlsGetnewsordnumService ?? throw new ArgumentNullException(nameof(olcSpOlsGetnewsordnumService));

        /*orderServiceCSV = new OrderServiceCSV(validator, repository, unitOfWork, environmentService, sordHeadService, sordLineService, recIdService, itemCache, sordoptions, oSSService, countryService, olcSordHeadService);*/
    }
      
    public async Task<OrderResultDto> CreateAsync(JObject value, OlcApilogger apilogger, CancellationToken cancellationToken = default)
    {
        var order = JsonParser.ParseObject<OrderJsonParamsDto>(value);
       
        await priceCalcCuponUsageService.UseCuponAsync(order.Cart.Cupons);

        var sh = new OlsSordhead();

        using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
        {
            var country = await GetCountry(order, cancellationToken);
            var oss = await GetOSS(country.Countryid, cancellationToken);

            await FillHeadAsync(sh, order, cancellationToken);
            await sordHeadService.AddAsync(sh, cancellationToken);

            var csh = new OlcSordhead();
            FillOlcHead(csh, sh, order, oss, apilogger);
            await olcSordHeadService.AddAsync(csh, cancellationToken);


            var sl = new List<OlsSordline>();
            var csl = new List<OlcSordline>();

            var preordersordlineid = new Dictionary<int, int>();


            await FillLinesAsync(sl, csl, order, sh.Sordid, oss, cancellationToken);

            foreach (var line in sl)
            {
                await sordLineService.AddAsync(line, cancellationToken);
                var res = await reserveService.DoFrameOrderReserve(sh, line, cancellationToken);

                if (res != null)
                { 
                    if (res.RemnantQty>0)
                    {
                        await reserveService.DoCentralWarehouseReserve(sh, line, res.RemnantQty, cancellationToken);
                    }
                }
            }
             
            foreach (var line in csl)
            {
                line.Sordlineid = line.Sordline.Sordlineid;
                line.Sordline = null!;
                if (preordersordlineid.ContainsKey(line.Sordlineid))
                {
                    line.Preordersordlineid = preordersordlineid[line.Sordlineid];
                }
                await olcSordLineService.AddAsync(line, cancellationToken);
            }

            tran.Commit();
        }

        return new OrderResultDto
        {
            Success = true,
            ErrorMessage = null,
            Sordid = sh!.Sordid
        };
    }

    private async Task FillHeadAsync(OlsSordhead sh, OrderJsonParamsDto order, CancellationToken cancellationToken)
    {
        var sordid = await recIdService.GetNewIdAsync("SordHead.SordID", cancellationToken);
        if (sordid == null)
        {
            throw new Exception("Cannot generate new id");
        }

        sh.Sordid = sordid.Lastid;
        sh.Partnid = 1;
        sh.Docnum = order.OrderNumber;

        sh.Sorddate = order.OrderDate!.Value;
        sh.Curid = order.Cart.Curid;
        sh.Paymid = order.PaymentId;
        sh.Paycid = null;
        sh.Sordstat = 10;
        sh.Note = order.Note;
        sh.Langid = "hu-HU";
        sh.Lastlinenum = order.Cart.Items.Count();

        sh.Gen = this.sordoptions.Value.Gen!.Value;
        sh.Sorddocid = this.sordoptions.Value.SordDocId!;
        sh.Cmpid = this.sordoptions.Value.Cmpid!.Value;

        var sd = await olsSorddocCacheService.GetSorddocAsync(sh.Sorddocid, cancellationToken);

        sh.Sordtype = sd.Type;
        sh.Partnid = this.sordoptions.Value.PartnId!.Value;
        sh.Addrid = this.sordoptions.Value.AddrId!.Value;

        sh.Addusrid = this.sordoptions.Value.AddUsrId!;
        sh.Adddate = DateTime.Now;


        if (sh.Docnum == default)
        {
            throw new Exception("Missing Docnum");
        }
    }

    private void FillOlcHead(OlcSordhead csh, OlsSordhead sh, OrderJsonParamsDto order, OSSResultDto oss, OlcApilogger apilogger)
    {
        var root = new XElement("sordhead");

        Utils.AddXElement(root, order, nameof(order.Sinv_name));
        Utils.AddXElement(root, order, nameof(order.Sinv_countryid));
        Utils.AddXElement(root, order, nameof(order.Sinv_postcode));
        Utils.AddXElement(root, order, nameof(order.Sinv_city));
        Utils.AddXElement(root, order, nameof(order.Sinv_building));
        Utils.AddXElement(root, order, nameof(order.Sinv_district));
        Utils.AddXElement(root, order, nameof(order.Sinv_door));
        Utils.AddXElement(root, order, nameof(order.Sinv_hnum));
        Utils.AddXElement(root, order, nameof(order.Sinv_floor));
        Utils.AddXElement(root, order, nameof(order.Sinv_place));
        Utils.AddXElement(root, order, nameof(order.Sinv_placetype));
        Utils.AddXElement(root, order, nameof(order.Sinv_stairway));

        Utils.AddXElement(root, order, nameof(order.Shipping_name));
        Utils.AddXElement(root, order, nameof(order.Shipping_countryid));
        Utils.AddXElement(root, order, nameof(order.Shipping_postcode));
        Utils.AddXElement(root, order, nameof(order.Shipping_city));
        Utils.AddXElement(root, order, nameof(order.Shipping_building));
        Utils.AddXElement(root, order, nameof(order.Shipping_district));
        Utils.AddXElement(root, order, nameof(order.Shipping_door));
        Utils.AddXElement(root, order, nameof(order.Shipping_hnum));
        Utils.AddXElement(root, order, nameof(order.Shipping_floor));
        Utils.AddXElement(root, order, nameof(order.Shipping_place));
        Utils.AddXElement(root, order, nameof(order.Shipping_placetype));
        Utils.AddXElement(root, order, nameof(order.Shipping_stairway));
        Utils.AddXElement(root, order, nameof(order.Phone));
        Utils.AddXElement(root, order, nameof(order.Email));
        Utils.AddXElement(root, order, nameof(order.ShippinPrc));
        Utils.AddXElement(root, order, nameof(order.PaymentFee));
        Utils.AddXElement(root, order, nameof(order.Paymenttransaciondata));
        Utils.AddXElement(root, order, nameof(order.Netgopartnid));
        Utils.AddXElement(root, order, nameof(order.Pppid));
        Utils.AddXElement(root, order, nameof(order.Glsid));
        Utils.AddXElement(root, order, nameof(order.Foxpostid));
        Utils.AddXElement(root, order, nameof(order.CentralRetailType));
        Utils.AddXElement(root, order, nameof(order.Exchangepackagesnumber));
        Utils.AddXElement(root, order, nameof(order.ShippingId));
        Utils.AddXElement(root, order, nameof(order.PaymentId));
        Utils.AddXElement(root, order, nameof(order.GiftCardLogId));
        
        if (order.Cart.Cupons.Length > 0)
        {
            root.Add(new XElement("Coupons", string.Join(",", order.Cart.Cupons.ToArray())));
        }

        if (apilogger != null)
        {
            root.Add(new XElement("Apiid", apilogger.Apiid));
        }
        csh.Loyaltycardno = order.Cart.LoyaltyCardNo;
        
        if (!string.IsNullOrEmpty(order.Pppid))
        {
            csh.Transfcond = 4;
            csh.Deliverylocation = order.Pppid;
        }
        else if (!string.IsNullOrEmpty(order.Glsid))
        {
            csh.Transfcond = 0;
            csh.Deliverylocation = order.Glsid;
        }
        else if (!string.IsNullOrEmpty(order.Foxpostid))
        {
            csh.Transfcond = 1;
            csh.Deliverylocation = order.Foxpostid;
        } else
        {
            csh.Transfcond = 3;
        }
        csh.Wid = order.Wid;
        csh.Bustypeid = oss.Bustypeid;

        csh.Data = root.ToString();
        csh.Sordid = sh.Sordid!;
        csh.Addusrid = sh.Addusrid;
        csh.Adddate = sh.Adddate;
    }

    

    private async Task FillLinesAsync(List<OlsSordline> sl, List<OlcSordline> csl, OrderJsonParamsDto order, int sordid, OSSResultDto oss, CancellationToken cancellationToken)
    {
        var num = 0;

        foreach (var item in order.Cart.Items)
        {
            num++;
            var nsl = new OlsSordline();

            var nid = await this.recIdService.GetNewIdAsync("SordLine.SordLineID", cancellationToken);
            if (nid == null)
            {
                throw new Exception("Cannot generate new id");
            }
            nsl.Sordlineid = nid.Lastid;
            nsl.Sordid = sordid;
            nsl.Linenum = num;
            nsl.Def = 1;

            if (!string.IsNullOrEmpty(item.ItemCode))
            {
                nsl.Itemid = (await this.itemCache.GetAsync(item.ItemCode, cancellationToken)).Value;
            } else
            {
                nsl.Itemid = item.Itemid!.Value;
            }
            
            nsl.Reqdate = DateTime.Today;
            nsl.Ref2 = null;
            
            nsl.Ordqty = Convert.ToDecimal(item!.Quantity!.Value);
            nsl.Movqty = 0;

             
            nsl.Selprctype = 2; // bruttó
            nsl.Selprc = item.SelPrc.GetValueOrDefault(0);
            nsl.Seltotprc = item.GrossPrc.GetValueOrDefault(0);


            nsl.Selprcprcid = null;
            nsl.Discpercnt = 0;
            nsl.Disctotval = 0;
            nsl.Taxid = oss.Taxid!;
            nsl.Sordlinestat = 10;
            nsl.Note = "";
            nsl.Resid = null;
            nsl.Ucdid = null;
            nsl.Pjpid = null;

            nsl.Gen = this.sordoptions.Value.Gen!.Value;
            nsl.Adddate = DateTime.Now;
            nsl.Addusrid = this.sordoptions.Value.AddUsrId!;
            sl.Add(nsl);
             
            var root = new XElement("sordline");

            Utils.AddXElement(root, item, nameof(item.OrignalSelPrc));
            Utils.AddXElement(root, item, nameof(item.OrignalTotprc));
            Utils.AddXElement(root, item, nameof(item.SelPrc));
            Utils.AddXElement(root, item, nameof(item.GrossPrc));
            Utils.AddXElement(root, item, nameof(item.NetVal));
            Utils.AddXElement(root, item, nameof(item.TaxVal));
            Utils.AddXElement(root, item, nameof(item.TotVal));

            var ncsl = new OlcSordline
            {
                Data = root.ToString(),
                Adddate = DateTime.Now,
                Addusrid = this.sordoptions.Value.AddUsrId!,
                Sordline = nsl
            };


            csl.Add(ncsl);
        }
    }


    private async Task<OlsCountry> GetCountry(OrderJsonParamsDto order, CancellationToken cancellationToken)
    {
        var c = await countryService.Query(p => p.Countryid == order.Sinv_countryid).
            FirstOrDefaultAsync(cancellationToken);

        if (c == null)
        {
            throw new ArgumentException("Misssing counryid", order.Sinv_countryid);
        }

        return c;
    }

    private async Task<OSSResultDto> GetOSS(string countryid, CancellationToken cancellationToken)
    {
        var oss = await oSSService.GetOss(new OSSParamsDto() { CoundtyId = countryid }, cancellationToken);
        if (oss == null)
        {
            throw new ArgumentException("Missing oss", countryid);
        }
        return oss;
    }

    public async Task<string> GetNewSordnum(OlsSordhead sh, CancellationToken cancellationToken = default)
    {
        var storeId = await recIdService.GetNewIdAsync("sp_ols_getnewsordnum", cancellationToken);
        if (storeId == null)
        {

            throw new Exception("Missing sp_ols_getnewsordnum storeId");
        }
        var sps = new List<SqlParameter>
        {
            new SqlParameter("storeId", storeId!.Lastid),
            new SqlParameter("sordDocId", sh.Sorddocid!),
            new SqlParameter("cmpId", sh.Cmpid),
            new SqlParameter("date", sh.Sorddate),
            new SqlParameter("store", 1)
        };

        await Repository.ExecuteStoredProcedure("sp_olc_sp_ols_getnewsordnum", sps);


        var s = await olcSpOlsGetnewsordnumService.Query(p => p.Id == storeId!.Lastid).FirstOrDefaultAsync(cancellationToken);

        if (s == null)
        {
            throw new Exception("Missing sp_olc_sp_ols_getnewsordnum record");
        }
        if (s.Result.HasValue && s.Result.Value != 0)
        {
            throw new Exception("Missing sp_olc_sp_ols_getnewsordnum record");
        } 
        return s.Docnum!;
    }
}
 