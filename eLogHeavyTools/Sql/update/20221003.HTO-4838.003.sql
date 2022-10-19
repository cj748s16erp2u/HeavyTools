create table olc_sorddoc (
    sorddocid			varchar(12)				not null,
	frameordersorddocid varchar(12)				null,
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_sorddoc primary key (sorddocid),
    constraint fk_olc_sorddoc_sorddocid foreign key (sorddocid) references ols_sorddoc (sorddocid),
    constraint fk_olc_sorddoc_frameordersorddocid foreign key (frameordersorddocid) references ols_sorddoc (sorddocid),
    constraint fk_olc_sorddoc_addusrid foreign key (addusrid) references cfw_user (usrid)
)
 