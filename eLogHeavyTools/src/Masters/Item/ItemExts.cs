using eLog.Base.Masters.Item;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    class ItemExts : EntityCollection<ItemExt, ItemExts>
    {
        public static ItemExts LoadDefByItemid(int? itemid)
        {
            if (!itemid.HasValue)
            {
                return ItemExts.New();
            }
            return Load(new Key
            {
                { ItemExt.FieldItemid.Name, itemid },
                { ItemExt.FieldDef.Name, 1 },

            });
        }
    }
}
