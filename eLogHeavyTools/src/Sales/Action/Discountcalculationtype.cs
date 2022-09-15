using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System.Collections.Generic;

namespace eLog.HeavyTools.Sales.Action
{
    public enum Discountcalculationtype
    {
        /// <summary>
        /// Szétosztás
        /// </summary>
        Division = 0,
        /// <summary>
        /// Egy termék
        /// </summary>
        OneProduct = 1,

        /// <summary>
        /// Csak kedvezmény sor
        /// </summary>
        ExtDiscountOnly=2,

        /// <summary>
        /// Csak feltétel sor
        /// </summary>
        ExtConditionOnly=3,
    }
    class DiscountcalculationtypeList : EnumListProvider<Discountcalculationtype>
    {
        public static readonly string ID = typeof(DiscountcalculationtypeList).FullName;


        public static DiscountcalculationtypeList New(string xml, string name)
        {
            var t = (DiscountcalculationtypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
        protected override IEnumerable<string> GetSkippedValues(Dictionary<string, object> args)
        {
            return new[] { "ExtDiscountOnly", "ExtConditionOnly", "OneProduct" };
        }
    }
    class ExtDiscountcalculationtypeList : EnumListProvider<Discountcalculationtype>
    {
        public static readonly string ID = typeof(ExtDiscountcalculationtypeList).FullName;


        public static ExtDiscountcalculationtypeList New(string xml, string name)
        {
            var t = (ExtDiscountcalculationtypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
        protected override IEnumerable<string> GetSkippedValues(Dictionary<string, object> args)
        {
            return new[] { "OneProduct" };
        }
    }
}
