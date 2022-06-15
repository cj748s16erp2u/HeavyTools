update [sv] set [valuestr] = 'P{0:000000}'
from [ols_sysval] [sv] (nolock)
where [sv].[sysvalid] = 'partner:DefPartnCode'
go
