-- new fields: olc_partncmp.secpaycid
/*
exec sp_insertcolumn
  @tablename     = 'olc_partncmp',
  @columndef     = 'secpaycid int null',
  @refcolumnname = 'secpaymid',
  @after         = 1,
  @customsql     = 'alter table olc_partncmp add constraint fk_olc_partncmp_secpaycid foreign key (secpaycid) references ols_paycond (paycid)',
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

if not exists(select 0 from sys.columns where object_id = object_id('olc_partncmp') and name = 'secpaycid')
  alter table olc_partncmp add secpaycid int null
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

alter table olc_partncmp add constraint fk_olc_partncmp_secpaycid foreign key (secpaycid) references ols_paycond (paycid)
go

alter table olc_partncmp alter column addusrid varchar(12) not null
go

drop table tmp__olc_partncmp
go
