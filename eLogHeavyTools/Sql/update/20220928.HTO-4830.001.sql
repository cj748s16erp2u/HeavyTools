-- new fields: olc_partner.taxid
/*
exec sp_insertcolumn
  @tablename     = 'olc_partner',
  @columndef     = 'taxid  varchar(12)  null',
  @refcolumnname = 'addusrid',
  @after         = 0,
  @customsql     = 'alter table olc_partner add constraint fk_olc_partner_taxid foreign key (taxid) references ols_tax (taxid)',
  @showonly      = 1
*/

select partnid, addusrid, adddate
into tmp__olc_partner
from olc_partner
go
if object_id('fk_olc_partner_addusrid') is not null
  alter table olc_partner drop constraint fk_olc_partner_addusrid
alter table olc_partner drop column addusrid
alter table olc_partner drop column adddate
go
if not exists(select 0 from sys.columns where object_id = object_id('olc_partner') and name = 'taxid')
  alter table olc_partner add taxid  varchar(12)  null
go
alter table olc_partner add
  addusrid  varchar(12) null,
  adddate   datetime    null
go
if object_id('olc_partner') is not null
  alter table olc_partner add constraint fk_olc_partner_addusrid foreign key (addusrid) references cfw_user(usrid)
go

update x set
  addusrid = t.addusrid,
  adddate = t.adddate
from olc_partner x
     join tmp__olc_partner t on t.partnid = x.partnid
go

alter table olc_partner add constraint fk_olc_partner_taxid foreign key (taxid) references ols_tax (taxid)
go
alter table olc_partner alter column addusrid varchar(12) not null
go

drop table tmp__olc_partner
go