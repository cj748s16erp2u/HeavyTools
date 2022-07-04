drop table olc_prctable

drop table olc_prctype

/*
	Ártábla ár típus
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

	insert into olc_prctype values ('Elõkalkulált beszerzési ár',1,'dev',getdate(),0)
	insert into olc_prctype values ('Nagyker ár',1,'dev',getdate(),0)
	insert into olc_prctype values ('Kisker ár',0,'dev',getdate(),0) 
	insert into olc_prctype values ('Outlet ár',0,'dev',getdate(),0)
	insert into olc_prctype values ('Disztribútor ár',1,'dev',getdate(),0) 
	insert into olc_prctype values ('Webshop ár',0,'dev',getdate(),0) 
	insert into olc_prctype values ('Bizományos ár',1,'dev',getdate(),0) 
	insert into olc_prctype values ('Másodosztályú ár',0,'dev',getdate(),0) 
	 