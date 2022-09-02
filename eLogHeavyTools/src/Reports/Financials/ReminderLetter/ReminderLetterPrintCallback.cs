using eProjectWeb.Framework;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Reports.Barcode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.ReminderLetter
{
    public class ReminderLetterPrintCallback : IReportCallbackProvider
    {
        public static readonly string ID = typeof(ReminderLetterPrintCallback).FullName;

        public static ReminderLetterPrintCallback New()
        {
            return (ReminderLetterPrintCallback)ObjectFactory.New(typeof(ReminderLetterPrintCallback));
        }

        protected virtual void ConvertPayvalToText(ReportCallbackExecuteArgs e)
        {
            System.Data.DataTable headTable = e.DataSet.Tables[0];

            bool payvaltext = headTable.Columns.Contains("payval_text") && headTable.Columns.Contains("payval_lngid") && headTable.Columns.Contains("payval");
            bool titleTranslate = headTable.Columns.Contains("title") && headTable.Columns.Contains("lngid");
            bool copyTranslate = headTable.Columns.Contains("copytext") && headTable.Columns.Contains("lngid");

            if (payvaltext || titleTranslate || copyTranslate)
            {
                foreach (DataRow dr in headTable.Rows)
                {
                    if (payvaltext) // fizetendo ertek kiirasa betuvel
                    {
                        var lngId = ConvertUtils.ToString(dr["payval_lngid"]);
                        if (!string.IsNullOrEmpty(lngId))
                        {
                            NumToStr converter = NumToStr.Create(lngId);
                            decimal? payval = ConvertUtils.ToDecimal(dr["payval"]);
                            dr["payval_text"] = converter.Convert(payval.GetValueOrDefault(), 0);
                        }
                    }
                }
            }
        }

        #region IReportCallbackProvider Members

        protected ReminderLetterPrintCallback()
        {
        }

        public virtual void Execute(ReportCallbackExecuteArgs e)
        {
            if (e.CallbackPos == ReportCallbackPos.BeforeSet)
            {
                // fizetendo kiirasa stoveggel
                ConvertPayvalToText(e);
            }

            if (e.CallbackPos == eProjectWeb.Framework.ReportCallbackPos.BeforeSet)
            {
                if (e.DataSet.Tables.Count == 0)
                    return;
                System.Data.DataTable dtHead = e.DataSet.Tables[0];
                if (dtHead.Rows.Count == 0)
                    return;


                if (dtHead.Columns.Contains("qr_show") && ConvertUtils.ToInt32(dtHead.Rows[0]["qr_show"]) == 1)
                {
                    var QRCodesGenerator = new Common.MagyarPostaCheckQRCode();
                    var lst = new Common.MagyarPostaCheckQRDataList();
                    var tetelAzonosito = 1;
                    foreach (DataRow drHead in dtHead.Rows)
                    {
                        if (dtHead.Columns.Contains("qr_bcimage") && dtHead.Columns.Contains("qr_show")) // QR kod
                        {
                            var qrCodeImage = drHead["qr_bcimage"];
                            var curId = Convert.ToString(drHead["remcur"]);
                            var show = Convert.ToInt32(drHead["qr_show"]);
                            if (qrCodeImage == DBNull.Value && curId == "HUF" && show == 1)
                            {
                                var bizAzon = 1.ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(7, '0'); //Convert.ToInt32(dr["sinvid"]).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(7, '0');
                                bizAzon = bizAzon.Substring(bizAzon.Length - 7, 7);
                                var bankAccNo = Convert.ToString(drHead["bankaccno"]);
                                bankAccNo = bankAccNo.Replace("-", "").Replace(" ", "");

                                lst.Add(new Common.MagyarPostaCheckQRData()
                                {

                                    tetelAzonosito = "rem-" + tetelAzonosito++, //Convert.ToInt32(dr["sinvid"]),
                                    strukturaAzonosito = "P1",
                                    szamlaAzonosito = Convert.ToString(drHead["ref1"]), // Convert.ToString(dr["docnum"]),
                                    szamlaKelte = Convert.ToDateTime(DateTime.Today.AddDays(-5)), //Convert.ToDateTime(dr["sinvdate"]),
                                    fizetesiHatarido = Convert.ToDateTime(DateTime.Today), // Convert.ToDateTime(dr["duedate"]),
                                    befizetoAzonosito = Convert.ToString(drHead["payerid"]).PadLeft(24, '0'),
                                    osszeg = Convert.ToInt32(drHead["payval"]),
                                    devizanem = "348", // HUF
                                    szamlaszam = bankAccNo,
                                    tranzakcioKod = "51",
                                    gyartoKod = "606",
                                    bizonylatAzonosito = bizAzon + Common.CommonUtils.PayerIdCDVGen(bizAzon),
                                    outputKod = "31",
                                    kozlemeny = eProjectWeb.Framework.Lang.Translator.TranslateNspace("$magyarpostacheck_message", this.GetType().Namespace, drHead["ref1"] /*dr["docnum"]*/)
                                });

                            }
                        }
                    }

                    byte[] qrCodeBytes = null;
                    var QRCodes = QRCodesGenerator.Generate(lst);

                    foreach (DataRow drHead in dtHead.Rows)
                    {

                        if (QRCodes.Items.Count > 0)
                        {
                            qrCodeBytes = QRCodes.Items.Where(x=>x.Data.befizetoAzonosito== Convert.ToString(drHead["payerid"]).PadLeft(24, '0')).Select(x=>x.PngImageBytes).FirstOrDefault();

                            if (qrCodeBytes != null)
                                drHead["qr_bcimage"] = qrCodeBytes;
                        }
                        else if (QRCodes.Log != null)
                            throw new MessageException(string.Join("<br/>", QRCodes.Log));
                    }
                }

                var bd = new List<BarcodeGeneratintData>();

                foreach (DataColumn column in dtHead.Columns)
                {
                    if (!column.ColumnName.StartsWith("qr_") && column.ColumnName.Contains("_bc"))
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

                foreach (DataRow drHead in dtHead.Rows)
                {
                    foreach (var generatintData in bd)
                    {
                        drHead[generatintData.ImagefieldFieldName] =
                            GenerateBarcode(drHead[generatintData.BarcodeFieldName].ToString(),
                                            drHead[generatintData.TypeFieldName].ToString());
                    }
                }
            }
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

        public void Prepare(ReportCallbackPrepareArgs e)
        {

        }

        public void BeforeSave(ReportCallbackSaveArgs e)
        {

        }

        #endregion
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
