using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using eLog.Base.Masters.Partner;
using eLog.Base.Sales.Sinv; 
using eProjectWeb.Framework.Data;


namespace eLog.HeavyTools.Sales.Retail
{
    internal class GenerateReceipt
    {
        public static string UpdateString(string txt)
        {
            return UpdateChars(txt);
        }

        public static string BySinvid(int sinvid)
        {
            //TODO kisker számla
            return "TODO...";
            /*
            var osh = OlcSinvHead.Load(sinvid);
            osh.LoadNavFields();

            if (osh != null && !StringN.IsNullOrEmpty(osh.Retailinvoiceimage))
            {
                return osh.Retailinvoiceimage.Value;
            }
            
            var sh = SinvHead.Load(sinvid);

            var receipt = new StringBuilder();

            var toPart = Partner.Load(sh.Partnid);
            var toAddr = PartnAddr.Load(sh.Addrid);
            if (sh.Corrtype == 1)
            {
                if (sh.Ptvattypid.Equals("BIZONYLAT"))
                {
                    receipt.AppendLine("Számviteli bizonylat");
                }
                else
                {
                    receipt.AppendLine("Készpénzfizetési számla");
                }
            }
            else
            {
                receipt.AppendLine("Jóváíró számla - Készpénz fizetés");

                var origsh = SinvHead.Load(sh.Origsinvid);
                if (origsh != null)
                {
                    receipt.AppendLine("Eredeti biz.:" + origsh.Docnum);
                }
            }

            receipt.AppendLine("");
            receipt.AppendFormat("Számlaszám: {0}", sh.Docnum);

            receipt.AppendLine("");
            receipt.AppendLine("------------------------------------------");
            receipt.AppendLine("MG Records Zrt.");
            receipt.AppendLine("1188 Budapest, Vezér u. 77/b");
            receipt.AppendLine("25019263-2-43");
            receipt.AppendLine("------------------------------------------");
            receipt.AppendFormat("Vevő: {0}", osh.Partnname);
            receipt.AppendLine();
            if (!string.IsNullOrEmpty(osh.Buyaddrpostcode) ||
                !string.IsNullOrEmpty(osh.Buyaddrcity) ||
                !string.IsNullOrEmpty(osh.Buyadd02) ||
                !string.IsNullOrEmpty(osh.Buyvatnum))
            {
                receipt.AppendFormat("{0} {1}, ", osh.Buyaddrpostcode, UpdateString(osh.Buyaddrcity));
                receipt.AppendLine();
                receipt.AppendFormat("{0}", UpdateString(osh.Buyadd02));
                receipt.AppendLine();
                if (!string.IsNullOrEmpty(osh.Buyvatnum))
                {
                    receipt.AppendLine(osh.Buyvatnum);
                }
            }

            receipt.AppendLine("------------------------------------------");
            receipt.AppendFormat("Számla kelte: {0}", sh.Sinvdate.Value.ToString("yyyy.MM.dd"));
            receipt.AppendLine();
            receipt.AppendLine();
            receipt.AppendLine();
            receipt.AppendLine("Cikkszám                              VTSZ");
            receipt.AppendLine("Megnevezés                                ");
            receipt.AppendLine("Egységár  Db                         Érték");
            receipt.AppendLine("------------------------------------------");

            decimal realtotval = 0;
            decimal totdiscount = 0;

            var sinvlines = SinvLines.Load(sh.PK);
            foreach (ReceptSinvLine line in SummarizeCart(sinvlines.AllRows))
            {
                var i = Base.Masters.Item.Item.Load(line.Itemid);

                var custtarid = "";
                if (StringN.HasValue(i.Custtarid))
                {
                    custtarid = i.Custtarid.Value;
                }
                receipt.AppendLine(UpdateLine(">                                        <", i.Itemcode.Value, custtarid));

                var itemname = i.Name01.Value;

                var color1 = SqlDataAdapter.ExecuteSingleValue(DB.Main, string.Format(@"
select top 1 pl.name 
from ols_itemprop ip 
join ols_propline pl on pl.propgrpid=ip.propgrpid and pl.value=ip.value
where ip.propgrpid=6 and ip.itemid={0}
", line.Itemid));
                if (color1 != null && color1 != DBNull.Value)
                {
                    itemname += " - " + color1;
                }

                var name = UpdateString(itemname);

                var max = 40;
                if (name.Length < max)
                {
                    max = name.Length;
                }
                receipt.AppendLine(name.Substring(0, max));

                realtotval += line.Seltotprc * line.Sinvqty;
                totdiscount += -line.Disctotval * line.Sinvqty;

                receipt.AppendLine(UpdateLine("        <  >                             <",
                              DecToIntToString(line.Seltotprc),
                              DecToIntToString(line.Sinvqty),
                              DecToString(line.Totval, 0)));

            }

            var totval = realtotval - totdiscount;
            var payval = (sh.Payval.Value);

            receipt.AppendLine("------------------------------------------");
            receipt.AppendLine(UpdateLine("Áru érték :                              <", DecToIntToString(realtotval)));
            //receipt.AppendLine(UpdateLine("Engedmény :                            <", DecToIntToString(totdiscount)));
            receipt.AppendLine("------------------------------------------");
            receipt.AppendLine(UpdateLine("Összesen  :                              <", DecToIntToString(totval)));
            receipt.AppendLine(UpdateLine("Kp. kerek.:                              <", DecToIntToString(payval - totval)));
            receipt.AppendLine("------------------------------------------");
            receipt.AppendLine(UpdateLine("Fizetendő  :                             <", DecToIntToString(payval)));

            receipt.AppendLine("");
            receipt.AppendLine("");
            receipt.AppendLine("ÁFA tartalom");
            receipt.AppendLine("ÁFA kulcs      Alap       ÁFA      Bruttó");
            receipt.AppendLine("------------------------------------------");
            decimal taxnetval = 0;
            decimal taxtaxval = 0;
            decimal taxtotval = 0;

            var taxs = SinvTaxes.Load(sh.PK);
            foreach (SinvTax tax in taxs.AllRows)
            {
                receipt.AppendLine(UpdateLine(">                  <          <          <", tax.Taxid.Value,
                                              DecToIntToString(tax.Netval.Value),
                                              DecToIntToString(tax.Taxval.Value),
                                              DecToIntToString(tax.Totval.Value)
                                       ));
                taxnetval += tax.Netval.Value;
                taxtaxval += tax.Taxval.Value;
                taxtotval += tax.Totval.Value;
            }

            receipt.AppendLine("-----------------------------------------");
            receipt.AppendLine(UpdateLine("Összesen           <          <         <",
                                          DecToIntToString(taxnetval),
                                          DecToIntToString(taxtaxval),
                                          DecToIntToString(taxtotval)));
            receipt.AppendLine("");
            receipt.AppendLine("");

            receipt.AppendLine("");
            receipt.AppendLine("");
            receipt.AppendLine(UpdateLine("                     =                    ", "Köszönjük a vásárlást!"));

            receipt.AppendLine();
            receipt.AppendLine();

            receipt.AppendLine("    " + string.Format("#Barcode:{0}#", sh.Docnum));
            receipt.AppendLine();
            receipt.AppendLine();


            var receiptData = UpdateChars(receipt.ToString().Replace(Environment.NewLine, "<br>"));
            osh = OlcSinvHead.Load(sh.Sinvid);
            if (osh == null)
            {
                osh = OlcSinvHead.CreateNew();
                osh.Sinvid = sh.Sinvid;
            }
            osh.Retailinvoiceimage = receiptData;
            osh.Save();
          
            return receiptData;  */
        }

        private static IEnumerable<ReceptSinvLine> SummarizeCart(IEnumerable<DataRow> allRows)
        {
            var newItems = new List<ReceptSinvLine>();
            foreach (SinvLine sl in allRows)
            {
                Mergeitem(newItems, sl);
            }
            return newItems;
        }

        private static void Mergeitem(List<ReceptSinvLine> newItems, SinvLine sl)
        {
            /*
            foreach (var item in newItems)
            {
                if ((item.Itemid == sl.Itemid && item.Seltotprc == sl.Seltotprc && item.Disctotval == sl.Disctotval ))
                {
                    item.Sinvqty++;
                    item.Sinvlineids.Add(sl.Sinvlineid.Value);
                    return;
                }
            }
            */
            var rsl = new ReceptSinvLine()
            {
                Disctotval = sl.Disctotval.Value,
                Itemid = sl.Itemid.Value,
                Seltotprc = sl.Seltotprc.Value,
                Sinvqty = sl.Sinvqty.Value,
                Totval = sl.Totval.Value,
            };

            rsl.Sinvlineids.Add(sl.Sinvlineid.Value);
            newItems.Add(rsl);
        }

        public static string DecToIntToString(decimal? value)
        {
            if (value == null)
            {
                value = 0;
            }
            return Decimal.ToInt32(value.Value).ToString("N0").Replace(',', '.').Replace(' ', ',');
        }

        private static string DecToString(decimal? value, int round)
        {
            if (value == null)
            {
                value = 0;
            }
            return value.Value.ToString("N" + round).Replace(',', '.').Replace(' ', ',');
        }

        private static string UpdateLine(string text, params string[] data)
        {
            return new ReceiptLine().Fill(text, data);
        }


        private static readonly Dictionary<char, char> _replacements = new Dictionary<char, char>()
                                                                           {
                                                                               /*{'Ö', 'O'},
                                                                               {'Ü', 'U'},
                                                                               {'Ó', 'O'},
                                                                               {'Ő', 'O'},
                                                                               {'Ú', 'U'},
                                                                               {'É', 'E'},
                                                                               {'Á', 'A'},
                                                                               {'Ű', 'U'},

                                                                               {'ö', 'o'},
                                                                               {'ü', 'u'},
                                                                               {'ó', 'o'},
                                                                               {'ő', 'o'},
                                                                               {'ú', 'u'},
                                                                               {'é', 'e'},
                                                                               {'á', 'a'},
                                                                               {'ű', 'u'},*/

                                                                               {'=', '§'},
                                                                           };

        private static string UpdateChars(string text)
        {
            var replaced = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                replaced.Append(_replacements.ContainsKey(character) ? _replacements[character] : character);
            }
            return replaced.ToString();
        }
    }

    internal class ReceiptLine
    {
        private const string Controllers = "<>=";

        private List<ReceiptLineItem> _texts;
        private int _length;
        private string _defalutText;

        public string Fill(string text, params string[] data)
        {
            _defalutText = text;
            _length = text.Length;
            var dataIndex = -1;
            _texts = new List<ReceiptLineItem>();
            for (int i = 0; i < text.Length; i++)
            {
                char curChar = text[i];
                if (Controllers.Contains(curChar.ToString()))
                {
                    dataIndex++;
                    _texts.Add(new ReceiptLineItem(i, curChar, data[dataIndex]));
                }
            }
            return GenerateReceipt.UpdateString(Render());
        }

        private string Render()
        {
            _defalutText = _defalutText.Replace(">", " ").Replace("<", " ").Replace("=", " ");
            var chars = new string[_length];
            for (var i = 0; i < _length; i++)
            {
                chars[i] = _defalutText[i].ToString();
            }
            foreach (var item in _texts)
            {
                var start = item.Start;
                if (item.Controller == '<')
                {
                    start = start - item.Data.Length;
                }
                if (item.Controller == '=')
                {
                    start = start - (item.Data.Length / 2);
                }

                for (int i = 0; i < item.Data.Length; i++)
                {
                    if (start + i < _length - 1 && start + i > -1)
                    {
                        chars[start + i] = item.Data[i].ToString();
                    }
                }
            }
            return string.Join("", chars);
        }
    }

    internal class ReceiptLineItem
    {
        public char Controller;
        public string Data;
        public int Start;

        public ReceiptLineItem(int start, char controller, string data)
        {
            Start = start;
            Controller = controller;
            if (string.IsNullOrEmpty(data))
            {
                data = "";
            }
            Data = data;
        }


    }

    internal class ReceptSinvLine
    {
        public List<int> Sinvlineids = new List<int>();
        public int Itemid { get; set; }
        public decimal Seltotprc { get; set; }
        public decimal Sinvqty { get; set; }
        public decimal Totval { get; set; }
        public decimal Disctotval { get; set; }
        public int? Ptid { get; set; }
    }

}