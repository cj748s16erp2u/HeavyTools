-- new fields: olc_partner.loyaltycardno,loyaltydiscpercnt,debcredsumvalue,regreprempid
/*
exec sp_insertcolumn
  @tablename     = 'olc_partner',
  @columndef     = 'loyaltycardno varchar(20) null, loyaltydiscpercnt numeric(9, 4) null, debcredsumvalue numeric(19, 6) null, regreprempid int null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = 'alter table olc_partner add constraint fk_olc_partner_regreprempid foreign key (regreprempid) references ols_employee (empid)',
  @showonly      = 1
*/

select partnid, addusrid, adddate
into tmp__olc_partner
from olc_partner
go

if object_id('fk_olc_partner_addusrid') is not null
  alter table olc_partner drop constraint fk_olc_partner_addusrid
alter table olc_partner drop column addusrid
alter table olc_partner drop column adddate
go

if not exists(select 0 from sys.columns where object_id = object_id('olc_partner') and name = 'loyaltycardno')
  alter table olc_partner add loyaltycardno varchar(20) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partner') and name = 'loyaltydiscpercnt')
  alter table olc_partner add loyaltydiscpercnt numeric(9, 4) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partner') and name = 'debcredsumvalue')
  alter table olc_partner add debcredsumvalue numeric(19, 6) null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partner') and name = 'regreprempid')
  alter table olc_partner add regreprempid int null
go

alter table olc_partner add
  addusrid  varchar(12) null,
  adddate   datetime    null
go

if object_id('olc_partner') is not null
  alter table olc_partner add constraint fk_olc_partner_addusrid foreign key (addusrid) references cfw_user(usrid)
go

update x set
  addusrid = t.addusrid,
  adddate = t.adddate
from olc_partner x
     join tmp__olc_partner t on t.partnid = x.partnid
go

alter table olc_partner add constraint fk_olc_partner_regreprempid foreign key (regreprempid) references ols_employee (empid)
go

alter table olc_partner alter column addusrid varchar(12) not null
go

drop table tmp__olc_partner
go
