using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Common.Matrix;
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
            Base.Warehouse.StockTran.StLine originalStLine = null;
            if (objects.SysParams.ActionID == ActionID.Modify)
            {
                var stLine = objects.Default as Base.Warehouse.StockTran.StLine;
                originalStLine = Base.Warehouse.StockTran.StLine.Load(stLine?.Stlineid);
            }

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
                else if (originalStLine != null)
                {
                    var stLine = objects.Default as Base.Warehouse.StockTran.StLine;
                    if (stLine != null)
                    {
                        var fields = originalStLine.Schema.Fields.ToList();
                        if (originalStLine.Schema.PKFields != null)
                        {
                            fields = fields.Except(originalStLine.Schema.PKFields).ToList();
                        }

                        stLine.RevertChanges();
                        stLine.ReLoad();

                        foreach (var f in fields)
                        {
                            stLine[f] = originalStLine[f];
                        }

                        stLine.Save();
                    }
                }

                throw;
            }
        }

        /// <summary>
        /// Vizsgalat, hogy az aktualis keszlet tranzakciohoz tartozik-e zona bejegyzes
        /// </summary>
        /// <param name="stid">Keszlet tranzakcio azonosito</param>
        /// <returns>A zona tranzakcio azonosito</returns>
        private int? GetWhZTranHeadId(int? stid)
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
        /// Vizsgalat, hogy az aktualis keszlet tranzakcio tetelhez tartozik-e zona bejegyzes
        /// </summary>
        /// <param name="stlineid">Keszlet tranzakcio tetel azonosito</param>
        /// <returns>True eseten letezik</returns>
        private bool CheckWhZTranLineExists(int? stlineid)
        {
            if (stlineid == null)
            {
                return false;
            }

            var key = new Key
            {
                [WhZone.WhZTran.OlcWhZTranLine.FieldStlineid.Name] = stlineid
            };

            var sql = $"select top 1 1 from [{WhZone.WhZTran.OlcWhZTranLine._TableName}] (nolock) where {key.ToSql()}";
            return ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, sql)) == 1;
        }

        /// <summary>
        /// Kulso WhZoneService meghivasa
        /// </summary>
        /// <param name="actionID">Aktualis muvelet (mentes, modositas, torles)</param>
        /// <param name="stLine">Akutalas keszlet tranzakcio tetel</param>
        /// <exception cref="ArgumentOutOfRangeException">Nem megfelelo muvelet lett meghatarozva</exception>
        /// <exception cref="MessageException">Szolgaltatas soran eloallt hibauzenet</exception>
        private void SaveWhZoneTranLine(string actionID, Base.Warehouse.StockTran.StLine stLine)
        {
            var whztid = this.GetWhZTranHeadId(stLine.Stid);
            if (stLine.Stid != null && whztid != null)
            {
                var tranLineService = WhZone.Common.WhZTranUtils.CreateTranLineService();
                var request = new WhZone.WhZTranService.WhZReceivingTranLineDto
                {
                    Whztid = whztid,
                    Stlineid = stLine.Stlineid,
                    AuthUser = Session.UserID,
                };

                var whzTranLineExists = this.CheckWhZTranLineExists(stLine.Stlineid);

                WhZone.WhZTranService.WhZReceivingTranLineDto result = null;
                try
                {
                    var addOrUpdate = true;
                    switch (actionID)
                    {
                        case ActionID.New:
                            addOrUpdate = true;
                            break;
                        case ActionID.Modify:
                            addOrUpdate = false;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(ActionID));
                    }

                    if (addOrUpdate)
                    {
                        result = tranLineService.Add(request);
                    }
                    else
                    {
                        result = tranLineService.Update(request);
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

        /// <summary>
        /// Beavatkozas a torlesi folyamatba:
        /// - ha hiba tortent a kulso szolgaltatas meghivasa soran, akkor az StLine bejegyzest nem kerul torlesre
        /// </summary>
        protected override void PreDelete(Key k)
        {
            base.PreDelete(k);

            var stLine = Base.Warehouse.StockTran.StLine.Load(k);
            this.DeleteWhZoneTranLine(stLine);
        }

        /// <summary>
        /// Kulso WhZoneService meghivasa
        /// </summary>
        /// <param name="stLine">Akutalas keszlet tranzakcio tetel</param>
        /// <exception cref="MessageException">Szolgaltatas soran eloallt hibauzenet</exception>
        private void DeleteWhZoneTranLine(Base.Warehouse.StockTran.StLine stLine)
        {
            if (stLine?.Stlineid != null)
            {
                var tranLineService = WhZone.Common.WhZTranUtils.CreateTranLineService();
                var request = new WhZone.WhZTranService.WhZTranLineDeleteDto
                {
                    Stlineid = stLine.Stlineid,
                    DeleteLoc = true,
                };

                try
                {
                    tranLineService.Delete(request);
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
            }
        }
    }
}
