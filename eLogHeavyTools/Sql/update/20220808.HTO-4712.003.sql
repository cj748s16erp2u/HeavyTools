-- new fields: olc_sordline.data
/*
exec sp_insertcolumn
  @tablename     = 'olc_sordline',
  @columndef     = 'data xml null ',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = '',
  @showonly      = 1
*/
select sordlineid, addusrid, adddate  into tmp__olc_sordline  from olc_sordline
go
if object_id('fk_olc_sordline_addusrid') is not null    alter table olc_sordline drop constraint fk_olc_sordline_addusrid
alter table olc_sordline drop column addusrid
alter table olc_sordline drop column adddate
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_sordline') and name = 'data')    alter table olc_sordline add data xml null
go
alter table olc_sordline add    addusrid  varchar(12) null,    adddate   datetime    null
go
if object_id('olc_sordline') is not null    alter table olc_sordline add constraint fk_olc_sordline_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate  from olc_sordline x       join tmp__olc_sordline t on t.sordlineid = x.sordlineid
go
alter table olc_sordline alter column addusrid varchar(12) not null
alter table olc_sordline alter column adddate datetime not null
go
drop table tmp__olc_sordline
go