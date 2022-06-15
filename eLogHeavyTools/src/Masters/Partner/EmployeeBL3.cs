using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    public class EmployeeBL3 : EmployeeBL
    {
        public static EmployeeBL3 New()
        {
            return (EmployeeBL3)ObjectFactory.New(typeof(PartnerBL));
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcEmployee)
            {
                var olc = (OlcEmployee)e;
                var ols = objects.Get<Employee>();

                olc.Empid = ols.Empid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        {
		    var olc = OlcEmployee.Load(k);
		    olc.State = DataRowState.Deleted;
		    olc.Save();

            base.Delete(k);
        }
    }
}