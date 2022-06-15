/*
	Cikk színállás 
*/
create table olc_itemcolor (
	icid 	    		varchar(3),
	name				varchar(200) NOT NULL,	
 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_itemcolor primary key (icid),
	constraint fk_olc_itemcolor_addusrid foreign key (addusrid) references cfw_user(usrid)
)
go