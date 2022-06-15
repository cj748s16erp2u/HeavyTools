using eLog.Base.Masters.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    public class ItemBL2: ItemBL
    {
        protected override bool PreSave(eProjectWeb.Framework.BL.BLObjectMap objects, eProjectWeb.Framework.Data.Entity e)
        {
            bool b = base.PreSave(objects, e);

            eLog.Base.Masters.Item.Item it = (eLog.Base.Masters.Item.Item)objects.Default;

            if (e is OlcItem)
            {
                OlcItem it2 = (OlcItem)e;
                if (it2.State == eProjectWeb.Framework.Data.DataRowState.Added)
                    it2.Itemid = it.Itemid;
            }
            return b;
        }

        public override void Validate(eProjectWeb.Framework.BL.BLObjectMap objects)
        {
            base.Validate(objects);

            OlcItem it2 = objects[typeof(OlcItem).Name] as OlcItem;
            if (it2 != null)
                eProjectWeb.Framework.RuleServer.Validate(objects, typeof(OlcItemRules));
        }

    }
}
