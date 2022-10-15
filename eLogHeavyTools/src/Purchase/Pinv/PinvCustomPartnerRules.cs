using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.Base.Sales.SContract;
using eLog.Base.Setup.Country;
using eLog.HeavyTools.Common;
using eProjectWeb.Framework.Rules;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvCustomPartnerRules : Base.Common.TypedBaseRuleSet<Base.Common.XCustValue.XCVal>
    {
        public PinvCustomPartnerRules() : base(true, false)
        {
            this.ERules[Base.Common.XCustValue.XCVal.FieldXcvcode.Name].Mandatory = false;

            this.AddCustomRule(this.CheckCustomPartner);
            this.AddCustomRule(this.CheckCustomPartnerAccno);
        }

        private void CheckCustomPartner(RuleValidateContext ctx, Base.Common.XCustValue.XCVal value)
        {
            string partnName = null, countryCode = null, postCode = null, city = null, add02 = null;
            if (!string.IsNullOrWhiteSpace(value.Xmldata))
            {
                var e = System.Xml.Linq.XElement.Parse(value.Xmldata);
                var partner = e.Element("Seller");
                if (partner != null)
                {
                    partnName = partner.Element("partnname")?.Value;
                    countryCode = partner.Element("selleraddrcountrycode")?.Value;
                    postCode = partner.Element("selleraddrpostcode")?.Value;
                    city = partner.Element("selleraddrcity")?.Value;
                    add02 = partner.Element("selleradd02")?.Value;
                }
            }

            this.CheckMandatory(ctx, partnName, "partnname");
            this.CheckMandatory(ctx, countryCode, "countryid");
            this.CheckMandatory(ctx, postCode, "postcode");
            this.CheckMandatory(ctx, city, "add01");
            this.CheckMandatory(ctx, add02, "add02");
        }

        private void CheckCustomPartnerAccno(RuleValidateContext ctx, Base.Common.XCustValue.XCVal value)
        {
            string countryCode = null, bankAccountNo = null;

            if (value == null)
                return;

            if (!string.IsNullOrWhiteSpace(value.Xmldata))
            {
                var e = System.Xml.Linq.XElement.Parse(value.Xmldata);
                var partner = e.Element("Seller");
                if (partner != null)
                {
                    countryCode = partner.Element("selleraddrcountrycode")?.Value;
                    bankAccountNo = partner.Element("sellerbankaccno")?.Value;
                }

                var c = new CheckBankAccountNo();
                if(!c.ValidateBankAccountNo(countryCode, bankAccountNo))
                    ctx.AddError("$rule_partnsellerbankaccno_checksum_error");
            }
        }

    }
}
