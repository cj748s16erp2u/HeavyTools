using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    public class ReceivingHeadBL3 : CodaInt.Base.Warehouse.StockTran.ReceivingHeadBL2
    {
        public static ReceivingHeadBL3 New3()
        {
            return (ReceivingHeadBL3)New();
        }

        /// <summary>
        /// Beavatkozas a mentesi folyamatba:
        /// - ha hiba tortent a kulso szolgaltatas meghivasa soran, akkor a mentett StHead bejegyzest torolni szukseges
        /// </summary>
        /// <returns>Mentett keszlet tranzakcio azonosito (null eseten a mentes meghiusult)</returns>
        public override Key Save(BLObjectMap objects, string langnamespace, bool skipMerge)
        {
            try
            {
                var key = base.Save(objects, langnamespace, skipMerge);
                if (key != null)
                {
                    var stHead = objects.Default as Base.Warehouse.StockTran.StHead;
                    if (stHead != null)
                    {
                        this.SaveWhZoneTran(objects.SysParams.ActionID, stHead);
                    }
                }

                return key;
            }
            catch
            {
                if (objects.SysParams.ActionID == ActionID.New)
                {
                    Key key = null;
                    if (objects.Default is Base.Warehouse.StockTran.StHead entity)
                    {
                        key = entity.PK;
                    }

                    Base.Warehouse.StockTran.StHead stHead = null;
                    if (key != null)
                    {
                        stHead = Base.Warehouse.StockTran.StHead.Load(key);
                    }

                    if (stHead != null)
                    {
                        this.Delete(stHead.PK);
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Mentes elotti vizsgalat, hogy van-e / szukseges-e zona tranzakciot letrehozni (towhzid ki van-e toltve)
        /// </summary>
        /// <returns>Ha nincs kitoltve, ellenben szukseges, akkor False</returns>
        /// <exception cref="MessageException">A zona tranzakciora nincs informacio meghatarozva (towhzid)</exception>
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var b = base.PreSave(objects, e);

            var stHead = e as Base.Warehouse.StockTran.StHead;
            if (stHead != null)
            {
                var needZoneTranHandling = this.CheckWhNeedZoneTranHandling(stHead.Towhid);

                string message = null;
                if (needZoneTranHandling)
                {
                    b = e.CustomData?.ContainsKey("towhzid") == true && ConvertUtils.ToInt32(stHead.GetCustomData("towhzid")) != null;
                    message = "$err_sthead_towhzid_mustbeset";

                    if (b && objects.SysParams.ActionID == ActionID.Modify && stHead.Stid != null)
                    {
                        b = this.CheckWhZTranHeadExists(stHead.Stid);
                        message = "$err_cant_delete_whztranhead";
                    }
                }
                else
                {
                    b = !(e.CustomData?.ContainsKey("towhzid") == true && ConvertUtils.ToInt32(stHead.GetCustomData("towhzid")) != null);
                    message = "$err_sthead_towhzid_cantbeset";
                }

                if (!b)
                {
                    throw new MessageException(message);
                }
            }

            return b;
        }

        /// <summary>
        /// Vizsgalat, hogy az aktualis keszlet tranzakciohoz tartozik-e zona bejegyzes
        /// </summary>
        /// <param name="stid">Keszlet tranzakcio azonosito</param>
        /// <returns>True eseten letezik</returns>
        public bool CheckWhZTranHeadExists(int? stid)
        {
            if (stid == null)
            {
                return false;
            }

            var key = new Key
            {
                [WhZone.WhZoneTran.OlcWhZTranHead.FieldStid.Name] = stid
            };

            var sql = $"select top 1 1 from [{WhZone.WhZoneTran.OlcWhZTranHead._TableName}] (nolock) where {key.ToSql()}";
            return ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql)) == 1;
        }

        /// <summary>
        /// Vizsgalat, hogy az aktualis raktar eseteben, kotelezo-e a zona keszlet kezeles
        /// </summary>
        /// <param name="whid">Raktar azonosito</param>
        /// <returns>True eseten kotelezo</returns>
        public bool CheckWhNeedZoneTranHandling(string whid)
        {
            if (string.IsNullOrWhiteSpace(whid))
            {
                return false;
            }

            var sql = $@"select top 1 [wh].[loctype]
from [ols_warehouse] [wh] (nolock)
where [wh].[whid] = {Utils.SqlToString(whid)}
";

            var locType = ConvertUtils.ToInt16(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql)).GetValueOrDefault();

            return locType == Setup.Warehouse.OlcWhZoneBL.WAREHOUSE_LOCTYPE_NEEDZONETRANHANDLING;
        }

        /// <summary>
        /// Kulso WhZoneService meghivasa
        /// </summary>
        /// <param name="actionID">Aktualis muvelet (mentes, modositas, torles)</param>
        /// <param name="stHead">Akutalas keszlet tranzakcio fej</param>
        /// <exception cref="ArgumentOutOfRangeException">Nem megfelelo muvelet lett meghatarozva</exception>
        /// <exception cref="MessageException">Szolgaltatas soran eloallt hibauzenet</exception>
        private void SaveWhZoneTran(string actionID, Base.Warehouse.StockTran.StHead stHead)
        {
            var towhzid = ConvertUtils.ToInt32(stHead.GetCustomData("towhzid"));
            if (towhzid != null)
            {
                //var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
                var authBase64 = this.CreateAuthentication();

                var httpClient = new System.Net.Http.HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);

                var url = this.GetCustomSettingsValue("WhZoneService-ServiceUrl");
                var tranService = new WhZone.WhZTranService.ReceivingClient(url, httpClient);
                var request = new WhZone.WhZTranService.WhZReceivingTranHeadDto
                {
                    Stid = stHead.Stid.Value,
                    Cmpid = stHead.Cmpid.Value,
                    Whztdate = stHead.Stdate.Value,
                    Towhzid = towhzid.Value,
                    AuthUser = Session.UserID,
                };

                WhZone.WhZTranService.WhZReceivingTranHeadDto result = null;
                try
                {
                    switch (actionID)
                    {
                        case ActionID.New:
                            result = tranService.Add(request);
                            break;
                        case ActionID.Modify:
                            result = tranService.Update(request);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(ActionID));
                    }
                }
                catch (WhZone.WhZTranService.ApiException ex)
                {
                    Log.Error(ex);
                    var response = ex.Response;
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        response = response.Substring(1, response.Length - 2);
                        throw new MessageException(response);
                    }

                    throw;
                }

                if (result?.Whztid == null)
                {
                    throw new MessageException("$err_unable_to_save_whztranhead");
                }
            }
        }

        /// <summary>
        /// Basic authentikacius kulcs eloallitasa
        /// </summary>
        /// <returns>Basic authentikacios kulcs</returns>
        private string CreateAuthentication()
        {
            var userId = this.GetCustomSettingsValue("WhZoneService-UserID");
            var password = this.GetCustomSettingsValue("WhZoneService-Password");
            if (password.StartsWith("*"))
            {
                password = password.Substring(1);
            }
            else
            {
                // TODO decrypt
            }

            var auth = $"{userId}:{password}";
            var authBase64 = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            return authBase64;
        }

        /// <summary>
        /// Parameter kiolvasasa es validalasa a CustomSettings-bol
        /// </summary>
        /// <param name="key">Paremter kulcs</param>
        /// <returns>Parameter ertek</returns>
        /// <exception cref="MessageException">A parameter nincs megadva vagy nincs erteke</exception>
        private string GetCustomSettingsValue(string key)
        {
            var value = CustomSettings.GetString(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new MessageException("$err_whzone_optionmissing".eLogTransl(key));
            }

            return value;
        }
    }
}
