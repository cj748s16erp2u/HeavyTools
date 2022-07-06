using System;
using System.Collections.Generic;
using System.Text;
using eLog.Base.Common;
using eLog.Base.Masters.Partner;
using eLog.Base.Setup.Company;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    // a base-ben EmployeeList.staticmezo hivatkozasok vannak, igy nem nagyon tudom normal modon felulirni, de uj nevet sem nagyon adnek a listanak
    // legalabbis a szokasos modon felulirva nem hajlando mukodni
    public class EmployeeList3 : DefaultListProvider
    {
        public static readonly string ID = typeof(EmployeeList3).FullName;
        protected static string m_queryString = "select e.empid, $$empname$$ empname\r\n $$moreFields$$ from ols_employee e (nolock)\r\n  join ols_partner p (nolock) on p.partnid = e.partnid\r\n";
        protected static QueryArg[] m_argsTemplate = new QueryArg[10]
        {
      new QueryArg("empid", "e", FieldType.Integer),
      new QueryArg("partnid", "e", FieldType.Integer, QueryFlags.MultipleAllowed),
      new QueryArg("cmpid", "e", FieldType.Integer, QueryFlags.MultipleAllowed),
      new QueryArg("partncmpid", FieldType.Integer, QueryFlags.Equals),
      new QueryArg("firstname", "e", FieldType.String, QueryFlags.Like),
      new QueryArg("lastname", "e", FieldType.String, QueryFlags.Like),
      new QueryArg("empname", "e", FieldType.String, QueryFlags.Like),
      new QueryArg("cmpemponly", FieldType.Boolean),
      new QueryArg("cmpcodes", "e", FieldType.String, QueryFlags.MultipleAllowed),
      new QueryArg("type", "e", FieldType.Integer, QueryFlags.BitwiseAnd)
        };
        protected static ListColumn[] m_columns = new ListColumn[2]
        {
      new ListColumn("empid", 0),
      new ListColumn("empname", 240)
        };
        protected int? m_typefilter = new int?();
        protected bool m_cmpEmpOnly = false;
        protected int[] m_cmpidFilter;
        protected string m_cmpCodesFilter;

        public EmployeeList3()
          : base(m_queryString, m_columns)
        {
            this.ValueFieldName = "empid";
            this.TextFieldName = "empname";
            this.ShowCodeFieldName = "";
            DefaultListProvider.SetCustomFunc(m_argsTemplate, "empname", new QueryArg.CustomArgStringDelegate(this.EmpnameCustomFilter));
            DefaultListProvider.SetCustomFunc(m_argsTemplate, "cmpemponly", new QueryArg.CustomArgStringDelegate(this.CmpEmpOnlyCustomFilter));
            DefaultListProvider.SetCustomFunc(m_argsTemplate, "cmpcodes", new QueryArg.CustomArgStringDelegate(this.CmpCodesCustomFilter));
        }

        public override void ApplyParameters(object[] param)
        {
            base.ApplyParameters(param);
            foreach (object obj in param)
            {
                if (obj is string && string.Equals("cmpemponly", (string)obj, StringComparison.OrdinalIgnoreCase))
                    this.m_cmpEmpOnly = true;
                if (obj != null)
                {
                    string lower = obj.ToString().ToLower();
                    if (lower.StartsWith("type="))
                        this.m_typefilter = new int?(int.Parse(lower.Substring(5)));
                }
            }
        }

        public void EmpnameCustomFilter(
          StringBuilder sb,
          QueryArg arg,
          string quotedFieldName,
          object argValue)
        {
            if (!(argValue is string) || string.IsNullOrEmpty((string)argValue))
                return;
            sb.AppendLine(this.GetEmpname() + " like '" + (string)argValue + "%'");
        }

        protected void CmpEmpOnlyCustomFilter(
          StringBuilder sb,
          QueryArg arg,
          string quotedFieldName,
          object argValue)
        {
            if (!ConvertUtils.ToBoolean(argValue).GetValueOrDefault(false))
                return;
            sb.AppendFormat("(p.cmpcodes = {0} or exists (select 0\r\n  from ols_company c (nolock)\r\n  where p.cmpcodes like '%' + c.cmpcode + '%'\r\n", (object)Utils.SqlToString((object)"*"));
            if (this.m_cmpidFilter != null)
                sb.AppendFormat("    and c.cmpid in ({0}))", (object)Utils.SqlToString((object)Utils.SqlInToString<int>((IEnumerable<int>)this.m_cmpidFilter)));
            else if (this.m_cmpCodesFilter != null)
                sb.AppendFormat("    and {0} like '%' + c.cmpcode + '%') or {0} = {1}", (object)Utils.SqlToString((object)this.m_cmpCodesFilter), (object)Utils.SqlToString((object)"*"));
            sb.AppendLine(")");
        }

        protected void CmpCodesCustomFilter(
          StringBuilder sb,
          QueryArg arg,
          string quotedFieldName,
          object argValue)
        {
            if (argValue == null)
                return;
            sb.AppendLine("(e.cmpid = -1 or exists (select 0\r\n  from ols_company c (nolock)\r\n  where e.cmpid = c.cmpid");
            if (this.m_cmpidFilter != null)
                sb.AppendFormat("    and c.cmpid in ({0}))", (object)Utils.SqlToString((object)Utils.SqlInToString<int>((IEnumerable<int>)this.m_cmpidFilter)));
            else if (this.m_cmpCodesFilter != null)
                sb.AppendFormat("    and {0} like '%' + c.cmpcode + '%') or {0} = {1}", (object)Utils.SqlToString((object)this.m_cmpCodesFilter), (object)Utils.SqlToString((object)"*"));
            sb.AppendLine(")");
        }

        protected override void BeforeSetupWhereFromArgs(
          Dictionary<string, object> args,
          QueryArg[] argsTemplate,
          ref string sql)
        {
            base.BeforeSetupWhereFromArgs(args, argsTemplate, ref sql);
            if (!this.m_typefilter.HasValue)
                return;
            args["type"] = (object)this.m_typefilter;
        }

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            if (args == null)
                args = new Dictionary<string, object>();
            if (args.ContainsKey("partncmpid"))
            {
                eLog.Base.Setup.Company.Company company = CompanyCache.Get(Convert.ToInt32(args["partncmpid"]));
                if (company != null)
                    args["partnid"] = (object)company.Partnid;
                args.Remove("partncmpid");
            }
            if (args.ContainsKey("cmpid2"))
            {
                args["cmpid"] = args["cmpid2"];
                this.m_cmpEmpOnly = true;
                args.Remove("cmpid2");
            }
            if (args.ContainsKey("cmpcodes2"))
            {
                args["cmpcodes"] = args["cmpcodes2"];
                this.m_cmpEmpOnly = true;
                args.Remove("cmpcodes2");
            }
            if (args.ContainsKey("cmpcodes"))
            {
                CommonUtils.AddSessionCompaniesFilter(args, "cmpcodes");
                object obj = args["cmpcodes"];
                if (obj is string)
                    this.m_cmpCodesFilter = (string)obj;
                else if (obj is string[])
                    this.m_cmpCodesFilter = string.Join("", (string[])obj);
            }
            if (args.ContainsKey("cmpid"))
            {
                CommonUtils.AddSessionCompaniesFilter(args, "cmpid");
                this.m_cmpidFilter = (int[])args["cmpid"];
            }
            args["cmpemponly"] = (object)this.m_cmpEmpOnly;
            var q = (this.GetQuerySqlArgs(args, m_argsTemplate) + "\n  and (rtrim($$empname$$) <> '')").Replace("$$empname$$", this.GetEmpname()) + "\norder by 2";

            if (args.ContainsKey("emptype"))
            {
                if (q.Contains(" where "))
                {
                    q = q.Replace(" where ", $" where e.type={Utils.SqlToString(args["emptype"])} and ");
                }
                else
                {
                    q += $" where e.type={Utils.SqlToString(args["emptype"])}";
                }

                q = q.Replace("$$moreFields$$", ", e.type emptype");
            }

            q = q.Replace("$$moreFields$$", string.Empty);

            return q;
        }

        private string GetEmpname() => Session.CultureInfo.ThreeLetterISOLanguageName.Equals("hun", StringComparison.OrdinalIgnoreCase) ? "e.lastname + ' ' + e.firstname" : "e.firstname + ' ' + e.lastname";
    }
}