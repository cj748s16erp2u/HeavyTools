create table olc_partner (
    partnid				int						not null,
    oldcode				varchar(10)             null, -- regi kod
	wsemail				varchar(max)            null, -- Webshop email
	invoicelang			int						null, -- Szamla nyelve
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partner primary key (partnid),
    constraint fk_olc_partner_partnid foreign key (partnid) references ols_partner (partnid),
    constraint fk_olc_partner_addusrid foreign key (addusrid) references cfw_user (usrid)
)