/*
	Webshop tábla
*/
create table olc_webshop ( 
	wid					varchar(12),
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


 