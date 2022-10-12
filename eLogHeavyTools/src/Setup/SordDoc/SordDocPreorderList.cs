using eLog.Base.Common;
using eLog.Base.Setup.SordDoc;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Setup.SordDoc
{
    internal class SordDocPreorderList : DefaultListProvider
    {
        public static readonly string ID;

        private static string m_queryString;

        private static ListColumn[] m_columns;

        private static QueryArg[] m_argsTemplate;

        protected bool m_useSessionCmpFilter = false;

        static SordDocPreorderList()
        {
            ID = typeof(SordDocPreorderList).FullName;
            m_queryString = "select sorddocid, name, delstat from " + Entity<Base.Setup.SordDoc.SordDoc>._TableName + " (nolock) ";
            m_columns = new ListColumn[3]
            {
                new ListColumn("sorddocid", 80),
                new ListColumn("name", 240),
                new ListColumn("delstat", 0)
            };

            m_argsTemplate = new QueryArg[]
            {
                new QueryArg("sorddocid", FieldType.String, QueryFlags.MultipleAllowed),
                new QueryArg("cmpid", FieldType.Integer, QueryFlags.MultipleAllowed),
                new QueryArg("cmpcodes", FieldType.String, QueryFlags.MultipleAllowed),
                new QueryArg("type", FieldType.Integer, QueryFlags.Equals),
            };
            DefaultListProvider.SetCustomFunc(m_argsTemplate, "cmpcodes", CommonUtils.CmpCodesCustomFilter);
        }

        public SordDocPreorderList()
            : base(m_queryString, m_columns)
        {
            ValueFieldName = "sorddocid";
            TextFieldName = "name";
            ShowCodeFieldName = "sorddocid";
        }

        protected override string GetQuerySql(Dictionary<string, object> args)
        {
            if (args == null)
            {
                args = new Dictionary<string, object>();
            }
            args.Add("type", 1);

            CommonUtils.ConvertFilterCmpIdToCmpCode(args);
            if (m_useSessionCmpFilter)
            {
                CommonUtils.AddSessionCompaniesFilter(args, "cmpcodes");
            }

            return GetQuerySqlArgs(args, m_argsTemplate);
        }

        public override void ApplyParameters(object[] param)
        {
            base.ApplyParameters(param);
            if (param == null)
            {
                return;
            }

            foreach (object obj in param)
            {
                if (obj != null)
                {
                    string a = obj.ToString();
                    if (string.Equals(a, "sessioncmp"))
                    {
                        m_useSessionCmpFilter = true;
                    }
                }
            }
        }
    }
}
 