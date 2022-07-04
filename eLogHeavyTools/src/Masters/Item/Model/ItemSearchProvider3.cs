using eLog.Base.Masters.Item;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Model
{
    public class ItemSearchProvider3 : ItemSearchProvider
    {
        public static new readonly string ID = typeof(ItemSearchProvider3).FullName;

         
        protected ItemSearchProvider3() : base()
        {
            ArgsTemplate = MergeQueryArgs(ArgsTemplate, argsTemplate2);
        }
        
        protected static QueryArg[] argsTemplate2 = new QueryArg[]
        {
            new QueryArg("imid", "ims", FieldType.Integer, QueryFlags.Equals)
        };

        protected const string SQL_Fields = @", ic.note note2";

        protected const string SQL_Joins = @"
	left join olc_item ic (nolock) on ic.itemid=itm.itemid
	left join olc_itemmodelseason ims (nolock) on ims.imsid= ic.imsid  ";

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var q = base.CreateQueryString(args, fmtonly);

            q = q.Replace("--$$morefields$$", SQL_Fields);
            q = q.Replace("--$$morejoins$$", SQL_Joins);

            return q;
        }

    }
}
