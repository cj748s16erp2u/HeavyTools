using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.PriceTable
{
    public enum PrcType
    {
        Original = 0, 		// Eredeti
        Actual = 1,  		// Aktuális
        BasisOfAction = 2,  // Akció alapja
		SalePrice=4			// Akciós ár
    }
	
    public class PrcTypeList : EnumListProvider<PrcType>
    {
        public static readonly string ID = typeof(PrcTypeList).FullName;

        public static PrcTypeList New(string xml, string name)
        {
            var t = (PrcTypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
    }

}
