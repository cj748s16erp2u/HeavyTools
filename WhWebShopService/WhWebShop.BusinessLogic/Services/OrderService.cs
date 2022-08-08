using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services
{
    [RegisterDI(Interface = typeof(IOrderService))]
    public class OrderService : LogicServiceBase<OlsSordhead>, IOrderService
    {
        private readonly ISordHeadService sordHeadService;
        private readonly ISordLineService sordLineService;
        private readonly IRecIdService recIdService;
        private readonly IItemCache itemCache;
        private readonly IOptions<Options.SordOptions> sordoptions;

        public OrderService(IValidator<OlsSordhead> validator, IRepository<OlsSordhead> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService, ISordHeadService sordHeadService, ISordLineService sordLineService, IRecIdService recIdService, IItemCache itemCache, IOptions<Options.SordOptions> sordoptions) : base(validator, repository, unitOfWork, environmentService)
        {
            this.sordHeadService = sordHeadService;
            this.sordLineService = sordLineService;
            this.recIdService = recIdService; 
            this.itemCache = itemCache;
            this.sordoptions = sordoptions;
        }

        public async Task<OrderResultDto> CreateAsync(OrderParamsDto parms, CancellationToken cancellationToken = default)
        { 
            using (var tran = await this.UnitOfWork.BeginTransactionAsync(cancellationToken))
            {
                var csv = SordOrderCSV.ParseCSVText(parms.Content);
                var lines = csv.GetLines();

                var sh = new OlsSordhead();
                await FillHeadAsync(sh, csv.GetHead(), cancellationToken);

                sh.Lastlinenum = lines.Count;

                await sordHeadService.AddAsync(sh, cancellationToken);
                 
                var sl = new List<OlsSordline>();
                await FillLinesAsync(sl, lines, sh.Sordid, cancellationToken);
 
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
                Sordid = -1
            };
        } 
        private async Task FillLinesAsync(List<OlsSordline> sl, List<SordOrderLineCSV> lines, int sordid, CancellationToken cancellationToken)
        {
            var num = 0;

            foreach (var item in lines)
            {
                num++;
                var nsl=new OlsSordline();

                var nid = await this.recIdService.GetNewIdAsync("SordLine.SordLineID", cancellationToken);
                if (nid == null)
                {
                    throw new Exception("Cannot generate new id");
                }
                nsl.Sordlineid = nid.Lastid;
                nsl.Sordid = sordid;
                nsl.Linenum = num;
                nsl.Def = 1;

                var i=await this.itemCache.GetAsync(item.ItemCode, cancellationToken);

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
                nsl.Taxid = "A27";
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

        private async Task FillHeadAsync(OlsSordhead sh, SordHeadOrderCSV head, CancellationToken cancellationToken)
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
            sh.Sorddate = DateTime.Today;
            sh.Curid = head.Curid!;
            sh.Paymid = "KP";
            sh.Paycid = null;
            sh.Sordstat = 10;
            sh.Note = head.Note;
            sh.Langid = "hu-HU";
            sh.Lastlinenum = 0;


            sh.Gen = this.sordoptions.Value.Gen!.Value;
            sh.Sorddocid = this.sordoptions.Value.SordDocId!;
            sh.Cmpid = this.sordoptions.Value.Cmpid!.Value;
            sh.Sordtype = this.sordoptions.Value.SordType!.Value;
            sh.Partnid = this.sordoptions.Value.PartnId!.Value;
            sh.Addrid = this.sordoptions.Value.AddrId!.Value;

            sh.Addusrid = this.sordoptions.Value.AddUsrId!;
            sh.Adddate= DateTime.Now;


            if (sh.Docnum == default)
            {
                throw new Exception("Missing Docnum");
            }
        } 
    }
}
