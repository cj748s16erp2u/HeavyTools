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
	colortype			int NULL,
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