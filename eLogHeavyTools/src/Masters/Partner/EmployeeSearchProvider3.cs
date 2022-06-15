using System;
using System.Collections.Generic;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework.Xml;

namespace eLog.HeavyTools.Masters.Partner
{
    public class EmployeeSearchProvider3 : EmployeeSearchProvider, IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(EmployeeSearchProvider);

        protected override string GetSearchXmlFileName()
        {
            return $"{baseType.Namespace}.{baseType.Name}";
        }

        protected override string GetColorizerID()
        {
            return baseType.Name;
        }

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        #endregion

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var query = base.CreateQueryString(args, fmtonly);

            query = query.Replace("--$$morefields$$", " , olc.ptel --$$morefields$$ ");

            query = query.Replace("--$$morejoins$$", " left join olc_employee olc (nolock) on olc.empid = e.empid --$$morejoins$$ ");

            return query;

        }
    }
}