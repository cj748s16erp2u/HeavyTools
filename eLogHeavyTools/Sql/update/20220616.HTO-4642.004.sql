-- new field: olc_itemmodelseason.oldcode
/*
exec sp_insertcolumn
  @tablename = 'olc_itemmodelseason',
  @columndef = 'oldcode	int null',
  @refcolumnname = 'addusrid',
  @after = 0,
  @customsql = '',
  @showonly = 1
*/
select imsid, addusrid, adddate, delstat  into tmp__olc_itemmodelseason  from olc_itemmodelseason
go
if object_id('fk_olc_itemmodelseason_addusrid') is not null    alter table olc_itemmodelseason drop constraint fk_olc_itemmodelseason_addusrid
alter table olc_itemmodelseason drop column addusrid
alter table olc_itemmodelseason drop column adddate
alter table olc_itemmodelseason drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_itemmodelseason') and name = 'oldcode int')    alter table olc_itemmodelseason add oldcode int null
go
alter table olc_itemmodelseason add    addusrid  varchar(12) null,    adddate   datetime    null,    delstat   int         null
go
if object_id('olc_itemmodelseason') is not null    alter table olc_itemmodelseason add constraint fk_olc_itemmodelseason_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_itemmodelseason x       join tmp__olc_itemmodelseason t on t.imsid = x.imsid
go
alter table olc_itemmodelseason alter column delstat int not null
go
drop table tmp__olc_itemmodelseason
go