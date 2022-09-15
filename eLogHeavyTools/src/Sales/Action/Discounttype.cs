using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System.Collections.Generic;

namespace eLog.HeavyTools.Sales.Action
{
    public enum Discounttype
    {
        /// <summary>
        /// Összeg
        /// </summary>
        Val = 0,
        /// <summary>
        /// Százalék
        /// </summary>
        Percent = 1,
        /// <summary>
        /// Kupon
        /// </summary>
        Cupon = 2

    }
    class DiscounttypeList : EnumListProvider<Discounttype>
    {
        public static readonly string ID = typeof(DiscounttypeList).FullName;


        public static DiscounttypeList New(string xml, string name)
        {
            var t = (DiscounttypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
    }
     
    class ExtDiscounttypeList : EnumListProvider<Discounttype>
    {
        public static readonly string ID = typeof(ExtDiscounttypeList).FullName;


        public static ExtDiscounttypeList New(string xml, string name)
        {
            var t = (ExtDiscounttypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
        protected override IEnumerable<string> GetSkippedValues(Dictionary<string, object> args)
        {
            var l = new List<string>();

            l.Add("Cupon");

            return l;
        }
    }
}
