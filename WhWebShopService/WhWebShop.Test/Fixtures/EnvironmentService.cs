using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;

namespace eLog.HeavyTools.Services.WhWebShop.Test.Fixtures
{
    internal class EnvironmentService : IEnvironmentService
    {
        public string? CurrentUserId => "dev";
    }
}
