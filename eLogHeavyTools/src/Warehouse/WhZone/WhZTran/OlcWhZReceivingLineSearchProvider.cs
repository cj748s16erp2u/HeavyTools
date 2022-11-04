using eLog.HeavyTools.Common.Data;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingLineSearchProvider : HTSearchProviderBase<OlcWhZTranLine>
    {
        public static readonly string ID = typeof(OlcWhZReceivingLineSearchProvider).FullName;

        protected static QueryArg[] argsTemplate = new[]
        {
           new QueryArg("whztid",FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("whztlineid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("stid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("stlineid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("itemid", FieldType.Integer, QueryFlags.MultipleAllowed),
           //new QueryArg("ordqty", FieldType.Integer, QueryFlags.MultipleAllowed)
        };
        public OlcWhZReceivingLineSearchProvider() : base("", argsTemplate, SearchProviderType.Default)
        {
        }

        protected override IList<OlcWhZTranLine> PrepareList(MSPCreateListArgs args)
        {
            List<OlcWhZTranLine> list = new List<OlcWhZTranLine>();

            if (args.CallType == MSPCreateListCallType.Search)
            {
                var tranLineService = GetWhZTranLineService();
                var request = ConvertToDto<WhZTranLineQueryDto>(args.Filters);

                try
                {
                    var temp = tranLineService.Query_erp(request);
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(temp);
                    if (obj is Newtonsoft.Json.Linq.JArray a)
                    {
                        var cnt = a.Count;
                        foreach (var item in a)
                        {
                            Dictionary<string, object> dic = item.ToObject<Dictionary<string, object>>();
                            OlcWhZTranLine line = ConvertToDto<OlcWhZTranLine>(dic);
                            list.Add(line);
                        }
                    }

                    return list;
                }
                catch (ApiException ex)
                {
                    Log.Error(ex);
                    var response = ex.Response;
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        response = Common.WhZTranUtils.ParseErrorResponse(response);
                        throw new MessageException(response);
                    }

                    throw;
                }
            }
            return list;
        }

        //private void SaveWhZReceivingTranLine(string actionID, OlcWhZTranLine whZTLine)
        //{
        //    var whztid = ConvertUtils.ToInt32(whZTLine.GetCustomData("whztlineid"));
        //    if (whztid != null)
        //    {

        //        var tranService = GetWhZTranLineService();
        //        var request = new WhZReceivingTranLineDto
        //        {
        //            Whztid = whZTHead.Whztid.Value,
        //            Cmpid = whZTHead.Cmpid.Value,
        //            Whzttype = WhZTranHead_Whzttype._1, //nem tudom, hogy itt kell megmondani, hogy 1-es type vagy backend mondja ezt meg
        //            Whztdate = whZTHead.Whztdate.Value,
        //            Closedate = whZTHead.Closedate.Value,
        //            Closeusrid = whZTHead.Closeusrid.Value,
        //            Whztstat = (WhZTranHead_Whztstat)whZTHead.Whztstat,
        //            Note = whZTHead.Note,
        //            Stid = whZTHead.Stid.Value,
        //            Towhzid = whZTHead.Towhzid.Value,
        //            AuthUser = Session.UserID,
        //        };

        //        WhZReceivingTranLineDto result = null;
        //        try
        //        {
        //            switch (actionID)
        //            {
        //                case ActionID.New:
        //                    result = tranService.Add(request);
        //                    break;
        //                case ActionID.Modify:
        //                    result = tranService.Update(request);
        //                    break;
        //                default:
        //                    throw new ArgumentOutOfRangeException(nameof(ActionID));
        //            }
        //        }
        //        catch (ApiException ex)
        //        {
        //            Log.Error(ex);
        //            var response = ex.Response;
        //            if (!string.IsNullOrWhiteSpace(response))
        //            {
        //                response = Common.WhZTranUtils.ParseErrorResponse(response);
        //                throw new MessageException(response);
        //            }

        //            throw;
        //        }

        //        if (result?.Whztid == null)
        //        {
        //            throw new MessageException("$err_unable_to_save_whztranhead");
        //        }
        //    }
        //}

        private WhZTranLineClient GetWhZTranLineService()
        {
            var authBase64 = Common.WhZTranUtils.CreateAuthentication();

            var httpClient = new System.Net.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);

            var url = Common.WhZTranUtils.GetServiceUrl();
            var tranService = new WhZTranLineClient(url, httpClient);

            return tranService;
        }
    }
}
