-- new fields: olc_action.validforsaleproducts
/*
exec sp_insertcolumn
  @tablename     = 'olc_action',
  @columndef     = 'validforsaleproducts int null',
  @refcolumnname = 'validdateto',
  @after         = 0,
  @customsql     = '',
  @showonly      = 1
*/

select aid, validdateto, validtotvalfrom, validtotvalto, purchasetype, filtercustomerstype, filteritems, filteritemsblock, count, addusrid, adddate, delstat  into tmp__olc_action  from olc_action
go
if object_id('fk_olc_action_addusrid') is not null    alter table olc_action drop constraint fk_olc_action_addusrid
alter table olc_action drop column validdateto
alter table olc_action drop column validtotvalfrom
alter table olc_action drop column validtotvalto
alter table olc_action drop column purchasetype
alter table olc_action drop column filtercustomerstype
alter table olc_action drop column filteritems
alter table olc_action drop column filteritemsblock
alter table olc_action drop column count
alter table olc_action drop column addusrid
alter table olc_action drop column adddate
alter table olc_action drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_action') and name = 'validforsaleproducts')    alter table olc_action add validforsaleproducts int null
go
alter table olc_action add    validdateto          datetime       null,    validtotvalfrom      numeric(19, 6) null,    validtotvalto        numeric(19, 6) null,    purchasetype         int            null,    filtercustomerstype  int            null,    filteritems          varchar(max)   null,    filteritemsblock     varchar(max)   null,    count                int            null,    addusrid             varchar(12)    null,    adddate              datetime       null,    delstat              int            null
go
if object_id('olc_action') is not null    alter table olc_action add constraint fk_olc_action_addusrid foreign key (addusrid) references cfw_user(usrid)
go
update x set    validdateto = t.validdateto,    validtotvalfrom = t.validtotvalfrom,    validtotvalto = t.validtotvalto,    purchasetype = t.purchasetype,    filtercustomerstype = t.filtercustomerstype,    filteritems = t.filteritems,    filteritemsblock = t.filteritemsblock,    count = t.count,    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_action x       join tmp__olc_action t on t.aid = x.aid
go
alter table olc_action alter column purchasetype int not null
alter table olc_action alter column addusrid varchar(12) not null
alter table olc_action alter column adddate datetime not null
alter table olc_action alter column delstat int not null
go
drop table tmp__olc_action
go