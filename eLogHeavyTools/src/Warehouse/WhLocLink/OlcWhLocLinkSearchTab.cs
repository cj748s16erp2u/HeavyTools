using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkSearchTab : SearchTabTemplate1
    {
        /// <summary>
        /// A method to create a new Search bar for the Warehouse LocLink screen.
        /// </summary>
        /// <param name="setup"></param>
        /// <returns>WhLocLinkSearchTab</returns>
        public static OlcWhLocLinkSearchTab New(DefaultPageSetup setup)
        {
            var tab = ObjectFactory.New<OlcWhLocLinkSearchTab>();
            tab.Initialize("OlcWhLocLink", setup, DefaultActions.Basic);
            return tab;
        }

        protected Control startDateCtrl;
        protected Control endDateCtrl;

        protected override void CreateBase()
        {
            base.CreateBase();

            startDateCtrl = this.SrcBar["startdate"];
            endDateCtrl = this.SrcBar["enddate"];
        }
    }
}
