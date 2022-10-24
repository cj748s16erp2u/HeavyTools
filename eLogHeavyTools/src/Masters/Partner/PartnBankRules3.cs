using eLog.Base.Common;
using eLog.Base.Masters.Partner;
using eLog.Base.Setup.Common;
using eProjectWeb.Framework.Rules;
using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Common;
using CodaInt.Base.Common.Mam;
using System.Net.NetworkInformation;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnBankRules3 : PartnBankRules
    {
        protected PartnBankRules3() : base()
        {
            AddCustomRule(PartnerBankAccnoRule3);
        }

        private void PartnerBankAccnoRule3(RuleValidateContext ctx, PartnBank value)
        {
            string countryCode = null, bankAccountNo = null;

            if (value is PartnBank)
            {
                PartnBank pb = (PartnBank)value;
                if (pb == null)
                    return;

                if (pb.State == eProjectWeb.Framework.Data.DataRowState.Modified ||
                    pb.State == eProjectWeb.Framework.Data.DataRowState.Added)
                {
                    countryCode = ConvertUtils.ToString(pb.Countryid);
                    bankAccountNo = ConvertUtils.ToString(pb.Accno);

                    if (!string.IsNullOrEmpty(countryCode) && !string.IsNullOrEmpty(bankAccountNo))
                    {
                        var c = new CheckBankAccountNo();
                        if (!c.ValidateBankAccountNo(countryCode, bankAccountNo))
                            ctx.AddErrorField(PartnBank.FieldAccno, "$rule_partnbankaccno_checksum_error");
                    }
                }
            }
        }

    }
}
