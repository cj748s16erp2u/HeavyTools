/*
drop table olc_actionext
drop table olc_actioncouponnumber
drop table olc_actioncountry
drop table olc_actionwebhop
drop table olc_action
drop table olc_actionretail
*/



/* Akciók és kuponok */

create table olc_action (
  aid						int identity    not null, -- Akció egyedi azonosító
  actiontype				int				not null, -- Akció típusa 0=kupon, 1=akció, 2=Törzskártya, 3=VIP kártya
  name						varchar(100)	null,	  -- Akció megnevezése
  
  singlecouponnumber		varchar(100)	null,	  -- Kupon kód
  couponunlimiteduse		int				null,     -- Kupon használata korlátlan alkalommal

  discounttype				int				not null, -- Kedvezmény típusa 0=összeg, 1=százalék, 2=kupon
  discountval				numeric(19,6)   null,     -- Kedvezmény értéke
  discountforfree			int				null,     -- Ingyenes fizetés
  discountfreetransportation int			null,     -- Ingyenes szállítás
  discountcalculationtype	int				not null, -- Kedvezmény számítás 0=Szétosztás, 1=egy termék
  discountaid				int				null,	  -- Kupon kedvezmény 

  validdatefrom				datetime		null,	  -- Érvényesség kezdete
  validdateto				datetime		null,	  -- Érvényesség vége
  validtotvalfrom			numeric(19,6)	null,	  -- Minimum rendelési összeg, bruttó adott devizában
  validtotvalto				numeric(19,6)	null,	  -- Maximum rendelési összeg, bruttó adott devizában

  purchasetype				int				not null, -- Vásárlás típusa 0=Bármely, 1=Csak az elsõ vásárláshoz, 2=Csak a legolcsóbb termék megvásárlásához, több termék vásárlásakor

  filtercustomers			varchar(max)    null,     -- Mely ügyfelekre vonatkozik
  filteritems				varchar(max)    null,     -- Mely cikkszámokra vonatkozik
  filteritemsblock			varchar(max)    null,     -- Mely cikkszámokra nem vonatkozik
  count						int				null,	  -- Hány terméknek kell a kosárban lennie
 
  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_action primary key (aid)
)
 


alter table olc_action add constraint fk_olc_action_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_action add constraint fk_olc_action_discountaid foreign key (discountaid) references olc_action (aid)

go
 
/* Akciók és kuponok melyik webshop-ban érvényesek */

create table olc_actionwebhop (
  awid						int identity    not null, -- Akció webshop egyedi azonosító
  aid						int				not null, -- Akció egyedi azonosító
  wid						varchar(12) 	not null, -- Webáruház egyedi azonosító

  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionwebhop primary key (awid)
)

alter table olc_actionwebhop add constraint fk_olc_actionwebhop_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actionwebhop add constraint fk_olc_actionwebhop_wid foreign key (wid) references olc_webshop (wid)

go

/* Akciók és kuponok melyik országban érvényesek */

create table olc_actioncountry (
  acid						int identity    not null, -- Akció ország egyedi azonosító
  aid						int				not null, -- Akció egyedi azonosító
  countryid					varchar(12)		not null, -- Ország egyedi azonosító

  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actioncountry primary key (acid)
)

alter table olc_actioncountry add constraint fk_olc_actioncountry_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actioncountry add constraint fk_olc_actioncountry_countryid foreign key (countryid) references ols_country (countryid)

go
 
/* Akció egyedi kuponkód */

create table olc_actioncouponnumber (
  acnid						int identity    not null, -- Egyedi kuponkód azonosító
  aid						int				not null, -- Akció egyedi azonosító
  couponnumber				varchar(100)	not null, -- Egyedi kupon kód
  used						int				not null, -- Felhasznált-e

  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actioncouponnumber primary key (acnid)
)

alter table olc_actioncouponnumber add constraint fk_olc_actioncouponnumber_addusrid foreign key (addusrid) references cfw_user (usrid)
 
go



/* Akciók kiegészítés */

create table olc_actionext (
  axid						int identity    not null, -- Akció kiegészítés egyedi azonosító
  aid						int				not null, -- Akció egyedi azonosító

  filteritems				varchar(max)    not null, -- Mely cikkszámokra vonatkozik
  filteritemsblock			varchar(max)    null,     -- Mely cikkszámokra nem vonatkozik
  count						int				not null, -- Hány db termék
  discounttype				int				not null, -- Kedvezmény típusa 0=összeg, 1=százalék
  discountval				numeric(19,6)   null,     -- Kedvezmény értéke

  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionext primary key (axid)
)

alter table olc_actionext add constraint fk_olc_actionext_addusrid foreign key (addusrid) references cfw_user (usrid) 

go




/* Akciók és kuponok melyik boltban érvényesek */

create table olc_actionretail (
  arid						int identity    not null, -- Akció bolt egyedi azonosító
  aid						int				not null, -- Akció egyedi azonosító
  whid						varchar(12)     not null, -- Bolt egyedi azonosító 

  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionretail primary key (arid)
)

alter table olc_actionretail add constraint fk_olc_actionretail_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actionretail add constraint fk_olc_actionretail_whid foreign key (whid) references ols_warehouse (whid)

go