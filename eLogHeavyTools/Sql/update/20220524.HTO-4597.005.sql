/* 
	Cikk szezon
*/  
create table olc_itemseason (
	isid 	    		varchar(12) NOT NULL,
	name				varchar(200) NOT NULL,

	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemseason primary key (isid),
	constraint fk_olc_itemseason_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go