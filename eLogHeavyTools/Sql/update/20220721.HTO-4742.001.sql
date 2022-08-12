-- new fields: olc_partncmp.relatedaccno, scontoinvoice, scontobelowaccno, scontoavoveaccno
-- new fields: el1, el2, el3, el4, el5, el6, el7, el8
-- new fields: transactonfeeaccno, domesticvaluerate, referencetype, discountaccounting, valuecurid

/*
exec sp_insertcolumn
  @tablename     = 'olc_partncmp',
  @columndef     = 'relatedaccno varchar(50) null,scontoinvoice integer null,scontobelowaccno varchar(50) null,scontoavoveaccno varchar(50) null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = null,
  @showonly      = 1
*/

/*
exec sp_insertcolumn
  @tablename     = 'olc_partncmp',
  @columndef     = 'el1 varchar(72) null,el2 varchar(72) null,el3 varchar(72) null,el4 varchar(72) null,el5 varchar(72) null,el6 varchar(72) null,el7 varchar(72) null,el8 varchar(72) null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = null,
  @showonly      = 1
*/

/*
exec sp_insertcolumn
  @tablename     = 'olc_partncmp',
  @columndef     = 'transactonfeeaccno varchar(50)  null,domesticvaluerate integer null,referencetype integer null,discountaccounting integer null,valuecurid varchar(12) null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = null,
  @showonly      = 1
*/

select partnid, cmpid, addusrid, adddate
into tmp__olc_partncmp
from olc_partncmp
go

if object_id('fk_olc_partncmp_addusrid') is not null
  alter table olc_partncmp drop constraint fk_olc_partncmp_addusrid
alter table olc_partncmp drop column addusrid
alter table olc_partncmp drop column adddate
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'relatedaccno')
  alter table olc_partncmp add relatedaccno varchar(50) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'scontoinvoice')
  alter table olc_partncmp add scontoinvoice integer null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'scontobelowaccno')
  alter table olc_partncmp add scontobelowaccno varchar(50) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'scontoavoveaccno')
  alter table olc_partncmp add scontoavoveaccno varchar(50) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el1')
  alter table olc_partncmp add el1 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el2')
  alter table olc_partncmp add el2 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el3')
  alter table olc_partncmp add el3 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el4')
  alter table olc_partncmp add el4 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el5')
  alter table olc_partncmp add el5 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el6')
  alter table olc_partncmp add el6 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el7')
  alter table olc_partncmp add el7 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'el8')
  alter table olc_partncmp add el8 varchar(72) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'transactonfeeaccno')
  alter table olc_partncmp add transactonfeeaccno varchar(50)  null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'domesticvaluerate')
  alter table olc_partncmp add domesticvaluerate integer null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'referencetype')
  alter table olc_partncmp add referencetype integer null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'discountaccounting')
  alter table olc_partncmp add discountaccounting integer null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'valuecurid')
  alter table olc_partncmp add valuecurid varchar(12) null
go

alter table olc_partncmp add
  addusrid  varchar(12) null,
  adddate   datetime    null
go

if object_id('olc_partncmp') is not null
  alter table olc_partncmp add constraint fk_olc_partncmp_addusrid foreign key (addusrid) references cfw_user(usrid)
go

update x set
  addusrid = t.addusrid,
  adddate = t.adddate
from olc_partncmp x
     join tmp__olc_partncmp t on t.partnid = x.partnid and t.cmpid = x.cmpid
go

alter table olc_partncmp alter column addusrid varchar(12) not null
go

drop table tmp__olc_partncmp
go