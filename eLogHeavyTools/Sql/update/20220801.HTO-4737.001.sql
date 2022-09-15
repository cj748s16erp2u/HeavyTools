insert into ols_bustype values ('OSS','Távértékesítés eu',0,'dev', getdate(),0)
 
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