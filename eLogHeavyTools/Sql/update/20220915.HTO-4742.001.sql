-- new field: olc_partnaddr.el3

/*
exec sp_insertcolumn
  @tablename     = 'olc_partnaddr',
  @columndef     = 'el3 varchar(72) null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = null,
  @showonly      = 1
*/

select addrid, addusrid, adddate
into tmp__olc_partnaddr
from olc_partnaddr
go
if object_id('fk_olc_partnaddr_addusrid') is not null
  alter table olc_partnaddr drop constraint fk_olc_partnaddr_addusrid
alter table olc_partnaddr drop column addusrid
alter table olc_partnaddr drop column adddate
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partnaddr') and name = 'el3')
  alter table olc_partnaddr add el3 varchar(72) null
go
alter table olc_partnaddr add
  addusrid  varchar(12) null,
  adddate   datetime    null
go

if object_id('olc_partnaddr') is not null
  alter table olc_partnaddr add constraint fk_olc_partnaddr_addusrid foreign key (addusrid) references cfw_user(usrid)
go

update x set
  addusrid = t.addusrid,
  adddate = t.adddate
from olc_partnaddr x
     join tmp__olc_partnaddr t on t.addrid = x.addrid
go

alter table olc_partnaddr alter column addusrid varchar(12) not null
go

drop table tmp__olc_partnaddr
go