 
drop table olc_itemmodel
drop table olc_itemmaingroup
drop table olc_itemmaingrouptype1
drop table olc_itemmaingrouptype2

/* 
	Cikk fõcsoport típus 1. 
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
	Cikk fõcsoport típus 2. 
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
 
 go
/* 
	Cikk fõcsoport
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
	code				varchar(6) NOT NULL,
	name				varchar(200) NOT NULL,
	unitid				varchar(12) NOT NULL,
	exclusivetype		int,
	netweight			numeric(19, 6),
	grossweight			numeric(19, 6),
	volume				numeric(19, 6),
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemmodel primary key (imid),
	constraint fk_olc_itemmodel_imgid foreign key (imgid) references olc_itemmaingroup(imgid),
	constraint fk_olc_itemmodel_addusrid foreign key (addusrid) references cfw_user(usrid)
)

go