using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Retail
{
    public class RetailVatnumCheck
    {
        public virtual void CheckPartnerVatNum(System.Xml.Linq.XElement vns, String vatnum)
        {
            if (vns != null)
            {
                var fill = vns.Element("fill");
                CheckVatNumFill(vatnum, fill, "$partner_vatnummandatory", "$partner_vatnumforbidden", Base.Masters.Partner.Partner.FieldVatnum);

                var format = vns.Element("format");
                CheckVatNumFormat(vatnum, format, "$partner_vatnumformat", Base.Masters.Partner.Partner.FieldVatnum);

                if (vns.Element("huncheckdigit") != null)
                    CheckVatNumHUNDigit(vatnum, "$partner_vatnumhundigit", Base.Masters.Partner.Partner.FieldVatnum);
            }
        }
        public virtual void CheckPartnerVatNumEU(System.Xml.Linq.XElement vns, String vatnum)
        {
            if (vns != null)
            {
                var fill = vns.Element("fill");
                CheckVatNumFill(vatnum, fill, "$partner_vatnumeumandatory", "$partner_vatnumeuforbidden", Base.Masters.Partner.Partner.FieldVatnumeu);

                var format = vns.Element("format");
                CheckVatNumFormat(vatnum, format, "$partner_vatnumeuformat", Base.Masters.Partner.Partner.FieldVatnumeu);
            }
        }
        public virtual void CheckPartnerGroupVatNum(System.Xml.Linq.XElement vns, String groupvatnum, String vatnum)
        {
            if (vns != null)
            {
                var fill = vns.Element("fill");
                CheckVatNumFill(groupvatnum, fill, "$partner_groupvatnummandatory", "$partner_groupvatnumforbidden", Base.Masters.Partner.Partner.FieldGroupvatnum);

                var format = vns.Element("format");
                CheckVatNumFormat(groupvatnum, format, "$partner_groupvatnumformat", Base.Masters.Partner.Partner.FieldGroupvatnum);

                if (vns.Element("huncheckdigit") != null)
                    CheckVatNumHUNDigit(groupvatnum, "$partner_groupvatnumhundigit", Base.Masters.Partner.Partner.FieldGroupvatnum);

                if (vns.Element("huncheck1") != null)
                    CheckPartnerVatNumHUN1(groupvatnum, vatnum);
            }
        }

        public void CheckVatNumFill(eProjectWeb.Framework.Rules.RuleValidateContext ctx, string vatnum, System.Xml.Linq.XElement fill, string msgMandatory, string msgForbidden, eProjectWeb.Framework.Data.Field field)
        {
            if (fill != null)
            {
                if (fill.Value == "mandatory")
                {
                    if (string.IsNullOrEmpty(vatnum))
                        ctx.AddErrorField(field, msgMandatory);
                }
                else if (fill.Value == "forbidden")
                {
                    if (!string.IsNullOrEmpty(vatnum))
                        ctx.AddErrorField(field, msgForbidden);
                }
            }
        }

        protected void CheckVatNumFill(string vatnum, System.Xml.Linq.XElement fill, string msgMandatory, string msgForbidden, eProjectWeb.Framework.Data.Field field)
        {
            if (fill != null)
            {
                if (fill.Value == "mandatory")
                {
                    if (string.IsNullOrEmpty(vatnum))
                        throw new MessageException(msgMandatory);
                }
                else if (fill.Value == "forbidden")
                {
                    if (!string.IsNullOrEmpty(vatnum))
                        throw new MessageException(msgForbidden);
                }
            }
        }
        protected void CheckVatNumFormat(string vatnum, System.Xml.Linq.XElement format, string msgFormat, eProjectWeb.Framework.Data.Field field)
        {
            if (format != null)
            {
                if (!string.IsNullOrEmpty(vatnum))
                {
                    var rx = new System.Text.RegularExpressions.Regex(format.Value);
                    if (!rx.IsMatch(vatnum))
                        throw new MessageException(msgFormat);
                }
            }
        }
        protected void CheckVatNumHUNDigit(string vatnum, string msg, eProjectWeb.Framework.Data.Field field)
        {
            if (string.IsNullOrEmpty(vatnum))
                return;

            var rx = new System.Text.RegularExpressions.Regex("^[0-9]{8}-[1-5]-[0-9][0-9]$");
            if (rx.IsMatch(vatnum))
            {
                var id = vatnum.Substring(0, 7);
                var csref = "" + vatnum[7];

                var cs = 0;
                var x = new int[] { 9, 7, 3, 1 };
                for (int i = 0; i < id.Length; i++)
                    cs += x[i % 4] * int.Parse("" + id[i]);
                cs %= 10;
                if (cs != 0)
                    cs = 10 - cs;

                if (!string.Equals(csref, cs.ToString()))
                    throw new MessageException(msg);
            }
        }


        protected void CheckPartnerVatNumHUN1(string groupvanum, string vatnum)
        {
            var rx4 = new System.Text.RegularExpressions.Regex("^[0-9]{8}-4-[0-9][0-9]$");
            var rx5 = new System.Text.RegularExpressions.Regex("^[0-9]{8}-5-[0-9][0-9]$");

            if (!string.IsNullOrEmpty(vatnum))
            {
                if (rx4.IsMatch(vatnum))
                {
                    // az adoszam mezoben csoport tag adoszam van, a csoportadoszamot kotelezo megadni
                    if (string.IsNullOrEmpty(groupvanum))
                    {
                        throw new MessageException("$partner_vatnumhungroupmissing");
                    }
                }

                if (rx5.IsMatch(vatnum))
                {
                    // az adoszam mezoben csoportadoszam van
                    throw new MessageException("$partner_vatnumhungroupformat");
                }
            }

            if (!string.IsNullOrEmpty(groupvanum) && rx5.IsMatch(groupvanum))
            {
                if (string.IsNullOrEmpty(vatnum) || !rx4.IsMatch(vatnum))
                {
                    // az adoszam mezoben csoport tag adoszam van, a csoportadoszamot kotelezo megadni 
                    if (string.IsNullOrEmpty(vatnum))
                    {
                        throw new MessageException("$partner_vatnumhungroupmissing");
                    }
                }
            }
        }
    }
}
