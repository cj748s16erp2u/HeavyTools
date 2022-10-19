using eLog.Base.Sales.Sord;
using eLog.Base.Warehouse.Reserve;
using eLog.HeavyTools.InterfaceCaller;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineBL3 : SordLineBL
    {
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcSordLine>();
            if (olc != null)
            {
                RuleServer.Validate<OlcSordLine, OlcSordLineRules>(objects);
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcSordLine)
            {
                var sordLine = objects.Get<SordLine>();
                ((OlcSordLine)e).Sordlineid = sordLine.Sordlineid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        { 
            if (!IsDeletePossible(k, out var reason))
            {
                throw new MessageException(reason);
            }
             
            var p = new SordLineDeleteParamDto();
            p.Sordlineid = ConvertUtils.ToInt32(k["sordlineid"]);
            var icbl = new InterfaceCallerBL();
            var res = icbl.SordlineDelete(p);
            res.CheckResult();
            return;
        }
    }
}