using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IOSSService))]
public class OSSService : LogicServiceBase<OlcTaxtransext>, IOSSService
{
    protected IOlsCountryService countryService;
    protected IOlsTaxtransService taxtransService;
    IOptions<Options.OSSOptions> ossoptions;


    public OSSService(IValidator<OlcTaxtransext> validator, IRepository<OlcTaxtransext> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService, IOlsCountryService countryService, IOlsTaxtransService taxtransService, IOptions<Options.OSSOptions> ossoptions) : base(validator, repository, unitOfWork, environmentService)
    {
        this.countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
        this.taxtransService = taxtransService ?? throw new ArgumentNullException(nameof(taxtransService));
        this.ossoptions = ossoptions ?? throw new ArgumentNullException(nameof(ossoptions));
    }

    public async Task<OSSResultDto?> GetOss(OSSParamsDto ossparam, CancellationToken cancellationtoken)
    {
        if (ossparam == null)
        {
            throw new ArgumentNullException(nameof(ossparam));
        }
        if (ossparam.CoundtyId == null)
        {
            throw new ArgumentNullException(nameof(ossparam.CoundtyId));
        }

        if (ossparam.CoundtyId == ossoptions.Value.HuCountryId)
        {
            return new OSSResultDto
            {
                Bustypeid = ossoptions.Value.HuBustypeid,
                Taxid = ossoptions.Value.HuTaxid,
                Success=true
            };
        }

        var c = await countryService.GetAsync(p => p.Countryid == ossparam.CoundtyId, cancellationtoken);
        if (c == null)
        {
            throw new ArgumentNullException(nameof(ossparam.CoundtyId), ossparam.CoundtyId);
        }

        if (c.Eutype == 1)
        {
            var e = await this.Query(p =>
                p.Countryid == ossparam.CoundtyId && DateTime.Today > p.Ttefrom && DateTime.Today < p.Tteto).
                FirstOrDefaultAsync(cancellationtoken);

            if (e == null)
            {
                throw new ArgumentOutOfRangeException(nameof(ossparam), ossparam.CoundtyId);
            }
            var t = await taxtransService.GetAsync(p => p.Ttid == e.Ttid, cancellationtoken);
            if (t == null)
            {
                throw new ArgumentOutOfRangeException(nameof(e.Ttid));
            }
            return new OSSResultDto
            {
                Bustypeid = t.Bustypeid,
                Taxid = e.Taxid,
                Success = true
            };
        } else {
            return new OSSResultDto
            {
                Bustypeid = ossoptions.Value.NoneEUBustypeid,
                Taxid = ossoptions.Value.HoneEUTaxid,
                Success = true
            };
        }
    }
}
