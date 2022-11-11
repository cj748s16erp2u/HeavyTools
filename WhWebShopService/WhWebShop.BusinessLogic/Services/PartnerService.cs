using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;
 
[RegisterDI(Interface = typeof(IPartnerService))]
public class PartnerService : IPartnerService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IOptions<PartnerOptions> partnerOption;
    private readonly IOlsPartnerService olsPartnerService;
    private readonly IOlcPartnerService lcPartnerService;
    private readonly IOlsPartnaddrService olspartnaddrService;
    private readonly IOlcPartnaddrService olcpartnaddrService;
    private readonly IOlsPartnaddrcmpService olsPartnaddrcmpService;
    private readonly IPartnVatTypCacheService partnVatTypCache;
    private readonly IOlsPartncmpService olsPartncmpService;
    private readonly ICountryCacheService countryCacheService;
    private readonly IOlsSysvalService olsSysvalService;
    private readonly IOlsRecidService olsRecidService;

    public PartnerService(IUnitOfWork unitOfWork,
                          IOptions<Options.PartnerOptions> partnerOption,
                          IOlsPartnerService olsPartnerService,
                          IOlcPartnerService lcPartnerService,
                          IOlsPartnaddrService olspartnaddrService,
                          IOlcPartnaddrService olcpartnaddrService,
                          IOlsPartnaddrcmpService olsPartnaddrcmpService,
                          IPartnVatTypCacheService partnVatTypCache,
                          IOlsPartncmpService olsPartncmpService,
                          ICountryCacheService countryCacheService,
                          IOlsSysvalService olsSysvalService,
                          IOlsRecidService olsRecidService)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.partnerOption = partnerOption ?? throw new ArgumentNullException(nameof(partnerOption));
        this.olsPartnerService = olsPartnerService ?? throw new ArgumentNullException(nameof(olsPartnerService));
        this.lcPartnerService = lcPartnerService ?? throw new ArgumentNullException(nameof(lcPartnerService));
        this.olspartnaddrService = olspartnaddrService ?? throw new ArgumentNullException(nameof(olspartnaddrService));
        this.olcpartnaddrService = olcpartnaddrService ?? throw new ArgumentNullException(nameof(olcpartnaddrService));
        this.olsPartnaddrcmpService = olsPartnaddrcmpService ?? throw new ArgumentNullException(nameof(olsPartnaddrcmpService));
        this.partnVatTypCache = partnVatTypCache ?? throw new ArgumentNullException(nameof(partnVatTypCache));
        this.olsPartncmpService = olsPartncmpService ?? throw new ArgumentNullException(nameof(olsPartncmpService));
        this.countryCacheService = countryCacheService ?? throw new ArgumentNullException(nameof(countryCacheService));
        this.olsSysvalService = olsSysvalService ?? throw new ArgumentNullException(nameof(olsSysvalService));
        this.olsRecidService = olsRecidService ?? throw new ArgumentNullException(nameof(olsRecidService));
    }

    public async Task<PartnerResultDto> CreatePartnersAsync(JObject value, OlcApilogger? apilogger, CancellationToken cancellationToken = default)
    {
        var partner = JsonParser.ParseObject<PartnerParamsDto>(value);


        var prDto = new PartnerResultDto();


        using (var tran = await unitOfWork.BeginTransactionAsync(cancellationToken))
        {
            var p = new OlsPartner();
            var pc = new OlcPartner();
            var pcc = new List<OlsPartncmp>();

            var shipA = new OlsPartnaddr();
            var shipAc = new OlcPartnaddr();
            var shipAcc = new List<OlsPartnaddrcmp>();

            var sinvA = new OlsPartnaddr();
            var sinvAc = new OlcPartnaddr();
            var sinvAcc = new List<OlsPartnaddrcmp>();
            
            p = await CreatePartner(partner, p, cancellationToken);
            pc = await CreateOlcPartner(partner, p, pc, cancellationToken);
            pcc = await CreatePartnerCmp(p, cancellationToken);

            var c = await countryCacheService.GetCountryAsync(partner.Sinv_countryid, cancellationToken);
            if (c == null)
            {
                throw new Exception($"Invalid Sinv_countryid: {partner.Sinv_countryid}");
            }

            sinvA = await CreateSinvPartnerAddress(partner, sinvA, p.Partnid, partnerOption, cancellationToken);
            shipA = await CreateShippingPartnerAddress(partner, shipA, p.Partnid, partnerOption, cancellationToken);

            shipAc = await CreateOlcPartnerAddress(shipA, shipAc, cancellationToken);
            sinvAc = await CreateOlcPartnerAddress(sinvA, sinvAc, cancellationToken);

            shipAcc = await CreatePartnerAddressCmp(shipA, partnerOption, cancellationToken);
            sinvAcc = await CreatePartnerAddressCmp(sinvA, partnerOption, cancellationToken);

            prDto.Partnid = p.Partnid;
            prDto.SinvAddrid = sinvA.Addrid;
            prDto.ShippingAddrid = shipA.Addrid;


            tran.Commit();
        }

        return prDto;
    }

    private async Task<List<OlsPartnaddrcmp>> CreatePartnerAddressCmp(OlsPartnaddr a, IOptions<PartnerOptions> partnerOption, CancellationToken cancellationToken)
    {
        var acc = new List<OlsPartnaddrcmp>();

        var acc1 = new OlsPartnaddrcmp();
        var acc2 = new OlsPartnaddrcmp();
        var acc3 = new OlsPartnaddrcmp();
        var acc4 = new OlsPartnaddrcmp();
        FillOlsPartnaddrcmp(acc1, 1, a.Addrid, partnerOption);
        FillOlsPartnaddrcmp(acc2, 2, a.Addrid, partnerOption);
        FillOlsPartnaddrcmp(acc3, 3, a.Addrid, partnerOption);
        FillOlsPartnaddrcmp(acc4, 4, a.Addrid, partnerOption);

        acc.Add(acc1);
        acc.Add(acc2);
        acc.Add(acc3);
        acc.Add(acc4);
        await olsPartnaddrcmpService.AddAsync(acc1, cancellationToken);
        await olsPartnaddrcmpService.AddAsync(acc2, cancellationToken);
        await olsPartnaddrcmpService.AddAsync(acc3, cancellationToken);
        await olsPartnaddrcmpService.AddAsync(acc4, cancellationToken);
        return acc;
    }

    private void FillOlsPartnaddrcmp(OlsPartnaddrcmp acc, int cmpid, int addrid, IOptions<PartnerOptions> partnerOption)
    {
        acc.Cmpid = cmpid;
        acc.Addrid = addrid;
        acc.Addusrid = partnerOption.Value.AddUsrId!;
        acc.Delstat = 0;
        acc.Adddate = DateTime.Now;
    }

    private async Task<OlcPartnaddr> CreateOlcPartnerAddress(OlsPartnaddr sinvA, OlcPartnaddr sinvAc, CancellationToken cancellationToken)
    {
        sinvAc.Addusrid = partnerOption.Value.AddUsrId!;
        sinvAc.Addrid = sinvA.Addrid;
        sinvA.Delstat = 0;
        return await olcpartnaddrService.AddAsync(sinvAc, cancellationToken);
    }


    private async Task<OlsPartnaddr> CreateShippingPartnerAddress(PartnerParamsDto partner, OlsPartnaddr shipA, int partnid, IOptions<PartnerOptions> partnerOption, CancellationToken cancellationToken)
    {
        var newid = await olsRecidService.GetNewIdAsync("PartnAddr.AddrID", cancellationToken);
        if (newid == null)
        {
            throw new Exception("Cannot create addrid");
        }

        shipA.Addrid = newid.Lastid;
        shipA.Partnid = partnid;
        shipA.Type = partnerOption.Value.ShipAddressType!.Value;
        shipA.Name = partner.Shipping_name;
        shipA.Countryid = partner.Shipping_countryid;
        shipA.Postcode = partner.Shipping_postcode;
        shipA.Add01 = partner.Shipping_city; 
        shipA.Tel = partner.Phone;
        shipA.Backordertype = partnerOption.Value.BackOrderType!.Value;


        var root = new XElement("addr");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_building), "building");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_district), "district");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_door), "door");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_hnum), "hnum");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_floor), "floor");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_place), "place");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_placetype), "placetype");
        Utils.AddXElement(root, partner, nameof(partner.Shipping_stairway), "stairway");
        shipA.Xmldata = root.ToString();
        shipA.Add02 = GenerateAddr02(root);
        shipA.Adddate = DateTime.Now;
        shipA.Addusrid = partnerOption.Value.AddUsrId!;

        return await olspartnaddrService.AddAsync(shipA, cancellationToken);
    }


    private async Task<OlsPartnaddr> CreateSinvPartnerAddress(PartnerParamsDto partner, OlsPartnaddr sinvA, int partnid, IOptions<PartnerOptions> partnerOption, CancellationToken cancellationToken)
    {
        var newid = await olsRecidService.GetNewIdAsync("PartnAddr.AddrID", cancellationToken);
        if (newid == null)
        {
            throw new Exception("Cannot create addrid");
        }

        sinvA.Def = 1;
        sinvA.Addrid = newid.Lastid;
        sinvA.Partnid = partnid;
        sinvA.Type = partnerOption.Value.SinvAddressType!.Value;
        sinvA.Name = partner.Sinv_name;
        sinvA.Countryid = partner.Sinv_countryid;
        sinvA.Postcode = partner.Sinv_postcode;
        sinvA.Add01 = partner.Sinv_city;
        sinvA.Tel = partner.Phone;
        sinvA.Backordertype = partnerOption.Value.BackOrderType!.Value;

        var root = new XElement("addr");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_building), "building");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_district), "district");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_door), "door");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_hnum), "hnum");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_floor), "floor");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_place), "place");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_placetype), "placetype");
        Utils.AddXElement(root, partner, nameof(partner.Sinv_stairway), "stairway");
        sinvA.Xmldata = root.ToString();
        sinvA.Add02 = GenerateAddr02(root);
        sinvA.Adddate = DateTime.Now;
        sinvA.Addusrid = partnerOption.Value.AddUsrId!;
        return await olspartnaddrService.AddAsync(sinvA, cancellationToken);
    }

    private string GenerateAddr02(XElement root)
    {
        string district = GetXmlValue(root, "district");
        string place = GetXmlValue(root, "place");
        string placetype = GetXmlValue(root, "placetype");
        string hnum = GetXmlValue(root, "hnum");
        string building = GetXmlValue(root, "building");
        string stairway = GetXmlValue(root, "stairway");
        string floor = GetXmlValue(root, "floor");
        string door = GetXmlValue(root, "door");

        var lst = new List<string>();
        if (!string.IsNullOrEmpty(district))
            lst.Add(string.Format("{0}. ker.", district));
        lst.Add(place);
        lst.Add(placetype);
        if (!string.IsNullOrEmpty(hnum))
            lst.Add(string.Format("{0}.", hnum));
        if (!string.IsNullOrEmpty(building))
            lst.Add(string.Format("{0}. ép.", building));
        if (!string.IsNullOrEmpty(stairway))
            lst.Add(string.Format("{0}. lph.", stairway));
        if (!string.IsNullOrEmpty(floor))
        {
            if (new System.Text.RegularExpressions.Regex(@"\d").IsMatch(floor))
                lst.Add(string.Format("{0}. em.", floor));
            else
                lst.Add(string.Format("{0}", floor));
        }
        if (!string.IsNullOrEmpty(door))
            lst.Add(string.Format("{0}. a.", door));

        string add02 = "";
        foreach (string s in lst)
            if (!string.IsNullOrEmpty(s))
            {
                if (add02 != "")
                    add02 += " ";
                add02 += s;
            }
        if (string.IsNullOrEmpty(add02))
            return null!;
        return add02;

    }

    private string GetXmlValue(XElement root, string name)
    {
        foreach (var d in root.Descendants())
        {
            if (d.Name == name)
            {
                return d.Value;
            }
        }
        return null!;
    }

    private async Task<List<OlsPartncmp>> CreatePartnerCmp(OlsPartner p, CancellationToken cancellationToken)
    {
        var pcc = new List<OlsPartncmp>();

        var pcc1 = new OlsPartncmp();
        var pcc2 = new OlsPartncmp();
        var pcc3 = new OlsPartncmp();
        var pcc4 = new OlsPartncmp();
        FillOlsPartncmp(pcc1, 1, p.Partnid, partnerOption);
        FillOlsPartncmp(pcc2, 2, p.Partnid, partnerOption);
        FillOlsPartncmp(pcc3, 3, p.Partnid, partnerOption);
        FillOlsPartncmp(pcc4, 4, p.Partnid, partnerOption);

        pcc.Add(pcc1);
        pcc.Add(pcc2);
        pcc.Add(pcc3);
        pcc.Add(pcc4);
        await olsPartncmpService.AddAsync(pcc1, cancellationToken);
        await olsPartncmpService.AddAsync(pcc2, cancellationToken);
        await olsPartncmpService.AddAsync(pcc3, cancellationToken);
        await olsPartncmpService.AddAsync(pcc4, cancellationToken);
        return pcc;
    }

    private async Task<OlcPartner> CreateOlcPartner(PartnerParamsDto partner, OlsPartner p, OlcPartner? pc, CancellationToken cancellationToken)
    {
        pc.Partnid = p.Partnid;
        pc.Oldcode = partner.WebpartnerID;
        pc.Wsemail = partner.Email;
        pc.Loyaltycardno = partner.Loyaltycardno;
        pc.Addusrid = partnerOption.Value.AddUsrId!;
        pc = await lcPartnerService.AddAsync(pc, cancellationToken);
        return pc!;
    }

    private async Task<OlsPartner> CreatePartner(PartnerParamsDto partner, OlsPartner? p, CancellationToken cancellationToken)
    {
        p.Cmpid = partnerOption.Value.Cmpid!.Value;
        p.Cmpcodes = "*";
        p.Type = partnerOption.Value.PartnerType!.Value;
        p.Name = partner.Name;
        p.Sname = new string(partner.Name.Take(20).ToArray());
        p.Extcode = partner.WebpartnerID;

        var pvt = await partnVatTypCache.GetAsync(partner.Ptvattypid, cancellationToken);
        if (pvt == null)
        {
            throw new Exception($"Invalid Ptvattypid: {partner.Ptvattypid}");
        }

        p.Ptvattypid = partner.Ptvattypid;

        var partnVatTyp = XElement.Parse(pvt.Xmldata!);

        var bl = new VatnumCheck();
        var vns = GetByName(partnVatTyp, "vatnum");
        bl.CheckPartnerVatNum(vns, partner.Vatnum);

        vns = GetByName(partnVatTyp, "vatnumeu");
        bl.CheckPartnerVatNumEU(vns, partner.Vatnumeu);

        vns = GetByName(partnVatTyp, "groupvatnum");
        bl.CheckPartnerGroupVatNum(vns, partner.Groupvatnum, partner.Vatnum);
         
        p.Vatnum = partner.Vatnum;
        p.Vatnumeu = partner.Vatnumeu;
        p.Groupvatnum = partner.Groupvatnum;
        p.Partncode = "WEB"+partner.WebpartnerID;


        p.Addusrid = partnerOption.Value.AddUsrId!;
        p.Adddate = DateTime.Now;

        var newid = await olsRecidService.GetNewIdAsync("Partner.PartnID", cancellationToken);
        if (newid== null)
        {
            throw new Exception("Cannot create partnid");
        }
        p.Partnid = newid.Lastid;

        p = await olsPartnerService.AddAsync(p, cancellationToken);
        p!.Partncode = (await GetPartncode(p.Partnid, cancellationToken))!;
        await olsPartnerService.UpdateAsync(p, cancellationToken);
        return p!;
    }

    private XElement GetByName(XElement xElement, string name)
    { 
        var points = xElement.Descendants(name);
        return points.FirstOrDefault()!;
    }

    private async Task<string?> GetPartncode(int partnid, CancellationToken cancellationToken)
    {
        var a = await olsSysvalService.GetAsync("partner:DefPartnCode", cancellationToken);

        var p = new GetNewPartnerCodeFormatResult(a!);

        return p.GetCode(partnid);
    }

    private void FillOlsPartncmp(OlsPartncmp pcc, int cmpid, int partnid, IOptions<PartnerOptions> po)
    {
        pcc.Cmpid = cmpid;
        pcc.Partnid = partnid;
        pcc.Type = po.Value.CmpType!.Value;
        pcc.Paymid = po.Value.Paymid;
        pcc.Credlimit = po.Value.Credlimit!.Value;
        pcc.Selprcincdiscnttype = po.Value.Selprcincdiscnttype!.Value;
        pcc.Posttype = po.Value.Posttype!.Value;
        pcc.Curid = po.Value.Curid!;
        pcc.Addusrid = po.Value.AddUsrId!;
        pcc.Adddate = DateTime.Now;
        pcc.Delstat = 0;
    }

    private class GetNewPartnerCodeFormatResult
    { 
        public string Mask { get; set; }
        public int? IDDisplacement { get; set; }

        public GetNewPartnerCodeFormatResult(OlsSysval x)
        {
            Mask = x.Valuestr!;
            IDDisplacement = (x?.Valueint).GetValueOrDefault(0);
        }

        public string GetCode(int? id)
        {
            if (!id.HasValue)
                return null!;
            return string.Format(Mask, id.Value + IDDisplacement.GetValueOrDefault(0));
        }
    }

    class VatnumCheck
    {
        public virtual void CheckPartnerVatNum(System.Xml.Linq.XElement vns, string? vatnum)
        {
            if (vns != null)
            {
                var fill = vns.Element("fill");
                CheckVatNumFill(vatnum, fill, "Az 'Adószám' mező kitöltése kötelező", "Az 'Adószám' mező nem tölthető ki");

                var format = vns.Element("format");
                CheckVatNumFormat(vatnum, format, "Az 'Adószám' nem felel meg a kívánt formátumnak");

                if (vns.Element("huncheckdigit") != null)
                    CheckVatNumHUNDigit(vatnum, "Az 'Adószám' hibás, kérem ellenőrizze!");
            }
        }
        public virtual void CheckPartnerVatNumEU(System.Xml.Linq.XElement? vns, string? vatnum)
        {
            if (vns != null)
            {
                var fill = vns.Element("fill");
                CheckVatNumFill(vatnum, fill, "Az 'EU adószám' mező kitöltése kötelező", "Az 'EU adószám' mező nem tölthető ki");

                var format = vns.Element("format");
                CheckVatNumFormat(vatnum, format, "Az 'EU adószám' nem felel meg a kívánt formátumnak");
            }
        }
        public virtual void CheckPartnerGroupVatNum(System.Xml.Linq.XElement? vns, string? groupvatnum, string? vatnum)
        {
            if (vns != null)
            {
                var fill = vns.Element("fill");
                CheckVatNumFill(groupvatnum, fill, "A 'Csoport adószám' mező kitöltése kötelező", "A 'Csoport adószám' mező nem tölthető ki");

                var format = vns.Element("format");
                CheckVatNumFormat(groupvatnum, format, "A 'Csoport adószám' nem felel meg a kívánt formátumnak");

                if (vns.Element("huncheckdigit") != null)
                    CheckVatNumHUNDigit(groupvatnum, "A 'Csoport adószám' hibás, kérem ellenőrizze!");

                if (vns.Element("huncheck1") != null)
                    CheckPartnerVatNumHUN1(groupvatnum, vatnum);
            }
        }

        public void CheckVatNumFill(string? vatnum, System.Xml.Linq.XElement? fill, string msgMandatory, string? msgForbidden)
        {
            if (fill != null)
            {
                if (fill.Value == "mandatory")
                {
                    if (string.IsNullOrEmpty(vatnum))
                        throw new Exception(msgMandatory);
                }
                else if (fill.Value == "forbidden")
                {
                    if (!string.IsNullOrEmpty(vatnum))
                        throw new Exception(msgForbidden);
                }
            }
        }
        
        protected void CheckVatNumFormat(string? vatnum, System.Xml.Linq.XElement? format, string msgFormat)
        {
            if (format != null)
            {
                if (!string.IsNullOrEmpty(vatnum))
                {
                    var rx = new System.Text.RegularExpressions.Regex(format.Value);
                    if (!rx.IsMatch(vatnum))
                        throw new Exception(msgFormat);
                }
            }
        }
        protected void CheckVatNumHUNDigit(string? vatnum, string msg)
        {
            if (string.IsNullOrEmpty(vatnum))
                return;

            var rx = new System.Text.RegularExpressions.Regex("^[0-9]{8}-[1-5]-[0-9][0-9]$");
            if (rx.IsMatch(vatnum))
            {
                var id = vatnum.Substring(0, 7);
                var csref = "" + vatnum[7];

                var cs = 0;
                var x = new int[] { 9, 7, 3, 1 };
                for (int i = 0; i < id.Length; i++)
                    cs += x[i % 4] * int.Parse("" + id[i]);
                cs %= 10;
                if (cs != 0)
                    cs = 10 - cs;

                if (!string.Equals(csref, cs.ToString()))
                    throw new Exception(msg);
            }
        }


        protected void CheckPartnerVatNumHUN1(string groupvanum, string vatnum)
        {
            var rx4 = new System.Text.RegularExpressions.Regex("^[0-9]{8}-4-[0-9][0-9]$");
            var rx5 = new System.Text.RegularExpressions.Regex("^[0-9]{8}-5-[0-9][0-9]$");

            if (!string.IsNullOrEmpty(vatnum))
            {
                if (rx4.IsMatch(vatnum))
                {
                    // az adoszam mezoben csoport tag adoszam van, a csoportadoszamot kotelezo megadni
                    if (string.IsNullOrEmpty(groupvanum))
                    {
                        throw new Exception("Az adószám mezőben csoport tag adószam van, a csoportadószámot kötelező megadni");
                    }
                }

                if (rx5.IsMatch(vatnum))
                {
                    // az adoszam mezoben csoportadoszam van
                    throw new Exception("A csoport adószám nem szerepelhet az adószám mezőben (külön mezőbe írandó)");
                }
            }

            if (!string.IsNullOrEmpty(groupvanum) && rx5.IsMatch(groupvanum))
            {
                if (string.IsNullOrEmpty(vatnum) || !rx4.IsMatch(vatnum))
                {
                    // az adoszam mezoben csoport tag adoszam van, a csoportadoszamot kotelezo megadni 
                    if (string.IsNullOrEmpty(vatnum))
                    {
                        throw new Exception("Az adószám mezőben csoport tag adószam van, a csoportadószámot kötelező megadni");
                    }
                }
            }
        }
    }
}
 