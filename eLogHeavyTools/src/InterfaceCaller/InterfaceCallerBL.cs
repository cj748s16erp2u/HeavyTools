using eLog.HeavyTools.Common.Json;
using eLog.HeavyTools.Sales.Retail.Cart;
using eProjectWeb.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eLog.HeavyTools.InterfaceCaller
{
    public class InterfaceCallerBL
    {
        public CalcJsonResultDto CartCalucate(CartData cartdata)
        {
            var c = new CalcJsonParamsDto
            {
                Wid = cartdata.Wid,
                FirstPurchase = cartdata.FirstPurchase,
                Curid = cartdata.Curid,
                LoyaltyCardNo = cartdata.LoyaltyCardNo,
                CountryId = cartdata.CountryId,
                Cupons = cartdata.Cupons.ToArray()
            };
            foreach (var crt in cartdata.Items)
            {
                c.Items.Add(new CartItemJson()
                {
                    CartId = crt.Cartid,
                    Itemid = crt.Itemid,
                    Quantity = 1
                });
            }

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var json = @"{ ""Cart"": " + JsonConvert.SerializeObject(c, Formatting.None, jsSettings) + @"}";


           return CallInterface<CalcJsonResultDto>("PriceCalc/calc", json, UpdateResult);

        } 

        public string UpdateResult(string result)
        {
            return result.Replace(@"""items""", @"""Items2""");
        }

        internal SordLineDeleteResultDto SordlineDelete(SordLineDeleteParamDto p)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var json = @"{ ""SordLine"": " + JsonConvert.SerializeObject(p, Formatting.None, jsSettings) + @"}";
            return CallInterface<SordLineDeleteResultDto>("Sord/sordlinedelete", json);
             
        }

        private T CallInterface<T>(string endpointurl, string json)
        {
            return CallInterface<T>(endpointurl, json, null);
        }

        internal ReserveResultDto ReserveDelete(ReserveParamsDto p)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var json = @"{ ""Reserve"": " + JsonConvert.SerializeObject(p, Formatting.None, jsSettings) + @"}";
            return CallInterface<ReserveResultDto>("Reserve/reservedelete", json);
        }

        internal ReserveResultDto Reserve(ReserveParamsDto p)
        {

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var json = @"{ ""Reserve"": " + JsonConvert.SerializeObject(p, Formatting.None, jsSettings) + @"}";
            return CallInterface<ReserveResultDto>("Reserve/reserve", json);

        }

        public void ResetAction(int? aid)
        {
            var t = new Thread(() => RealResetAction(aid));
            t.Start();
        }

        private void RealResetAction(int? aid) {
            var c = new CalcResetJsonParamsDto
            {
                Aid = aid
            };

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var json = @"{ ""Reset"": " + JsonConvert.SerializeObject(c, Formatting.None, jsSettings) + @"}";

            CallInterface<NullResult>("PriceCalc/reset", json, UpdateResult2);
        }

        private string UpdateResult2(string arg)
        {
            return arg.Replace(@"""items""", @"""Items2""");
        }

        private T CallInterface<T>(string endpointurl, string json, Func<string, string> updateResult)
        {
            var url = CustomSettings.GetString("WhWebShopServiceUrl") + endpointurl;
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            using (HttpMessageHandler handler = new HttpClientHandler())
            {
                var httpClient = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                var plainTextBytes = Encoding.UTF8.GetBytes(CustomSettings.GetString("WhWebShopServiceUsernameAndPassword"));
                string val = Convert.ToBase64String(plainTextBytes); 


                var request = GetRequestMessage(url, val, json);
                var response = httpClient.SendAsync(request).Result;
                var parms = response.Content.ReadAsStringAsync().Result;
                if (updateResult != null)
                {
                    parms = UpdateResult(parms);
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        var t = typeof(T);

                        if (t == typeof(NullResult))
                        {
                            return (T)Activator.CreateInstance(typeof(T));
                        }
                        return JsonParser.ParseObject<T>(JObject.Parse(parms));
                    }
                    catch (Exception)
                    {
                        throw new MessageException(parms);
                    }
                } else
                {
                    throw new MessageException(response.StatusCode.ToString() + " " + response);
                }
            }
        }

        private HttpRequestMessage GetRequestMessage(string uri, string accessBasic, string jsonPayload)
        { 
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Headers = { { "Authorization", $"Basic {accessBasic}" } }
            }; 
            httpRequestMessage.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            return httpRequestMessage;
        }

        internal void ClearCartCache()
        {
            var json = @"{}";
            CallInterface<NullResult>("PriceCalc/cartcachereset", json);
        }

        internal void ReloadActions()
        {
            var json = @"{""Reset"": {""Aid"": null}}";

            CallInterface<NullResult>("PriceCalc/reset", json);
        }
    }
    public class NullResult
    {

    }
}
