using eLog.HeavyTools.Warehouse.WhZone.Common;
using eLog.HeavyTools.Warehouse.WhZone.WhZTran;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
using eLog.HeavyTools.Warehouse.WhZStockMap;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZStockMap
{
    public class WhZStockMapSearchProvider : MemorySearchProvider<WhZStockMapDto>
    {
        public static readonly string ID = typeof(WhZStockMapSearchProvider).FullName;
        public WhZStockMapSearchProvider()
            : base("", Enumerable.Empty<QueryArg>().ToArray(), SearchProviderType.Default)
        {

        }

        private WhZStockMapClient GetWhZStockMapService()
        {
            var authBase64 = WhZTranUtils.CreateAuthentication();



            var httpClient = new System.Net.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);



            var url = WhZTranUtils.GetServiceUrl();
            var stockMapService = new WhZStockMapClient(url, httpClient);



            return stockMapService;
        }

        protected override IList<WhZStockMapDto> PrepareList(string sql, MSPCreateListArgs args)
        {
            IList<WhZStockMapDto> list = new List<WhZStockMapDto>();
            if (args.CallType == MSPCreateListCallType.Search)
            {
                var stockMapService = GetWhZStockMapService();

                WhZStockMapQueryDto body = new WhZStockMapQueryDto();

                
                ICollection<WhZStockMapQDto> result;
                SetFilters(args, body);
                try
                {
                    result = stockMapService.Query(body);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            var temp = new WhZStockMapDto
                            {
                                Whid = item.Whid,
                                Whzoneid = item.Whzoneid,
                                Whcode = item.Whcode,
                                Whname = item.Whname,
                                Actqty = item.Actqty,
                                Resqty = item.Resqty,
                                Itemid = item.Itemid,
                                Itemcode = item.Itemcode,
                                Whloccode = item.Whloccode,
                                Itemname = item.Itemname,
                                Provqty = item.Provqty,
                                Reqqty = item.Reqqty,
                                Recqty = item.Recqty,
                                Whlocid = item.Whlocid,
                                Whlocname = item.Whlocname,
                                Whzonecode = item.Whzonecode,
                                Whzonename = item.Whzonename,
                            };
                            list.Add(temp);
                        }
                    }
                }
                catch (ApiException ex)
                {
                    Log.Error(ex);
                    var response = ex.Response;
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        response = WhZTranUtils.ParseErrorResponse(response);
                        throw new MessageException(response);
                    }



                    throw;
                }



                if (result == null)
                {
                    throw new MessageException("$err_empty_list");
                }
            }
            return list;
        }

        private void SetFilters(MSPCreateListArgs args, WhZStockMapQueryDto body)
        {
            if (args.Filters.TryGetValue("Whid", out var whids))
            {
                body.Whid = new List<string>();
                if (whids is string)
                {
                    body.Whid.Add(whids.ToString());
                }
                else
                {
                    foreach (var whid in (IEnumerable<object>)whids)
                    {
                        body.Whid.Add(whid.ToString());
                    }
                }
            }
            if (args.Filters.TryGetValue("Whzonecode", out var whzonecode))
            {
                body.Whzonecode = whzonecode.ToString();
            }
            if (args.Filters.TryGetValue("Itemcode", out var itemcode))
            {
                body.Itemcode = itemcode.ToString();

            }
            if (args.Filters.TryGetValue("Itemname", out var itemname))
            {
                body.Itemname = itemname.ToString();

            }
            if (args.Filters.TryGetValue("Barcode", out var barcode))
            {
                body.Barcode = barcode.ToString();

            }
            if (args.Filters.TryGetValue("Cmpid", out var cmpids))
            {
                body.Cmpid = new List<int>();
                if (cmpids is Int64)
                {
                    body.Cmpid.Add(Convert.ToInt32(cmpids));
                }
                else
                {
                    foreach (var cmpid in (IEnumerable<object>)cmpids)
                    {
                        body.Cmpid.Add(Convert.ToInt32(cmpid));
                    }
                }

            }
            if (args.Filters.TryGetValue("Whloccode", out var whloccode))
            {
                body.Whloccode = whloccode.ToString();

            }
            if (args.Filters.TryGetValue("Delstat", out var delstat))
            {
                body.Delstat = Convert.ToInt32(delstat);
            }
            if (args.Filters.TryGetValue("Nonzero", out var nonzero))
            {
                body.Nonzerostock = (bool)nonzero;
            }
        }
    }
}
