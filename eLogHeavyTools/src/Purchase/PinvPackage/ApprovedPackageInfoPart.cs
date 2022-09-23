using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageInfoPart : eProjectWeb.Framework.UI.Templates.TabInfoPartTemplate1
    {
        public ApprovedPackageInfoPart() : base("info", ApprovedPackageBL.ID + ".GetInfo")
        {
        }
    }
}
