using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class SordLineDto : ErrorMessageDto
{
    public SordLineDto()
    {
    }

    public SordLineDto(string errormessage) : base(errormessage)
    {
    }

    public SordLineDto(MessageExceptionResultDto messageExceptionResultDto) : base(messageExceptionResultDto)
    {
    }
     
    public int? Sordlineid { get; set; } = null!;
}
