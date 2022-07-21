using eLog.Base.Purchase.Pord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.Pord
{
    public class PordHeadBL3 : PordHeadBL
    {
        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcPordHead olc)
            {
                var ols = objects.Get<PordHead>();

                olc.Pordid = ols.Pordid;
            }

            return base.PreSave(objects, e);
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcPordHead>();
            if (olc != null)
            {
                RuleServer.Validate<OlcPordHead, OlcPordHeadRules>(objects);
            }
        }

        public override void Delete(Key k)
        {
            var olc = OlcPordHead.Load(k);
            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.Delete(k);
        }
    }
}