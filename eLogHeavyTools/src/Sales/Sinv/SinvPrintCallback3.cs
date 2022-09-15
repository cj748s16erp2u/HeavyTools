using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using System.Data;
using eProjectWeb.Framework.Reports.Barcode;

namespace eLog.HeavyTools.Sales.Sinv
{
    public class SinvPrintCallback3: Base.Sales.Sinv.SinvPrintCallback
    {
        public override void Execute(ReportCallbackExecuteArgs e)
        {
            base.Execute(e);

            if (e.CallbackPos == eProjectWeb.Framework.ReportCallbackPos.BeforeSet)
            {
                if (e.DataSet.Tables.Count == 0)
                    return;
                System.Data.DataTable dtHead = e.DataSet.Tables[0];
                if (dtHead.Rows.Count == 0)
                    return;

                var bd = new List<BarcodeGeneratintData>();

                foreach (DataColumn column in dtHead.Columns)
                {
                    if (column.ColumnName.Contains("_bc"))
                    {
                        var fieldname = column.ColumnName.Substring(0, column.ColumnName.Length - 3);
                        if (dtHead.Columns.Contains(fieldname + "_bcimage") &&
                            dtHead.Columns.Contains(fieldname + "_bctype"))
                        {
                            bd.Add(new BarcodeGeneratintData(column.ColumnName, fieldname + "_bcimage",
                                                             fieldname + "_bctype"));
                        }
                    }
                }

                byte[] qrCodeBytes = null;
                if (dtHead.Columns.Contains("qr_bcimage") && dtHead.Columns.Contains("qr_show")) // QR kod
                {
                    var dr = dtHead.Rows[0];
                    var qrCodeImage = dr["qr_bcimage"];
                    var curId = Convert.ToString(dr["curid"]);
                    var show = Convert.ToInt32(dr["qr_show"]);
                    var payVal = Convert.ToInt32(dr["payval"]);
                    if (qrCodeImage == DBNull.Value && curId == "HUF" && payVal > 0 && show == 1)
                    {
                        var bizAzon = Convert.ToInt32(dr["sinvid"]).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(7, '0');
                        bizAzon = bizAzon.Substring(bizAzon.Length - 7, 7);
                        var bankAccNo = Convert.ToString(dr["seller_bankaccno"]);
                        bankAccNo = bankAccNo.Replace("-", "").Replace(" ", "");

                        var x = new Common.MagyarPostaCheckQRCode();
                        var lst = new Common.MagyarPostaCheckQRDataList();
                        lst.Add(new Common.MagyarPostaCheckQRData()
                        {
                            tetelAzonosito = "sinv-" + Convert.ToInt32(dr["sinvid"]),
                            strukturaAzonosito = "P1",
                            szamlaAzonosito = Convert.ToString(dr["docnum"]),
                            szamlaKelte = Convert.ToDateTime(dr["sinvdate"]),
                            fizetesiHatarido = Convert.ToDateTime(dr["duedate"]),
                            befizetoAzonosito = Convert.ToString(dr["payerid"]).PadLeft(24, '0'),
                            osszeg = payVal,
                            devizanem = "348", // HUF
                            szamlaszam = bankAccNo,
                            tranzakcioKod = "51",
                            gyartoKod = "606",
                            bizonylatAzonosito = bizAzon + Common.CommonUtils.PayerIdCDVGen(bizAzon),
                            outputKod = "31",
                            kozlemeny = eProjectWeb.Framework.Lang.Translator.TranslateNspace("$magyarpostacheck_message", this.GetType().Namespace, dr["docnum"])
                        });

                        var y = x.Generate(lst);

                        if (y.Items.Count > 0)
                        {
                            qrCodeBytes = y.Items[0].PngImageBytes;
                        }
                        else if (y.Log != null)
                            throw new MessageException(string.Join("<br/>", y.Log));
                    }
                }

                foreach (DataRow drHead in dtHead.Rows)
                {
                    foreach (var generatintData in bd)
                    {
                        drHead[generatintData.ImagefieldFieldName] =
                            GenerateBarcode(drHead[generatintData.BarcodeFieldName].ToString(),
                                            drHead[generatintData.TypeFieldName].ToString());
                    }

                    if (qrCodeBytes != null)
                        drHead["qr_bcimage"] = qrCodeBytes;
                }

                // megjegyzesben fix szoveg forditasa - formatum: <$sinv_note_consumpplcname>
                if (dtHead.Columns.Contains("shnote"))
                {
                    var rx = new System.Text.RegularExpressions.Regex(@"<(\$\w+)>", System.Text.RegularExpressions.RegexOptions.Multiline);
                    foreach (DataRow drHead in dtHead.Rows)
                    {
                        var note = ConvertUtils.ToString(drHead["shnote"]);
                        if (!string.IsNullOrEmpty(note))
                        {
                            var note2 = rx.Replace(note, m =>
                            {
                                var s = eProjectWeb.Framework.Lang.Translator.TranslateNspace(m.Groups[1].Value, GetType().Namespace);
                                return s;
                            });

                            if (!string.Equals(note2, note))
                                drHead["shnote"] = note2;
                        }
                    }
                }
            }

            // eredeti peldany tarolasa egy konyvtarban
            if (e.CallbackPos == ReportCallbackPos.AfterExport && e.DataSet.Tables.Count > 0)
            {
                System.Data.DataTable dtHead = e.DataSet.Tables[0];
                if (dtHead.Rows.Count > 0 &&
                    string.Equals(System.IO.Path.GetExtension(e.ExportFile), ".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    var drHead = dtHead.Rows[0];
                    var sinvStat = Convert.ToInt32(drHead["sinvstat"]);
                    if (sinvStat >= 100)
                        SavePDF(e.ExportFile, drHead);
                }
            }
        }

        protected string SafeFileName(string fileName)
        {
            var s = "";
            foreach (var c in fileName)
            {
                if ("<>:\"\\|?*".Contains("" + c)) // torlendo
                    continue;
                else if ("/".Contains("" + c)) // alhuzasra cserelendo
                    s += "_";
                else
                    s += c;
            }
            return s;
        }

        protected void SavePDF(string fileName, DataRow dr)
        {
            var docnum = Convert.ToString(dr["docnum"]);
            var f = GetPDFPath(docnum);
            if (!string.IsNullOrEmpty(f) && !System.IO.File.Exists(f))
                System.IO.File.Copy(fileName, f);
        }

        public string GetPDFPath(string sinvDocNum)
        {
            var folder = CustomSettings.GetString("Invoice-Folder");
            if (string.IsNullOrEmpty(folder))
                return null;

            if (!System.IO.Path.IsPathRooted(folder))
                folder = System.IO.Path.GetFullPath(System.IO.Path.Combine(Globals.SiteRoot, folder));

            var f = SafeFileName(sinvDocNum);

            f = System.IO.Path.Combine(folder, f + ".pdf");
            return f;
        }

        private byte[] GenerateBarcode(string barcode, string type)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return null;
            }
            switch (type)
            {
                case "Code39":
                    return new Barcode_Code39 { Barcode = Barcode_Code39.UpdateBarcode(barcode) }.GetBitmapBytes();
                case "Code128B":
                    return new BarcodeCode128 { Barcode = barcode, }.GetBitmapBytes();
                case "DataMatrix":
                    return BarcodeDatamatrix.GetBitmapBytes(barcode);
                case "PDF417":
                    return new PDF417().GetBitmapBytes(barcode);
                case "qrcodeH":
                    return QRCode.GetBitmapBytes(barcode, QRErrorCorrectLevel.H);
                default:
                    throw new MessageException("$missingBarcodeType");
            }
        }
    }

    internal class BarcodeGeneratintData
    {
        public BarcodeGeneratintData(string barcode, string imagefield, string type)
        {
            BarcodeFieldName = barcode;
            ImagefieldFieldName = imagefield;
            TypeFieldName = type;
        }

        public string TypeFieldName { get; set; }
        public string ImagefieldFieldName { get; set; }
        public string BarcodeFieldName { get; set; }
    }
}
