using System;
using System.Collections.Generic;
using System.Text;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI;
using eProjectWeb.Framework.UI.Commands;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.Rules;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmpCustTab : DetailSearchTabTemplate1
    {
        public static OlcPartnCmpCustTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup custom)
        {
            OlcPartnCmpCustTab t = (OlcPartnCmpCustTab)ObjectFactory.New(typeof(OlcPartnCmpCustTab));
            t.Initialize("PartnCmpCust", custom, "$noroot_partner",
                    eProjectWeb.Framework.UI.Templates.DefaultActions.Modify | eProjectWeb.Framework.UI.Templates.DefaultActions.View);
            return t;
        }

        protected OlcPartnCmpCustTab()
        {
        }

    }
}
