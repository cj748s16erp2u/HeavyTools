using eLog.Base.Warehouse.StockTran;
using eLog.HeavyTools.Common.Data;
using eLog.HeavyTools.Setup.Warehouse;
using eLog.HeavyTools.Warehouse.WhZone.WhZTranService;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public class OlcWhZReceivingHeadSearchProvider : HTSearchProviderBase<OlcWhZTranHead>
    {
        public static readonly string ID = typeof(OlcWhZReceivingHeadSearchProvider).FullName;


        protected static QueryArg[] argsTemplate = new[]
        {
           new QueryArg("Whztid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Cmpid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Fromdate", FieldType.DateTime, QueryFlags.Greater | QueryFlags.Equals),
           new QueryArg("Todate", FieldType.DateTime, QueryFlags.Less | QueryFlags.Equals ),
           new QueryArg("Fromwhzid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Towhzid", FieldType.Integer, QueryFlags.MultipleAllowed),
           new QueryArg("Stid", FieldType.Integer, QueryFlags.MultipleAllowed)
        };

        public OlcWhZReceivingHeadSearchProvider() : base("", argsTemplate, SearchProviderType.Default)
        {

        }

        //public override void DropList()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override IList<OlcWhZTranHead> PrepareList(string sql, MSPCreateListArgs args)
        //{
        //    throw new NotImplementedException();
        //}

        protected override IList<OlcWhZTranHead> PrepareList(MSPCreateListArgs args)
        {
            List<OlcWhZTranHead> list = new List<OlcWhZTranHead>();
            
            if (args.CallType == MSPCreateListCallType.Search)
            {
                var tranService = GetWhZTranService();
                var request = ConvertToDto<WhZTranHeadQueryDto>(args.Filters);

                try
                {                 
                    var temp = tranService.Query_erp(request);
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(temp);
                    if (obj is Newtonsoft.Json.Linq.JArray a)
                    {
                        var cnt = a.Count;
                        foreach (var item in a)
                        {
                            Dictionary<string, object> dic = item.ToObject<Dictionary<string, object>>();
                            OlcWhZTranHead head = ConvertToDto<OlcWhZTranHead>(dic);
                            list.Add(head);
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

        private void SaveWhZReceivingTranHead(string actionID, OlcWhZTranHead whZTHead)
        {
            //var whztid = ConvertUtils.ToInt32(whZTHead.GetCustomData("whztid"));
            //if (whztid != null)
            //{

            //    var tranService = GetWhZTranService();
            //    var request = new WhZReceivingTranHeadDto
            //    {
            //        Whztid = whZTHead.Whztid.Value,
            //        Cmpid = whZTHead.Cmpid.Value,
            //        Whzttype = WhZTranHead_Whzttype._1, //nem tudom, hogy itt kell megmondani, hogy 1-es type vagy backend mondja ezt meg
            //        Whztdate = whZTHead.Whztdate.Value,
            //        Closedate = whZTHead.Closedate.Value,
            //        Closeusrid = whZTHead.Closeusrid.Value,
            //        Whztstat = (WhZTranHead_Whztstat)whZTHead.Whztstat,
            //        Note = whZTHead.Note,
            //        Stid = whZTHead.Stid.Value,
            //        Towhzid = whZTHead.Towhzid.Value,
            //        AuthUser = Session.UserID,
            //    };

            //    WhZReceivingTranHeadDto result = null;
            //    try
            //    {
            //        switch (actionID)
            //        {
            //            case ActionID.New:
            //                result = tranService.Add(request);
            //                break;
            //            case ActionID.Modify:
            //                result = tranService.Update(request);
            //                break;
            //            default:
            //                throw new ArgumentOutOfRangeException(nameof(ActionID));
            //        }
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

            //    if (result?.Whztid == null)
            //    {
            //        throw new MessageException("$err_unable_to_save_whztranhead");
            //    }
            //}
        }

        private WhZTranClient GetWhZTranService()
        {
            var authBase64 = Common.WhZTranUtils.CreateAuthentication();

            var httpClient = new System.Net.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);

            var url = Common.WhZTranUtils.GetServiceUrl();
            var tranService = new WhZTranClient(url, httpClient);

            return tranService;
        }
    }
}
