using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    public class ReceivingLineBL3 : CodaInt.Base.Warehouse.StockTran.ReceivingLineBL2
    {
        /// <summary>
        /// Beavatkozas a mentesi folyamatba:
        /// - ha hiba tortent a kulso szolgaltatas meghivasa soran, akkor a mentett StLine bejegyzest torolni szukseges
        /// </summary>
        /// <returns>Mentett keszlet tranzakcio azonosito (null eseten a mentes meghiusult)</returns>
        public override Key Save(BLObjectMap objects, string langnamespace, bool skipMerge)
        {
            try
            {
                var key = base.Save(objects, langnamespace, skipMerge);
                if (key != null)
                {
                    var stLine = objects.Default as Base.Warehouse.StockTran.StLine;
                    if (stLine != null)
                    {
                        this.SaveWhZoneTranLine(objects.SysParams.ActionID, stLine);
                    }
                }

                return key;
            }
            catch
            {
                if (objects.SysParams.ActionID == ActionID.New)
                {
                    Key key = null;
                    if (objects.Default is Base.Warehouse.StockTran.StLine entity)
                    {
                        key = entity.PK;
                    }

                    Base.Warehouse.StockTran.StLine stLine = null;
                    if (key != null)
                    {
                        stLine = Base.Warehouse.StockTran.StLine.Load(key);
                    }

                    if (stLine != null)
                    {
                        this.Delete(stLine.PK);
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Vizsgalat, hogy az aktualis keszlet tranzakciohoz tartozik-e zona bejegyzes
        /// </summary>
        /// <param name="stid">Keszlet tranzakcio azonosito</param>
        /// <returns>True eseten letezik</returns>
        private int? CheckWhZTranHeadExists(int? stid)
        {
            if (stid == null)
            {
                return null;
            }

            var key = new Key
            {
                [WhZone.WhZTran.OlcWhZTranHead.FieldStid.Name] = stid
            };

            var sql = $"select top 1 [{WhZone.WhZTran.OlcWhZTranHead.FieldWhztid.Name}] from [{WhZone.WhZTran.OlcWhZTranHead._TableName}] (nolock) where {key.ToSql()}";
            return ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql));
        }

        /// <summary>
        /// Kulso WhZoneService meghivasa
        /// </summary>
        /// <param name="actionID">Aktualis muvelet (mentes, modositas, torles)</param>
        /// <param name="stHead">Akutalas keszlet tranzakcio tetel</param>
        /// <exception cref="ArgumentOutOfRangeException">Nem megfelelo muvelet lett meghatarozva</exception>
        /// <exception cref="MessageException">Szolgaltatas soran eloallt hibauzenet</exception>
        private void SaveWhZoneTranLine(string actionID, Base.Warehouse.StockTran.StLine stLine)
        {
            var whztid = this.CheckWhZTranHeadExists(stLine.Stid);
            if (stLine.Stid != null && whztid != null)
            {
                var authBase64 = WhZone.Common.WhZTranUtils.CreateAuthentication();

                var httpClient = new System.Net.Http.HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authBase64);

                var url = WhZone.Common.WhZTranUtils.GetServiceUrl();
                var tranLineService = new WhZone.WhZTranService.WhZTranLineClient(url, httpClient);
                var request = new WhZone.WhZTranService.WhZReceivingTranLineDto
                {
                    Whztid = whztid,
                    Stlineid = stLine.Stlineid,
                    AuthUser = Session.UserID,
                };

                WhZone.WhZTranService.WhZReceivingTranLineDto result = null;
                try
                {
                    switch (actionID)
                    {
                        case ActionID.New:
                            result = tranLineService.Add(request);
                            break;
                        case ActionID.Modify:
                            result = tranLineService.Update(request);
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
                        response = WhZone.Common.WhZTranUtils.ParseErrorResponse(response);
                        throw new MessageException(response);
                    }

                    throw;
                }

                if (result?.Whztlineid == null)
                {
                    throw new MessageException("$err_unable_to_save_whztranline");
                }
            }
        }
    }
}
