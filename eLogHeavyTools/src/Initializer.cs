using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;

namespace eLog.HeavyTools
{
    public class Initializer : IModule
    {
        public const string ModuleName = "eLog";

        public void Initialize()
        {
            eLog.Base.Sales.Sord.SordStOptions sordStOptions = eLog.Base.Sales.Sord.SordStBL.Options;
            sordStOptions.BackOrderStrictFilters = false;
            sordStOptions.ShowSordIfNoFreeStock = false;
            sordStOptions.StDefQtySrc = eLog.Base.Sales.Sord.SordStDefQtySrc.Stock;
            sordStOptions.StOrdQtySrc = eLog.Base.Sales.Sord.SordStOrdQtySrc.RemQty;
            sordStOptions.StDispMovQtySrc = eLog.Base.Sales.Sord.SordStDispMovQtySrc.Qty;
            sordStOptions.ErrorOnStockShortage = true;
            sordStOptions.DisableStLineDispQty = false;

            // Vevoi rendelesbol keszletmozgas generalasa: ha a vevoi rendelesre van mar nyitott keszletmozgas, akkor hiba
            eLog.Base.Sales.Sord.SordStGenBLBase.PendingStockTranNotAllowed = true;

            // Vevoi rendelesbol kivet generalas - tobb vevoi rendeles tobb kivetre
            eLog.Base.Sales.Sord.SordStGenCommon.GenType = eLog.Base.Sales.Sord.SordStGenType.More;

            // Lehessen Feladott statuszbol egybol Lezartba allitani a raktar tranzakciokat
            eLog.Base.Warehouse.StockTran.StHeadBL.AllowDirectClose = true;

            // Szallito rendeles tetel felvitelnel jegyezze meg egy fejen belul a kert szallitas datumot es a kovetkezo felvitelnel kinalja fel
            eLog.Base.Purchase.Pord.PordLineEditCommon.RememberLastLineReqDate = true;
            // Vevoi rendeles tetel felvitelnel jegyezze meg egy fejen belul a kert szallitas datumot es a kovetkezo felvitelnel kinalja fel
            eLog.Base.Sales.Sord.SordLineEditCommon.RememberLastLineReqDate = true;

            // Bevétből készülhet Szállítói számla
            eLog.Base.Warehouse.StockTran.StHeadBL.PinvFromReceiving = true;

            // Bevet es szallito rendeles felvitelnel a projekt alapjan ne toltse ki a partnert
            eLog.Base.Warehouse.StockTran.StHeadEditTab.FillPartnerByProject[eLog.Base.Warehouse.StockTran.StDocType.Receiving] = false;
            eLog.Base.Purchase.Pord.PordHeadEditTabBase<Base.Purchase.Pord.PordHead, Base.Purchase.Pord.PordHeadRules, Base.Purchase.Pord.PordHeadBL>.FillPartnerByProject = false;

            // Keszletmozgasbol keszult vevoi szamlara kezzel fel lehet-e tenni olyan tetelt, amit keszletezni is kell (nem csak szolgaltatast lehet kezzel ratenni, hanem barmit) 
            eLog.Base.Sales.Sinv.SinvLineRules.ManualNonSTLineAllowedOnSTInv = true;
            // Engedelyezett a 0 mennyisegu tetelsor
            Base.Sales.Sinv.SinvLineRules.ZeroQtyNotAllowed = false;
            // Vevoi szamlaban engedelyezett a negativ eladasi ar, mert a tobbi szamlazo programban allitanak ki ilyen szamlat, amit nem kell fogadnunk
            Base.Sales.Sinv.SinvLineRules.NegativSelPrcNotAllowed = false;

            // Gyujtoszamla - A partner cimei lehetnek-e elteroek? (ebben az esetben a szamlan a szamlazasi es a szallitasi cim meg fog egyezni)
            eLog.Base.Sales.Sinv.SinvCreateBL.AllowDifferentAddresses = true;
            // Kivetbol szamla - A kivet fejben levo projekt azonositot ne masoljuk at a szamlafejbe
            eLog.Base.Sales.Sinv.SinvCreateBL.ProjIdInHead = false;
            // Kivetbol szamla - A kivet fejben levo projekt azonositot masoljuk at a szamla tetelbe
            eLog.Base.Sales.Sinv.SinvCreateBL.ProjIdInLine = true;

            // Vevoi szamlaban hasznalunk-e raktarat, ha igen, akkor kotelezo-e kitolteni?
            eLog.Base.Sales.Sinv.SinvHeadBL.WhUsage = eLog.Base.Sales.Sinv.SinvWhUsage.NotUsed;
            // A vevoi szamlaban kotelezo-e bankot megadni?
            eLog.Base.Sales.Sinv.SinvHeadBL.BankUsage = eLog.Base.Sales.Sinv.SinvBankUsage.Mandatory;

            // Vevoi szamla - Helyesbito szamlanal nem erdekes a vevo vagy annak cimenek aktiv/rejtett statusza
            eLog.Base.Sales.Sinv.SinvHeadRules.PartnerStatCheckAtCorrection = false;

            // Bustypeid: Alapértelmezett átvétel kikapcsolása
            Base.Sales.Sinv.SinvHeadBL.CopySinvBusTypeId = false;
            Base.Purchase.Pinv.PinvHeadBL.CopyPinvBusTypeId = false;
            //Base.Warehouse.StockTran.StHeadBL.CopyStBusTypeId = false;
            Base.Warehouse.StockTran.StHeadBL.CheckBusTypeIdMandatory = new int[] {
                (int)eLog.Base.Warehouse.StockTran.StHeadGen.Manual,
                (int)eLog.Base.Warehouse.StockTran.StHeadGen.Storno,
                (int)eLog.Base.Warehouse.StockTran.StHeadGen.Correct,
            };

            // Szallitoi rendeles - Rejtett cikkszam is rogzitheto
            eLog.Base.Purchase.Pord.PordLineRules.AllowHiddenItems = true;

            // Eloleg beszamitas - Az eloleg beszamitas arfolyama az elolegszamla arfolyama legyen
            eLog.Base.Sales.Sinv.SinvAdvance.Sinvadvanceratesrctype = Base.Sales.Sinv.SinvAdvanceRateSrcType.Final;

            // Eloleg beszamitas - Maradek erteket nem futtatja ki, mindig az arfolyammal szamol (meg akkor is, ha ezzel valamelyik szamlan tobblet vagy hiany lesz)
            eLog.Base.Purchase.Pinv.PinvAdvance.Pinvadvancecompleteremainvaluetype = Base.Purchase.Pinv.PinvAdvanceCompleteRemainValueType.None;
            // Eloleg beszamitas - Az eloleg beszamitas arfolyama az elolegszamla arfolyama legyen
            eLog.Base.Purchase.Pinv.PinvAdvance.Pinvadvanceratesrctype = Base.Purchase.Pinv.PinvAdvanceRateSrcType.Final;

            // Vevoi szamla grid - Generaltunk mar szallitoi szamlat oszlop megjelenitese
            eLog.Base.Sales.Sinv.SinvHeadSearchProvider.ShowICPinv = true;

            // az ofc_dochlelm tablaban nem a normal uzleti logika szerinti adatokat taroljuk, igy ne szoljon, hogy az egyes elemszinteket nem szabad kitolteni
            CodaInt.Base.Common.DocHeadLineElement.OfcDocHLElmRules.TurnOffMandatoryDisallowed = true;

            // Vevoi szamlara elado bankszamlaszam felkinalasa
            // Ha van olyan bankszamla, aminek a penzneme megegyzik a megadott penznemmel, akkor azt valasztja, ha nem akkor az alapertelmezett bankszamlat (mindegy, hogy annak mi a penzneme). Ha alapertelmezett sincs, akkor olyat, ahol nincs megadva penznem
            eLog.Base.Masters.Common.MastersSqlFunctions.GetCompanyBankAccNoDefaultDefaultBehavior = Base.Masters.Common.MastersSqlFunctions.GetCompanyBankAccNoDefaultBehavior.CurIdMatchOrDefault;

            eLog.Base.Warehouse.StockTran.StHeadSearchProvider.DefaultOrderByStr = " ORDER BY adddate DESC";
            eLog.Base.Sales.Sord.SordHeadSearchProvider.DefaultOrderByStr = " ORDER BY adddate DESC";
            eLog.Base.Sales.Sord.SordLineAllSearchProvider.DefaultOrderByStr = " ORDER BY sl.adddate DESC";
            eLog.Base.Sales.Sinv.SinvHeadSearchProvider.DefaultOrderByStr = " ORDER BY adddate DESC";
            eLog.Base.Purchase.Pord.PordHeadSearchProvider.DefaultOrderByStr = " ORDER BY adddate DESC";
            eLog.Base.Purchase.Pord.PordLineAllSearchProvider.DefaultOrderByStr = " ORDER BY adddate DESC";
            eLog.Base.Purchase.Pinv.PinvHeadSearchProvider.DefaultOrderByStr = " ORDER BY adddate DESC";
            eLog.Base.Warehouse.Transport.TranspHeadSearchProvider.DefaultOrderByStr = " ORDER BY th.adddate DESC";
            eLog.Base.Project.ProjectSearchProvider.DefaultOrderByStr = "\norder by pj.adddate desc";

            eLog.Base.Masters.PriceTable.PriceCalc.Impl = new eLog.Base.Masters.PriceTable.PriceCalcCached();
            eLog.Base.Masters.PriceTable.CachedPriceCalculator.AutoLoad = true;

            // by CsJ @ 20150827: ideiglenesen lehessen a cikk mertekegyseget modositani
            Base.Masters.Item.ItemRules.ValidateItemUnitidEditable = false;

            // Alapertelmezesben, CODA feladas elott kuldjon-e a program ellenorzo XML-t, vagy sem.
            CodaInt.Base.Bookkeeping.Common.DocHeadBL.BookCheck = true;

            // Kimeno szamla (sinv) kontirozas ablakon, az idoszak gomb kezelje-e az InterCompany-s bejovo szamlat (pinv) is.
            CodaInt.Base.Bookkeeping.SinvPost.SinvDocPostBL.UpdateICPinvPeriodOnSinvChange = true;

            // Vevoi szamla helyesbitesnel az AFA teljesites datumat az eredeti szamlabol vegyuk
            eLog.Base.Sales.Sinv.SinvCorrectBL.SinvCorrectionTaxDateRecalc = false;

            // Keszletmozgasnal brutto/netto eladasi ar allitas a szerkeszto kepernyokre
            eLog.Base.Warehouse.StockTran.StLineEditTab.ShowSelPrcType = true;
            // Vevoi szamlanal brutto/netto eladasi ar allitas a szerkeszto kepernyokre
            eLog.Base.Sales.Sinv.SinvLineEditTab.ShowSelPrcType = true;
            // Vevoi szerződésnél brutto/netto eladasi ar allitas a szerkeszto kepernyokre
            eLog.Base.Sales.SContract.SContrPerformEditTab.ShowSelPrcType = true;

            // Lezart keszletmozgason is modosithato a projekt
            // (ez itt csak a validalasra vontakozik. A szerkeszto kepernyot kulon kell megoldani)
            eLog.Base.Warehouse.StockTran.StHeadRules.ClosedFieldsModifyable.Add("projid");

            // Kivet - A projekt alapjan ne kinaljuk fel automatikusan a partnert
            eLog.Base.Warehouse.StockTran.StHeadEditTab.FillPartnerByProject[eLog.Base.Warehouse.StockTran.StDocType.Issuing] = false;

            // Pénztár tranzakció tételen a hivatkozás mezők közül a szállítói rendelés módosítható lezárt tranzakción is
            eLog.Finance.Base.PettyCash.PcTran.PcTranLineRules.ClosedPcTranModifyableReferenceFields.Add(eLog.Finance.Base.PettyCash.PctLineDoc.PctLineDoc.RefType.Pordline, "pordlineid");

            // Szallitoi szamla - Finance feladas gomb az elso fulon
            CodaInt.Base.Purchase.Pinv.PinvHeadSearchTab2.ShowCodaPostButton = true;

            // Szallito szamla egyeztetes (jogositas) szallitoi rendeleshez - Nem teljesithetjuk tul az erteket
            eLog.Base.Purchase.Pinv.CostLineRules.CheckPordValueOverload = true;

            // Koltsegsor import (szallitoi szamlaban) - mezolista
            eLog.Base.Purchase.Pinv.PinvAssignmentTab.ImportFields = new string[] { "costtypeid", "realcostval", "el3|$costlineel3", "el4|$costlineel4", "el5|$costlineel5"/*, "el6|$costlineel6", "el7|$costlineel7", "ref5|$costlineref5"*/, "note" };

            // kovetkezo cikkszam funkcio engedelyezese
            eLog.Base.Masters.Item.ItemBL.AllowGenNextItem = true;

            // Szallitoi szamla feladhato (Finance-be), akkor is, ha nincs lezarva
            CodaInt.Base.Bookkeeping.PinvPost.PinvDocPostBL.AllowPostOpenedPinv = CodaInt.Base.Bookkeeping.PinvPost.PinvDocPostBL.AllowPostOpenedPinvEnum.Yes;

            // Vevoi szamla tetel - A tetelek sorrendjenek modositasahoz szukseges gombok latszodjanak
            Base.Sales.Sinv.SinvLineSearchTab.ShowLinenumUpDownButtons = true;

            // Kivet tetel felvitel - A kivalsztott munkalaprol felkinaljuk a kontirozast a kivet tetelre
            CodaInt.Base.Warehouse.StockTran.IssuingLineEditTab2.CopyElementsFromSvcWorksheet = true;

            // Felhasznalok listajaban nem csak nev eleje egyezosegre lehessen keresni
            eProjectWeb.Framework.Data.GlobalDefaultListProviderSettings.OverrideFilterAnywhere[eProjectWeb.Framework.UI.Maintenance.User.UserList.ID] = true;

            // Penztarbizonylaton szerepelhessenek kulonbozo partnerre szolo szamlak
            eLog.Finance.Base.PettyCash.PcTran.PcTranLineRules.CheckSinvSamePartner = false;

            // Szallitoi szamla Eszkoz fulek
            CodaInt.Base.Purchase.Pinv.PinvHeadSearchPageB2.AddAssetTabs = true;

            // Kontirozas import (szallitoi szamla kontirozasban) - mezolista
            CodaInt.Base.Bookkeeping.PinvPost.PinvDocPostSearchTab.ImportFields = new string[] { "descr", "debcredtypeU", "el1", "el2", "el3", "el4", "el5", "el6", "el7", "el8", "taxcode", "val" };

            // osztaly felulirasok

            // Employee
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.EmployeeBL), typeof(Masters.Partner.EmployeeBL3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.EmployeeEditTab), typeof(Masters.Partner.EmployeeEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.EmployeeSearchProvider), typeof(Masters.Partner.EmployeeSearchProvider3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.EmployeeList), typeof(Masters.Partner.EmployeeList3));

            // PartnAddr
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.PartnAddrBL), typeof(Masters.Partner.PartnAddrBL3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.PartnAddrEditTab), typeof(Masters.Partner.PartnAddrEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.PartnAddrSearchProvider), typeof(Masters.Partner.PartnAddrSearchProvider3));

            // Partner
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.PartnerBL), typeof(Masters.Partner.PartnerBL3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.PartnerEditTab), typeof(Masters.Partner.PartnerEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Masters.Partner.PartnerSearchProvider), typeof(Masters.Partner.PartnerSearchProvider3));

            // Warehouse
            ObjectFactory.AddRemap(typeof(Base.Setup.Warehouse.WarehouseSearchPage), typeof(Setup.Warehouse.WarehouseSearchPage3));
            ObjectFactory.AddRemap(typeof(Base.Setup.Warehouse.WarehouseSearchTab), typeof(Setup.Warehouse.WarehouseSearchTab3));

            // Item
            ObjectFactory.AddRemap(typeof(Base.Masters.Item.ItemEditTab), typeof(Masters.Item.ItemEditTab2));
            ObjectFactory.AddRemap(typeof(Base.Masters.Item.ItemBL), typeof(Masters.Item.ItemBL3));

            // Bank
            ObjectFactory.AddRemap(typeof(U4Ext.Bank.Base.Transaction.EfxBankTranLineSearchTab), typeof(BankTran.EfxBankTranLineSearchTab3));
            ObjectFactory.AddRemap(typeof(U4Ext.Bank.Base.Transaction.CifEbankTransBL), typeof(BankTran.CifEbankTransBL3));

            // Sord
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordHeadEditTab2), typeof(Sales.Sord.SordHeadEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordHeadBL), typeof(Sales.Sord.SordHeadBL3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordHeadSearchProvider), typeof(Sales.Sord.SordHeadSearchProvider3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordHeadSearchTab), typeof(Sales.Sord.SordHeadSearchTab3));

            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineAllEditTab2), typeof(Sales.Sord.SordLineAllEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineAllSearchProvider), typeof(Sales.Sord.SordLineAllSearchProvider3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineAllBL), typeof(Sales.Sord.SordLineAllBL3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineBL), typeof(Sales.Sord.SordLineBL3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineEditTab), typeof(Sales.Sord.SordLineEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineEditTab), typeof(Sales.Sord.SordLineEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Sales.Sord.SordLineSearchProvider), typeof(Sales.Sord.SordLineSearchProvider3));

            // Pord
            ObjectFactory.AddRemap(typeof(Base.Purchase.Pord.PordHeadBL), typeof(Purchase.Pord.PordHeadBL3));
            ObjectFactory.AddRemap(typeof(Base.Purchase.Pord.PordHeadEditTab), typeof(Purchase.Pord.PordHeadEditTab3));
            ObjectFactory.AddRemap(typeof(Base.Purchase.Pord.PordHeadSearchTab), typeof(Purchase.Pord.PordHeadSearchTab3));
        }
    }
}
