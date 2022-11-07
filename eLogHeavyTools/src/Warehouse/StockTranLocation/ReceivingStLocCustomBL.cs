using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Warehouse.WhZone.Common;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomBL : DefaultBL1<ReceivingStLocCustomDto, ReceivingStLocCustomRules>
    {
        public static readonly string ID = typeof(ReceivingStLocCustomBL).FullName;

        public static T New<T>() where T : ReceivingStLocCustomBL
        {
            return ObjectFactory.New<T>();
        }

        public static ReceivingStLocCustomBL New()
        {
            return New<ReceivingStLocCustomBL>();
        }

        protected ReceivingStLocCustomBL() { }

        protected override Entity GetEntityInternal(Key k)
        {
            if ((Functions & DefaultBLFunctions.GetEntity) == 0)
            {
                throw new InvalidOperationException();
            }

            if (k is null)
            {
                return null;
            }

            var fieldName = nameof(ReceivingStLocCustomDto.Whztlocid);
            if (k.TryGetValue(fieldName, out var o) && o != null)
            {
                var whztlocid = ConvertUtils.ToInt32(o);
                if (whztlocid != null)
                {
                    var tranLocService = WhZTranUtils.CreateTranLocService();
                    var entity = tranLocService.Get(whztlocid.Value);
                    return this.ConvertServiceResponseToDto(entity);
                }
            }

            return null;
        }

        public ReceivingStLocCustomDto ConvertServiceResponseToDto(WhZone.WhZTranService.WhZTranLocDto entity)
        {
            if (entity is null)
            {
                return null;
            }

            var dto = new ReceivingStLocCustomDto
            {
                Whztlocid = entity.Whztlocid,
                Whztid = entity.Whztid,
                Whztlineid = entity.Whztlineid,
                Whid = entity.Whid,
                Whname = entity.Whname,
                Whzoneid = entity.Whzoneid,
                Whzonecode = entity.Whzonecode,
                Whzonename = entity.Whzonename,
                Whlocid = entity.Whlocid,
                Whloccode = entity.Whloccode,
                Whlocname = entity.Whlocname,
                Itemid = entity.Itemid,
                Itemcode = entity.Itemcode,
                Itemname01 = entity.Itemname01,
                Whztltype = entity.Whztltype,
                Ordqty = ConvertUtils.ToDecimal(entity.Ordqty),
                Dispqty = ConvertUtils.ToDecimal(entity.Dispqty),
                Movqty = ConvertUtils.ToDecimal(entity.Movqty),
                Addusrid = entity.Addusrid,
                Adddate = entity.Adddate?.DateTime,
            };

            return dto;
        }

        public WhZStockMapQDto GetStock(string whid, int? whzoneid, int? itemid, int? whlocid)
        {
            ICollection<string> whids = null;
            if (!string.IsNullOrWhiteSpace(whid))
            {
                whids = new[] { whid };
            }

            var query = new WhZStockMapQueryDto
            {
                Whid = whids,
                Whzoneid = whzoneid,
                Itemid = itemid,
                Whlocid = whlocid,
            };

            var tranStockMapService = WhZTranUtils.CreateStockMapService();
            var stockMap = tranStockMapService.Get(query);
            return stockMap;
        }
    }
}
