using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class PriceCalcActionResultDto
{
    public List<PriceCalcActionResultItemDto> ActionResults { get; set; } = new List<PriceCalcActionResultItemDto>();
}

public class PriceCalcActionResultItemDto
{
    public int Aid { get; private set; } = -1;
    public bool Success { get; private set; } = false;
    public string ErrorText { get; private set; } = ""; 
    public PriceCalcActionResultItemDto(int aid)
    {
        Aid = aid;
        Success = true;
    }
    public PriceCalcActionResultItemDto(int aid, string errortext)
    {
        Aid = aid;
        Success = false;
        ErrorText = errortext;
    }

}