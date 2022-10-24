/***************************************/
/* Helykód kapcsolás				   */
/***************************************/

create table olc_whloclink (
  whllid                    int identity    not null, -- kulcs
  whid                      varchar(12)     not null, -- Raktár
  whzoneid                  int             null, -- Zóna
  whlocid                   int             not null, -- Helykód
  overfillthreshold         numeric(19, 6)  null, -- Túltöltési küszöb
  startdate                 datetime        not null, -- Érvényesség kezdete
  enddate                   datetime        not null, -- Érvényesség vége
  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítve
  constraint pk_olc_whloclink primary key (whllid)
)

alter table olc_whloclink add constraint fk_olc_whloclink_whid foreign key (whid) references ols_warehouse (whid)
alter table olc_whloclink add constraint fk_olc_whloclink_whzoneid foreign key (whzoneid) references olc_whzone (whzoneid)
alter table olc_whloclink add constraint fk_olc_whloclink_whlocid foreign key (whlocid) references olc_whlocation (whlocid)
alter table olc_whloclink add constraint fk_olc_whloclink_addusrid foreign key (addusrid) references cfw_user (usrid)

create index idx_olc_whloclink_whid on olc_whloclink (whid)
create index idx_olc_whloclink_whzoneid on olc_whloclink (whzoneid)
create index idx_olc_whloclink_whlocid on olc_whloclink (whlocid)
go


/***************************************/
/* Helykód kapcsolás kiegészítés   	   */
/***************************************/

create table olc_whloclinkline (
  whlllineid                    int identity    not null, -- kulcs
  whllid                        int             not null, -- fej hivatkozás
  whlocid                       int             not null, -- Helykód hivatkozás
  whllinktype                   int             not null, -- Kapcsolat tipusa (1 - master, 2 - kapcsolt)
  addusrid                      varchar(12)     not null, -- Rögzítõ
  adddate                       datetime        not null, -- Rögzítve
  constraint pk_olc_whloclinkline primary key (whlllineid)
)

alter table olc_whloclinkline add constraint fk_olc_whloclinkline_whllid foreign key (whllid) references olc_whloclink (whllid)
alter table olc_whloclinkline add constraint fk_olc_whloclinkline_whlocid foreign key (whlocid) references olc_whlocation (whlocid)
alter table olc_whloclinkline add constraint fk_olc_whloclinkline_addusrid foreign key (addusrid) references cfw_user (usrid)

create unique index ux_olc_whloclinkline_whllid_whlocid on olc_whloclinkline (whllid, whlocid)
go