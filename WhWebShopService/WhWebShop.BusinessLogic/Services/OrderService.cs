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
    private readonly IItemCache itemCache;
    private readonly IOptions<Options.SordOptions> sordoptions;
    private readonly IOSSService oSSService;
    private readonly IOlsCountryService countryService;
    private readonly IOlcSordheadService olcSordHeadService;
    private readonly IOlcSordlineService olcSordLineService;


    //OrderServiceCSV orderServiceCSV;

    public OrderService(IValidator<OlsSordhead> validator, IRepository<OlsSordhead> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService, IOlsSordheadService sordHeadService, IOlsSordlineService sordLineService, IOlsRecidService recIdService, IItemCache itemCache, IOptions<Options.SordOptions> sordoptions, IOSSService oSSService, IOlsCountryService countryService, IOlcSordheadService olcSordHeadService, IOlcSordlineService olcSordLineService) : base(validator, repository, unitOfWork, environmentService)
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

        /*orderServiceCSV = new OrderServiceCSV(validator, repository, unitOfWork, environmentService, sordHeadService, sordLineService, recIdService, itemCache, sordoptions, oSSService, countryService, olcSordHeadService);*/
    }

    public async Task<OrderResultDto> CreateOldAsync(OrderParamsDto parms, OlcApilogger apilogger, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //return await orderServiceCSV.CreateOldAsync2Async(parms, apilogger, cancellationToken);
    }
  
      
    public async Task<OrderResultDto> CreateAsync(JObject value, OlcApilogger apilogger, CancellationToken cancellationToken = default)
    {
        var order = JsonParser.ParseObject<OrderJsonParamsDto>(value); 
        var cart = PriceCalcBL.DoCalc(order!.Cart);

        var sh = new OlsSordhead();

        using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
        {
            var country = await GetCountry(order, cancellationToken);
            var oss = await GetOSS(country.Countryid, cancellationToken);

            await FillHeadAsync(sh, order, cart, cancellationToken);
            await sordHeadService.AddAsync(sh, cancellationToken);

            var csh = new OlcSordhead();
            FillOlcHead(csh, sh, cart ,order, oss, apilogger);
            await olcSordHeadService.AddAsync(csh, cancellationToken);


            var sl = new List<OlsSordline>();
            var csl = new List<OlcSordline>();

            await FillLinesAsync(sl, csl, cart, sh.Sordid, oss, cancellationToken);

            foreach (var line in sl)
            {
                await sordLineService.AddAsync(line, cancellationToken);
            }

            foreach (var line in csl)
            {
                line.Sordlineid = line.Sordline.Sordlineid;
                line.Sordline = null!;
                await olcSordLineService.AddAsync(line, cancellationToken);
            }

            tran.Commit();
        }

        return new OrderResultDto
        {
            Success = true,
            ErrorMessage = null,
            Sordid = sh!.Sordid,
            Cart= cart
        };
    }

    private async Task FillHeadAsync(OlsSordhead sh, OrderJsonParamsDto order, CalcJsonResultDto cart, CancellationToken cancellationToken)
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
        sh.Curid = cart.Curid;
        sh.Paymid = "KP";
        sh.Paycid = null;
        sh.Sordstat = 10;
        sh.Note = order.Note;
        sh.Langid = "hu-HU";
        sh.Lastlinenum = cart.Items.Count();

        sh.Gen = this.sordoptions.Value.Gen!.Value;
        sh.Sorddocid = this.sordoptions.Value.SordDocId!;
        sh.Cmpid = this.sordoptions.Value.Cmpid!.Value;
        sh.Sordtype = this.sordoptions.Value.SordType!.Value;
        sh.Partnid = this.sordoptions.Value.PartnId!.Value;
        sh.Addrid = this.sordoptions.Value.AddrId!.Value;

        sh.Addusrid = this.sordoptions.Value.AddUsrId!;
        sh.Adddate = DateTime.Now;


        if (sh.Docnum == default)
        {
            throw new Exception("Missing Docnum");
        }
    }

    private void FillOlcHead(OlcSordhead csh, OlsSordhead sh, CalcJsonResultDto cart, OrderJsonParamsDto order, OSSResultDto oss, OlcApilogger apilogger)
    {
        var root = new XElement("sordhead");

        AddXElement(root, order, nameof(order.Sinv_name));
        AddXElement(root, order, nameof(order.Sinv_countryid));
        AddXElement(root, order, nameof(order.Sinv_postcode));
        AddXElement(root, order, nameof(order.Sinv_city));
        AddXElement(root, order, nameof(order.Sinv_building));
        AddXElement(root, order, nameof(order.Sinv_district));
        AddXElement(root, order, nameof(order.Sinv_door));
        AddXElement(root, order, nameof(order.Sinv_hnum));
        AddXElement(root, order, nameof(order.Sinv_floor));
        AddXElement(root, order, nameof(order.Sinv_place));
        AddXElement(root, order, nameof(order.Sinv_placetype));
        AddXElement(root, order, nameof(order.Sinv_stairway));

        AddXElement(root, order, nameof(order.Shipping_name));
        AddXElement(root, order, nameof(order.Shipping_countryid));
        AddXElement(root, order, nameof(order.Shipping_postcode));
        AddXElement(root, order, nameof(order.Shipping_city));
        AddXElement(root, order, nameof(order.Shipping_building));
        AddXElement(root, order, nameof(order.Shipping_district));
        AddXElement(root, order, nameof(order.Shipping_door));
        AddXElement(root, order, nameof(order.Shipping_hnum));
        AddXElement(root, order, nameof(order.Shipping_floor));
        AddXElement(root, order, nameof(order.Shipping_place));
        AddXElement(root, order, nameof(order.Shipping_placetype));
        AddXElement(root, order, nameof(order.Shipping_stairway));
        AddXElement(root, order, nameof(order.Phone));
        AddXElement(root, order, nameof(order.Email));
        AddXElement(root, order, nameof(order.ShippinPrc));
        AddXElement(root, order, nameof(order.Paymenttransaciondata));
        AddXElement(root, order, nameof(order.Netgopartnid));
        AddXElement(root, order, nameof(order.Pppid));
        AddXElement(root, order, nameof(order.Glsid));
        AddXElement(root, order, nameof(order.Foxpostid));
        AddXElement(root, order, nameof(order.CentralRetailType));
        AddXElement(root, order, nameof(order.Exchangepackagesnumber));
        AddXElement(root, order, nameof(order.ShippingId));
        AddXElement(root, order, nameof(order.PaymentId));
        AddXElement(root, order, nameof(order.GiftCardLogId));
        
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

    private void AddXElement(XElement root, object o, string fieldname)
    {
        if (o == null)
        {
            return;
        }
        var field = o.GetType().GetProperty(fieldname);
        var value = field!.GetValue(o);

        if (value != null)
        {
            var x = new XElement(fieldname)
            {
                Value = value.ToString()
            };
            root.Add(x);
        }
    }

    private async Task FillLinesAsync(List<OlsSordline> sl, List<OlcSordline> csl, CalcJsonResultDto cart, int sordid, OSSResultDto oss, CancellationToken cancellationToken)
    {
        var num = 0;

        foreach (var item in cart.Items)
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

            var i = await this.itemCache.GetAsync(item.ItemCode, cancellationToken);

            nsl.Itemid = i.Value;
            nsl.Reqdate = DateTime.Today;
            nsl.Ref2 = null;
            
            nsl.Ordqty = Convert.ToDecimal(item!.Quantity!.Value);
            nsl.Movqty = 0;
            nsl.Selprctype = 2; // bruttó
            nsl.Selprc = item.SelPrc!.Value;
            nsl.Seltotprc = item.SetTotPrc!.Value;

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

            AddXElement(root, item, nameof(item.OrigSelVal));
            AddXElement(root, item, nameof(item.SelPrc));
            AddXElement(root, item, nameof(item.SetTotPrc));
            AddXElement(root, item, nameof(item.SelVal));
            AddXElement(root, item, nameof(item.SelTotVal));

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
}
 