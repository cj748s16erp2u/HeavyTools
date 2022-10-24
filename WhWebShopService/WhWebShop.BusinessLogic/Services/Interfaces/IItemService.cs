using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

public interface IItemService
{
    Task<ItemDto> GetItems(CancellationToken cancellationToken = default);
    Task<EmptyDto> RecalcItemPriceAsync(CancellationToken cancellationToken = default);
}
