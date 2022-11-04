using eLog.HeavyTools.Common.Data;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLocSearchProvider : HTSearchProviderBase<OlcWhZTranLoc>
    {
        public static readonly string ID = typeof(OlcWhZReceivingLocSearchProvider).FullName;

        protected static QueryArg[] argsTemplate = new[]
        {
           new QueryArg("Whztlocid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Whztid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Whztlineid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Whid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Whlocid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Whztltype", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Ordqty", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Dispqty", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Movqty", FieldType.Integer, QueryFlags.MultipleAllowed)
        };
        public OlcWhZReceivingLocSearchProvider() : base("", argsTemplate, SearchProviderType.Default)
        {
        }

        protected override IList<OlcWhZTranLoc> PrepareList(MSPCreateListArgs args)
        {
            return new List<OlcWhZTranLoc>();
            //List<OlcWhZTranLoc> list = new List<OlcWhZTranLoc>();

            //if (args.CallType == MSPCreateListCallType.Search)
            //{
            //    var tranLineService = GetWhZStockMapClientService();
            //    var request = ConvertToDto<WhZStockMapQDto>(args.Filters);

            //    try
            //    {
            //        var temp = tranLineService.Query_erp();
            //        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(temp);
            //        if (obj is Newtonsoft.Json.Linq.JArray a)
            //        {
            //            var cnt = a.Count;
            //            foreach (var item in a)
            //            {
            //                Dictionary<string, object> dic = item.ToObject<Dictionary<string, object>>();
            //                OlcWhZTranLoc line = ConvertToDto<OlcWhZTranLoc>(dic);
            //                list.Add(line);
            //            }
            //        }

            //        return list;
            //    }
            //    catch (ApiException ex)
            //    {
            //        Log.Error(ex);
            //        var response = ex.Response;
            //        if (!string.IsNullOrWhiteSpace(response))
            //        {
            //            response = Common.WhZTranUtils.ParseErrorResponse(response);
            //            throw new MessageException(response);
            //        }

            //        throw;
            //    }
            //}
            //return list;
        }

        //private WhZStockMapClient GetWhZStockMapClientService()
        //{
        //    var authBase64 = Common.WhZTranUtils.CreateAuthentication();

        //    var httpClient = new System.Net.Http.HttpClient();
        //    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);

        //    var url = Common.WhZTranUtils.GetServiceUrl();
        //    var tranService = new WhZStockMapClient(url, httpClient);

        //    return tranService;
        //}
    }
}
