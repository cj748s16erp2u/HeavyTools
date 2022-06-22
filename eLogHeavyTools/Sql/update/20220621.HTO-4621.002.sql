-- new fields: olc_employee.oldcode
/*
exec sp_insertcolumn
  @tablename     = 'olc_employee',
  @columndef     = 'oldcode varchar(10)',
  @refcolumnname = 'empid',
  @after         = 1,
  @customsql     = '',
  @showonly      = 1
*/

select empid, privtel, addusrid, adddate
into tmp__olc_employee
from olc_employee
go

if object_id('fk_olc_employee_addusrid') is not null
  alter table olc_employee drop constraint fk_olc_employee_addusrid

alter table olc_employee drop column privtel
alter table olc_employee drop column addusrid
alter table olc_employee drop column adddate
go

if not exists(select 0 from sys.columns where object_id = object_id('olc_employee') and name = 'oldcode')
  alter table olc_employee add oldcode varchar(10)
go

alter table olc_employee add
  privtel   varchar(40) null,
  addusrid  varchar(12) null,
  adddate   datetime    null
go

if object_id('olc_employee') is not null
  alter table olc_employee add constraint fk_olc_employee_addusrid foreign key (addusrid) references cfw_user(usrid)
go

update x set
  privtel = t.privtel,
  addusrid = t.addusrid,
  adddate = t.adddate
from olc_employee x
     join tmp__olc_employee t on t.empid = x.empid
go

alter table olc_employee alter column addusrid varchar(12) not null
go

drop table tmp__olc_employee
go
