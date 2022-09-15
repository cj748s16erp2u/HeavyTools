 -- new fields: olc_action.isdiscount
/*
exec sp_insertcolumn
  @tablename     = 'olc_actionext',
  @columndef     = 'isdiscount int not null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = '',
  @showonly      = 1
*/
select axid, addusrid, adddate, delstat  into tmp__olc_actionext  from olc_actionext
go
if object_id('fk_olc_actionext_addusrid') is not null    alter table olc_actionext drop constraint fk_olc_actionext_addusrid
alter table olc_actionext drop column addusrid
alter table olc_actionext drop column adddate
alter table olc_actionext drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_actionext') and name = 'isdiscount')    alter table olc_actionext add isdiscount int not null
go
alter table olc_actionext add    addusrid  varchar(12) null,    adddate   datetime    null,    delstat   int         null
go
if object_id('olc_actionext') is not null    alter table olc_actionext add constraint fk_olc_actionext_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_actionext x       join tmp__olc_actionext t on t.axid = x.axid
go
alter table olc_actionext alter column addusrid varchar(12) not null
alter table olc_actionext alter column adddate datetime not null
alter table olc_actionext alter column delstat int not null
go
drop table tmp__olc_actionext
go