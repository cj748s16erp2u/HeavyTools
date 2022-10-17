using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class MessageExceptionResultDto
{
    public string Id { get; set; } = null!;
    public string NameSpace { get; set; } = null!;
    public object[] Param { get; set; } = null!;
    public string ErrKey { get; internal set; } = null!;
}
