--drop table olc_whlocprio

/***************************************/
/* Els�dleges helyk�d				   */
/***************************************/

create table olc_whlocprio (
  whlpid                    int identity    not null, -- kulcs
  itemid                    int             not null, -- cikk hivatkozas
  whid                      varchar(12)     not null, -- Rakt�r
  whzoneid                  int             null, -- Z�na
  whlocid                   int             not null, -- Helyk�d
  whpriotype                int             not null, -- T�pus (1 - els�dleges, 2 - m�sodlagos)
  refilllimit               numeric(19, 6)  null, -- �jrat�lt�si limit
  startdate                 datetime        not null, -- �rv�nyess�g kezdete
  enddate                   datetime        not null, -- �rv�nyess�g v�ge
  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�tve
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