-- new field: olc_itemmodel.isimported
/*
exec sp_insertcolumn
  @tablename = 'olc_itemmodel',
  @columndef = 'isimported int',
  @refcolumnname = 'addusrid',
  @after = 0,
  @customsql = '',
  @showonly = 1
*/
 select imid, addusrid, adddate, delstat  into tmp__olc_itemmodel  from olc_itemmodel
go
if object_id('fk_olc_itemmodel_addusrid') is not null    alter table olc_itemmodel drop constraint fk_olc_itemmodel_addusrid
alter table olc_itemmodel drop column addusrid
alter table olc_itemmodel drop column adddate
alter table olc_itemmodel drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_itemmodel') and name = 'isimported')    alter table olc_itemmodel add isimported int
go
alter table olc_itemmodel add    addusrid  varchar(12) null,    adddate   datetime    null,    delstat   int         null
go
if object_id('olc_itemmodel') is not null    alter table olc_itemmodel add constraint fk_olc_itemmodel_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_itemmodel x       join tmp__olc_itemmodel t on t.imid = x.imid
go
alter table olc_itemmodel alter column delstat int not null
go
drop table tmp__olc_itemmodel
go