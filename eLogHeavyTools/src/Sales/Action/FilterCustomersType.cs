using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System.Collections.Generic;

namespace eLog.HeavyTools.Sales.Action
{
    public enum FilterCustomersType
    {
        All,
        OnlyLoyalty,
        NotForLoyalty,
        NotForResale
    }

    public class FilterCustomersTypeList : EnumListProvider<FilterCustomersType>
    {
        public static readonly string ID = typeof(FilterCustomersTypeList).FullName;


        public static FilterCustomersTypeList New(string xml, string name)
        {
            var t = (FilterCustomersTypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        } 
        public override string GetValuesJSON(Dictionary<string, object> args)
        {
            var c= base.GetValuesJSON(args);
            Columns[1].Width = 350;
            return c;
        } 
    }
}
