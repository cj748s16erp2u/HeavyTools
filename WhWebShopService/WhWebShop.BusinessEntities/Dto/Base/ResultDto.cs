using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Base
{
    public class ResultDto
    {
        public bool? Success { get; set; } = null!;
        public string? ErrorMessage { get; set; } = null;
    }
}
