using eLog.Base.Masters.UniqueCard;
using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Reserve;
using eLog.HeavyTools.InterfaceCaller;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.Reserve
{
    internal class ReserveBL3 : ReserveBL
    {
        public override Key Save(BLObjectMap objects, string langnamespace, bool skipMerge)
        {
            var r = (Base.Warehouse.Reserve.Reserve)objects.Default;
 
            var c = 0;
            foreach (var item in objects)
            {
                c++;
            }
            if (c > 2)
            {
                throw new NotImplementedException();
            }

            if (!r.Adddate.HasValue)
            {
                r.Adddate = DateTime.Now;
                r.Addusrid = Session.UserID;
            }

            var p = new ReserveParamsDto();
            p.From(r);

            var icbl= new InterfaceCallerBL();
            var res = icbl.Reserve(p);

            res.CheckResult();

            r.Resid = res.Resid;
            r.ReLoad();

            return r.PK;
        }


        public override void Delete(Key k)
        {
            var r = Base.Warehouse.Reserve.Reserve.Load(ConvertUtils.ToInt32(k["resid"]));
            var p = new ReserveParamsDto();
            p.From(r); 
            var icbl = new InterfaceCallerBL();
            var res = icbl.ReserveDelete(p);
            res.CheckResult();
            return;
        }
    }
}
