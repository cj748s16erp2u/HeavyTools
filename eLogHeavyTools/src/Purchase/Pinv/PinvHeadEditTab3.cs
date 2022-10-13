using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.Xml;
using eLog.Base.Purchase.Pinv;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvHeadEditTab3 : CodaInt.Base.Purchase.Pinv.PinvHeadEditTab2
    {
        protected Control m_add02;
        protected List<Control> m_ctrlsCustomPartnerData;
        protected List<Control> m_ctrlsCustomPartnerAddrParts;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.m_add02 = this.EditGroup1["add02"];
            this.m_add02.SetOnChangedWhenExists(new ControlEvent(this.OnAdd02Changed));
            this.m_ctrlsCustomPartnerData = this.EditGroup1.ControlArray.Where(c => c.CustomData != null && c.CustomData.StartsWith("custompartner")).GroupBy(c => c.DataField).Select(g => g.FirstOrDefault()).ToList();
            this.m_ctrlsCustomPartnerData.ForEach(c => c.Visible = false);
            var addrParts = "district-place-placetype-hnum-building-stairway-floor-door".Split('-');
            this.m_ctrlsCustomPartnerAddrParts = this.m_ctrlsCustomPartnerData.Where(c => addrParts.Contains(c.DataField)).ToList();
            this.m_ctrlsCustomPartnerAddrParts.ForEach(c => c.SetOnChanged(new ControlEvent(this.OnCustomAddrPartsChanged)));
        }

        protected override PinvHead DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);

            if (e == null)
            {
                return e;
            }

            // egyedi partner adatok visszatoltese az xmldata mezobol
            this.RestoreCustomPartnerData(e);

            return e;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, PinvHead e)
        {
            var map = base.SaveControlsToBLObjectMap(args, e);

            // egyedi partner adatok tarolasa az xmldata mezoben
            var xcVal = this.StoreCustomPartnerData(args, e);
            if (xcVal != null)
            {
                map.Add(xcVal);
            }

            return map;
        }

        protected string CustomPartnerDataField(Control c)
        {
            if (c.CustomData == null || !c.CustomData.StartsWith("custompartner:"))
            {
                return null;
            }

            var field = c.CustomData.Remove(0, 14);
            return field;
        }

        // egyedi partner adatok betoltese az xmldata mezobol
        protected void RestoreCustomPartnerData(PinvHead pinvHead)
        {
            if (pinvHead?.Partnid != null && Common.PurchaseSqlFunctions3.IsPinvCustomPartner(pinvHead.Partnid))
            {
                var bl = PinvHeadBL3.New();
                var xcVal = bl.LoadCustomPartnerData(pinvHead.Pinvid);

                var xmlData = new XmlManiputeStr(() => xcVal?.Xmldata, value => xcVal.Xmldata = value, "pinvhead");
                var xmlPartner = xmlData.GetX("Seller");

                foreach (var c in this.m_ctrlsCustomPartnerData)
                {
                    var field = this.CustomPartnerDataField(c);
                    if (string.IsNullOrWhiteSpace(field))
                    {
                        continue;
                    }

                    var oldValue = (string)xmlPartner?.Element(field);
                    var newValue = c.GetStringValue();
                    if (!Utils.Equals(oldValue, newValue))
                    {
                        c.Value = oldValue;
                    }
                }
            }
        }

        // egyedi partner adatok tarolasa az xmldata mezoben
        protected Base.Common.XCustValue.XCVal StoreCustomPartnerData(PageUpdateArgs args, PinvHead pinvHead)
        {
            if (pinvHead.Partnid.HasValue)
            {
                var bl = PinvHeadBL3.New();
                var xcVal = bl.LoadCustomPartnerData(pinvHead.Pinvid);

                if (Common.PurchaseSqlFunctions3.IsPinvCustomPartner(pinvHead.Partnid))
                {
                    if (xcVal == null)
                    {
                        xcVal = Base.Common.XCustValue.XCVal.CreateNew();
                    }

                    var xmlData = new XmlManiputeStr(() => xcVal.Xmldata, value => xcVal.Xmldata = value, "pinvhead");
                    var xmlPartner = xmlData.GetX("Seller") ?? new System.Xml.Linq.XElement("Seller");

                    foreach (var c in this.m_ctrlsCustomPartnerData)
                    {
                        var field = this.CustomPartnerDataField(c);
                        if (string.IsNullOrWhiteSpace(field))
                        {
                            continue;
                        }

                        if (!c.Visible)
                        {
                            continue;
                        }

                        var oldValue = (string)xmlPartner.Element(field);
                        var newValue = c.GetStringValue();
                        if (!Utils.Equals(oldValue, newValue))
                        {
                            var xmlField = xmlPartner.Element(field);
                            xmlField?.Remove();
                            xmlPartner.Add(new System.Xml.Linq.XElement(field, newValue));
                        }
                    }

                    xmlData.SetX(xmlPartner);
                }
                else if (xcVal != null)
                {
                    var xmlData = new XmlManiputeStr(() => xcVal.Xmldata, value => xcVal.Xmldata = value, "pinvhead");
                    var xmlPartner = xmlData.GetX("Seller");
                    xmlPartner?.Remove();

                    if (!xmlData.FullElement.HasAttributes && !xmlData.FullElement.HasElements)
                    {
                        xcVal.State = DataRowState.Deleted;
                    }
                }

                return xcVal;
            }

            return null;
        }

        protected void OnAdd02Changed(PageUpdateArgs args)
        {
            if (this.PageLoadDataBinding || args.ActionID == ActionID.View || this.m_add02.Disabled)
            {
                return;
            }

            var add02 = this.m_add02.GetStringValue();

            var bl = Base.Masters.Partner.PartnAddrBL.New();
            Dictionary<string, string> dict;
            using (new eProjectWeb.Framework.Lang.NS(typeof(Base.Masters.Partner.Partner).Namespace))
            {
                dict = bl.SplitAdd02(add02);
            }

            foreach (var c in this.m_ctrlsCustomPartnerAddrParts)
            {
                string s;
                if (dict.TryGetValue(c.DataField, out s))
                {
                    c.Value = s;
                }
            }
        }

        protected void OnCustomAddrPartsChanged(PageUpdateArgs args)
        {
            if (this.PageLoadDataBinding || args.ActionID == ActionID.View || args.Control.Disabled)
            {
                return;
            }

            var addrParts = this.m_ctrlsCustomPartnerAddrParts.ToDictionary(c => c.DataField, c => c.Value);
            var bl = Base.Masters.Partner.PartnAddrBL.New();
            string addr02;
            using (new eProjectWeb.Framework.Lang.NS(typeof(Base.Masters.Partner.Partner).Namespace))
            {
                addr02 = bl.GetAdd02FromParts(addrParts);
            }

            this.m_add02.SetValue(addr02);
        }

        protected PinvHeadCorrType? GetPinvCorrType(PageUpdateArgs args)
        {
            var pinvHeadCorrType = this.Action2CorrType(OrigActionID ?? args.ActionID);
            if (pinvHeadCorrType == 0)
            {
                return null;
            }

            return pinvHeadCorrType;
        }

        protected void SetCustomPartnerDataVisibility(PageUpdateArgs args, int? partnId)
        {
            bool visible = Common.PurchaseSqlFunctions3.IsPinvCustomPartner(partnId);
            m_ctrlsCustomPartnerData.ForEach(c => c.Visible = visible);

            //var o = m_ctrlsCustomPartnerData.Find(c => c.Field == "vatnum");
            //if (o != null)
            //    btnVatnumCheck.Visible = visible;
            //else
            //    btnVatnumCheck.Visible = false;

            if (visible && (args.ActionID == ActionID.New || args.ActionID == ActionID.Modify))
            {
                bool partsEnabled = Base.Common.SysvalCache.GetValueOrDefault("partnaddrparts").ValueInt != 0;

                if (this.GetPinvCorrType(args).GetValueOrDefault(PinvHeadCorrType.Normal) != PinvHeadCorrType.Normal)
                {
                    // storno/helyesbitonel nem modosithatoak az adatok
                    //m_add02.SetDisabled(true);
                    m_ctrlsCustomPartnerData.ForEach(c => c.Disabled = true);
                }
                else
                {
                    m_add02.SetDisabled(partsEnabled);
                    m_ctrlsCustomPartnerAddrParts.ForEach(c => c.Disabled = !partsEnabled);
                }
            }
        }

        protected override void OnPartnerChanged(PageUpdateArgs args)
        {
            base.OnPartnerChanged(args);

            var partnId = this.m_partnCode.GetValue<int>();
            this.SetCustomPartnerDataVisibility(args, partnId);
        }

    }
}
