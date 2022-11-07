using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Enums
{
    [Flags]
    public enum UserOption
    {
        MustChangePassword = 1,
        CannotChangePassword = 2,
        PasswordNeverExpires = 4,
        AccountDisabled = 8,
        AccountLockedOut = 16,

        All = MustChangePassword | CannotChangePassword | PasswordNeverExpires | AccountDisabled | AccountLockedOut,
    }
}
