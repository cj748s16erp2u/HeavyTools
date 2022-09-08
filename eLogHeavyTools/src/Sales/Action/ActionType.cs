using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Action
{
    public enum ActionType
    {
        /// <summary>
        /// Kupon
        /// </summary>
        Cupon = 0,
        /// <summary>
        /// Akció
        /// </summary>
        Action = 1,
        /// <summary>
        /// Törzsvásárló kártya
        /// </summary>
        Loyaltycardno = 2,
        /// <summary>
        /// VIP kártya
        /// </summary>
        VIP = 3
    }

    class ActionTypeList : EnumListProvider<ActionType>
    {
        public static readonly string ID = typeof(ActionTypeList).FullName;


        public static ActionTypeList New(string xml, string name)
        {
            var t = (ActionTypeList)ObjectFactory.New(typeof(XmlBaseDefaultListProvider));
            return t;
        }
    }


}
