using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.Finance.Coda.Common.Element;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.Xml;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmpCustEditTab : eProjectWeb.Framework.UI.Templates.EditTabTemplate1<OlcPartnCmp, OlcPartnCmpRules, OlcPartnCmpCustBL>
    {
        protected LayoutTable EditGroup2;
        protected LayoutTable EditGroup3;
        protected LayoutTable EditGroup4;
        protected LayoutTable EditGroup5;

        #region RelatedAccno
        protected Control ctrlRelAccnoEl1Code;
        protected Control ctrlRelAccnoEl1Name;
        protected Control ctrlRelAccnoEl2Code;
        protected Control ctrlRelAccnoEl2Name;
        protected Control ctrlRelAccnoEl3Code;
        protected Control ctrlRelAccnoEl3Name;
        protected Control ctrlRelAccnoEl4Code;
        protected Control ctrlRelAccnoEl4Name;
        protected Control ctrlRelAccnoEl5Code;
        protected Control ctrlRelAccnoEl5Name;
        protected Control ctrlRelAccnoEl6Code;
        protected Control ctrlRelAccnoEl6Name;
        protected Control ctrlRelAccnoEl7Code;
        protected Control ctrlRelAccnoEl7Name;
        protected Control ctrlRelAccnoEl8Code;
        protected Control ctrlRelAccnoEl8Name;
        #endregion

        #region ScontoBelowAccno
        protected Control ctrlScontBelowAccnoEl1Code;
        protected Control ctrlScontBelowAccnoEl1Name;
        protected Control ctrlScontBelowAccnoEl2Code;
        protected Control ctrlScontBelowAccnoEl2Name;
        protected Control ctrlScontBelowAccnoEl3Code;
        protected Control ctrlScontBelowAccnoEl3Name;
        protected Control ctrlScontBelowAccnoEl4Code;
        protected Control ctrlScontBelowAccnoEl4Name;
        protected Control ctrlScontBelowAccnoEl5Code;
        protected Control ctrlScontBelowAccnoEl5Name;
        protected Control ctrlScontBelowAccnoEl6Code;
        protected Control ctrlScontBelowAccnoEl6Name;
        protected Control ctrlScontBelowAccnoEl7Code;
        protected Control ctrlScontBelowAccnoEl7Name;
        protected Control ctrlScontBelowAccnoEl8Code;
        protected Control ctrlScontBelowAccnoEl8Name;
        #endregion

        #region ScontoAboveAccno
        protected Control ctrlScontAboveAccnoEl1Code;
        protected Control ctrlScontAboveAccnoEl1Name;
        protected Control ctrlScontAboveAccnoEl2Code;
        protected Control ctrlScontAboveAccnoEl2Name;
        protected Control ctrlScontAboveAccnoEl3Code;
        protected Control ctrlScontAboveAccnoEl3Name;
        protected Control ctrlScontAboveAccnoEl4Code;
        protected Control ctrlScontAboveAccnoEl4Name;
        protected Control ctrlScontAboveAccnoEl5Code;
        protected Control ctrlScontAboveAccnoEl5Name;
        protected Control ctrlScontAboveAccnoEl6Code;
        protected Control ctrlScontAboveAccnoEl6Name;
        protected Control ctrlScontAboveAccnoEl7Code;
        protected Control ctrlScontAboveAccnoEl7Name;
        protected Control ctrlScontAboveAccnoEl8Code;
        protected Control ctrlScontAboveAccnoEl8Name;
        #endregion

        #region TransactionFeeAccno
        protected Control ctrlTransFeeAccnoEl1Code;
        protected Control ctrlTransFeeAccnoEl1Name;
        protected Control ctrlTransFeeAccnoEl2Code;
        protected Control ctrlTransFeeAccnoEl2Name;
        protected Control ctrlTransFeeAccnoEl3Code;
        protected Control ctrlTransFeeAccnoEl3Name;
        protected Control ctrlTransFeeAccnoEl4Code;
        protected Control ctrlTransFeeAccnoEl4Name;
        protected Control ctrlTransFeeAccnoEl5Code;
        protected Control ctrlTransFeeAccnoEl5Name;
        protected Control ctrlTransFeeAccnoEl6Code;
        protected Control ctrlTransFeeAccnoEl6Name;
        protected Control ctrlTransFeeAccnoEl7Code;
        protected Control ctrlTransFeeAccnoEl7Name;
        protected Control ctrlTransFeeAccnoEl8Code;
        protected Control ctrlTransFeeAccnoEl8Name;
        #endregion

        #region ElmLevel
        protected Control ctrlEl1Level;
        protected Control ctrlEl2Level;
        protected Control ctrlEl3Level;
        protected Control ctrlEl4Level;
        protected Control ctrlEl5Level;
        protected Control ctrlEl6Level;
        protected Control ctrlEl7Level;
        protected Control ctrlEl8Level;
        #endregion

        #region ElmPrefix
        protected Control ctrlEl1Prefix;
        protected Control ctrlEl2Prefix;
        protected Control ctrlEl3Prefix;
        protected Control ctrlEl4Prefix;
        protected Control ctrlEl5Prefix;
        protected Control ctrlEl6Prefix;
        protected Control ctrlEl7Prefix;
        protected Control ctrlEl8Prefix;
        #endregion

        protected Control ctrlcmpid_grp2;
        protected Control ctrlcmpid_grp3;
        protected Control ctrlcmpid_grp4;
        protected Control ctrlcmpid_grp5;

        protected Control ctrlRelatedaccno;
        protected Control ctrlScontobelowaccno;
        protected Control ctrlScontoaboveaccno;
        protected Control ctrlTransactionfeeaccno;

        public static OlcPartnCmpCustEditTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup custom)
        {
            var t = ObjectFactory.New<OlcPartnCmpCustEditTab>();
            t.Initialize("PartnCmpCust", custom);
            return t;
        }

        protected OlcPartnCmpCustEditTab() : base()
        {
        }

        protected override void CreateBase()
        {
            base.CreateBase();

            #region RelatedAccno
            EditGroup2 = this["EditGroup2"] as LayoutTable;
            if (EditGroup2 != null)
            {
                ctrlRelatedaccno= EditGroup2["relatedaccno"];
                ctrlcmpid_grp2 = EditGroup2["cmpid_grp2"];
                ctrlRelAccnoEl1Code = EditGroup2["relaccno_el1_code"];
                ctrlRelAccnoEl1Name = EditGroup2["relaccno_el1_name"];
                ctrlRelAccnoEl2Code = EditGroup2["relaccno_el2_code"];
                ctrlRelAccnoEl2Name = EditGroup2["relaccno_el2_name"];
                ctrlRelAccnoEl3Code = EditGroup2["relaccno_el3_code"];
                ctrlRelAccnoEl3Name = EditGroup2["relaccno_el3_name"];
                ctrlRelAccnoEl4Code = EditGroup2["relaccno_el4_code"];
                ctrlRelAccnoEl4Name = EditGroup2["relaccno_el4_name"];
                ctrlRelAccnoEl5Code = EditGroup2["relaccno_el5_code"];
                ctrlRelAccnoEl5Name = EditGroup2["relaccno_el5_name"];
                ctrlRelAccnoEl6Code = EditGroup2["relaccno_el6_code"];
                ctrlRelAccnoEl6Name = EditGroup2["relaccno_el6_name"];
                ctrlRelAccnoEl7Code = EditGroup2["relaccno_el7_code"];
                ctrlRelAccnoEl7Name = EditGroup2["relaccno_el7_name"];
                ctrlRelAccnoEl8Code = EditGroup2["relaccno_el8_code"];
                ctrlRelAccnoEl8Name = EditGroup2["relaccno_el8_name"];

            }
            #endregion

            #region ScontoBelowAccno
            EditGroup3 = this["EditGroup3"] as LayoutTable;
            if (EditGroup3 != null)
            {
                ctrlScontobelowaccno = EditGroup3["scontobelowaccno"];
                ctrlcmpid_grp3 = EditGroup3["cmpid_grp3"];
                ctrlScontBelowAccnoEl1Code = EditGroup3["scbaccno_el1_code"];
                ctrlScontBelowAccnoEl1Name = EditGroup3["scbaccno_el1_name"];
                ctrlScontBelowAccnoEl2Code = EditGroup3["scbaccno_el2_code"];
                ctrlScontBelowAccnoEl2Name = EditGroup3["scbaccno_el2_name"];
                ctrlScontBelowAccnoEl3Code = EditGroup3["scbaccno_el3_code"];
                ctrlScontBelowAccnoEl3Name = EditGroup3["scbaccno_el3_name"];
                ctrlScontBelowAccnoEl4Code = EditGroup3["scbaccno_el4_code"];
                ctrlScontBelowAccnoEl4Name = EditGroup3["scbaccno_el4_name"];
                ctrlScontBelowAccnoEl5Code = EditGroup3["scbaccno_el5_code"];
                ctrlScontBelowAccnoEl5Name = EditGroup3["scbaccno_el5_name"];
                ctrlScontBelowAccnoEl6Code = EditGroup3["scbaccno_el6_code"];
                ctrlScontBelowAccnoEl6Name = EditGroup3["scbaccno_el6_name"];
                ctrlScontBelowAccnoEl7Code = EditGroup3["scbaccno_el7_code"];
                ctrlScontBelowAccnoEl7Name = EditGroup3["scbaccno_el7_name"];
                ctrlScontBelowAccnoEl8Code = EditGroup3["scbaccno_el8_code"];
                ctrlScontBelowAccnoEl8Name = EditGroup3["scbaccno_el8_name"];
            }
            #endregion

            #region ScontoAboveAccno
            EditGroup4 = this["EditGroup4"] as LayoutTable;
            if (EditGroup4 != null)
            {
                ctrlScontoaboveaccno = EditGroup4["scontoaboveaccno"];
                ctrlcmpid_grp4 = EditGroup4["cmpid_grp4"];
                ctrlScontAboveAccnoEl1Code = EditGroup4["scaaccno_el1_code"];
                ctrlScontAboveAccnoEl1Name = EditGroup4["scaaccno_el1_name"];
                ctrlScontAboveAccnoEl2Code = EditGroup4["scaaccno_el2_code"];
                ctrlScontAboveAccnoEl2Name = EditGroup4["scaaccno_el2_name"];
                ctrlScontAboveAccnoEl3Code = EditGroup4["scaaccno_el3_code"];
                ctrlScontAboveAccnoEl3Name = EditGroup4["scaaccno_el3_name"];
                ctrlScontAboveAccnoEl4Code = EditGroup4["scaaccno_el4_code"];
                ctrlScontAboveAccnoEl4Name = EditGroup4["scaaccno_el4_name"];
                ctrlScontAboveAccnoEl5Code = EditGroup4["scaaccno_el5_code"];
                ctrlScontAboveAccnoEl5Name = EditGroup4["scaaccno_el5_name"];
                ctrlScontAboveAccnoEl6Code = EditGroup4["scaaccno_el6_code"];
                ctrlScontAboveAccnoEl6Name = EditGroup4["scaaccno_el6_name"];
                ctrlScontAboveAccnoEl7Code = EditGroup4["scaaccno_el7_code"];
                ctrlScontAboveAccnoEl7Name = EditGroup4["scaaccno_el7_name"];
                ctrlScontAboveAccnoEl8Code = EditGroup4["scaaccno_el8_code"];
                ctrlScontAboveAccnoEl8Name = EditGroup4["scaaccno_el8_name"];
            }
            #endregion

            #region TransactionFeeAccno
            EditGroup5 = this["EditGroup5"] as LayoutTable;
            if (EditGroup5 != null)
            {
                ctrlTransactionfeeaccno = EditGroup5["transactionfeeaccno"];
                ctrlcmpid_grp5 = EditGroup5["cmpid_grp5"];
                ctrlTransFeeAccnoEl1Code = EditGroup5["transaccno_el1_code"];
                ctrlTransFeeAccnoEl1Name = EditGroup5["transaccno_el1_name"];
                ctrlTransFeeAccnoEl2Code = EditGroup5["transaccno_el2_code"];
                ctrlTransFeeAccnoEl2Name = EditGroup5["transaccno_el2_name"];
                ctrlTransFeeAccnoEl3Code = EditGroup5["transaccno_el3_code"];
                ctrlTransFeeAccnoEl3Name = EditGroup5["transaccno_el3_name"];
                ctrlTransFeeAccnoEl4Code = EditGroup5["transaccno_el4_code"];
                ctrlTransFeeAccnoEl4Name = EditGroup5["transaccno_el4_name"];
                ctrlTransFeeAccnoEl5Code = EditGroup5["transaccno_el5_code"];
                ctrlTransFeeAccnoEl5Name = EditGroup5["transaccno_el5_name"];
                ctrlTransFeeAccnoEl6Code = EditGroup5["transaccno_el6_code"];
                ctrlTransFeeAccnoEl6Name = EditGroup5["transaccno_el6_name"];
                ctrlTransFeeAccnoEl7Code = EditGroup5["transaccno_el7_code"];
                ctrlTransFeeAccnoEl7Name = EditGroup5["transaccno_el7_name"];
                ctrlTransFeeAccnoEl8Code = EditGroup5["transaccno_el8_code"];
                ctrlTransFeeAccnoEl8Name = EditGroup5["transaccno_el8_name"];
            }
            #endregion

            #region ElmLevel
            ctrlEl1Level = EditGroup1["el1level"];
            ctrlEl2Level = EditGroup1["el2level"];
            ctrlEl3Level = EditGroup1["el3level"];
            ctrlEl4Level = EditGroup1["el4level"];
            ctrlEl5Level = EditGroup1["el5level"];
            ctrlEl6Level = EditGroup1["el6level"];
            ctrlEl7Level = EditGroup1["el7level"];
            ctrlEl8Level = EditGroup1["el8level"];
            #endregion

            #region ElmPrefix
            ctrlEl1Prefix = EditGroup1["el1prefix"];
            ctrlEl2Prefix = EditGroup1["el2prefix"];
            ctrlEl3Prefix = EditGroup1["el3prefix"];
            ctrlEl4Prefix = EditGroup1["el4prefix"];
            ctrlEl5Prefix = EditGroup1["el5prefix"];
            ctrlEl6Prefix = EditGroup1["el6prefix"];
            ctrlEl7Prefix = EditGroup1["el7prefix"];
            ctrlEl8Prefix = EditGroup1["el8prefix"];
            #endregion

        }

        protected override OlcPartnCmp DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);
            if (e == null)
            {
                return null;
            }

            eLog.Base.Masters.Partner.Partner p = eLog.Base.Masters.Partner.Partner.Load(e.Partnid);

            SetupElLevel(args, ctrlEl1Level, 1);
            SetupElLevel(args, ctrlEl2Level, 2);
            SetupElLevel(args, ctrlEl3Level, 3);
            SetupElLevel(args, ctrlEl4Level, 4);
            SetupElLevel(args, ctrlEl5Level, 5);
            SetupElLevel(args, ctrlEl6Level, 6);
            SetupElLevel(args, ctrlEl7Level, 7);
            SetupElLevel(args, ctrlEl8Level, 8);

            if (args.ActionID != ActionID.New)
            {
                //Key key = new Key(new Field[] { OlcPartnCmp.FieldPartnid, OlcPartnCmp.FieldCmpid },
                //    new object[] { e.PK["partnid"], e.PK["cmpid"] });
                var olcPartnCmp = OlcPartnCmp.Load(e.PK);
                if (olcPartnCmp != null)
                {
                    ctrlcmpid_grp2.SetValue(olcPartnCmp.Cmpid);
                    ctrlcmpid_grp3.SetValue(olcPartnCmp.Cmpid);
                    ctrlcmpid_grp4.SetValue(olcPartnCmp.Cmpid);
                    ctrlcmpid_grp5.SetValue(olcPartnCmp.Cmpid);

                    // RelatedAccno
                    ctrlRelatedaccno.SetValue(olcPartnCmp.Relatedaccno.ToStr());
                    SetupSelectors(args, EditGroup2, olcPartnCmp.Relatedaccno.ToStr(), "relaccno", false);
                    // ScontoBelowAccno
                    ctrlScontobelowaccno.SetValue(olcPartnCmp.Scontobelowaccno.ToStr());
                    SetupSelectors(args, EditGroup3, olcPartnCmp.Scontobelowaccno.ToStr(), "scbaccno", false);
                    // ScontoAboveAccno
                    ctrlScontoaboveaccno.SetValue(olcPartnCmp.Scontoaboveaccno.ToStr());
                    SetupSelectors(args, EditGroup4, olcPartnCmp.Scontoaboveaccno.ToStr(), "scaaccno", false);
                    // TransactionFeeAccno
                    ctrlTransactionfeeaccno.SetValue(olcPartnCmp.Transactionfeeaccno.ToStr());
                    SetupSelectors(args, EditGroup5, olcPartnCmp.Transactionfeeaccno.ToStr(), "transaccno", false);
                }
            }

            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, OlcPartnCmp pc)
        {
            var map = base.SaveControlsToBLObjectMap(args, pc);

            var relAcccode = GetAcccode(EditGroup2, "relaccno");
            pc.Relatedaccno = string.IsNullOrEmpty(relAcccode.Replace(".","")) ? "" : relAcccode.ToString();
            var scontBelowAcccode = GetAcccode(EditGroup3, "scbaccno");
            pc.Scontobelowaccno = string.IsNullOrEmpty(scontBelowAcccode.Replace(".", "")) ? "" : scontBelowAcccode.ToString();
            var scontAboveAcccode = GetAcccode(EditGroup4, "scaaccno");
            pc.Scontoaboveaccno = string.IsNullOrEmpty(scontAboveAcccode.Replace(".", "")) ? "" : scontAboveAcccode.ToString();
            var transAcccode = GetAcccode(EditGroup5, "transaccno");
            pc.Transactionfeeaccno = string.IsNullOrEmpty(transAcccode.Replace(".", "")) ? "" : transAcccode.ToString();

            return map;
        }

        private void SetupElLevel(PageUpdateArgs args, Control ctrlLevel, int level)
        {
            if (ctrlLevel != null)
            {
                ctrlLevel.Value = level;
                ctrlLevel.FireEvent(Control.Event_OnChanged, args, false);
            }
        }

        private void SetupSelectors(PageUpdateArgs args, LayoutTable l, string def, string ctrlDef, bool setVisibility)
        {
            if (!string.IsNullOrEmpty(def))
            {
                var maskParts = def.Split('.');

                if (l != null)
                {
                    for (int level = 1; level <= 8; level++)
                    {
                        var ctrlCode = l[ctrlDef.ToString() + "_el" + level.ToString() + "_code"];
                        var ctrlName = l[ctrlDef.ToString() + "_el" + level.ToString() + "_name"];
                        var ctrlPrefix = EditGroup1["el" + level.ToString() + "prefix"];

                        if (ctrlCode != null && ctrlName != null && ctrlPrefix != null)
                        {
                            SetupElSelector(args, maskParts, level - 1, ctrlCode, ctrlName, ctrlPrefix, setVisibility);
                        }
                    }
                }
            }
        }

        protected virtual void SetupElSelector(PageUpdateArgs args, string[] maskParts, int index, Control ctrlCode, Control ctrlName, Control ctrlPrefix, bool setVisibility)
        {
            bool justPrefix;
            if (ctrlPrefix != null && ctrlCode != null)
            {
                var prefix = PrepareElPrefix(maskParts, index, out justPrefix);
                ctrlPrefix.Value = prefix;
                ctrlPrefix.FireEvent(Control.Event_OnChanged, args, false);
                if (setVisibility)
                {
                    ctrlCode.Disabled = !justPrefix || args.ActionID == ActionID.View;
                }
                ctrlCode.Value = !justPrefix ? prefix : null;
                ctrlCode.FireEvent(Control.Event_OnChanged, args, false);
                if (ctrlName != null && setVisibility)
                {
                    ctrlName.Disabled = ctrlCode.Disabled;
                }
            }
        }

        protected virtual string PrepareElPrefix(string[] prefixes, int index, out bool justPrefix)
        {
            var prefix = prefixes != null && index >= 0 && index < prefixes.Length ? prefixes[index] : null;
            var ret = (prefix ?? "").Replace("*", "");
            justPrefix = !string.Equals(ret, prefix, StringComparison.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(prefix))
                justPrefix = true;
            return ret;
        }

        protected virtual string GetAcccode(LayoutTable l, string ctrlDef)
        {
            var sep = "";
            var acccode = "";

            if (l != null)
            {
                for (int level = 1; level <= 8; level++)
                {
                    var ctrl = l[ctrlDef.ToString() + "_el" + level.ToString() + "_code"];
                    if (ctrl != null)
                    {
                        acccode += GetAcccodeParts(ctrl, ref sep);
                    }
                }
            }

            return acccode;
        }

        protected virtual string GetAcccodeParts(Control ctrlCode, ref string sep)
        {
            var ret = "";
            if (ctrlCode != null && ctrlCode.Visible)
            {
                var v = ConvertUtils.ToString(ctrlCode.Value);
                if (!string.IsNullOrEmpty(v))
                {
                    ret = sep + v;
                    sep = ".";
                }
                else
                {
                    ret = sep;
                    sep = ".";
                }
            }

            return ret;
        }

    }
}
