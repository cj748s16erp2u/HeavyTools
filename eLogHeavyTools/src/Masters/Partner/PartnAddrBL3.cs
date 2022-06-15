using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnAddrBL3 : PartnAddrBL
    {
        public static new PartnAddrBL3 New()
        {
            return (PartnAddrBL3)ObjectFactory.New(typeof(PartnerBL));
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcPartnAddr)
            {
                var olc = (OlcPartnAddr)e;
                var ols = objects.Get<PartnAddr>();

                olc.Addrid = ols.Addrid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        {
            var olc = OlcPartnAddr.Load(k);
            olc.State = DataRowState.Deleted;
            olc.Save();

            base.Delete(k);
        }
    }
}