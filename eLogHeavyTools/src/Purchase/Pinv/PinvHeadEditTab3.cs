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
using CodaInt.Base.Bookkeeping.Common;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Sinv;
using eLog.Base.Setup.Country;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvHeadEditTab3 : CodaInt.Base.Purchase.Pinv.PinvHeadEditTab2
    {
        protected Control m_add02;
        protected List<Control> m_ctrlsCustomPartnerData;
        protected List<Control> m_ctrlsCustomPartnerAddrParts;
        protected List<Control> m_ctrlCustomGroupSeparators;

        protected Control ctrlCmpCode;
        protected Control ctrlElmcode;
        protected Control ctrlUmbrellaElementChooseKey;

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

            this.m_ctrlCustomGroupSeparators = this.EditGroup1.ControlArray.Where(c => c.CustomData != null && c.DataField.StartsWith("gscustom")).GroupBy(c => c.DataField).Select(g => g.FirstOrDefault()).ToList();
            this.m_ctrlCustomGroupSeparators.ForEach(c => c.Visible = false);

            ctrlCmpCode = EditGroup1["cmpcode"];
            ctrlElmcode = EditGroup1["elmcode"];
            ctrlUmbrellaElementChooseKey = EditGroup1["umbrellaelementchoosekey_umbrellaelementchoosekey"];

            if (ctrlUmbrellaElementChooseKey != null)
                ctrlUmbrellaElementChooseKey.SetOnChanged(new ControlEvent(ctrlUmbrellaElementChooseKey_OnChanged));
        }

        protected override PinvHead DefaultPageLoad(PageUpdateArgs args)
        {
            var pinvHead = base.DefaultPageLoad(args);

            if (pinvHead == null)
            {
                return pinvHead;
            }

            var company = eLog.Base.Setup.Company.CompanyCache.Get(Convert.ToInt32(pinvHead?.Cmpid));
            if (ctrlCmpCode != null) ctrlCmpCode.Value = company?.Codacode;

            var partnerElmlevel = CodaInt.Base.Setup.Company.CompanyLineCache.Get(Convert.ToInt32(pinvHead?.Cmpid), (int)CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
            var partnId = this.m_partnCode.GetValue<int>();
            DocLine dl = DocLine.CreateNew();
            var partnCode = Partner.Load(partnId)?.Partncode;
            dl.El2 = partnCode?.ToString();
            SetCustomPartnerDataVisibility(args, dl, pinvHead.Cmpid, partnerElmlevel);
            this.RestoreCustomPartnerData(pinvHead); // umbrella adatok visszatoltese az xml mezobol

            // a RestoreUmbrellaData visszatolti az adatokat, de ha null, akkor felulirja a SetUmbrellaDataVisibility-ben
            // beallitott elmlevel-t, elmcode-t is, ezert ujra megadjuk, ha umbrellaelem
            var umbrellaElement = DocLineBL.GetUmbrellaElement(dl, partnerElmlevel);

            if (DocLineBL.IsUmbrellaElement(umbrellaElement, pinvHead.Cmpid, partnerElmlevel))
            {
                var ctrlelmlevel = m_ctrlsCustomPartnerData.First(x => x.DataField == "elmlevel");
                if (partnerElmlevel != null && ctrlelmlevel.GetValue() == null) ctrlelmlevel.SetValue(partnerElmlevel);
                if (ctrlElmcode != null && ctrlElmcode.GetValue() == null) ctrlElmcode.SetValue(umbrellaElement);
            }

            return pinvHead;
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
            if (pinvHead?.Partnid != null)
            {
                DocLine dl = DocLine.CreateNew();
                var partnCode = Partner.Load(pinvHead.Partnid)?.Partncode;
                dl.El2 = partnCode?.ToString();
                var partnerElmlevel = CodaInt.Base.Setup.Company.CompanyLineCache.Get(pinvHead.Cmpid, (int)CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
                var umbrellaElement = DocLineBL.GetUmbrellaElement(dl, partnerElmlevel);
                if (DocLineBL.IsUmbrellaElement(umbrellaElement, pinvHead.Cmpid, 2))
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
        }

        // egyedi partner adatok tarolasa az xmldata mezoben
        protected Base.Common.XCustValue.XCVal StoreCustomPartnerData(PageUpdateArgs args, PinvHead pinvHead)
        {
            if (pinvHead.Partnid.HasValue)
            {
                var bl = PinvHeadBL3.New();
                var xcVal = bl.LoadCustomPartnerData(pinvHead.Pinvid);

                DocLine dl = DocLine.CreateNew();
                var partnCode = Partner.Load(pinvHead.Partnid)?.Partncode;
                dl.El2 = partnCode?.ToString();
                var partnerElmlevel = CodaInt.Base.Setup.Company.CompanyLineCache.Get(pinvHead.Cmpid, (int)CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
                var umbrellaElement = DocLineBL.GetUmbrellaElement(dl, partnerElmlevel);
                if (DocLineBL.IsUmbrellaElement(umbrellaElement, pinvHead.Cmpid, partnerElmlevel))
                {
                    if (xcVal == null)
                    {
                        xcVal = Base.Common.XCustValue.XCVal.CreateNew();
                    }

                    var xmlData = new XmlManiputeStr(() => xcVal.Xmldata, value => xcVal.Xmldata = value, "pinvhead");
                    var xmlPartner = xmlData.GetX("Seller") ?? new System.Xml.Linq.XElement("Seller");

                    bool changed = false;
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
                            changed = true;
                            var xmlField = xmlPartner.Element(field);
                            xmlField?.Remove();
                            xmlPartner.Add(new System.Xml.Linq.XElement(field, newValue));
                        }
                    }

                    // ha korabbi trader kodot hasznalunk, akkor beirjuk, hogy mar fel van adva
                    var tradercode = xmlPartner.Element("TraderCode");
                    if (tradercode != null && !tradercode.IsEmpty)
                    {
                        if (changed)
                        {
                            var prov = (UmbrellaPartnSelectSearchProvider)eProjectWeb.Framework.SearchServer.Get(UmbrellaPartnSelectSearchProvider.ID);

                            var argstemplate = new Dictionary<string, object>()
                                {
                                    { "tradernamecode", tradercode.Value },
                                };

                            var lines = prov.SearchDataSet(argstemplate).Tables[0];
                            var umbrelladata = lines.Rows.Cast<System.Data.DataRow>()
                                .Select(x => new {
                                    Name = ConvertUtils.ToString(x["name"]),
                                    Country = ConvertUtils.ToString(x["countryid"]),
                                    PostCode = ConvertUtils.ToString(x["postcode"]),
                                    Add1 = ConvertUtils.ToString(x["add1"]),
                                    Add2 = ConvertUtils.ToString(x["add2"]),
                                    Vat = ConvertUtils.ToString(x["vat"]),
                                    TraderCode = ConvertUtils.ToString(x["tradernamecode"])
                                })?.FirstOrDefault();

                            if (m_ctrlsCustomPartnerData.Find(x => x.Field == "postcode_postcode").GetStringValue() == umbrelladata.PostCode &&
                                m_ctrlsCustomPartnerData.Find(x => x.Field == "add01_city").GetStringValue() == umbrelladata.Add1 &&
                                m_ctrlsCustomPartnerData.Find(x => x.Field == "add02").GetStringValue() == umbrelladata.Add2 
                                //&& m_ctrlsCustomPartnerData.Find(x => x.Field == "vatnum").GetStringValue() == umbrelladata.Vat
                                )
                            {
                                xmlPartner.SetAttributeValue("Posted", true);
                            }
                            else
                            {
                                xmlPartner.Attribute("Posted")?.Remove();
                                xmlPartner.Element("TraderCode")?.Remove();
                            }
                        }
                        else
                        {
                            xmlPartner.SetAttributeValue("Posted", true);
                        }
                    }
                    else
                    {
                        //xmlPartner.SetAttributeValue("Posted", false);
                        xmlPartner.Attribute("Posted")?.Remove();
                        xmlPartner.Element("TraderCode")?.Remove();
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
            //if (this.PageLoadDataBinding || args.ActionID == ActionID.View || this.m_add02.Disabled)
            if (this.PageLoadDataBinding || args.ActionID == ActionID.View)
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

        protected void SetCustomPartnerDataVisibility(PageUpdateArgs args, DocLine docLine, int? cmpid, int? elmlevel)
        {
            var umbrellaElement = DocLineBL.GetUmbrellaElement(docLine, elmlevel);
            SetCustomPartnerDataVisibility(args, umbrellaElement, cmpid, elmlevel);
        }

        protected void SetCustomPartnerDataVisibility(PageUpdateArgs args, string umbrellaElement, int? cmpid, int? elmlevel)
        {
            var partnerElmlevel = CodaInt.Base.Setup.Company.CompanyLineCache.Get(cmpid, (int)CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
            if (elmlevel != partnerElmlevel) return;

            bool visible = DocLineBL.IsUmbrellaElement(umbrellaElement, cmpid, elmlevel);
            m_ctrlsCustomPartnerData.ForEach(c => c.Visible = visible);
            m_ctrlCustomGroupSeparators.ForEach(c => c.Visible = visible);
            ctrlUmbrellaElementChooseKey.SetVisible(visible);

            if (visible)
            {
                ctrlElmcode.SetValue(umbrellaElement);
            }

            // ha nem lathatoak az umbrella adatok, akkor null-ozuk oket
            if (!visible)
            {
                m_ctrlsCustomPartnerData.ForEach(c => c.Value = null);
                ctrlUmbrellaElementChooseKey.SetValue(null);
                ctrlElmcode.SetValue(null);
            }

            // ha lathatoak az umbrella adatok, akkor beallitjuk az elmlevel-t
            if (visible) m_ctrlsCustomPartnerData.Find(x => x.Field == "elmlevel").SetValue(elmlevel);

            // az elmlevel soha nem lathato
            m_ctrlsCustomPartnerData.Find(x => x.Field == "elmlevel").SetVisible(false);

            // az elmcode soha nem lathato
            m_ctrlsCustomPartnerData.Find(x => x.Field == "elmcode").SetVisible(false);

            // a tradercode soha nem lathato
            //m_ctrlsCustomPartnerData.Find(x => x.Field == "tradernamecode").SetVisible(false);

            if (visible && args.ActionID == eProjectWeb.Framework.BL.ActionID.View)
            {
                m_ctrlsCustomPartnerData.ForEach(c => c.Disabled = true);
            }

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

            return;
        }

        protected override void OnCmpIdChanged(PageUpdateArgs args)
        {
            base.OnCmpIdChanged(args);

            var cmpId = this.m_cmpId.GetValue<int>();
            var company = eLog.Base.Setup.Company.CompanyCache.Get(cmpId.Value);
            if (ctrlCmpCode != null) ctrlCmpCode.Value = company?.Codacode;
        }

        protected override void OnPartnerChanged(PageUpdateArgs args)
        {
            if (this.PageLoadDataBinding)
                return;

            base.OnPartnerChanged(args);

            var partnId = this.m_partnCode.GetValue<int>();
            var partnCode = Partner.Load(partnId)?.Partncode;

            this.SetCustomPartnerDataVisibility(args, partnCode?.ToString(), m_cmpId.GetInt32Value(), 2);
        }

        private void ctrlUmbrellaElementChooseKey_OnChanged(PageUpdateArgs args)
        {
            if (this.PageLoadDataBinding)
                return;

            if (ctrlUmbrellaElementChooseKey != null)
            {
                var key = ctrlUmbrellaElementChooseKey.GetStringValue();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    var s = key.Split('|');
                    Array.Resize(ref s, 4);

                    var prov = (UmbrellaPartnSelectSearchProvider)eProjectWeb.Framework.SearchServer.Get(UmbrellaPartnSelectSearchProvider.ID);

                    int? elmlevel = null;
                    int? temporaryid = null;

                    int o;
                    if (int.TryParse(s[2], out o)) elmlevel = o;
                    if (int.TryParse(s[3], out o)) temporaryid = o;

                    var argstemplate = new Dictionary<string, object>() {
                                           { "cmpcode", s[0] },
                                           { "elmcode", s[1] },
                                           { "elmlevel", elmlevel },
                                           { "temporaryid", temporaryid },
                                       };
                    var lines = prov.SearchDataSet(argstemplate).Tables[0];
                    var umbrelladata = lines.Rows.Cast<System.Data.DataRow>().
                        Select(x => new { Name = ConvertUtils.ToString(x["name"]),
                                            Country = ConvertUtils.ToString(x["countryid"]),
                                            PostCode = ConvertUtils.ToString(x["postcode"]),
                                            Add1 = ConvertUtils.ToString(x["add1"]),
                                            Add2 = ConvertUtils.ToString(x["add2"]),
                                            Vat = ConvertUtils.ToString(x["vat"]), 
                                            BankAccno = ConvertUtils.ToString(x["sort_acnum"]).Replace("-",""),
                                            TraderCode = ConvertUtils.ToString(x["tradernamecode"]) })?.FirstOrDefault();

                    // trader
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "tradernamecode").SetValue(umbrelladata.TraderCode);
                    // postcode selector!
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "postcode_postcode").SetValue(umbrelladata.PostCode);
                    // partnname
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "partnname").SetValue(umbrelladata.Name);
                    // iranyitoszam es varos orszagkod fuggoek
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "postcode_postcode").SetValue(umbrelladata.PostCode);
                    if (!string.IsNullOrEmpty(umbrelladata.Country))
                    {
                        m_ctrlsCustomPartnerData.Find(x => x.Field == "countryid").SetValue(umbrelladata.Country);
                        m_ctrlsCustomPartnerData.Find(x => x.Field == "add01_city").SetValue(umbrelladata.Add1);
                    }
                    else
                    {
                        m_ctrlsCustomPartnerData.Find(x => x.Field == "countryid").SetValue(null);
                        m_ctrlsCustomPartnerData.Find(x => x.Field == "add01_city").SetValue(null);
                    }
                    // szamlaszam
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "partnbankaccno").SetValue(umbrelladata.BankAccno);
                    // adoszam
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "vatnum").SetValue(umbrelladata.Vat);
                    // cim => felbontas
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "add02").SetValue(umbrelladata.Add2, true);
                }
                else
                {
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "tradernamecode").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "postcode_postcode").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "partnname").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "postcode").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "countryid").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "add01_city").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "partnbankaccno").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "vatnum").SetValue(null);
                    m_ctrlsCustomPartnerData.Find(x => x.Field == "add02").SetValue(null);
                }
            }
        }


    }
}
