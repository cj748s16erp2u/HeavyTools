using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.Reports;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Commands;
using eProjectWeb.Framework.UI.Templates;
using System.Xml;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterParamsTab2 : TabPage2
    {
        Control ctrlCmpcode;
        Control ctrlCmpid;
        Control ctrlGllevel;
        Control ctrlPelementlevel;
        Control ctrlPcodelike;
        Control ctrlRemdate2;
        Control ctrlRemdate;
        Control ctrlDuedateParam2;
        Control ctrlDuedateParam;
        Control ctrlLngId;
        Control ctrlAction;

        public const int BTNPRINTORDER = 30;

        protected ReportSettings ReportSettings = new ReportSettings();
        protected string ReportCallback;
        protected DialogBox dlgSimpleMessage;

        public static ReminderLetterParamsTab2 New(string xmlTypeName)
        {
            var t = eProjectWeb.Framework.ObjectFactory.New<ReminderLetterParamsTab2>();
            t.Initialize(xmlTypeName);
            return t;
        }

        protected LayoutTable EditGroup1;

        protected override void Initialize(string labelID)
        {
            base.Initialize(labelID);
            CreateBase();
        }
        protected virtual void CreateBase()
        {
            CreateControls();

            CreateDefaultPrintActions();

            EditGroup1 = (LayoutTable)this["EditGroup1"];

            if (EditGroup1 != null)
            {
                ctrlCmpcode = EditGroup1["cmpcode"];
                ctrlCmpcode.SetOnChangedWhenExists(new ControlEvent(cmpcodeChanged));
                ctrlGllevel = EditGroup1["gllevel"];
                ctrlPelementlevel = EditGroup1["pelementlevel"];
                ctrlPcodelike = EditGroup1["pcodelike"];
                ctrlRemdate2 = EditGroup1["remdate2"];
                ctrlRemdate2.SetOnChangedWhenExists(new ControlEvent(remdate2Changed));
                ctrlRemdate = EditGroup1["remdate"];
                ctrlDuedateParam2 = EditGroup1["duedate_param2"];
                ctrlDuedateParam2.SetOnChangedWhenExists(new ControlEvent(duedate_param2Changed));
                ctrlDuedateParam = EditGroup1["duedate_param"];
                ctrlLngId = EditGroup1["lngid"];
                ctrlAction = EditGroup1["action"];
                ctrlCmpid = EditGroup1["cmpid"];
            }

            OnPageActivate += ReminderLetterParamsTabFKT_OnPageActivate;
            ReportCallback = ReminderLetterPrintCallback.ID;
        }

        private void duedate_param2Changed(PageUpdateArgs args)
        {
            ctrlDuedateParam.SetValue((ctrlDuedateParam2.GetValue<DateTime>()).Value.ToString("yyyy'/'MM'/'dd"));
        }

        private void ReminderLetterParamsTabFKT_OnPageActivate(PageUpdateArgs args)
        {
            cmpcodeChanged(args);
            ctrlLngId.SetValue("hu-HU");
        }

        private void remdate2Changed(PageUpdateArgs args)
        {
            ctrlRemdate.SetValue((ctrlRemdate2.GetValue<DateTime>()).Value.ToString("yyyy'/'MM'/'dd"));
        }

        private void cmpcodeChanged(PageUpdateArgs args)
        {
            var cmpCodaCode = ctrlCmpcode.GetStringValue();
            var gllevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.LedgerElmLevelType)?.ValueInt;
            ctrlGllevel.SetValue(gllevel);
            var pelementlevel = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.PartnerElmLevelType)?.ValueInt;
            ctrlPelementlevel.SetValue(pelementlevel);
            var pcodelike = CodaInt.Base.Setup.Company.CompanyLineCache.GetByCodaCode(cmpCodaCode, global::CODALink.Common.CompanyLineRecTypes.PartnerPrefix)?.ValueStr;
            if (string.IsNullOrWhiteSpace(pcodelike)) pcodelike = "null";
            ctrlPcodelike.SetValue(pcodelike);
            var cmpid = Base.Setup.Company.CompanyCache.GetByCodaCode(cmpCodaCode)?.Cmpid;
            ctrlCmpid.SetValue(cmpid);
        }

        protected virtual void CreateDefaultPrintActions()
        {
            if (ReportSettings.ReportFiles.Count > 0)
            {
                Button btnPrint = new Button(ActionID.Print, BTNPRINTORDER);
                AddCmd(btnPrint);
                SetButtonAction(ActionID.Print, new ControlEvent(m_cmdPrint_onClick));
                btnPrint.SetShortcutAction(DefaultShortcut.Print);
            }
        }

        protected override void CreateControls()
        {
            base.CreateControls();

            Button btnCancel = new Button("cancel",40);
            AddCmd(btnCancel);
            SetButtonAction(btnCancel.ID, eProjectWeb.Framework.UI.Actions.EditCancelAction.Instance);
        }

        protected void m_cmdPrint_onClick(PageUpdateArgs args)
        {
            string err = "";
            foreach (var c in this.EditGroup1.ControlArray)
            {
                if (c.Mandatory && (c.Value == null || c.GetStringValue() == string.Empty))
                {
                    err += string.Concat('"', c.Label, '"', " ", eProjectWeb.Framework.Lang.Translator.Translate("$parameter_required"), "\n");
                }
            }

            if (err.Count() > 0)
            {
                throw new MessageException(string.Join(Environment.NewLine, err));
            }

            PrintClick(args, ctrlAction.GetValue<int>(), ReportCallback);
        }

        protected void PrintClick(PageUpdateArgs args, int? printAction, string callback)
        {
            PrintClick(args, printAction, null, callback);
        }

        protected virtual void PrintClick(PageUpdateArgs args, int? printAction, Key k, string callback)
        {
            PrintClick(args, printAction, k, callback, null);
        }

        protected virtual void PrintClick(PageUpdateArgs args, int? printAction, Key k, string callback, string reportType = null)
        {
            Guid repGuid = Guid.Empty;
            PrintArgs printArgs;
            StringDict param = (k != null ? k : new StringDict());
            param["user"] = Session.UserID;

            string err = "";
            foreach (var c in this.EditGroup1.ControlArray)
            {
                if (c.Mandatory && (c.Value == null || c.GetStringValue() == string.Empty))
                {
                    err += string.Concat('"', c.Label, '"', " ", eProjectWeb.Framework.Lang.Translator.Translate("$parameter_required"), "\n");
                    continue;
                }

                if (!c.Mandatory && (c.Value == null || c.GetStringValue() == string.Empty))
                {
                    param[c.Field] = null;
                    continue;
                }

                param[c.Field] = c.Value;
            }

            var lngid = Convert.ToString(param["lngid"])?.Substring(0, 2).ToUpper(); // hu-HU elso 2 karakter csak
            var language = eProjectWeb.Framework.Reports.SSRSUtils.GetLanguageID(lngid);
            param["language"] = language;

            var dbLink = eProjectWeb.Framework.Data.DBConfig.GetDatabaseLink(eProjectWeb.Framework.Session.Catalog, CodaInt.Base.Module.CodaDBConnID);
            param["codadb"] = dbLink.Database;

            if (printAction == null)
                printArgs = new PrintArgs(ReportSettings, param, args, callback);
            else
                printArgs = new PrintArgs(ReportSettings, param, args, callback, printAction.GetValueOrDefault());

            //if ((printArgs.Param == null || printArgs.Param.Count == 0))
            //{
            //    printArgs.CanPrint = false;
            //    PrintNoSel(args);
            //}

            if (printArgs.CanPrint)
            {
                printArgs.ReportType = reportType;
                PrintMethod(printArgs);
            }
        }

        protected virtual void PrintMethod(PrintArgs param)
        {
            PrintMethod(param, null);
        }

        protected virtual void PrintMethod(PrintArgs param, string nspace)
        {
            if (nspace == null)
                nspace = GetNamespaceName();

            CreatePDFs(param, nspace);
        }

        // Lehet fizetesi emlekezteto (severity=1) es felszolitas (severity=2)
        // Partnerenkent (elmcode) kell eloallitani egy-egy PDF-et.
        // Mindegyik PDF-hez tartozhat (vevoi) szamla, melyet szinten ki kell nyomtatni
        // Az osszes PDF-et megfeleloen elnevezve, egy konyvtarba kell lementeni
        //
        // Az osszes adatot a tartolt eljaras adja vissza (szukseges mezok: elmcode, severity, invoicenumber).
        // Ezt a program lefuttatja, meghatarozza belole a partnereket es a severity-ket
        // Ezekre csoportositva hivja meg a nyomtatast
        protected void CreatePDFs(PrintArgs printArgs, string nspace)
        {
            // adatok levalogatasa
            ReportParams repParams = new ReportParams(nspace, this.ReportSettings, printArgs.Param, printArgs.FileSettings, printArgs.PrintAction);
            if (!string.IsNullOrEmpty(printArgs.ReportType))
            {
                repParams.ReportType = printArgs.ReportType;
            }

            ReportUtils.WriteReportParams(repParams);

            DateTime date = DateTime.Today;
            object o;
            if (repParams.Params.TryGetValue("remdate2", out o))
            {
                if (o is DateTime)
                    date = (DateTime)o;
            }

            DateTime dueDate = DateTime.MaxValue;
            if (repParams.Params.TryGetValue("duedate_param2", out o))
            {
                if (o is DateTime)
                    dueDate = (DateTime)o;
            }

            var errors = new List<string>();

            string sql = "exec " + repParams.ReportProcName + " " + Utils.SqlToString(repParams.ReportGuid);
            List<ListItem> lst;
            int count = 0;
            var bpid = BackgroundProcess.Start("$reminderlettersgenerating".eLogTransl());
            try
            {
                using (var dSet = new System.Data.DataSet())
                {
                    BackgroundProcess.SetProgress(bpid, "$reminderlettersgenerating_getlist".eLogTransl());

                    using (var da = DB.CreateDA(DB.Main, sql))
                        da.Fill(dSet);

                    if (dSet.Tables.Count == 0)
                        return;
                    var dt = dSet.Tables[0];
                    if (dt.Rows.Count == 0)
                        return;

                    var creator = ReportUtils.GetInvoiceCreator("InvoiceCreator");

                    lst = dSet.Tables[0].Rows
                        .Cast<System.Data.DataRow>()
                        .GroupBy(r => new { PartnCode = Convert.ToString(r["elmcode"]), Severity = Convert.ToInt32(r["severity"]) })
                        .Select(x => new ListItem() { PartnCode = x.Key.PartnCode, Severity = x.Key.Severity, Rows = x })
                        .ToList();
                    // PDF-el eloallitasa partnerenkent, tipusonkent (emlekezteto/felszolito)
                    foreach (var item in lst)
                    {
                        count++;
                        BackgroundProcess.SetProgress(bpid, "$reminderlettersgenerating_generate".eLogTransl(count, lst.Count));
                        if (BackgroundProcess.NeedStop(bpid)) { BackgroundProcess.Aborted(bpid); break; }

                        item.Invoices = new List<InvoiceInfo>(item.Rows.Select(r => Convert.ToString(r["invoicenumber"])).Distinct().Select(n => new InvoiceInfo() { InvoiceNumber = n }));

                        if (item.Severity != 1 && item.Severity != 2)
                            continue; // ismeretlen tipus

                        try
                        {
                            using (var dSet1 = dSet.Clone())
                            {
                                // az eredeti dataset alapjan letrehozunk egy ujat. Az elso tabla fogja tartalmaz az eredeti elso tabla vonatkozo sorait, a tobbi tabla sorait egy az egyben masoljuk
                                var dt1 = dSet1.Tables[0];
                                foreach (var dr in item.Rows)
                                    dt1.Rows.Add(dr.ItemArray);
                                for (int i = 1; i < dSet.Tables.Count; i++)
                                {
                                    var dtS = dSet.Tables[i];
                                    var dtT = dSet1.Tables[i];
                                    foreach (System.Data.DataRow dr in dtS.Rows)
                                        dtT.Rows.Add(dr.ItemArray);
                                }

                                // PDF eloallitasa
                                var pDict = new Dictionary<string, object>();
                                pDict["reportType"] = repParams.ReportType;
                                pDict["reportFile"] = repParams.ReportFileName;
                                pDict["reportProc"] = repParams.ReportProcName;
                                pDict["sid"] = Session.SidText;
                                pDict["guidText"] = repParams.ReportGuid.ToString();
                                pDict["xmlTypeName"] = repParams.Nspace;
                                pDict["callback"] = printArgs.Callback;
                                pDict["dset"] = dSet1;

                                item.FileName = creator.Generate(pDict);
                                item.Rows = null; // mar nincs szukseg az adatokra
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"{item.PartnCode}: {ex.Message}");
                            if (!(ex is MessageException))
                                Log.Error($"{GetType().FullName} - Partner: {item.PartnCode}, severity: {item.Severity} - {ex}");
                        }
                    }
                }

                // fajlok elnevezese, szukseges szamlak nyomtatasa
                var rootPath = CustomSettings.GetString("ReminderLettersTargetPath");
                if (string.IsNullOrEmpty(rootPath))
                    rootPath = Globals.ReportsTempFolder;
                if (!System.IO.Path.IsPathRooted(rootPath))
                    rootPath = System.IO.Path.Combine(!string.IsNullOrEmpty(Globals.WritableRoot) ? Globals.WritableRoot : Globals.SiteRoot, rootPath);
                var dir = System.IO.Path.Combine(rootPath, date.Year.ToString("G"), date.ToString("yyyy'-'MM'-'dd"));
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                count = 0;
                foreach (var item in lst)
                {
                    count++;
                    BackgroundProcess.SetProgress(bpid, "$reminderlettersgenerating_gen2".eLogTransl(count, lst.Count));
                    if (BackgroundProcess.NeedStop(bpid)) { BackgroundProcess.Aborted(bpid); break; }

                    if (string.IsNullOrEmpty(item.FileName))
                        continue;

                    var fileName = "";
                    if (item.Severity == 1) // emlekezteto
                    {
                        fileName = string.Format("{0}_{1:yyyy'-'MM'-'dd}_emlekezteto.pdf", item.PartnCode, date);
                    }
                    else if (item.Severity == 2) // felszolito
                    {
                        fileName = string.Format("{0}_{1:yyyy'-'MM'-'dd}.pdf", item.PartnCode, date);
                    }

                    fileName = System.IO.Path.Combine(dir, fileName);

                    System.IO.File.Copy(item.FileName, fileName, true);
                    try { System.IO.File.Delete(item.FileName); } catch { }
                    item.FileName = fileName;

                    if (item.Severity == 2) // felszolitas, a szamlat is ki kell nyomtatni
                    {
                        if (item.Invoices.Count > 0)
                        {
                            var spbl = Base.Sales.Sinv.SinvPrintBL.New() as Sales.Sinv.SinvPrintBL3;
                            var spc = new Sales.Sinv.SinvPrintCallback3();
                            var k = Key.InAtToSql.CreateKey(Base.Sales.Sinv.SinvHead.FieldDocnum, item.Invoices.Select(i => i.InvoiceNumber));
                            var sinvHeads = SqlDataAdapter.GetList<Base.Sales.Sinv.SinvHead>(k);
                            foreach (var sinvHead in sinvHeads)
                            {
                                var inv = item.Invoices.FirstOrDefault(i => string.Equals(i.InvoiceNumber, sinvHead.Docnum));
                                if (inv == null)
                                {
                                    // Ugyanaz a szamlaszam tobbszor szerepel a sinvhead-ben. Ezt a szamla(szamo)t a ciklus egy korabbi iteracioja mar feldolgozta.
                                    continue;
                                }

                                if (sinvHead.Sinvdocid == "ATALANY")
                                    inv.PDFFileName = string.Format("{0}_{1:yyyy'-'MM'-'dd}_atalany.pdf", item.PartnCode, date);
                                else if (sinvHead.Sinvdocid == "FELSZOLIT")
                                    inv.PDFFileName = string.Format("{0}_{1:yyyy'-'MM'-'dd}_szamla.pdf", item.PartnCode, date);

                                if (inv.PDFFileName == null || sinvHead.Duedate < dueDate)
                                {
                                    item.Invoices.Remove(inv);
                                    continue; // csak atalanyt vagy felszolitasi dijat kell nyomtatni
                                }
                                inv.Sinvhead = sinvHead;

                                var fnCount = item.Invoices.Count(i => i.PDFFileName != null && System.IO.Path.GetFileNameWithoutExtension(i.PDFFileName).StartsWith(System.IO.Path.GetFileNameWithoutExtension(inv.PDFFileName)));
                                if (fnCount > 1) // ha van mar ilyen nevu fajl (egy partnerhez tobb szamlat kell nyomtatni, ha kiegeszitjuk egy postfix-szel)
                                    inv.PDFFileName = System.IO.Path.GetFileNameWithoutExtension(inv.PDFFileName) + fnCount.ToString("G") + System.IO.Path.GetExtension(inv.PDFFileName);

                                inv.PDFFileName = System.IO.Path.Combine(dir, inv.PDFFileName);

                                var sinvFileName = spc.GetPDFPath(sinvHead.Docnum);
                                var invSaved = System.IO.File.Exists(sinvFileName);
                                if (!invSaved) // ha a szamla le van mentve, akkor nem kell a PDF-et generalni
                                    sinvFileName = spbl.GetOrGeneratePDF(sinvHead: sinvHead); // szamla PDF letrehozasa

                                System.IO.File.Copy(sinvFileName, inv.PDFFileName, true);

                                if (!invSaved)
                                    try { System.IO.File.Delete(sinvFileName); } catch { }
                            }
                        }
                    }
                }

                // letrehozott PDF-ek osszefuzese (hogy a felhasznalonak vissza tudjuk kuldeni)
                var fl1 = lst.Select(i => i.FileName).Distinct();
                var fl2 = lst.SelectMany(i => i.Invoices).Select(i => i.PDFFileName).Where(fileName => !string.IsNullOrEmpty(fileName)).Distinct();
                var fl = fl1.Union(fl2);
                if (fl.Any())
                {
                    BackgroundProcess.SetProgress(bpid, "$reminderlettersgenerating_mergefiles".eLogTransl());
                    var zipFile = ZipFiles(fl);

                    var sett = new eProjectWeb.Framework.UI.Script.DownloadSettings
                    {
                        ServicePrefix = "Services/DownloadService.ashx",
                        RealFileName = System.IO.Path.GetFileName(zipFile),
                        StoredFileName = zipFile,
                        ContentType = "application/zip"
                    };

                    printArgs.PageUpdateArgs.AddExecCommand(new eProjectWeb.Framework.UI.Script.DownloadStep(sett));
                }
            }
            finally
            {
                BackgroundProcess.Completed(bpid);
            }

            if (errors.Count > 0)
                printArgs.PageUpdateArgs.ShowDialog(dlgSimpleMessage, null, string.Join("<br/>", errors));
        }

        protected string ZipFiles(IEnumerable<string> fileNames)
        {
            var packager = eProjectWeb.Framework.Package.PackageUtils.GetPackage(eProjectWeb.Framework.Package.PackageUtils.ZipPackageType, "ZipPackager");
            if (packager == null)
                throw new eProjectWeb.Framework.Package.PackageException("ZipPackager not found!");

            var zipFile = System.IO.Path.Combine(Globals.ReportsTempFolder, Utils.GetTempFilenameWithoutExtension() + ".zip");
            packager.Compress(zipFile, fileNames);

            return zipFile;
        }

        protected class ListItem
        {
            public string PartnCode { get; set; }
            public int Severity { get; set; }
            public IEnumerable<System.Data.DataRow> Rows { get; set; }
            /// <summary>
            /// A sorok alapjan generalt PDF
            /// </summary>
            public string FileName { get; set; }
            public List<InvoiceInfo> Invoices { get; set; }
        }

        protected class InvoiceInfo
        {
            public string InvoiceNumber { get; set; }
            public Base.Sales.Sinv.SinvHead Sinvhead { get; set; }
            public string PDFFileName { get; set; }
        }

        protected bool CustomProcessObj(XmlReader reader, string localName, out object createdObj, out bool requireAttributes)
        {
            return ReportUtils.CustomProcessObj(ReportSettings, reader, localName, out createdObj, out requireAttributes);
        }

        protected override void ReadControlsFromXML(System.Xml.XmlReader reader)
        {
            eProjectWeb.Framework.Xml.XmlUtils.ReadXml(this, reader, null,
                new eProjectWeb.Framework.Xml.ProcessElementObjectDelegate(ProcessObj),
                new eProjectWeb.Framework.Xml.CustomProcessElementValueDelegate(CustomProcessObj), null,
                new eProjectWeb.Framework.Xml.CreateObjectDelegate(ObjectCreator));
        }
    }
}
