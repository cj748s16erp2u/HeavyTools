drop table olc_prctable

drop table olc_prctype

/*
	�rt�bla �r t�pus
*/
create table olc_prctype (
	ptid				int identity(1, 1), 
	name				varchar(100) not null,

	isnet				int not null,
	 
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_prctypen primary key (ptid), 
	constraint fk_olc_prctype_addusrid foreign key (addusrid) references cfw_user(usrid)
)

	insert into olc_prctype values ('El�kalkul�lt beszerz�si �r',1,'dev',getdate(),0)
	insert into olc_prctype values ('Nagyker �r',1,'dev',getdate(),0)
	insert into olc_prctype values ('Kisker �r',0,'dev',getdate(),0) 
	insert into olc_prctype values ('Outlet �r',0,'dev',getdate(),0)
	insert into olc_prctype values ('Disztrib�tor �r',1,'dev',getdate(),0) 
	insert into olc_prctype values ('Webshop �r',0,'dev',getdate(),0) 
	insert into olc_prctype values ('Bizom�nyos �r',1,'dev',getdate(),0) 
	insert into olc_prctype values ('M�sodoszt�ly� �r',0,'dev',getdate(),0) 
	 