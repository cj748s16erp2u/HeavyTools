/*
drop table olc_actionext
drop table olc_actioncouponnumber
drop table olc_actioncountry
drop table olc_actionwebhop
drop table olc_action
drop table olc_actionretail
*/



/* Akci�k �s kuponok */

create table olc_action (
  aid						int identity    not null, -- Akci� egyedi azonos�t�
  actiontype				int				not null, -- Akci� t�pusa 0=kupon, 1=akci�, 2=T�rzsk�rtya, 3=VIP k�rtya
  name						varchar(100)	null,	  -- Akci� megnevez�se
  
  singlecouponnumber		varchar(100)	null,	  -- Kupon k�d
  couponunlimiteduse		int				null,     -- Kupon haszn�lata korl�tlan alkalommal

  discounttype				int				not null, -- Kedvezm�ny t�pusa 0=�sszeg, 1=sz�zal�k, 2=kupon
  discountval				numeric(19,6)   null,     -- Kedvezm�ny �rt�ke
  discountforfree			int				null,     -- Ingyenes fizet�s
  discountfreetransportation int			null,     -- Ingyenes sz�ll�t�s
  discountcalculationtype	int				not null, -- Kedvezm�ny sz�m�t�s 0=Sz�toszt�s, 1=egy term�k
  discountaid				int				null,	  -- Kupon kedvezm�ny 

  validdatefrom				datetime		null,	  -- �rv�nyess�g kezdete
  validdateto				datetime		null,	  -- �rv�nyess�g v�ge
  validtotvalfrom			numeric(19,6)	null,	  -- Minimum rendel�si �sszeg, brutt� adott deviz�ban
  validtotvalto				numeric(19,6)	null,	  -- Maximum rendel�si �sszeg, brutt� adott deviz�ban

  purchasetype				int				not null, -- V�s�rl�s t�pusa 0=B�rmely, 1=Csak az els� v�s�rl�shoz, 2=Csak a legolcs�bb term�k megv�s�rl�s�hoz, t�bb term�k v�s�rl�sakor

  filtercustomers			varchar(max)    null,     -- Mely �gyfelekre vonatkozik
  filteritems				varchar(max)    null,     -- Mely cikksz�mokra vonatkozik
  filteritemsblock			varchar(max)    null,     -- Mely cikksz�mokra nem vonatkozik
  count						int				null,	  -- H�ny term�knek kell a kos�rban lennie
 
  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_action primary key (aid)
)
 


alter table olc_action add constraint fk_olc_action_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_action add constraint fk_olc_action_discountaid foreign key (discountaid) references olc_action (aid)

go
 
/* Akci�k �s kuponok melyik webshop-ban �rv�nyesek */

create table olc_actionwebhop (
  awid						int identity    not null, -- Akci� webshop egyedi azonos�t�
  aid						int				not null, -- Akci� egyedi azonos�t�
  wid						varchar(12) 	not null, -- Web�ruh�z egyedi azonos�t�

  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionwebhop primary key (awid)
)

alter table olc_actionwebhop add constraint fk_olc_actionwebhop_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actionwebhop add constraint fk_olc_actionwebhop_wid foreign key (wid) references olc_webshop (wid)

go

/* Akci�k �s kuponok melyik orsz�gban �rv�nyesek */

create table olc_actioncountry (
  acid						int identity    not null, -- Akci� orsz�g egyedi azonos�t�
  aid						int				not null, -- Akci� egyedi azonos�t�
  countryid					varchar(12)		not null, -- Orsz�g egyedi azonos�t�

  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actioncountry primary key (acid)
)

alter table olc_actioncountry add constraint fk_olc_actioncountry_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actioncountry add constraint fk_olc_actioncountry_countryid foreign key (countryid) references ols_country (countryid)

go
 
/* Akci� egyedi kuponk�d */

create table olc_actioncouponnumber (
  acnid						int identity    not null, -- Egyedi kuponk�d azonos�t�
  aid						int				not null, -- Akci� egyedi azonos�t�
  couponnumber				varchar(100)	not null, -- Egyedi kupon k�d
  used						int				not null, -- Felhaszn�lt-e

  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actioncouponnumber primary key (acnid)
)

alter table olc_actioncouponnumber add constraint fk_olc_actioncouponnumber_addusrid foreign key (addusrid) references cfw_user (usrid)
 
go



/* Akci�k kieg�sz�t�s */

create table olc_actionext (
  axid						int identity    not null, -- Akci� kieg�sz�t�s egyedi azonos�t�
  aid						int				not null, -- Akci� egyedi azonos�t�

  filteritems				varchar(max)    not null, -- Mely cikksz�mokra vonatkozik
  filteritemsblock			varchar(max)    null,     -- Mely cikksz�mokra nem vonatkozik
  count						int				not null, -- H�ny db term�k
  discounttype				int				not null, -- Kedvezm�ny t�pusa 0=�sszeg, 1=sz�zal�k
  discountval				numeric(19,6)   null,     -- Kedvezm�ny �rt�ke

  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionext primary key (axid)
)

alter table olc_actionext add constraint fk_olc_actionext_addusrid foreign key (addusrid) references cfw_user (usrid) 

go




/* Akci�k �s kuponok melyik boltban �rv�nyesek */

create table olc_actionretail (
  arid						int identity    not null, -- Akci� bolt egyedi azonos�t�
  aid						int				not null, -- Akci� egyedi azonos�t�
  whid						varchar(12)     not null, -- Bolt egyedi azonos�t� 

  addusrid                  varchar(12)     not null, -- R�gz�t�
  adddate                   datetime        not null, -- R�gz�t�s d�tuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionretail primary key (arid)
)

alter table olc_actionretail add constraint fk_olc_actionretail_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actionretail add constraint fk_olc_actionretail_whid foreign key (whid) references ols_warehouse (whid)

go