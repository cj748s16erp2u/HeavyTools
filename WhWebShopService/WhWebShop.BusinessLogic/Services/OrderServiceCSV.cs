using System.Globalization;
using System.Xml.Linq;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Sord;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

internal class OrderServiceCSV : OrderService
{
    private static string CUSTOMERSINVPREFIX = "customersinv_";
    private static string CUSTOMERSHIPPINGPREFIX = "customershipping_";
     
    public OrderServiceCSV(IValidator<OlsSordhead> validator, IRepository<OlsSordhead> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService, ISordHeadService sordHeadService, ISordLineService sordLineService, IRecIdService recIdService, IItemCache itemCache, IOptions<SordOptions> sordoptions, IOSSService oSSService, ICountryService countryService, IOlcSordHeadService olcSordHeadService) : base(validator, repository, unitOfWork, environmentService, sordHeadService, sordLineService, recIdService, itemCache, sordoptions, oSSService, countryService, olcSordHeadService)
    {
        this.sordHeadService = sordHeadService ?? throw new ArgumentNullException(nameof(sordHeadService));
        this.sordLineService = sordLineService ?? throw new ArgumentNullException(nameof(sordLineService));
        this.recIdService = recIdService ?? throw new ArgumentNullException(nameof(recIdService));
        this.itemCache = itemCache ?? throw new ArgumentNullException(nameof(itemCache));
        this.sordoptions = sordoptions ?? throw new ArgumentNullException(nameof(sordoptions));
        this.oSSService = oSSService ?? throw new ArgumentNullException(nameof(oSSService));
        this.countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
        this.olcSordHeadService = olcSordHeadService ?? throw new ArgumentNullException(nameof(olcSordHeadService));
    }

    private readonly ISordHeadService sordHeadService;
    private readonly ISordLineService sordLineService;
    private readonly IRecIdService recIdService;
    private readonly IItemCache itemCache;
    private readonly IOptions<Options.SordOptions> sordoptions;
    private readonly IOSSService oSSService;
    private readonly ICountryService countryService;
    private readonly IOlcSordHeadService olcSordHeadService;


    public async Task<OrderResultDto> CreateOldAsync2Async(OrderParamsDto parms, OlcApilogger apilogger, CancellationToken cancellationToken)
    {
        OlsSordhead? sh;
        using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
        {
            var csv = SordOrderCSV.ParseCSVText(parms.Content);
            var head = csv.GetHead();
            var lines = csv.GetLines();

            var c = await countryService.Query(p => p.Name == head.CountryName).
                FirstOrDefaultAsync(cancellationToken);

            if (c == null)
            {
                throw new ArgumentException("Misssing counry", head.CountryName);
            }

            var oss = await oSSService.GetOss(new OSSParamsDto() { CoundtyId = c.Countryid }, cancellationToken);
            if (oss == null)
            {
                throw new ArgumentException("Missing oss", c.Countryid);
            }

            sh = new OlsSordhead();

            await FillHeadAsync(sh, head, lines.Count, cancellationToken);
            await sordHeadService.AddAsync(sh, cancellationToken);

            var csh = new OlcSordhead();
            FillOlcHead(csh, sh, head, apilogger);
            await olcSordHeadService.AddAsync(csh, cancellationToken);


            var sl = new List<OlsSordline>();
            await FillLinesAsync(sl, lines, sh.Sordid, oss, cancellationToken);

            foreach (var line in sl)
            {
                await sordLineService.AddAsync(line, cancellationToken);
            }

            tran.Commit();
        }

        return new OrderResultDto
        {
            Success = true,
            ErrorMessage = null,
            Sordid = sh.Sordid!
        };
    }


    private async Task FillHeadAsync(OlsSordhead sh, SordHeadOrderCSV head, int count, CancellationToken cancellationToken)
    {
        if (head.Curid is null)
        {
            throw new Exception("MissingCurid");
        }
        var sordid = await recIdService.GetNewIdAsync("SordHead.SordID", cancellationToken);
        if (sordid == null)
        {
            throw new Exception("Cannot generate new id");
        }

        sh.Sordid = sordid.Lastid;
        sh.Partnid = 1;
        sh.Docnum = head.Ordernum!;
        sh.Sorddate = ConvertToDate(head.OrderDate, nameof(head.OrderDate));
        sh.Curid = head.Curid!;
        sh.Paymid = "KP";
        sh.Paycid = null;
        sh.Sordstat = 10;
        sh.Note = head.Note;
        sh.Langid = "hu-HU";
        sh.Lastlinenum = count;

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

    private DateTime ConvertToDate(string? str, string name)
    {
        if (str == null)
        {
            throw new ArgumentNullException(name, str);
        }
        try
        {
            return DateTime.Parse(str, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            throw new ArgumentException(name, str);
        }
    }


    private void FillOlcHead(OlcSordhead csh, OlsSordhead sordhead, SordHeadOrderCSV head, OlcApilogger apilogger)
    {
        var root = new XElement("sordhead");
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}name", head.SinvName));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}country", head.CountryName));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}postcode", head.SinvPostcode));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}city", head.SinvCity));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}address", head.SinvAddr));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}building", head.SinvBuilding));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}district", head.SinvDistrict));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}door", head.SinvDoor));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}hnum", head.SinvHNum));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}level", head.SinvLevel));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}place", head.SinvPlace));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}placetype", head.SinvPlaceType));
        root.Add(new XElement($"{CUSTOMERSINVPREFIX}stairs", head.SinvStairs));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}name", head.ContactPerson));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}postcode", head.ShippingPostcode));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}city", head.ShippingCity));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}address", head.ShippingAddr));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}phone", head.Phone));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}email", head.Email));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}loyaltycardno", head.LoyaltyCardno));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}rate", head.Rate));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}shippinprc", head.ShippinPrc));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}paymentandshippingmethod", head.PaymentAndShippingMethod));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}paymenttransaciondata", head.PaymentTransacionData));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}netgopartnid", head.NetGoPartnId));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}destinationcountrycode", head.DestinationCountryCode));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}pppid", head.PPPid));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}glsid", head.GLSId));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}foxpostid", head.FoxPostId));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}taxnum", head.Taxnum));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}htshipid", head.HTShipId));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}centralRetailType", head.CentralRetailType));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}exchangepackagesnumber", head.ExchangePackagesNumber));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}shippingmagiccode", head.ShippingMagicCode));
        root.Add(new XElement($"{CUSTOMERSHIPPINGPREFIX}paymentmagiccode", head.PaymentMagicCode));

        if (apilogger != null)
        {
            root.Add(new XElement("Apiid", apilogger.Apiid));
        }

        csh.Data = root.ToString();
        csh.Sordid = sordhead.Sordid!;
        csh.Addusrid = sordhead.Addusrid;
        csh.Adddate = sordhead.Adddate;
    }

    private async Task FillLinesAsync(List<OlsSordline> sl, List<SordOrderLineCSV> lines, int sordid, OSSResultDto oss, CancellationToken cancellationToken)
    {
        var num = 0;

        foreach (var item in lines)
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
            nsl.Ordqty = ConvertToInteger(item.OrdQty, nameof(nsl.Ordqty));
            nsl.Movqty = 0;
            nsl.Selprctype = 2; // bruttó
            nsl.Selprc = ConvertToDecimal(item.NetPrc, nameof(nsl.Selprc));
            nsl.Seltotprc = ConvertToDecimal(item.TaxVal, nameof(nsl.Selprc));

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
        }
    }

    private decimal ConvertToDecimal(string? str, string name)
    {
        if (str == null)
        {
            return 0;
        }
        try
        {
            return decimal.Parse(str, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            throw new ArgumentException(name, str);
        }
    }

    private static int ConvertToInteger(string? str, string name)
    {
        if (str == null)
        {
            return 0;
        }
        try
        {
            return int.Parse(str);

        }
        catch (Exception)
        {
            throw new ArgumentException(name, str);
        }
    }
}
