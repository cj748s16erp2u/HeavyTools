create table olc_partnaddr (
    addrid				int						not null,
    oldcode				varchar(10)             null, -- regi kod
	glnnum				varchar(15)             null, -- GLN szam
	bname				varchar(30)             null, -- Epulet neve
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partnaddr primary key (addrid),
    constraint fk_olc_partnaddr_addrid foreign key (addrid) references ols_partnaddr (addrid),
    constraint fk_olc_partnaddr_addusrid foreign key (addusrid) references cfw_user (usrid)
)