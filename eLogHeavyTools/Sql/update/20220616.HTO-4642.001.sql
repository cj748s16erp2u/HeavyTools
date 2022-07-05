-- new field: olc_item.catalogpagenumber, iscollectionarticlenumber
/*
exec sp_insertcolumn
  @tablename = 'olc_item',
  @columndef = 'catalogpagenumber	int null, iscollectionarticlenumber int null',
  @refcolumnname = 'addusrid',
  @after = 0,
  @customsql = '',
  @showonly = 1
*/ 
select itemid, addusrid, adddate, delstat  into tmp__olc_item  from olc_item
go
if object_id('fk_olc_item_addusrid') is not null    alter table olc_item drop constraint fk_olc_item_addusrid
alter table olc_item drop column addusrid
alter table olc_item drop column adddate
alter table olc_item drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_item') and name = 'catalogpagenumber int')    alter table olc_item add catalogpagenumber int null
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_item') and name = 'iscollectionarticlenumber')    alter table olc_item add iscollectionarticlenumber int null
go
alter table olc_item add    addusrid  varchar(12) null,    adddate   datetime    null,    delstat   int         null
go
if object_id('olc_item') is not null    alter table olc_item add constraint fk_olc_item_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_item x       join tmp__olc_item t on t.itemid = x.itemid
go
alter table olc_item alter column delstat int not null
go
drop table tmp__olc_item
go