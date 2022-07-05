
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
