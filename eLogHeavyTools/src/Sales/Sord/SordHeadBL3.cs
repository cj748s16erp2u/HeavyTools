using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordHeadBL3 : SordHeadBL
    {
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcSordHead>();
            if (olc != null)
            {
                RuleServer.Validate<OlcSordHead, OlcSordHeadRules>(objects);
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcSordHead olc)
            {
                var ols = objects.Get<SordHead>();

                if (!olc.Sordapprovalstat.HasValue)
                {
                    var defaultValue = CustomSettings.GetInt("SordHeadFefaultApproval");

                    olc.Sordapprovalstat = defaultValue;
                }

                olc.Sordid = ols.Sordid;
            }

            if (e is OlcSordLine)
            {
                var sordLine = objects.Get<SordLine>();
                ((OlcSordLine)e).Sordlineid = sordLine.Sordlineid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        {
            var olc = OlcSordHead.Load(k);
            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.Delete(k);
        }
    }
}