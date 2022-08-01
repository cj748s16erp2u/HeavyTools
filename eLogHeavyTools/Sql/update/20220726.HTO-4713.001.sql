/* Aj�nd�k k�rtya       */

create table olc_giftcard (
  gcid						int identity    not null, -- Aj�nd�k k�rtya egyedi azonos�t�
  barcode					varchar(40)		null,	  -- Aj�nd�k k�rtya vonalk�d
  pincode					varchar(4)		null,	  -- Aj�nd�k k�rtya pin k�d
  prc						numeric(19,6)   not null, -- Aj�nd�k k�rtya �rt�k
  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_giftcard primary key (gcid)
)
 
alter table olc_giftcard add constraint fk_olc_giftcard_addusrid foreign key (addusrid) references cfw_user (usrid)

create table olc_giftcardlog (
  gclid						int identity    not null, -- Aj�nd�k k�rtya log egyedi azonos�t�
  gcid						int             not null, -- Aj�nd�k k�rtya egyedi azonos�t�
  sinvlineid				int			    null,     -- Aj�nd�k k�rtya felt�lt�s
  sinvid					int			    null,     -- Aj�nd�k k�rtya lev�s�rl�s
  val						numeric(19,6)   not null, -- Aj�nd�k k�rtya �rt�k v�ltoz�s
  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_giftcardlog primary key (gclid)
)

alter table olc_giftcardlog add constraint fk_olc_giftcardlog_gcid foreign key (gcid) references olc_giftcard (gcid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_sinvlineid foreign key (sinvlineid) references ols_sinvline (sinvlineid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_sinvidid foreign key (sinvid) references ols_sinvhead (sinvid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_addusrid foreign key (addusrid) references cfw_user (usrid)
