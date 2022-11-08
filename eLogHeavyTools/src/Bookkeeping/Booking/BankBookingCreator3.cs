using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodaInt.Base.AccRual;
using CodaInt.Base.Bookkeeping.Booking;
using CodaInt.Base.Setup.Dr;
using eProjectWeb.Framework.Data;
using U4Ext.Bank.Base.Bookkeeping.Booking;

namespace eLog.HeavyTools.Bookkeeping.Booking
{
    public partial class BankBookingCreator3 : BankBookingCreator
    {
        /// <summary>
        /// <see cref="BankCreationContext"/> letrehozasa, kezdeti beallitasa
        /// </summary>
        protected override T CreateCreationContext<T>(BookingContext bookingContext, ISourceHeadWithHeads data)
        {
            BankCreationContext3 context;

            if (typeof(BankCreationContext3).IsAssignableFrom(typeof(T)))
            {
                context = (BankCreationContext3)(object)base.CreateCreationContext<T>(bookingContext, data);
            }
            else
            {
                context = base.CreateCreationContext<BankCreationContext3>(bookingContext, data);
            }

            return (T)(CreationContext)context;
        }

        /// <summary>
        /// Banknak beállított árfolyam szabály használata, egyéb árfolyam szabályok törlése
        /// </summary>
        protected override IEnumerable<IDrRuleWithRules> GetDrRules(BookingContext context, CreationContext creationContext)
        {
            var rules = base.GetDrRules(context, creationContext);
            if (rules is null)
            {
                return rules;
            }

            var bankCreationContext = creationContext as BankCreationContext;
            if (bankCreationContext != null)
            {
                var sourceHead = bankCreationContext.BankSourceHead;
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

        /// <summary>
        /// El1-8 meghatározása, a kiegyenlített sorok által
        /// </summary>
        protected override void AfterPostProcessDocLines(BankBookingContext bankContext, BankCreationContext bankCreationContext)
        {
            base.AfterPostProcessDocLines(bankContext, bankCreationContext);

            this.SetNonMatchedElements(bankCreationContext);
        }

        /// <summary>
        /// El1-8 meghatározása, a kiegyenlített sorok által
        /// </summary>
        private void SetNonMatchedElements(BankCreationContext bankCreationContext)
        {
            if (bankCreationContext.PostProcessing == CreationContext.PostProcessType.Value)
            {
                var otherLines = bankCreationContext.DocLines
                    .Select(dl => dl.Value)
                    .Where(dl => dl.DrLine.Acctype == (int)DrLineAccType.Optional);
                var matchedLines = otherLines
                    .Where(dl => dl.ElementProcessors?.Any(elInfo => elInfo.MatchableOasDocLine != null) == true);

                var nonMatchedLines = otherLines.Except(matchedLines);
                if (nonMatchedLines.Any())
                {
                    var accountCode = matchedLines
                        .Select(dl => dl.DocLine)
                        .Select(dl => string.Join(".", new string[] { dl.El1, dl.El2, dl.El3, dl.El4, dl.El5, dl.El6, dl.El7, dl.El8 }))
                        .OrderBy(ac => ac, StringComparer.InvariantCultureIgnoreCase)
                        .FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(accountCode))
                    {
                        var accountCodes = accountCode.Split(new[] { '.' }, StringSplitOptions.None);

                        foreach (var docLineInfo in nonMatchedLines)
                        {
                            for (var ruleLevel = 1; ruleLevel <= 8; ruleLevel++)
                            {
                                if (ruleLevel < accountCodes.Length && !string.IsNullOrWhiteSpace(accountCodes[ruleLevel - 1]))
                                {
                                    docLineInfo.DocLine[$"el{ruleLevel}"] = accountCodes[ruleLevel - 1];
                                    bankCreationContext.AddProcessLog($"Setting spec. non matched DocLine's El{ruleLevel}: {accountCodes[ruleLevel - 1]}");
                                }
                                else
                                {
                                    docLineInfo.DocLine[$"el{ruleLevel}"] = null;
                                    bankCreationContext.AddProcessLog($"Setting spec. non matched DocLine's El{ruleLevel}: <NULL>");
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Eltérő partnerkódok esetén, nincs feldolgozás
        /// </summary>
        protected override IEnumerable<BankOasDocLineExt> LoadReferredLines(IEnumerable<BankCreationContext> context, string sqls)
        {
            var list = base.LoadReferredLines(context, sqls);

            var contextWithPartnCodes = context
                .OfType<BankCreationContext3>()
                .SelectMany(c => c.BankSourceLines.Select(sl =>
                {
                    var bankAccNum = new BankAccountNumber(c.DocHead.Cmpcode, sl.Acnum, sl.Acnum);
                    return new
                    {
                        context = c,
                        sourceLine = sl,
                        partnCodes = c.PartnerCodes?
                            .Select(pc => new { partnerCode = pc, bankAccNum = new BankAccountNumber(pc.Cmpcode, pc.Acnum, pc.Iban) })
                            .Where(x => ((IEqualityComparer<BankAccountNumber>)BankAccountNumber.Comparer).Equals(bankAccNum, x.bankAccNum))
                            .Select(x => x.partnerCode)
                            .ToList()
                    };
                }));
            var mergedWithReferredLines = contextWithPartnCodes
                .GroupJoin(list, c => c.sourceLine.Id, l => l.__Id, (c, referredLines) => new
                {
                    c.context,
                    c.partnCodes,
                    list = referredLines
                });
            list = mergedWithReferredLines
                .SelectMany(x => x.partnCodes?.Any() != true
                    ? x.list
                    : x.list
                        .Join(x.partnCodes, l => l.El2, pc => pc.Elmcode, (l, pc) => new { l, exists = pc != null }, StringComparer.InvariantCultureIgnoreCase)
                        .Where(l => l.exists)
                        .Select(l => l.l))
                .Distinct()
                .ToList();

            var groupped = list.GroupBy(l => l.El2, StringComparer.InvariantCultureIgnoreCase);
            if (groupped.Count() > 1)
            {
                return Enumerable.Empty<BankOasDocLineExt>();
            }

            return list;
        }

        /// <summary>
        /// Egyedi partner szűrés
        /// </summary>
        protected override string CreateReferredLoadSql(BankCreationContext context, BankCreationContext.PostProcessBankArgs args, IBankBookingSourceLine sourceLine, DocLineInfo docLine, IEnumerable<ElmBankList> partnerCodes, int matchType, IEnumerable<RuleExtension> matchingFilterSettings)
        {
            var context3 = context as BankCreationContext3;
            if (context3 != null)
            {
                context3.PartnerCodes = partnerCodes
                    .Select(pc => new ElmBankList3
                    {
                        Acnum = pc.Acnum,
                        Cmpcode = pc.Cmpcode,
                        Elmcode = pc.Elmcode,
                        Elmlevel = pc.Elmlevel,
                        Iban = pc.Iban
                    })
                    .ToList();
            }

            var nullPartnerCodes = Enumerable.Empty<ElmBankList>();
            return base.CreateReferredLoadSql(context, args, sourceLine, docLine, nullPartnerCodes, matchType, matchingFilterSettings);
        }

        /// <summary>
        /// Egyedi közlemény kezelés: számlaszámok kezelése, tól-ig tartomány kezelése
        /// </summary>
        protected override Key CreateKeyForSimpleMatchingFilterSettings(CreationContext context, IEnumerable<DocLineInfo> docLines, string sourceValue, RuleExtValue srcValueRule, RuleExtValue fieldRule, IDictionary<string, string> queryTableAlias)
        {
            if (string.Equals(sourceValue, "IBookingSourceLine.note", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.CreateNoteKeyForSimpleMatchingFilterSettings(context, docLines, sourceValue, srcValueRule, fieldRule, queryTableAlias);
            }

            return base.CreateKeyForSimpleMatchingFilterSettings(context, docLines, sourceValue, srcValueRule, fieldRule, queryTableAlias);
        }

        protected const string CONST_SrcNoteValuePattern = "([0-9]{4}\\s*\\/\\s*)?(0?[0-9]{4})(\\s*-\\s*(0?[0-9]{4}))?";
        protected static readonly Regex CONST_SrcNoteValueRegex = new Regex(CONST_SrcNoteValuePattern, RegexOptions.IgnoreCase);

        /// <summary>
        /// Közleményben küldött számlaszámok egyedi logika alapján
        /// </summary>
        private Key CreateNoteKeyForSimpleMatchingFilterSettings(CreationContext context, IEnumerable<DocLineInfo> docLines, string sourceValue, RuleExtValue srcValueRule, RuleExtValue fieldRule, IDictionary<string, string> queryTableAlias)
        {
            var match = CONST_SrcValueRegex.Match(sourceValue);
            if (match.Success)
            {
                if (match.Groups.Count >= 6 && match.Groups[3].Success && !string.IsNullOrWhiteSpace(match.Groups[3].Value) && match.Groups[5].Success && !string.IsNullOrWhiteSpace(match.Groups[5].Value))
                {
                    var field = this.GetFieldForMatchingQuerySQL(fieldRule.Value, queryTableAlias);
                    var value = this.GetValueFromContext<string>(context, docLines, match.Groups[3].Value, match.Groups[5].Value, match.Groups[4].Value);
                    var values = CONST_SrcNoteValueRegex.Matches(value);
                    if (values.Count > 0)
                    {
                        var keys = new List<Key>();

                        foreach (Match m in values)
                        {
                            if (m.Success)
                            {
                                var prefix = m.Groups[1].Value;
                                var lastValue = m.Groups[2].Value;
                                var start = this.GetInvoiceNumber(prefix, lastValue);
                                string end = null;
                                if (m.Groups[3].Success)
                                {
                                    end = m.Groups[4].Value;
                                }

                                var subKey = this.AddKeyByRule(fieldRule.FilterType, srcValueRule.FilterType, fieldRule.MinLength, field, start);
                                if (subKey != null)
                                {
                                    keys.Add(subKey);
                                }

                                if (!string.IsNullOrWhiteSpace(end))
                                {
                                    var nextValue = this.GetNextInvoiceNumber(prefix, ref lastValue, end);
                                    while (!string.IsNullOrWhiteSpace(nextValue))
                                    {
                                        subKey = this.AddKeyByRule(fieldRule.FilterType, srcValueRule.FilterType, fieldRule.MinLength, field, nextValue);
                                        if (subKey != null)
                                        {
                                            keys.Add(subKey);
                                        }

                                        nextValue = this.GetNextInvoiceNumber(prefix, ref lastValue, end);
                                    }
                                }
                            }
                        }

                        if (keys.Count > 0)
                        {
                            return new Key
                            {
                                ["__FILT__"] = Key.OrAtToSql.FromArray(keys.Cast<object>().ToArray())
                            };
                        }

                        return null;
                    }
                }
            }

            // base logika alkalmazasa
            return base.CreateKeyForSimpleMatchingFilterSettings(context, docLines, sourceValue, srcValueRule, fieldRule, queryTableAlias);
        }

        /// <summary>
        /// Számlaszám létrehozása
        /// </summary>
        private string GetInvoiceNumber(string prefix, string invoiceNum)
        {
            return $"{prefix}{invoiceNum}";
        }

        /// <summary>
        /// Következő számlaszám meghatározása
        /// </summary>
        private string GetNextInvoiceNumber(string prefix, ref string lastValue, string end)
        {
            if (!string.IsNullOrWhiteSpace(lastValue))
            {
                var iLastValue = int.Parse(lastValue);
                var iEnd = int.Parse(end);
                if (iLastValue < iEnd)
                {
                    lastValue = $"{++iLastValue}".PadLeft(lastValue.Length, '0');
                    return this.GetInvoiceNumber(prefix, lastValue);
                }
            }

            return null;
        }
    }
}
