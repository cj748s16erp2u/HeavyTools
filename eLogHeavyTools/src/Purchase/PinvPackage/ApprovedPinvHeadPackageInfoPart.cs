using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPinvHeadPackageInfoPart : eProjectWeb.Framework.UI.Templates.TabInfoPartTemplate1
    {
        public ApprovedPinvHeadPackageInfoPart() : base("info", ApprovedPinvHeadPackageBL.ID + ".GetInfo")
        {
        }
    }
}
