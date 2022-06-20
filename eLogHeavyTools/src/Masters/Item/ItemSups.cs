using eLog.Base.Masters.Item;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    public class ItemSups : EntityCollection<ItemSup, ItemSups>
    {
        public static ItemSups LoadByItemid(int? itemid)
        {
            if (!itemid.HasValue)
            {
                return ItemSups.New();
            }
            return Load(new Key
            {
                { ItemSup.FieldItemid.Name, itemid }
            });
        }
    }
}
