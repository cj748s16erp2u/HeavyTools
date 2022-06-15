--drop table olc_whzone

/***********************/
/* Raktár / Zóna       */
/***********************/
create table olc_whzone (
  whzoneid                  int identity    not null, -- Zóna egyedi azonosító
  whid                      varchar(12)     not null, -- Raktár hivatkozás
  whzonecode                varchar(12)     not null, -- Kód
  name                      varchar(40)     not null, -- Megnevezés
  pickingtype               int             not null, -- Szedés módja (kézi, targoncás)
  pickingcartaccessible     int             not null, -- Szedőkocsi bevihető: igen/nem
  isbackground              int             not null, -- Háttér zóna: igen/nem
  ispuffer                  int             not null, -- Puffer zóna: igen/nem
  locdefvolume              numeric(19,6)   null,     -- helykód alapért. térfogat (m3)
  locdefoverfillthreshold   numeric(19,6)   null,     -- helykód alapért. Túltöltési küszöb (%)
  locdefismulti             int             null,     -- helykód alapért. Több cikk engedélyezése
  note                      varchar(200)    null,     -- Megjegyzés
  addusrid                  varchar(12)     not null, -- Rögzítő
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_whzone primary key (whzoneid)
)

alter table olc_whzone add constraint fk_olc_whzone_whid foreign key (whid) references ols_warehouse (whid)
alter table olc_whzone add constraint fk_olc_whzone_addusrid foreign key (addusrid) references cfw_user (usrid)

create index idx_olc_whzone_whid on olc_whzone (whid)
create index idx_olc_whzone_whzonecode on olc_whzone (whid, whzonecode)
