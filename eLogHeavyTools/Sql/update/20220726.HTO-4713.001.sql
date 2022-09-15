/* Ajándék kártya       */

create table olc_giftcard (
  gcid						int identity    not null, -- Ajándék kártya egyedi azonosító
  barcode					varchar(40)		null,	  -- Ajándék kártya vonalkód
  pincode					varchar(4)		null,	  -- Ajándék kártya pin kód
  prc						numeric(19,6)   not null, -- Ajándék kártya érték
  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_giftcard primary key (gcid)
)
 
alter table olc_giftcard add constraint fk_olc_giftcard_addusrid foreign key (addusrid) references cfw_user (usrid)

create table olc_giftcardlog (
  gclid						int identity    not null, -- Ajándék kártya log egyedi azonosító
  gcid						int             not null, -- Ajándék kártya egyedi azonosító
  sinvlineid				int			    null,     -- Ajándék kártya feltöltés
  sinvid					int			    null,     -- Ajándék kártya levásárlás
  val						numeric(19,6)   not null, -- Ajándék kártya érték változás
  addusrid                  varchar(12)     not null, -- Rögzítõ
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_giftcardlog primary key (gclid)
)

alter table olc_giftcardlog add constraint fk_olc_giftcardlog_gcid foreign key (gcid) references olc_giftcard (gcid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_sinvlineid foreign key (sinvlineid) references ols_sinvline (sinvlineid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_sinvidid foreign key (sinvid) references ols_sinvhead (sinvid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_addusrid foreign key (addusrid) references cfw_user (usrid)
