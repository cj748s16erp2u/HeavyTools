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
go

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
    taxid               varchar(12)             null,
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partner primary key (partnid),
    constraint fk_olc_partner_partnid foreign key (partnid) references ols_partner (partnid),
    constraint fk_olc_partner_taxid foreign key (taxid) references ols_tax (taxid),
    constraint fk_olc_partner_addusrid foreign key (addusrid) references cfw_user (usrid),
	constraint fk_olc_partner_regreprempid foreign key (regreprempid) references ols_employee (empid)
)
go

/*********************************/
/* Partner-vállalat kiegészítés  */
/*********************************/
create table olc_partncmp (
    partnid                             int         not null,
    cmpid                               int         not null,
    secpaymid                   varchar(12)             null, -- Masodlagos fizetesi mod
    secpaycid                           int             null,
    relatedaccno                varchar(50)             null, /* kapcsolodo banki elszamolo (technikai utkozo) szamla szamlakodja */
    scontoinvoice               integer                 null, /* skonto csak a plusszos szamlak utan, 0: nem, 1: igen, 2: nincs skonto */
    scontobelowaccno            varchar(50)             null, /* 3% alatti skonto szamlakodja */
    scontoaboveaccno            varchar(50)             null, /* 3% feletti skonto szamlakodja */
    el1                         varchar(72)             null,
    el2                         varchar(72)             null, /* szallito elemkodja (el2) */
    el3                         varchar(72)             null, /* ertekesitesi egyseg kodja (el3) */
    el4                         varchar(72)             null,
    el5                         varchar(72)             null,
    el6                         varchar(72)             null,
    el7                         varchar(72)             null,
    el8                         varchar(72)             null,
    transactionfeeaccno         varchar(50)             null, /* tranzakcios dij szamlakodja */
    domesticvaluerate           integer                 null, /* elem ertek es hazai ertek aranyositasa szukseges, 0: nem, 1: igen */
    referencetype               integer                 null, /* referencia tipusa, tobbfele */
    discountaccounting          integer                 null, /* kedvezmeny konyvelese, 0: nem, 1: igen */
    valuecurid                  varchar(12)             null, /* ertek devizanem */
    addusrid                    varchar(12)         not null,
    adddate                     datetime                null,
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
  [bustypeid]				  [varchar](12)         null, -- melyik webáruházból jött egy rendelés
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_sordhead] primary key ([sordid])
)

alter table [olc_sordhead] add constraint [fk_olc_sordhead_sordid] foreign key ([sordid]) references [ols_sordhead] ([sordid])
alter table [olc_sordhead] add constraint [fk_olc_sordhead_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
alter table [olc_sordhead] add constraint [fk_olc_sordhead_wid] foreign key ([wid]) references [olc_webshop] ([wid])
alter table [olc_sordhead] add constraint [fk_olc_sordhead_bustypeid] foreign key ([bustypeid]) references [ols_bustype] ([bustypeid])

/***************************************/
/* Vevői rendelés tétel kiegészítés    */
/***************************************/
create table [olc_sordline] (
  [sordlineid]                [int]             not null, -- primary key, foreign key
  [confqty]                   [numeric](19, 6)      null, -- visszaigazolt mennyiség
  [confdeldate]               [datetime]            null, -- visszaigazolt szállítási határidő
  [preordersordlineid]        [int]                 null, -- előrendelés azonosító
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_sordline] primary key ([sordlineid])
)

alter table [olc_sordline] add constraint [fk_olc_sordline_sordlineid] foreign key ([sordlineid]) references [ols_sordline] ([sordlineid])
alter table [olc_sordline] add constraint [fk_olc_sordline_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
alter table olc_sordline add constraint fk_olc_sordline_preordersordlineid foreign key (preordersordlineid) references ols_sordline (sordlineid)

go

/* Ajándék kártya       */

create table olc_giftcard (
  gcid						int identity    not null, -- Ajándék kártya egyedi azonosító
  barcode					varchar(40)		null,	  -- Ajándék kártya vonalkód
  pincode					varchar(4)		null,	  -- Ajándék kártya pin kód
  prc						numeric(19,6)   not null, -- Ajándék kártya érték
  addusrid                  varchar(12)     not null, -- Rögzítő
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
  addusrid                  varchar(12)     not null, -- Rögzítő
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_giftcardlog primary key (gclid)
)

alter table olc_giftcardlog add constraint fk_olc_giftcardlog_gcid foreign key (gcid) references olc_giftcard (gcid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_sinvlineid foreign key (sinvlineid) references ols_sinvline (sinvlineid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_sinvidid foreign key (sinvid) references ols_sinvhead (sinvid)
alter table olc_giftcardlog add constraint fk_olc_giftcardlog_addusrid foreign key (addusrid) references cfw_user (usrid)


go

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
  isactive 					int 			null,     -- Aktív?
  isextcondition			int				not null, -- Összetett feltétel?
  isextdiscount				int				not null, -- Összetett kedvezmény?
  priority 					int 			null,     -- Akció prioritás
  curid 					varchar(12)  	null,     -- Akció pénznem
  
  singlecouponnumber		varchar(100)	null,	  -- Kupon kód
  couponunlimiteduse		int				null,     -- Kupon használata korlátlan alkalommal

  discounttype				int				null, -- Kedvezmény típusa 0=összeg, 1=százalék, 2=kupon
  discountval				numeric(19,6)   null,     -- Kedvezmény értéke
  discountforfree			int				null,     -- Ingyenes fizetés
  discountfreetransportation int			null,     -- Ingyenes szállítás
  discountcalculationtype	int				not null, -- Kedvezmény számítás 0=Szétosztás, 1=egy termék
  discountaid				int				null,	  -- Kupon kedvezmény 

  validdatefrom				datetime		null,	  -- Érvényesség kezdete
  validdateto				datetime		null,	  -- Érvényesség vége
  validtotvalfrom			numeric(19,6)	null,	  -- Minimum rendelési összeg, bruttó adott devizában
  validforsaleproducts 		int 			null,     -- Akciós termékekre érvényes
  validtotvalto				numeric(19,6)	null,	  -- Maximum rendelési összeg, bruttó adott devizában
  purchasetype				int				not null, -- Vásárlás típusa 0=Bármely, 1=Csak az első vásárláshoz, 2=Csak a legolcsóbb termék megvásárlásához, több termék vásárlásakor

  filtercustomerstype		int    			null,     -- Mely ügyfelekre vonatkozik 0=Minden ügyél, 1=Csak törzsvásárlókra, 2=Törzsvásárlókra NEM, 3=Viszonteladókra NEM
  filteritems				varchar(max)    null,     -- Mely cikkszámokra vonatkozik
  filteritemsblock			varchar(max)    null,     -- Mely cikkszámokra nem vonatkozik
  count						int				null,	  -- Hány terméknek kell a kosárban lennie
  note 						varchar(max)    NULL	  -- Megjegyzés
  netgoid 					int 		    NULL	  -- NetGO id interface
  blockmessage 				varchar(200) 	null      -- Blokkra nyomtatási üzenet
  
  addusrid                  varchar(12)     not null, -- Rögzítő
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

  addusrid                  varchar(12)     not null, -- Rögzítő
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

  addusrid                  varchar(12)     not null, -- Rögzítő
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

  addusrid                  varchar(12)     not null, -- Rögzítő
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
  discounttype				int				null, -- Kedvezmény típusa 0=összeg, 1=százalék
  discountval				numeric(19,6)   null,     -- Kedvezmény értéke
  discountcalculationtype 	int 			null,	  -- 0=Erre nem, 1=Erre igen
  isdiscount 				int 			not null, -- Kedvezmény?
  
  addusrid                  varchar(12)     not null, -- Rögzítő
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

  addusrid                  varchar(12)     not null, -- Rögzítő
  adddate                   datetime        not null, -- Rögzítés dátuma
  delstat                   int             not null, -- Rejtett
  constraint pk_olc_actionretail primary key (arid)
)

alter table olc_actionretail add constraint fk_olc_actionretail_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_actionretail add constraint fk_olc_actionretail_whid foreign key (whid) references ols_warehouse (whid)

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


create table olc_cart (
	cartid                  	int identity    not null,
	itemid                  	int not null,
	
	orignalSelPrc numeric(19,6) null,
	orignalGrossPrc numeric(19,6) null,
	orignalTotVal numeric(19,6) null,
	selPrc numeric(19,6) null,
	grossPrc numeric(19,6) null,
	netVal numeric(19,6) null,
	taxVal numeric(19,6) null,
	totVal numeric(19,6) null,
	aid	int null,
  
	addusrid                  varchar(12)     not null,
	adddate                   datetime        not null,
	delstat                   int             not null,
	constraint pk_olc_cart primary key (cartid)
)

alter table olc_cart add constraint fk_olc_cart_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_cart add constraint fk_olc_cart_aid foreign key (aid) references olc_action (aid)
alter table olc_cart add constraint fk_olc_cart_itemid foreign key (itemid) references ols_item (itemid)
go

/***************************************/
/* Elsődleges helykód				   */
/***************************************/

create table olc_whlocprio (
  whlpid                    int identity    not null, -- kulcs
  itemid                    int             not null, -- cikk hivatkozas
  whid                      varchar(12)     not null, -- Raktár
  whzoneid                  int             null, -- Zóna
  whlocid                   int             not null, -- Helykód
  whpriotype                int             not null, -- Típus (1 - elsődleges, 2 - másodlagos)
  refilllimit               numeric(19, 6)  null, -- Újratöltési limit
  startdate                 datetime        not null, -- Érvényesség kezdete
  enddate                   datetime        not null, -- Érvényesség vége
  addusrid                  varchar(12)     not null, -- Rögzítő
  adddate                   datetime        not null, -- Rögzítve
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
go

 create table olc_stline (
  stlineid					int				not null,  
  origstlineid				int			    null,

  addusrid                  varchar(12)     not null,
  adddate                   datetime        not null,
  constraint pk_olc_stline primary key (stlineid)
)

alter table olc_stline add constraint fk_olc_stline_stid foreign key (stlineid) references ols_stline (stlineid)
alter table olc_stline add constraint fk_olc_stline_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_stline add constraint fk_olc_stline_origstlineid foreign key (origstlineid) references ols_stline (stlineid)
go

create table olc_tmp_sordsord (
   ssid                 uniqueidentifier     not null,
   sordlineid           int                  not null,
   sordid               int                  not null,
   linenum              int                  not null, 
   itemid               int                  not null,
   itemcode				varchar(25)			 null,
   name01				varchar(100)		 null,
   name02				nvarchar(200)		 null,
   qty                  numeric(19,6)        null,
   reqdate              datetime             not null,
   confqty				datetime             null,
   confdeldate			datetime             null,
   ref2                 varchar(30)          null,
   pendingqty			numeric(19,6)        not null,
   fullordqty           numeric(19,6)        not null,
   fullmovqty           numeric(19,6)        not null,
   ordqty               numeric(19,6)        not null,
   movqty               numeric(19,6)        not null,
   selprc               numeric(19,6)        not null,
   seltotprc            numeric(19,6)        null,
   selprctype           int                  null,
   selprcprcid          int                  null,
   discpercnt           numeric(9,4)         not null,
   discpercntprcid      int                  null,
   discval              numeric(19,6)        not null,
   disctotval           numeric(19,6)        null,
   taxid                varchar(12)          not null,
   sordlinestat         int                  not null,
   note                 varchar(200)         null,
   resid                int                  null,
   ucdid                int                  null,
   pjpid                int                  null,
   gen                  int                  not null,
   addusrid             varchar(12)          not null,
   adddate              datetime             not null,
   constraint pk_tmp_sordsord_sordline primary key (sordlineid)
)
go

 -- drop table olc_sordline_res

 create table olc_sordline_res (
  sordlineidres				int identity    not null,
  sordlineid				int				not null,  
  resid						int			    not null,
  preordersordlineid		int				null, 
  addusrid                  varchar(12)     not null,
  adddate                   datetime        not null,

  constraint pk_olc_sordline_res primary key (sordlineidres)
)

alter table olc_sordline_res add constraint fk_olc_sordline_res_sordid foreign key (sordlineid) references ols_sordline (sordlineid)
alter table olc_sordline_res add constraint fk_olc_sordline_res_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_sordline_res add constraint fk_olc_sordline_res_resid foreign key (resid) references ols_reserve (resid)
go
