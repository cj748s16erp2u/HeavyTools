-- new fields: olc_action.netgoid
/*
exec sp_insertcolumn
  @tablename     = 'olc_action',
  @columndef     = 'netgoid int null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = '',
  @showonly      = 1
*/
select aid, addusrid, adddate, delstat  into tmp__olc_action  from olc_action
go
if object_id('fk_olc_action_addusrid') is not null    alter table olc_action drop constraint fk_olc_action_addusrid
alter table olc_action drop column addusrid
alter table olc_action drop column adddate
alter table olc_action drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_action') and name = 'netgoid')    alter table olc_action add netgoid int null
go
alter table olc_action add    addusrid  varchar(12) null,    adddate   datetime    null,    delstat   int         null
go
if object_id('olc_action') is not null    alter table olc_action add constraint fk_olc_action_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_action x       join tmp__olc_action t on t.aid = x.aid
go
alter table olc_action alter column addusrid varchar(12) not null
alter table olc_action alter column adddate datetime not null
alter table olc_action alter column delstat int not null
go
drop table tmp__olc_action
go