﻿using System;
using System.Collections.Generic;
using eLog.Base.Sales.Sord;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineAllSearchProvider3 : SordLineAllSearchProvider
    {
        protected Type baseType = typeof(SordLineAllSearchProvider);

        protected static QueryArg[] m_argsTemplate3 = new QueryArg[]
        {
            new QueryArg("confdeldatefrom", "confdeldate", "olcsl", FieldType.DateTime,QueryFlags.Greater | QueryFlags.Equals),
            new QueryArg("confdeldateto", "confdeldate", "olcsl", FieldType.DateTime,QueryFlags.Less),
        };

        public SordLineAllSearchProvider3()
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

            query = query.Replace("--$$morefields$$", " ,olcsl.confdeldate, olcsl.confqty --$$morefields$$");
            query = query.Replace("--$$morejoins$$", " left join olc_sordline olcsl (nolock) on olcsl.sordlineid = sl.sordlineid --$$morejoins$$ ");

            return query;
        }
    }
}