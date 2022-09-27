-- Szallitoi szamla - Dokumentum csatoltas
update t
set valuevar = t2.value + (case when t2.value <> '' then ',' else '' end) + 'pinv'
from ols_sysval t
     cross apply (select isnull(convert(varchar(200), t.valuevar), '') value) t2
where t.sysvalid = 'attachvisible'
  and t2.value not like '%pinv%'
go