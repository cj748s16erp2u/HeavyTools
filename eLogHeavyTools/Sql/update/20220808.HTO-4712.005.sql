﻿-- new fields: olc_action.priority
/*
exec sp_insertcolumn
  @tablename     = 'olc_action',
  @columndef     = 'priority int null ',
  @refcolumnname = 'singlecouponnumber',
  @after         = 0,
  @customsql     = '',
  @showonly      = 1
*/

select aid, singlecouponnumber, couponunlimiteduse, discounttype, discountval, discountforfree, discountfreetransportation, discountcalculationtype, discountaid, validdatefrom, validdateto, validtotvalfrom, validtotvalto, purchasetype, filtercustomers, filteritems, filteritemsblock, count, addusrid, adddate, delstat  into tmp__olc_action  from olc_action
go
if object_id('fk_olc_action_addusrid') is not null    alter table olc_action drop constraint fk_olc_action_addusrid
if object_id('fk_olc_action_discountaid') is not null    alter table olc_action drop constraint fk_olc_action_discountaid
alter table olc_action drop column singlecouponnumber
alter table olc_action drop column couponunlimiteduse
alter table olc_action drop column discounttype
alter table olc_action drop column discountval
alter table olc_action drop column discountforfree
alter table olc_action drop column discountfreetransportation
alter table olc_action drop column discountcalculationtype
alter table olc_action drop column discountaid
alter table olc_action drop column validdatefrom
alter table olc_action drop column validdateto
alter table olc_action drop column validtotvalfrom
alter table olc_action drop column validtotvalto
alter table olc_action drop column purchasetype
alter table olc_action drop column filtercustomers
alter table olc_action drop column filteritems
alter table olc_action drop column filteritemsblock
alter table olc_action drop column count
alter table olc_action drop column addusrid
alter table olc_action drop column adddate
alter table olc_action drop column delstat
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_action') and name = 'priority')    alter table olc_action add priority int null
go
alter table olc_action add    singlecouponnumber          varchar(100)   null,    couponunlimiteduse          int            null,    discounttype                int            null,    discountval                 numeric(19, 6) null,    discountforfree             int            null,    discountfreetransportation  int            null,    discountcalculationtype     int            null,    discountaid                 int            null,    validdatefrom               datetime       null,    validdateto                 datetime       null,    validtotvalfrom             numeric(19, 6) null,    validtotvalto               numeric(19, 6) null,    purchasetype                int            null,    filtercustomers             varchar(max)   null,    filteritems                 varchar(max)   null,    filteritemsblock            varchar(max)   null,    count                       int            null,    addusrid                    varchar(12)    null,    adddate                     datetime       null,    delstat                     int            null
go
if object_id('olc_action') is not null    alter table olc_action add constraint fk_olc_action_addusrid foreign key (addusrid) references cfw_user(usrid)
if object_id('olc_action') is not null    alter table olc_action add constraint fk_olc_action_discountaid foreign key (discountaid) references olc_action(aid)
go
update x set    singlecouponnumber = t.singlecouponnumber,    couponunlimiteduse = t.couponunlimiteduse,    discounttype = t.discounttype,    discountval = t.discountval,    discountforfree = t.discountforfree,    discountfreetransportation = t.discountfreetransportation,    discountcalculationtype = t.discountcalculationtype,    discountaid = t.discountaid,    validdatefrom = t.validdatefrom,    validdateto = t.validdateto,    validtotvalfrom = t.validtotvalfrom,    validtotvalto = t.validtotvalto,    purchasetype = t.purchasetype,    filtercustomers = t.filtercustomers,    filteritems = t.filteritems,    filteritemsblock = t.filteritemsblock,    count = t.count,    addusrid = t.addusrid,    adddate = t.adddate,    delstat = t.delstat  from olc_action x       join tmp__olc_action t on t.aid = x.aid
go
alter table olc_action alter column discounttype int not null
alter table olc_action alter column discountcalculationtype int not null
alter table olc_action alter column purchasetype int not null
alter table olc_action alter column addusrid varchar(12) not null
alter table olc_action alter column adddate datetime not null
alter table olc_action alter column delstat int not null
go
drop table tmp__olc_action
go