delete [tl]
from [ols_typeline] [tl] (nolock)
where [tl].[typegrpid] = 118
  and [tl].[value] = 1

delete [tl]
from [ols_typeline] [tl] (nolock)
where [tl].[typegrpid] = 118
  and [tl].[value] = 2

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 501, 501, 'Központi', 'K', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 502, 502, 'Retail', 'R', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 503, 503, 'Franchise partner', 'FP', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 504, 504, 'Bizományos', 'B', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 505, 505, 'Útonlévő', 'UT', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 506, 506, 'Selejt', 'S', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 507, 507, 'Hiány', 'H', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 508, 508, 'Import', 'I', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 509, 509, 'Külső raktár', 'KR', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (118, 510, 510, 'Minőségbiztosítási raktár', 'MR', 'dev', getdate(), 0)
