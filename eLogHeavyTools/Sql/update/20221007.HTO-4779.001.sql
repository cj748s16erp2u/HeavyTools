/***************************************/
/* Helyk�d kapcsol�s				   */
/***************************************/

create table olc_whloclink (
  whllid                    int identity    not null, -- kulcs
  whid                      varchar(12)     not null, -- Rakt�r
  whzoneid                  int             null, -- Z�na
  whlocid                   int             not null, -- Helyk�d
  overfillthreshold         numeric(19, 6)  null, -- T�lt�lt�si k�sz�b
  startdate                 datetime        not null, -- �rv�nyess�g kezdete
  enddate                   datetime        not null, -- �rv�nyess�g v�ge
  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�tve
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
/* Helyk�d kapcsol�s kieg�sz�t�s   	   */
/***************************************/

create table olc_whloclinkline (
  whlllineid                    int identity    not null, -- kulcs
  whllid                        int             not null, -- fej hivatkoz�s
  whlocid                       int             not null, -- Helyk�d hivatkoz�s
  whllinktype                   int             not null, -- Kapcsolat tipusa (1 - master, 2 - kapcsolt)
  addusrid                      varchar(12)     not null, -- R�gz�t�
  adddate                       datetime        not null, -- R�gz�tve
  constraint pk_olc_whloclinkline primary key (whlllineid)
)

alter table olc_whloclinkline add constraint fk_olc_whloclinkline_whllid foreign key (whllid) references olc_whloclink (whllid)
alter table olc_whloclinkline add constraint fk_olc_whloclinkline_whlocid foreign key (whlocid) references olc_whlocation (whlocid)
alter table olc_whloclinkline add constraint fk_olc_whloclinkline_addusrid foreign key (addusrid) references cfw_user (usrid)

create unique index ux_olc_whloclinkline_whllid_whlocid on olc_whloclinkline (whllid, whlocid)
go