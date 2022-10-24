using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
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

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;


[RegisterDI(Interface = typeof(IB2BService))]
internal class B2BService : LogicServiceBase<B2BPartnerTmp>, IB2BService
{
    public B2BService(IValidator<B2BPartnerTmp> validator, IRepository<B2BPartnerTmp> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
    }

    public B2BDto GetPartners()
    {
        var ps = new List<B2BPartnerDto>();
 
        FormattableString sql = $@"
--select * from ols_typehead 
--select * from ols_typeline where typegrpid=2

select ROW_NUMBER() OVER (ORDER BY p.partnid)  id, p.partncode, p.name, 

	a2.add01 selname,
    a2.countryid selcountry,
	pae2.info.value('district[1]', 'varchar(100)') as seladdrdistrict,
    pae2.info.value('place[1]', 'varchar(100)') as seladdrplace,
    pae2.info.value('placetype[1]', 'varchar(100)') as seladdrplacetype,
    pae2.info.value('hnum[1]', 'varchar(100)') as seladdrhnum,
    pae2.info.value('building[1]', 'varchar(100)') as seladdrbuilding,
    pae2.info.value('stairway[1]', 'varchar(100)') as seladdrstairway,
    pae2.info.value('floor[1]', 'varchar(100)') as seladdrfloor,
    pae2.info.value('door[1]', 'varchar(100)') as seladdrdoor,

	a3.add01 sinvname,
    a3.countryid sinvcountry,
	pae3.info.value('district[1]', 'varchar(100)') as sinvaddrdistrict,
    pae3.info.value('place[1]', 'varchar(100)') as sinvaddrplace,
    pae3.info.value('placetype[1]', 'varchar(100)') as sinvaddrplacetype,
    pae3.info.value('hnum[1]', 'varchar(100)') as sinvaddrhnum,
    pae3.info.value('building[1]', 'varchar(100)') as sinvaddrbuilding,
    pae3.info.value('stairway[1]', 'varchar(100)') as sinvaddrstairway,
    pae3.info.value('floor[1]', 'varchar(100)') as sinvaddrfloor,
    pae3.info.value('door[1]', 'varchar(100)') as sinvaddrdoor,
	 
	a.tel,
	cp.wsemail,
	cp.loyaltycardno,
	p.vatnum,
	p.vatnumeu,
	p.groupvatnum, p.adddate, p.addusrid
	  
  from ols_partner p
  left join olc_partner cp on cp.partnid=p.partnid
  left join ols_partnaddr a on a.partnid=p.partnid and (a.type & 2)<>0 and a.def=1
  left join ols_partnaddr a2 on a2.partnid=p.partnid and (a2.type & 1)<>0
   outer apply (
	select top 1 * from ols_partnaddr a3 where a3.partnid=p.partnid and (a3.type & 32)<>0
  ) a3
  outer apply a2.xmldata.nodes('addr') as pae2(info)
  outer apply a3.xmldata.nodes('addr') as pae3(info)
 where (p.type & 6) <> 0 
   and p.delstat=0 
";

        var srrts = Repository.ExecuteSql<B2BPartnerTmp>(sql);

        foreach (var item in srrts)
        { 
            var p = new B2BPartnerDto
            {
                PartnCode = item.Partncode,
                Name = item.Name,
                SelName = item.selname,
                Selcountry = item.Selcountry,
                SelAddrDistrict = item.Seladdrdistrict,
                SelAddrPlace = item.Seladdrplace,
                SelAddrPlacetype = item.Seladdrplacetype,
                SelAddrHnum = item.Seladdrhnum,
                SelAddrBuilding = item.Seladdrbuilding,
                SelAddrStairway = item.Seladdrstairway,
                SelAddrFloor = item.Seladdrfloor,
                SelAddrDoor = item.Seladdrdoor,
                SinvName = item.Sinvname,
                Sinvcountry = item.Sinvcountry,
                SinvAddrDistrict = item.Sinvaddrdistrict,
                SinvAddrPlace = item.Sinvaddrplace,
                SinvAddrPlacetype = item.Sinvaddrplacetype,
                SinvAddrHnum = item.Sinvaddrhnum,
                SinvAddrBuilding = item.Sinvaddrbuilding,
                SinvAddrStairway = item.Sinvaddrstairway,
                SinvAddrFloor = item.Sinvaddrfloor,
                SinvAddrDoor = item.Sinvaddrdoor,
                Tel = item.Tel,
                Email = item.Wsemail,
                Loyaltycardno = item.Loyaltycardno,
                Vatnum = item.Vatnum,
                VatnumEu = item.Vatnumeu,
                GroupVatnum = item.Groupvatnum
            };
            ps.Add(p);
        };


        var d = new B2BDto
        {
            Partners = ps.ToArray()
        };
        return d;
    }
}
