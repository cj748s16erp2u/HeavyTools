using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Action
{
    public enum Purchasetype
    {
        /// <summary>
        /// Összes vásárlás
        /// </summary>
        All = 0,
        /// <summary>
        /// Első vásárlás
        /// </summary>
        First = 1,
        /// <summary>
        /// Csak a legolcsóbb termék megvásárlásához, több termék vásárlásakor
        /// </summary>
        Cheapest = 2
    }
    class PurchasetypeList : EnumListProvider<Purchasetype>
    {
        public static readonly string ID = typeof(PurchasetypeList).FullName;


        public static PurchasetypeList New(string xml, string name)
        {
            var t = (PurchasetypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
    }
}
