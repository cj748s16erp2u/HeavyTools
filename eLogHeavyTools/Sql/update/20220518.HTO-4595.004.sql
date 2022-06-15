create table olc_partncmp (
    partnid				int						not null,
	cmpid				int						not null,
    secpaymid			varchar(12)             null, -- Masodlagos fizetesi mod
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_partncmp primary key (partnid, cmpid), -- composite primary key
    constraint fk_olc_partncmp_partnid foreign key (partnid) references ols_partner (partnid),
	constraint fk_olc_partncmp_cmpid foreign key (cmpid) references ols_company (cmpid),
    constraint fk_olc_partncmp_addusrid foreign key (addusrid) references cfw_user (usrid)
)