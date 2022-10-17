using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class OlcStHeadBL : DefaultBL1<OlcStHead, OlcStHeadRules>
    {
        public static readonly string ID = typeof(OlcStHeadBL).FullName;

        protected OlcStHeadBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcStHeadBL New()
        {
            return (OlcStHeadBL)ObjectFactory.New(typeof(OlcStHeadBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcStHead).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcStHeadRules));
        }
    }

}
