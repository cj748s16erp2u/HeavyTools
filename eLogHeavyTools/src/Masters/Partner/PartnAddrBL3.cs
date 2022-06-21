using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnAddrBL3 : PartnAddrBL
    {
        public static PartnAddrBL3 New3()
        {
            return (PartnAddrBL3)New();
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcPartnAddr>();
            if (olc != null)
            {
                RuleServer.Validate<OlcPartnAddr, OlcPartnAddrRules>(objects);
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcPartnAddr olc)
            {
                var ols = objects.Get<PartnAddr>();

                olc.Addrid = ols.Addrid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        {
            var olc = OlcPartnAddr.Load(k);
            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.Delete(k);
        }
    }
}