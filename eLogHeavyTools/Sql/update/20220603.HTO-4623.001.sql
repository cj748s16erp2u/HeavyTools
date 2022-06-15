create table olc_prctype (
	tpid				int identity(1, 1), 
	name				varchar(100) not null,

	isnet				int not null,
	 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_prctypen primary key (tpid), 
	constraint fk_olc_prctype_addusrid foreign key (addusrid) references cfw_user(usrid)
)

go   
create table olc_prctable (
	prcid				int identity(1, 1),
	ptid 	    		int not null,

	partnid				int null,
	addrid				int null,
	curid				varchar(12) not null,
	startdate			datetime not null,
	enddate				datetime not null,
	prc					numeric(16,9)not null,

	imid				int not null,				 
	isid				varchar(12) null,
	icid				varchar(3) null,
	itemid				int,

	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,

	constraint pk_olc_prctable primary key (prcid),
	constraint fk_olc_prctable_tpid foreign key (ptid) references olc_prctype(tpid),
	constraint fk_olc_prctable_partnid foreign key (partnid) references ols_partner(partnid),
	constraint fk_olc_prctable_addrid foreign key (addrid) references ols_partnaddr(addrid),
	constraint fk_olc_prctable_curid foreign key (curid) references ols_currency(curid), 
	constraint fk_olc_prctable_imid foreign key (imid) references olc_itemmodel(imid),
	constraint fk_olc_prctable_isid foreign key (isid) references olc_itemseason(isid),
	constraint fk_olc_prctable_icid foreign key (icid) references olc_itemcolor(icid),
	constraint fk_olc_prctable_itemid foreign key (itemid) references ols_item(itemid), 
	constraint fk_olc_prctable_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go
 

insert into olc_prctype values ('Elõkalkulált beszerzési ár',1,'dev',getdate(),0) 
insert into olc_prctype values ('Eredeti nagyker ár',1,'dev',getdate(),0) 
insert into olc_prctype values ('Akciós ár alapja',1,'dev',getdate(),0) 
insert into olc_prctype values ('Aktuális nagyker ár',1,'dev',getdate(),0) 
insert into olc_prctype values ('Disztribútor ár',1,'dev',getdate(),0) 
insert into olc_prctype values ('Bizományos ár',1,'dev',getdate(),0) 
insert into olc_prctype values ('Kisker ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('Outlet ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('Másodosztályú ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('.cz webshop ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('.sk webshop ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('.ro webshop ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('.com webshop ár',0,'dev',getdate(),0) 
insert into olc_prctype values ('.hu webshop ár',0,'dev',getdate(),0) 
