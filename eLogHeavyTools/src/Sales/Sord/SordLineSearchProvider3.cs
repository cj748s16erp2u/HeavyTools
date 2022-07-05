using System;
using System.Collections.Generic;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineSearchProvider3 : SordLineSearchProvider
    {
        protected Type baseType = typeof(SordLineSearchProvider);
        
        protected static QueryArg[] m_argsTemplate3 = new QueryArg[]
        {
            new QueryArg("confdeldatefrom", "confdeldate", "olcsl", FieldType.DateTime,QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("confdeldateto", "confdeldate", "olcsl", FieldType.DateTime,QueryFlags.Less),
        };

        public SordLineSearchProvider3()
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

            query = query.Replace("itemid_name3, i.unitid", " itemid_name3, i.unitid ,olcsl.confdeldate, olcsl.confqty ");
            query = query.Replace("x.remtotval else x.remnetval * (1 + t.percnt / 100.0) end) as remtotval) x2", "x.remtotval else x.remnetval * (1 + t.percnt / 100.0) end) as remtotval) x2 left join olc_sordline olcsl (nolock) on olcsl.sordlineid = sl.sordlineid ");

            return query;
        }
    }
}