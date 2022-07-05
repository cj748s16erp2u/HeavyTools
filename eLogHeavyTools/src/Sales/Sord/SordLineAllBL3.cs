using eLog.Base.Sales.Sord;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineAllBL3 : SordLineAllBL
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
            var olc = OlcSordLine.Load(k);

            if (olc != null)
            {
                olc.State = DataRowState.Deleted;
                olc.Save();
            }

            base.Delete(k);
        }
    }
}