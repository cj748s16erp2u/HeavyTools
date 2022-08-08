/* 
	Cikk főcsoport típus 1. 
*/
create table olc_itemmaingrouptype1 (
	imgt1id 	    	varchar(1),
	groupname			varchar(200) NOT NULL,	
	grouplastnum		int not null,
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmaingrouptype1 primary key (imgt1id),
	constraint fk_olc_itemmaingrouptype1_addusrid foreign key (addusrid) references cfw_user(usrid)
)

/* 
	Cikk főcsoport típus 2. 
*/
create table olc_itemmaingrouptype2 (
	imgt2id	    		varchar(1),
	groupname			varchar(200) NOT NULL,
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmaingrouptype2 primary key (imgt2id),
	constraint fk_olc_itemmaingrouptype2_addusrid foreign key (addusrid) references cfw_user(usrid)
)

/* 
	Cikk méretsor táblázat fej
*/
create table olc_itemsizerangehead (
	isrhid 	    		int identity(1, 1),
	code				varchar(12) NOT NULL,
	name				varchar(200) NOT NULL,

	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemsizerangehead primary key (isrhid),
	constraint fk_olc_itemsizerangehead_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go
 
/* 
	Cikk méretsor táblázat tétel
*/
create table olc_itemsizerangeline (
	isrlid 	    		int identity(1, 1),
	isrhid				int not null,
	size				varchar(3) NOT NULL,
	ordernum			int,
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemsizerangeline primary key (isrlid),
	constraint fk_olc_itemsizerangeline_isrhid foreign key (isrhid) references olc_itemsizerangehead(isrhid),
	constraint fk_olc_itemsizerangeline_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go
   
/* 
	Cikk főcsoport
*/    
create table olc_itemmaingroup (
	imgid 	    		int identity(1, 1),
	imgt1id	    		varchar(1) NOT NULL,
	code				varchar(3) NOT NULL,
	name				varchar(200) NOT NULL,
	imgt2id	    		varchar(1) NOT NULL,
	isrhid 	    		int NOT NULL,
	itemgrpid			varchar(12) NOT NULL, 
	 
	oldcode				varchar(5), 
	maingrouplastnum	int not null,
	 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmaingroup primary key (imgid),

	constraint fk_olc_itemmaingroup_imgt1 foreign key (imgt1id) references olc_itemmaingrouptype1(imgt1id),
	constraint fk_olc_itemmaingroup_imgt2 foreign key (imgt2id) references olc_itemmaingrouptype2(imgt2id),
	constraint fk_olc_itemmaingroup_isrhid foreign key (isrhid) references olc_itemsizerangehead(isrhid),
	constraint fk_olc_itemmaingroup_itemgrpid foreign key (itemgrpid) references ols_itemgroup(itemgrpid),
	constraint fk_olc_itemmaingroup_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/* 
	Cikk modell
*/  
create table olc_itemmodel (
	imid 	    		int identity(1, 1),
	imgid 	    		int not null,
	code				varchar(8) NOT NULL,
	name				varchar(200) NOT NULL,
	unitid				varchar(12) NOT NULL,
	exclusivetype		int,
	netweight			numeric(19, 6),
	grossweight			numeric(19, 6),
	volume				numeric(19, 6),
	
	isimported 			int,
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmodel primary key (imid),
	constraint fk_olc_itemmodel_imgid foreign key (imgid) references olc_itemmaingroup(imgid),
	constraint fk_olc_itemmodel_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/*
	Cikk színállás 
*/
create table olc_itemcolor (
	icid 	    		varchar(3),
	name				varchar(200) NOT NULL,	
	oldcode				int null,
 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemcolor primary key (icid),
	constraint fk_olc_itemcolor_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/* 
	Cikk szezon
*/  
create table olc_itemseason (
	isid 	    		varchar(12) NOT NULL,
	name				varchar(200) NOT NULL,
	oldcode 			varchar(12),
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemseason primary key (isid),
	constraint fk_olc_itemseason_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/* 
	Cikk modell szezon összekapcsolás
*/  
create table olc_itemmodelseason (
	imsid				int identity(1, 1),
	imid 	    		int,
	isid				varchar(12),
	icid				varchar(3),
	 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmodelseason primary key (imsid),
	constraint fk_olc_itemmodelseason_imid foreign key (imid) references olc_itemmodel(imid),
	constraint fk_olc_itemmodelseason_isid foreign key (isid) references olc_itemseason(isid),
	constraint fk_olc_itemmodelseason_icid foreign key (icid) references olc_itemcolor(icid),
	constraint fk_olc_itemmodelseason_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/*
	Cikk kiegészítések
*/
create table olc_item (
	itemid				int,
	imsid				int,
	isrlid				int,
	colortype1			int,
	colorname			varchar(100),
	colortype2			int,
	colortype3			int,
	materialtype		int,
	patterntype			int,
	patterntype2		int,
	catalogpagenumber	int null, 
	iscollectionarticlenumber int null,
	note 				varchar(2000),
	addusrid			varchar(12),
	adddate				datetime,
	constraint pk_olc_item primary key (itemid),
	constraint fk_olc_item_imsid foreign key (imsid) references olc_itemmodelseason(imsid),
	constraint fk_olc_item_isrlid foreign key (isrlid) references olc_itemsizerangeline(isrlid),
	constraint fk_olc_item_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/*
	Modell ártábla típus
*/
create table olc_prctype (
	ptid				int identity(1, 1), 
	name				varchar(100) not null,

	isnet				int not null,
	 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_prctypen primary key (ptid), 
	constraint fk_olc_prctype_addusrid foreign key (addusrid) references cfw_user(usrid)
)




/*
	Modell ártábla
*/
create table olc_prctable (
	prcid				int identity(1, 1),
	ptid 	    		int not null,

	prctype             int not null,  /* 0 Eredeti ár, 1 Aktuális ár, 2 Akció alapja */
	wid					varchar(12) null,

	partnid				int null,
	addrid				int null,
	curid				varchar(12),
	startdate			datetime not null,
	enddate				datetime not null,
	prc					numeric(16,9),

	imid				int null,				 
	isid				varchar(12) null,
	icid				varchar(3) null,
	itemid				int,

	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,

	constraint pk_olc_prctable primary key (prcid),
	constraint fk_olc_prctable_tpid foreign key (ptid) references olc_prctype(ptid),
	constraint fk_olc_prctable_partnid foreign key (partnid) references ols_partner(partnid),
	constraint fk_olc_prctable_addrid foreign key (addrid) references ols_partnaddr(addrid),
	constraint fk_olc_prctable_curid foreign key (curid) references ols_currency(curid), 
	constraint fk_olc_prctable_imid foreign key (imid) references olc_itemmodel(imid),
	constraint fk_olc_prctable_isid foreign key (isid) references olc_itemseason(isid),
	constraint fk_olc_prctable_icid foreign key (icid) references olc_itemcolor(icid),
	constraint fk_olc_prctable_itemid foreign key (itemid) references ols_item(itemid), 
	constraint fk_olc_prctable_wid foreign key (wid) references olc_webshop(wid), 
	constraint fk_olc_prctable_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/****************************/
/* Alkalmazott kiegészítés  */
/****************************/
create table olc_employee (
    empid				int						not null,
    ptel				varchar(40)             null, -- privat telefon
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_employee primary key (empid),
    constraint fk_olc_employee_empid foreign key (empid) references ols_employee (empid),
    constraint fk_olc_employee_addusrid foreign key (addusrid) references cfw_user (usrid)
)
go

/****************************/
/* Partner cím kiegészítés  */
/****************************/
create table olc_partnaddr (
    addrid				int						not null,
    oldcode				varchar(10)             null, -- regi kod
	glnnum				varchar(15)             null, -- GLN szam
	buildingname		varchar(50)             null, -- Epulet neve
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partnaddr primary key (addrid),
    constraint fk_olc_partnaddr_addrid foreign key (addrid) references ols_partnaddr (addrid),
    constraint fk_olc_partnaddr_addusrid foreign key (addusrid) references cfw_user (usrid)
)

/****************************/
/* Partner kiegészítés      */
/****************************/
create table olc_partner (
    partnid				int						not null,
    oldcode				varchar(10)             null, -- regi kod
	wsemail				varchar(max)            null, -- Webshop email
	invlngid			varchar(12)				null, -- Szamla nyelve
	loyaltycardno       varchar(20)             null, -- torszkartya szama
	loyaltydiscpercnt   numeric(9, 4)           null, -- torzsvevo kedvezmeny
	loyaltyturnover     numeric(19, 6)          null, -- torzskartya forgalom
	regreprempid        int                     null, -- teruleti kepviselo
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partner primary key (partnid),
    constraint fk_olc_partner_partnid foreign key (partnid) references ols_partner (partnid),
    constraint fk_olc_partner_addusrid foreign key (addusrid) references cfw_user (usrid),
	constraint fk_olc_partner_regreprempid foreign key (regreprempid) references ols_employee (empid)
)

/*********************************/
/* Partner-vállalat kiegészítés  */
/*********************************/
create table olc_partncmp (
    partnid				int						not null,
	cmpid				int						not null,
    secpaymid			varchar(12)             null, -- Masodlagos fizetesi mod
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partncmp primary key (partnid, cmpid), -- composite primary key
    constraint fk_olc_partncmp_partnid foreign key (partnid) references ols_partner (partnid),
	constraint fk_olc_partncmp_cmpid foreign key (cmpid) references ols_company (cmpid),
    constraint fk_olc_partncmp_partnid_cmpid foreign key (partnid, cmpid) references ols_partncmp (partnid, cmpid),
    constraint fk_olc_partncmp_secpaymid foreign key (secpaymid) references ols_paymethod (paymid),
    constraint fk_olc_partncmp_addusrid foreign key (addusrid) references cfw_user (usrid)
)

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
go

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
go

/*
	Webshop tábla
*/
create table olc_webshop (
	wid					int identity(1, 1),
	sname				varchar(12),
	name				varchar(100),
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,

	constraint pk_olc_webshop primary key (wid),
	constraint fk_olc_webshop_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go


insert into olc_webshop values ('cz', '.cz webshop','dev',GETDATE(),0)
insert into olc_webshop values ('sk', '.sk webshop','dev',GETDATE(),0)
insert into olc_webshop values ('ro', '.ro webshop','dev',GETDATE(),0)
insert into olc_webshop values ('com','.com webshop','dev',GETDATE(),0)
insert into olc_webshop values ('hu','.hu webshop','dev',GETDATE(),0)


 go
 
 
/***************************************/
/* Vevői rendelés kiegészítés          */
/***************************************/
create table [olc_sordhead] (
  [sordid]                    [int]             not null, -- primary key, foreign key
  [sordapprovalstat]          [int]             not null, -- jóváhagyás (jóváhagyandó, jóváhagyott, automatikusan jóváhagyott, elutasított)
  [loyaltycardno]             [varchar](50)         null, -- törzskáryta
  [transfcond]                [int]                 null, -- szállítási feltétel (GLS, FoxPost, Bolt, cím)
  [deliverylocation]          [varchar](200)        null, -- csomagpont
  [data]                      [xml]                 null, -- számlázási adatok (customer prefix: név, cím, szállítási cím, telefon, email; coupon prefix lista: kuponkód, ...)
  [regreprempid]    		  [int]                 null, -- területi képviselő 
  [clerkempid]                [int]                 null, -- ügyintéző
  [wid]						  [varchar](12)         null, -- melyik webáruházból jött egy rendelés
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_sordhead] primary key ([sordid])
)

alter table [olc_sordhead] add constraint [fk_olc_sordhead_sordid] foreign key ([sordid]) references [ols_sordhead] ([sordid])
alter table [olc_sordhead] add constraint [fk_olc_sordhead_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
alter table [olc_sordhead] add constraint [fk_olc_sordhead_wid] foreign key ([wid]) references [olc_webshop] ([wid])

/***************************************/
/* Vevői rendelés tétel kiegészítés    */
/***************************************/
create table [olc_sordline] (
  [sordlineid]                [int]             not null, -- primary key, foreign key
  [confqty]                   [numeric](19, 6)      null, -- visszaigazolt mennyiség
  [confdeldate]               [datetime]            null, -- visszaigazolt szállítási határidő
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_sordline] primary key ([sordlineid])
)

alter table [olc_sordline] add constraint [fk_olc_sordline_sordlineid] foreign key ([sordlineid]) references [ols_sordline] ([sordlineid])
alter table [olc_sordline] add constraint [fk_olc_sordline_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
go

create table olc_taxtransext (  
	tteid 	    		int identity(1, 1),
	ttid	int not null,

	countryid			varchar(12) not null,
	taxid				varchar(12) not null, 
	ttefrom				datetime not null,
	tteto				datetime not null,
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,

	constraint pk_olc_taxtransext primary key (tteid),
	constraint fk_olc_taxtransext_addusrid foreign key (addusrid) references cfw_user(usrid),
	constraint fk_olc_taxtransext_ttid foreign key (ttid) references ols_taxtrans(ttid),
	constraint fk_olc_taxtransext_countryid foreign key (countryid) references ols_country(countryid),
	constraint fk_olc_taxtransext_taxid foreign key (taxid) references ols_tax(taxid),
)

go
/***************************************/
/* Szállítói rendelés kiegészítés          */
/***************************************/
create table [olc_pordhead] (
  [pordid]                    [int]             not null, -- primary key, foreign key
  [paritytypeid]              [int]                 null, -- paritás
  [advance]                   numeric(19,6)     	null, -- előleg összege
  [clerkempid]                [int]                 null, -- ügyintéző
  [locality]                  [varchar](50)     	null, -- helység
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_pordhead] primary key ([pordid])
)

alter table [olc_pordhead] add constraint [fk_olc_pordhead_pordid] foreign key ([pordid]) references [ols_pordhead] ([pordid])
alter table [olc_pordhead] add constraint [fk_olc_pordhead_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
go
