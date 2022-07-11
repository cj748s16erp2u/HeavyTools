using eProjectWeb.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnerRules : eLog.Base.Common.TypedBaseRuleSet<OlcPartner>
    {
        public OlcPartnerRules() : base(true, false)
        {
            this.ERules[OlcPartner.FieldPartnid.Name].Mandatory = false;
            this.ERules[OlcPartner.FieldLoyaltydiscpercnt.Name].MinValue = 0;
            this.ERules[OlcPartner.FieldLoyaltydiscpercnt.Name].MaxValue = 100;
        }
    }
}
