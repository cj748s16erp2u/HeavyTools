-- new field: olc_item.webcategory
/*
exec sp_insertcolumn
  @tablename = 'olc_item',
  @columndef = 'webcategory	varchar(6)',
  @refcolumnname = 'addusrid',
  @after = 0,
  @customsql = '',
  @showonly = 1
*/
select itemid, addusrid, adddate  into tmp__olc_item  from olc_item
go
if object_id('fk_olc_item_addusrid') is not null    alter table olc_item drop constraint fk_olc_item_addusrid
alter table olc_item drop column addusrid
alter table olc_item drop column adddate
go
alter table olc_item add webcategory varchar(6)
go
alter table olc_item add    addusrid  varchar(12) null,    adddate   datetime    null
go
if object_id('olc_item') is not null    alter table olc_item add constraint fk_olc_item_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate  from olc_item x       join tmp__olc_item t on t.itemid = x.itemid
go
go
drop table tmp__olc_item
go