using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP4U.Log;

public enum LogCategory
{
    [Description("General")]
    General,

    [Description("Authentication")]
    Authentication,

    [Description("Audit")]
    Audit
}
