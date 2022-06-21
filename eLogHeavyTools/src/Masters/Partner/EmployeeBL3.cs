using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    public class EmployeeBL3 : EmployeeBL
    {
        public static EmployeeBL3 New3()
        {
            return (EmployeeBL3)New();
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var olc = objects.Get<OlcEmployee>();
            if (olc != null)
            {
                RuleServer.Validate<OlcEmployee, OlcEmployeeRules>(objects);
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            if (e is OlcEmployee olc)
            {
                var ols = objects.Get<Employee>();

                olc.Empid = ols.Empid;
            }

            return base.PreSave(objects, e);
        }

        public override void Delete(Key k)
        {
		    var olc = OlcEmployee.Load(k);
            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.Delete(k);
        }
    }
}