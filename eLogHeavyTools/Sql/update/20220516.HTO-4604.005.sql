update [tl] set [delstat] = 1
from [ols_typeline] [tl] (nolock)
where [tl].[typegrpid] = 20
  and [tl].[value] = 1

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (20, 501, 501, 'Zóna+helykód', 'ZH', 'dev', getdate(), 0)
