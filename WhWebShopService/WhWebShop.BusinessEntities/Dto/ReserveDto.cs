using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class ReserveDto : ErrorMessageDto
{
    public int? Resid { get; set; } = null!;
    public ReserveDto(ReserveParamsDto param)
    {
        this.Resid = param.Resid;
    }

    public ReserveDto()
    {
    }

    public ReserveDto(string errormessage) : base(errormessage)
    {
    }

    public ReserveDto(MessageExceptionResultDto messageExceptionResultDto) : base(messageExceptionResultDto)
    {
    }
}
