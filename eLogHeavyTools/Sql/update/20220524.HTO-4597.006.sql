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
