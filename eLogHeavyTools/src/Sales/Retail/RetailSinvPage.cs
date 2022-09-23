using eLog.Base;
using eLog.Base.Masters.Item;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Sinv;
using eLog.Base.Setup.ItemGroup;
using eLog.Base.Setup.PartnVatType;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Retail
{
    internal class RetailSinvPage : TabbedPageInfoProvider
    {
        public static readonly string ID = typeof(RetailSinvPage).FullName;
        
        public RetailSinvPage()
            : base("RetailSales")
        {
            Tabs.AddTab(delegate { return RetailSinvTab.New(); });
        }
    }

    internal class RetailSinvTab : TabPage2
    {

        Selector partnid_partncode;
        Combo addrid;
        Textbox partnname;
        Textbox buyvatnum;
        Textbox buyvatnumeu;
        Textbox buygroupvatnum;
        Combo countryid;
        Combo regid;
        Selector postcode_postcode;
        Selector add01_city;
        Textbox add02;
        Textbox district;
        Textbox place;
        Textbox placetype;
        Textbox hnum;
        Textbox building;
        Textbox stairway;
        Textbox floor;
        Textbox door; 
        Combo ptvattypid;

        protected eLog.Base.Common.XmlValueStore AddrPartsStore;
        LayoutTable editGroup1;
         
        public static RetailSinvTab New()
        {
            var t = (RetailSinvTab)ObjectFactory.New(typeof(RetailSinvTab));
            t.Initialize("RetailSinv");
            return t;
        }
        protected override void Initialize(string labelID)
        {
            base.Initialize(labelID);
            CreateControls();

            partnid_partncode = (Selector)FindRenderable("partnid_partncode");
            addrid = (Combo)FindRenderable("addrid");
            partnname = (Textbox)FindRenderable("partnname");
            buyvatnum = (Textbox)FindRenderable("buyvatnum");
            buyvatnumeu = (Textbox)FindRenderable("buyvatnumeu");
            buygroupvatnum = (Textbox)FindRenderable("buygroupvatnum");
            countryid = (Combo)FindRenderable("countryid");
            regid = (Combo)FindRenderable("regid");
            postcode_postcode = (Selector)FindRenderable("postcode_postcode");
            add01_city = (Selector)FindRenderable("add01_city");
            district = (Textbox)FindRenderable("district");
            place = (Textbox)FindRenderable("place");
            placetype = (Textbox)FindRenderable("placetype");
            hnum = (Textbox)FindRenderable("hnum");
            building = (Textbox)FindRenderable("building");
            stairway = (Textbox)FindRenderable("stairway");
            floor = (Textbox)FindRenderable("floor");
            door = (Textbox)FindRenderable("door");
            add02 = (Textbox)FindRenderable("add02"); 
            ptvattypid = (Combo)FindRenderable("ptvattypid");

            var c = new Button("close2") { Shortcut = eProjectWeb.Framework.UI.Commands.ShortcutKeys.Key_ESC };
            AddCmd(c);
            c.SetOnClick(OnCLose);
            OnPageLoad += RetailSinvTab_OnPageLoad;
            partnid_partncode.SetOnChanged(OnPartnidChanged);
            addrid.SetOnChanged(OnAddrChanged);

            editGroup1 = (LayoutTable)this["EditGroup2"];

            this.AddrPartsStore = new Base.Common.XmlValueStore(editGroup1, "addr");
            AddrPartsStore.AddControl("district");
            AddrPartsStore.AddControl("place");
            AddrPartsStore.AddControl("placetype");
            AddrPartsStore.AddControl("hnum");
            AddrPartsStore.AddControl("building");
            AddrPartsStore.AddControl("stairway");
            AddrPartsStore.AddControl("floor");
            AddrPartsStore.AddControl("door");
            foreach (Control cc in this.AddrPartsStore.Store.Values)
                if (cc != null)
                    cc.SetOnChanged(AddressPart_OnChanged);
            var csinv = new Button("createsinv") { Shortcut = eProjectWeb.Framework.UI.Commands.ShortcutKeys.Key_F9 };
            AddCmd(csinv);
            csinv.SetOnClick(OnCreateSinv);
        }

        private void OnCreateSinv(PageUpdateArgs args)
        {
            if (partnid_partncode.Value == null)
            {
                throw new MessageException("$missingpartnid_partncode");
            }

            if (addrid.Value == null)
            {
                throw new MessageException("$missingpartnid_addrid");
            }
            var partnVatTyp = PartnVatTyp.Load(ConvertUtils.ToString(ptvattypid.Value));
            var bl = new RetailVatnumCheck();
            var vns = partnVatTyp.XmlData.GetX("vatnum");
            bl.CheckPartnerVatNum(vns, ConvertUtils.ToString(buyvatnum.Value));

            vns = partnVatTyp.XmlData.GetX("vatnumeu");
            bl.CheckPartnerVatNumEU(vns, ConvertUtils.ToString(buyvatnumeu.Value));

            vns = partnVatTyp.XmlData.GetX("groupvatnum");
            bl.CheckPartnerGroupVatNum(vns, ConvertUtils.ToString(buygroupvatnum.Value), ConvertUtils.ToString(buyvatnum.Value));


            if (!partnVatTyp.Ptvattypid.Equals("BIZONYLAT"))
            {
                if (postcode_postcode.Value.ToString().Length < 4)
                {
                    throw new MessageException("$postcode_req");
                }
                if (add01_city.Value.ToString().Length < 2)
                {
                    throw new MessageException("$city_req");
                }
                if (place.Value.ToString().Length < 2)
                {
                    throw new MessageException("$place_req");
                }
            }
            
            /*
            var sh = GereateSinv();
            var data = GenerateReceipt.BySinvid(sh.Sinvid.Value);
            //args.AddExecCommand(string.Format("PrintMG('{0}')", data));
            */
            args.ClosePage("");
        }
        private SinvHead GereateSinv()
        {
            SinvHeadBL headBL = SinvHeadBL.New();
            var sh = SinvHead.CreateNew();
            sh.Cmpid = Session.CompanyFlags;

            sh.Ptvattypid = ConvertUtils.ToString(ptvattypid.Value);
             
            if (sh.Ptvattypid.Equals("BIZONYLAT"))
            {
                sh.Sinvdocid = CustomSettings.GetString("Retail-sinvdocid-noninvoice", "");
            }
            else
            {
                sh.Sinvdocid = CustomSettings.GetString("Retail-sinvdocid", "");
            }

            sh.Sinvdate = DateTime.Today;
            sh.Partnid = ConvertUtils.ToInt32(partnid_partncode.Value);
            sh.Addrid = ConvertUtils.ToInt32(addrid.Value);
            sh.Delpartnid = sh.Partnid;
            sh.Deladdrid = sh.Addrid;
            sh.Curid = "HUF";
            sh.Docdate = DateTime.Today;
            sh.Paymid = null; //ConvertUtils.ToString(retailpaymethod.Value);
            sh.Duedate = DateTime.Today;
            sh.Taxdatetype = 0;
            sh.Taxdate = DateTime.Today;
            sh.Gen = 9876;
             
            sh.Partnname = ConvertUtils.ToString(partnname.Value);
             
            var xml = "<sinvhead>";
            xml += CreateXML(buyvatnum, "buyvatnum");
            xml += CreateXML(countryid, "buyaddrcountrycode");
            xml += CreateXML(postcode_postcode, "buyaddrpostcode");
            xml += CreateXML(add01_city, "buyaddrcity");
            xml += CreateXML(district, "buyaddrdistrict");
            xml += CreateXML(add02, "buyadd02");
            xml += CreateXML(place, "buyaddrplace");
            xml += CreateXML(placetype, "buyaddrplacetype");
            xml += CreateXML(hnum, "buyaddrhnum");
            xml += CreateXML(building, "buyaddrbuilding");
            xml += CreateXML(stairway, "buyaddrstairway");
            xml += CreateXML(floor, "buyaddrfloor");
            xml += CreateXML(door, "buyaddrdoor");

            xml += "</sinvhead>";

            sh.Xmldata = xml;
             
            var map = headBL.CreateBLObjects();
            map.Default = sh;
            headBL.Save(map);

            sh = SinvHead.Load(sh.Sinvid);
            sh.Partnname = ConvertUtils.ToString(partnname.Value);
            sh.Save();
             
            var sls = new List<SinvLine>();
            /* Tétel */
            var lineBL = SinvLineBL.New();
            foreach (var ci in SqlDataAdapter.Query(@"select itemid, prc, originalprc, count(0) qty from olc_retail group by itemid, prc,originalprc").AllRows)
            {
                var originalprc = ConvertUtils.ToDecimal(ci["originalprc"]);
                var prc = ConvertUtils.ToDecimal(ci["prc"]);
                var item = Item.Load(ConvertUtils.ToInt32(ci["itemid"]));
                var qty = ConvertUtils.ToInt32(ci["qty"]);

                var itemgrp = ItemGroup.Load(item.Itemgrpid);
                var sl = SinvLine.CreateNew();

                sl.Itemid = item.Itemid;
                sl.Descr01 = item.Name01;
                sl.Descr02 = item.Name02;
                sl.Sinvqty = qty;
                sl.Selprctype = (int)SelPrcType.Gross;
                sl.Discpercnt = 0;

                sl.Seltotprc = prc;
                sl.Taxid = itemgrp.Taxid;
                sl.Gen = 98765;
                sl.Sinvid = sh.Sinvid;
                sl.Corrtype = (int)SinvLineCorrList.Values.Normal;

                if (!sl.Seltotprc.HasValue)
                {
                    throw new MessageException("$nopriceforallitem");
                }

                var lineMap = lineBL.CreateBLObjects();
                lineMap.Default = sl;
                lineBL.Save(lineMap);
                sls.Add(sl);
            }

            using (var ns = new NS("eLog"))
            {
                string msg;
                var success = headBL.Close(sh.Sinvid.Value, out msg);
                if (success != 0)
                {
                    throw new Exception(msg);
                }
            }

            /*
            var ch = OlcSinvHead.CreateNew();
            ch.Sinvid = sh.Sinvid;
            ch.Retailinvoiceimage = GenerateRetailinvoiceimage(sh, sls);
            ch.Save();
            */

            SqlDataAdapter.ExecuteNonQuery(DB.Main, @"delete olc_retail");

            return SinvHead.Load(sh.Sinvid);
        }

        private StringN GenerateRetailinvoiceimage(SinvHead sh, List<SinvLine> sls)
        {
            return "";
        }

        private string CreateXML(Selector c, string v)
        {
            return CreateXML(ConvertUtils.ToString(c.Value), v);
        }

        private string CreateXML(Combo c, string v)
        {
            return CreateXML(ConvertUtils.ToString(c.Value), v);
        }

        private string CreateXML(Textbox tb, string tab)
        {
            return CreateXML(ConvertUtils.ToString(tb.Value), tab);
        }

        private string CreateXML(string v, string tab)
        {
            if (v != null)
            {
                return "<" + tab + ">" + v + "</" + tab + ">";
            }
            return "";
        }

        private void OnAddrChanged(PageUpdateArgs args)
        {
            UpdateAdd(args);
        }

        private void UpdateAdd(PageUpdateArgs args)
        {
            var a = PartnAddr.Load(ConvertUtils.ToInt32(addrid.Value));

            if (a != null)
            {
                partnname.Value = a.Name;
                countryid.Value = a.Countryid;
                regid.Value = a.Regid = a.Regid;
                postcode_postcode.Value = a.Postcode;
                add01_city.Value = a.Add01;
                add02.Value = a.Add02;
                this.AddrPartsStore.SetControlsValue(StringN.ConvertToString(a.Xmldata), args);
            }
        }

        private void OnPartnidChanged(PageUpdateArgs args)
        {
            if (partnid_partncode.Value != null)
            {
                var p = Partner.Load(ConvertUtils.ToInt32(partnid_partncode.Value));
                buyvatnum.Value = p.Vatnum;
                addrid.Value = null;
                ptvattypid.Value = p.Ptvattypid;
            } else
            {
                buyvatnum.Value = null;
                addrid.Value = null;
                ptvattypid.Value = null;
            }
        }

        private void RetailSinvTab_OnPageLoad(PageUpdateArgs args)
        {
            var pid = CustomSettings.GetInt("Retail-partnid", -1);

            if (pid > -1)
            {
                partnid_partncode.Value = pid;
            }

            var ai = CustomSettings.GetInt("Retail-addrid", -1);
            if (ai > -1)
            {
                addrid.Value = ai;
            }

            foreach (var pp in SqlDataAdapter.Query(@"select p.partnid, a.addrid from olc_cart c (nolock) 
join olc_partner p (nolock) on p.loyaltycardno=c.loyaltyCardNo
join ols_partnaddr a (nolock) on a.partnid=p.partnid and a.def=1").AllRows)
            {
                partnid_partncode.Value = pp["partnid"];
                addrid.Value = pp["addrid"];

                break;
            }

            foreach (var r in SqlDataAdapter.
                Query(@"select sum(totVal) prc, count(0) c from olc_cart where itemid is not null").AllRows)
            {
                ((Intbox)FindRenderable("itemcount")).Value = ConvertUtils.ToInt32(r["c"]);
                ((Numberbox)FindRenderable("price")).Value = ConvertUtils.ToDecimal(r["prc"]);
                break;
            }
            UpdateAdd(args);
            OnPartnidChanged(args);
        }

        private void OnCLose(PageUpdateArgs args)
        {
            args.ClosePage("");
        }
        protected void AddressPart_OnChanged(PageUpdateArgs args)
        {
            var addrParts = this.AddrPartsStore.GetValues();

            var bl = PartnAddrBL.New();
            string addr02 = bl.GetAdd02FromParts(addrParts);
            if (add02 != null)
                add02.Value = addr02;
        } 

    }
}