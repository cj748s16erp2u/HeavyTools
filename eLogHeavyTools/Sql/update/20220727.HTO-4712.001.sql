-- new fields: olc_sordhead.wid
/*
exec sp_insertcolumn
  @tablename     = 'olc_sordhead',
  @columndef     = 'wid varchar(12) null ',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = 'alter table olc_sordhead add constraint fk_olc_sordhead_wid foreign key (wid) references olc_webshop (wid)',
  @showonly      = 1
*/

select sordid, addusrid, adddate  into tmp__olc_sordhead  from olc_sordhead
go
if object_id('fk_olc_sordhead_addusrid') is not null    alter table olc_sordhead drop constraint fk_olc_sordhead_addusrid
alter table olc_sordhead drop column addusrid
alter table olc_sordhead drop column adddate
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_sordhead') and name = 'wid')    alter table olc_sordhead add wid varchar(12) null
go
alter table olc_sordhead add    addusrid  varchar(12) null,    adddate   datetime    null
go
if object_id('olc_sordhead') is not null    alter table olc_sordhead add constraint fk_olc_sordhead_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    addusrid = t.addusrid,    adddate = t.adddate  from olc_sordhead x       join tmp__olc_sordhead t on t.sordid = x.sordid
go
alter table olc_sordhead add constraint fk_olc_sordhead_wid foreign key (wid) references olc_webshop (wid)
go
alter table olc_sordhead alter column addusrid varchar(12) not null
alter table olc_sordhead alter column adddate datetime not null
go
drop table tmp__olc_sordhead
go

