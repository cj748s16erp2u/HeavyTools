using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.Test.Fixtures
{
    internal class EnvironmentService : IEnvironmentService
    {
        public string? CurrentUserId => "dev";
    }
}
