

create table imp_colorexception (
	ice 	    		int identity(1, 1),
	modelnumber varchar(200) not null,
	colourcode varchar(200) not null,
	colorbalance varchar(200) not null,
	color1 varchar(200) null,
	color2 varchar(200) null,
	color3 varchar(200) null,
	sample1 varchar(200) null,
	sample2 varchar(200) null,
	season varchar(200) not null,
	constraint pk_imp_colorexception primary key (ice)
)