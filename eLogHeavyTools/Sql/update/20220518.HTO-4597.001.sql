/* 
	Cikk fõcsoport típus 1. 
*/
create table olc_itemmaingrouptype1 (
	imgt1 	    		varchar(1),
	groupname			varchar(200) NOT NULL,	
	grouplastnum		int not null,
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmaingrouptype1 primary key (imgt1),
	constraint fk_olc_itemmaingrouptype1_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go

/* 
	Cikk fõcsoport típus 2. 
*/
create table olc_itemmaingrouptype2 (
	imgt2 	    		varchar(1),
	groupname			varchar(200) NOT NULL,
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmaingrouptype2 primary key (imgt2),
	constraint fk_olc_itemmaingrouptype2_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go
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
	Cikk fõcsoport
*/    
create table olc_itemmaingroup (
	imgid 	    		int identity(1, 1),
	imgt1 	    		varchar(1) NOT NULL,
	code				varchar(3) NOT NULL,
	name				varchar(200) NOT NULL,
	imgt2 	    		varchar(1) NOT NULL,
	isrhid 	    		int NOT NULL,
	itemgrpid			varchar(12) NOT NULL,
	 
	oldcode				varchar(5), 
	maingrouplastnum	int not null,
	 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmaingroup primary key (imgid),

	constraint fk_olc_itemmaingroup_imgt1 foreign key (imgt1) references olc_itemmaingrouptype1(imgt1),
	constraint fk_olc_itemmaingroup_imgt2 foreign key (imgt2) references olc_itemmaingrouptype2(imgt2),
	constraint fk_olc_itemmaingroup_isrhid foreign key (isrhid) references olc_itemsizerangehead(isrhid),
	constraint fk_olc_itemmaingroup_itemgrpid foreign key (itemgrpid) references ols_itemgroup(itemgrpid),
	constraint fk_olc_itemmaingroup_addusrid foreign key (addusrid) references cfw_user(usrid)
)

go
 