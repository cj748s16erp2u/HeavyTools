using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodaInt.Base.Bookkeeping.Booking;
using U4Ext.Bank.Base.Bookkeeping.Booking;

namespace eLog.HeavyTools.Bookkeeping.Booking
{
    public class BankBookingRemover3 : BankBookingRemover
    {
        /// <summary>
        /// Banknak beállított árfolyam szabály használata, egyéb árfolyam szabályok törlése
        /// </summary>
        protected override IEnumerable<IDrRuleWithRules> GetDrRules(BookingContext context, DeletionContext deletionContext)
        {
            var rules = base.GetDrRules(context, deletionContext);
            if (rules is null)
            {
                return rules;
            }

            var bankDeletionContext = deletionContext as BankDeletionContext;
            if (bankDeletionContext != null)
            {
                var sourceHead = bankDeletionContext.BankSourceHead;
                if (sourceHead != null)
                {
                    var removeRuleRefType = (int)(sourceHead.BankCmp.Curratetype != 1
                        ? CodaInt.Base.Bookkeeping.Booking.RuleProcessors.RuleRateProcessor.RateRuleRefType.CurStock
                        : CodaInt.Base.Bookkeeping.Booking.RuleProcessors.RuleRateProcessor.RateRuleRefType.CurList);

                    rules = rules
                        .Select(r => new DrRuleWithRules
                        {
                            Master = r.Master,
                            Details = r.Where(r1 => r1.Ruletype != (int)CodaInt.Base.Setup.Rule.OfcRuleRuleType.Rate || r1.Rulereftype != removeRuleRefType).ToList()
                        })
                        .ToList();
                }
            }

            return rules;
        }
    }
}
