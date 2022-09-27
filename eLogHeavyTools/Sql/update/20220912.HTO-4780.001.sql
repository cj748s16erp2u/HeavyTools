--drop table olc_whlocprio

/***************************************/
/* Elsõdleges helykód				   */
/***************************************/

create table olc_whlocprio (
  whlpid                    int identity    not null, -- kulcs
  itemid                    int             not null, -- cikk hivatkozas
  whid                      varchar(12)     not null, -- Raktár
  whzoneid                  int             null, -- Zóna
  whlocid                   int             not null, -- Helykód
  whpriotype                int             not null, -- Típus (1 - elsõdleges, 2 - másodlagos)
  refilllimit               numeric(19, 6)  null, -- Újratöltési limit
  startdate                 datetime        not null, -- Érvényesség kezdete
  enddate                   datetime        not null, -- Érvényesség vége
  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítve
  constraint pk_olc_whlocprio primary key (whlpid)
)

alter table olc_whlocprio add constraint fk_olc_whlocprio_itemid foreign key (itemid) references ols_item (itemid)
alter table olc_whlocprio add constraint fk_olc_whlocprio_whid foreign key (whid) references ols_warehouse (whid)
alter table olc_whlocprio add constraint fk_olc_whlocprio_whzoneid foreign key (whzoneid) references olc_whzone (whzoneid)
alter table olc_whlocprio add constraint fk_olc_whlocprio_whlocid foreign key (whlocid) references olc_whlocation (whlocid)
alter table olc_whlocprio add constraint fk_olc_whlocprio_addusrid foreign key (addusrid) references cfw_user (usrid)

create index idx_olc_whlocprio_itemid on olc_whlocprio (itemid)
create index idx_olc_whlocprio_whid on olc_whlocprio (whid)
create index idx_olc_whlocprio_whzoneid on olc_whlocprio (whzoneid)
create index idx_olc_whlocprio_whlocid on olc_whlocprio (whlocid)