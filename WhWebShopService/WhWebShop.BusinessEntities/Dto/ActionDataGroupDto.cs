using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class ActionDataGroupDto : OlcAction
{
    public List<OlcActionext> OlcActionexts { get; set; } = new List<OlcActionext> { };

    public List<OlcActioncouponnumber> OlcActioncouponnumbers { get; set; } = new List<OlcActioncouponnumber> { };

    public List<OlcActionwebhop> OlcActionwebhops { get; set; } = new List<OlcActionwebhop> { };
    public List<OlcActionretail> OlcActionretails { get; set; } = new List<OlcActionretail> { };

}
