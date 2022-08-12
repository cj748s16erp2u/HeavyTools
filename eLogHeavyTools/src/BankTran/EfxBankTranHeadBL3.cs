using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.BankTran
{
    public class EfxBankTranHeadBL3 : U4Ext.Bank.Base.Transaction.EfxBankTranHeadBL
    {
        public override List<string> BankDocProcess(string cmpCode, int? bankId, string fileName, bool moveFile, string interfaceName)
        {
            var elmLevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCode, CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;

            if (!elmLevel.HasValue)
            {
                var msg = eProjectWeb.Framework.Lang.Translator.Translate("$msg_bankdocprocess_missing_companyline", nameof(CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType));
                return new List<string>() { msg };
            }

            var result = base.BankDocProcess(cmpCode, bankId, fileName, moveFile, interfaceName);

            return result;
        }

        public override IEnumerable<Key> FillOpenBalance(IEnumerable<Key> keys, int? dateType)
        {
            var tranHeads = keys.Select(k => U4Ext.Bank.Base.Transaction.EfxBankTranHead.Load(k)).ToArray();
            var cmpCodes = tranHeads.Select(th => (string)th.Cmpcode).Distinct().ToArray();
            var elmLevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodes.First(), CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;

            if (!elmLevel.HasValue)
                return null;

            return base.FillOpenBalance(keys, dateType);
        }

        public override IEnumerable<Key> FillFinDebCredValues(IEnumerable<Key> keys)
        {
            var tranHeads = keys.Select(k => U4Ext.Bank.Base.Transaction.EfxBankTranHead.Load(k)).ToArray();
            var cmpCodes = tranHeads.Select(th => (string)th.Cmpcode).Distinct().ToArray();
            var elmLevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodes.First(), CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;

            if (!elmLevel.HasValue)
                return null;

            return base.FillFinDebCredValues(keys);
        }

        public override void FillOtherDebCredValues(IEnumerable<Key> keys)
        {
            var tranHeads = keys.Select(k => U4Ext.Bank.Base.Transaction.EfxBankTranHead.Load(k)).ToArray();
            var cmpCodes = tranHeads.Select(th => (string)th.Cmpcode).Distinct().ToArray();
            var elmLevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodes.First(), CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;

            if (!elmLevel.HasValue)
                return;

            base.FillOtherDebCredValues(keys);
        }

        public override IEnumerable<Key> FillPostDebCredValues(IEnumerable<Key> keys)
        {
            var tranHeads = keys.Select(k => U4Ext.Bank.Base.Transaction.EfxBankTranHead.Load(k)).ToArray();
            var cmpCodes = tranHeads.Select(th => (string)th.Cmpcode).Distinct().ToArray();
            var elmLevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodes.First(), CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;

            if (!elmLevel.HasValue)
                return null;

            return base.FillPostDebCredValues(keys);
        }

    }
}
