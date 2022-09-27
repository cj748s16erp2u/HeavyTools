go
 
create table olc_apilogger (  
	apiid 	    		int identity(1, 1),
	command				varchar(100) not null, 
	request				varchar(max) not null,
	response			varchar(max) null,
	constraint pk_olc_apilogger primary key (apiid),
)
go