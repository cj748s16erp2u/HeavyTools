using System;
using System.Reflection;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;

static class UnitTestDetector
{

    private static bool _runningFromNUnit = false;

    static UnitTestDetector()
    {
        foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Can't do something like this as it will load the nUnit assembly
            // if (assem == typeof(NUnit.Framework.Assert))

            if (assem.FullName.ToLowerInvariant().StartsWith("microsoft.testplatform"))
            {
                _runningFromNUnit = true;
                break;
            }
        }
    }

    public static bool IsRunningFromNUnit
    {
        get { return _runningFromNUnit; }
    }
}
