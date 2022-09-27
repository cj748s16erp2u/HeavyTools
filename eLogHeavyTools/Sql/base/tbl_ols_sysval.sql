update [sv] set [valuestr] = 'P{0:000000}'
from [ols_sysval] [sv] (nolock)
where [sv].[sysvalid] = 'partner:DefPartnCode'
go

-- Szallitoi szamla - dokumentum csatolas
update t
set valuevar = t2.value + (case when t2.value <> '' then ',' else '' end) + 'pinv'
from ols_sysval t
     cross apply (select isnull(convert(varchar(200), t.valuevar), '') value) t2
where t.sysvalid = 'attachvisible'
  and t2.value not like '%pinv%'
go