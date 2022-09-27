using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    class OlcItemRules : eLog.Base.Common.TypedBaseRuleSet<OlcItem>
    {
        public OlcItemRules()
            : base(true)
        {
            ERules[OlcItem.FieldItemid.Name].Mandatory = false;  // PK, FK
            ERules[OlcItem.FieldColortype1.Name].Mandatory = false;
            AddCustomRule(Colortype1Mandatory);
        }

        private void Colortype1Mandatory(RuleValidateContext ctx, OlcItem value)
        {
            if (value.Colortype1.HasValue)
            {
                if (string.IsNullOrEmpty(value.Colorname))
                {
                    ctx.AddErrorField(OlcItem.FieldColortype1, "$missingcolortype1");
                }
            }
        }
    }
}
 