using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;

[System.Diagnostics.DebuggerStepThrough]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RegisterDIAttribute : Attribute
{
    public Type Interface { get; init; } = null!;
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Scoped;
}
