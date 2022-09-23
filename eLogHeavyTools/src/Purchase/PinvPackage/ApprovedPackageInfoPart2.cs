using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageInfoPart2 : eProjectWeb.Framework.UI.Templates.TabInfoPartTemplate1
    {
        public ApprovedPackageInfoPart2() : base("info", ApprovedPackageBL.ID + ".GetInfo2")
        {
            EntityKeyName = eProjectWeb.Framework.Consts.RootEntityKey + "," + eProjectWeb.Framework.Consts.DetailEntityKey;
        }

    }
}
