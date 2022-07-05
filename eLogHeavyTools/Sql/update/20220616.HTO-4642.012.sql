-- new field: olc_itemseason.oldcode
/*
exec sp_insertcolumn
  @tablename = 'olc_itemseason',
  @columndef = 'oldcode varchar(12)',
  @refcolumnname = 'addusrid',
  @after = 0,
  @customsql = '',
  @showonly = 1
*/
select isid, addusrid, adddate, delstat  into tmp__olc_itemseason  from olc_itemseason
go
if object_id('fk_olc_itemseason_addusrid') is not null    alter table olc_itemseason drop constraint fk_olc_itemseason_addusrid
alter table olc_itemseason drop column addusrid
alter table olc_itemseason drop column adddate
alter table olc_itemseason drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_itemseason') and name = 'oldcode')    alter table olc_itemseason add oldcode varchar(12)
go
alter table olc_itemseason add    addusrid  varchar(12) null,    adddate   datetime    null,    delstat   int         null
go
if object_id('olc_itemseason') is not null    alter table olc_itemseason add constraint fk_olc_itemseason_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_itemseason x       join tmp__olc_itemseason t on t.isid = x.isid
go
alter table olc_itemseason alter column delstat int not null
go
drop table tmp__olc_itemseason
go