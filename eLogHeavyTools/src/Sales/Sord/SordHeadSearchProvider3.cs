using System;
using System.Collections.Generic;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordHeadSearchProvider3 : SordHeadSearchProvider
    {
        protected Type baseType = typeof(SordHeadSearchProvider);

        protected static QueryArg[] m_argsTemplate3 = new QueryArg[]
        {
            new QueryArg("sordapprovalstat", "olc", FieldType.Integer, QueryFlags.MultipleAllowed)
        };

        public SordHeadSearchProvider3()
        {
            this.ArgsTemplate = MergeQueryArgs(this.ArgsTemplate, m_argsTemplate3);
        }

        protected override string GetColorizerID()
        {
            return baseType.Name;
        }

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetSearchXmlFileName()
        {
            return GetNamespaceName() + "." + baseType.Name;
        }
        
        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var query = base.CreateQueryString(args, fmtonly);

            query = query.Replace("--$$morejoins$$", " left join olc_sordhead olc (nolock) on olc.sordid = sh.sordid --$$morejoins$$ ");
            query = query.Replace("--$$morefields$$", " ,olc.sordapprovalstat --$$morefields$$");

            return query;
        }
    }
}