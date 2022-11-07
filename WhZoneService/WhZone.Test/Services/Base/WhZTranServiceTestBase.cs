using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.Test.Base;
using eLog.HeavyTools.Services.WhZone.Test.Fixtures;

namespace eLog.HeavyTools.Services.WhZone.Test.Services.Base;

public abstract class WhZTranServiceTestBase : TestBase<OlcWhztranhead, IWhZTranService>
{
    protected WhZTranServiceTestBase(
        ITestOutputHelper testOutputHelper,
        TestFixture fixture) : base(testOutputHelper, fixture)
    {
    }
}
