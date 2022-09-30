using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Assortment
{
    public class OlcItemAssortmentBL : DefaultBL1<OlcItemAssortment, OlcItemAssortmentRules>
    {
        public static readonly string ID = typeof(OlcItemAssortmentBL).FullName;

        protected OlcItemAssortmentBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemAssortmentBL New()
        {
            return (OlcItemAssortmentBL)ObjectFactory.New(typeof(OlcItemAssortmentBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemAssortment).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemAssortmentRules));

        }
    }

    public class OlcItemAssortmentRules : TypedBaseRuleSet<OlcItemAssortment>
    {
        public OlcItemAssortmentRules()
            : base(true)
        {

        }
    }


}
