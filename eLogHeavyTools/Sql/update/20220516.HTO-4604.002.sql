--drop table olc_whlocation

/***************************/
/* Helykód                 */
/***************************/
create table olc_whlocation (
  whlocid                   int identity    not null, -- Helykód egyedi azonosító
  whid                      varchar(12)     not null, -- Raktár hivatkozás
  whzoneid                  int             null,     -- Zóna hivatkozás
  whloccode                 varchar(20)     not null, -- Helykód kód
  name                      varchar(200)    null,     -- Helykód név
  loctype                   int             not null, -- Helykód típus (normál, mozgó, átadó terület)
  movloctype                int             null,     -- Mozgó helykód típus (raklap, rekesz, kocsi, szedőkocsi, ...)
  volume                    numeric(19,6)   null,     -- térfogat (m3)
  overfillthreshold         numeric(19,6)   null,     -- Túltöltési küszöb (%)
  ismulti                   int             null,     -- Több cikk engedélyezése
  crawlorder                int             null,     -- Bejárási sorrend
  capacity                  numeric(19,6)   null,     -- Mozgó helykód kapacitása
  capunitid                 varchar(12)     null,     -- Mozgó helykód kapacitás mértékegység
  note                      varchar(200)    null,     -- Megjegyzés
  addusrid                  varchar(12)     not null, -- Rögzítő
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_whlocation primary key (whlocid)
)

alter table olc_whlocation add constraint fk_olc_whlocation_whid foreign key (whid) references ols_warehouse (whid)
alter table olc_whlocation add constraint fk_olc_whlocation_whzoneid foreign key (whzoneid) references olc_whzone (whzoneid)
alter table olc_whlocation add constraint fk_olc_whlocation_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_whlocation add constraint fk_olc_whlocation_capunitid foreign key (capunitid) references ols_unit (unitid)

create index idx_olc_whlocation_whid on olc_whlocation (whid)
create index idx_olc_whlocation_whzoneid on olc_whlocation (whid, whzoneid)
create unique index uq_olc_whlocation_whloccode on olc_whlocation (whid, whzoneid, whloccode)
create index idx_olc_whlocation_loctype on olc_whlocation (whid, whzoneid, loctype)
