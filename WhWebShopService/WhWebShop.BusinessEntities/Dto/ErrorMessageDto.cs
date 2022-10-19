using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class ErrorMessageDto
{
    public ErrorMessageDto()
    {

    }
    public MessageExceptionResultDto ErrorMessage { get; set; } = null!;

    public ErrorMessageDto(string errormessage)
    {
        ErrorMessage = new MessageExceptionResultDto() { Id = errormessage };
    }
 
    public ErrorMessageDto(MessageExceptionResultDto messageExceptionResultDto)
    {
        this.ErrorMessage = messageExceptionResultDto;
    }
}
