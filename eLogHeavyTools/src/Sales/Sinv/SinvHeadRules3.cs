using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework.Data;
using System.Xml.Linq;
using eLog.Base.Sales.Common;
using eLog.Base.Sales.Sinv;
using eLog.HeavyTools.Common;

namespace eLog.HeavyTools.Sales.Sinv
{
    public class SinvHeadRules3 : SinvHeadRules
    {
        protected SinvHeadRules3()
        {
            this.AddCustomRule(this.CheckCustomPartnerAccno);
        }

        protected void CheckCustomPartnerAccno(RuleValidateContext ctx, SinvHead sinvHead)
        {
            string countryCode = null, bankAccountNo = null;

            if (SalesSqlFunctions.IsSinvCustomPartner(sinvHead.Partnid) && sinvHead.Corrtype != 2 && (sinvHead.State != DataRowState.Added || sinvHead.Sinvstat != 5))
            {
                if (!sinvHead.Xmldata.IsNullOrEmpty())
                {
                    XElement partner = XElement.Parse(sinvHead.Xmldata);
                    countryCode = partner.Element("buyaddrcountrycode")?.Value;
                    bankAccountNo = partner.Element("buyerbankaccno")?.Value;
                }

                var c = new CheckBankAccountNo();
                if (!c.ValidateBankAccountNo(countryCode, bankAccountNo))
                    ctx.AddError("$rule_partnsbuyerbankaccno_checksum_error");
            }
        }

    }
}
