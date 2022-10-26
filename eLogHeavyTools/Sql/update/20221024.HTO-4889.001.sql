create table olc_costline (
  costlineid      int             not null,
  othtrlinedocid  varchar(20)     not null,
  addusrid        varchar(12)     not null,
  adddate         datetime        not null,
  constraint pk_olc_costline primary key (costlineid),
  constraint fk_olc_costline_costlineid foreign key (costlineid) references ols_costline (costlineid),
  constraint fk_olc_costline_othtrlinedocid foreign key (othtrlinedocid) references ofc_othtrlinedoc (othtrlinedocid),
  constraint fk_olc_costline_addusrid foreign key (addusrid) references cfw_user (usrid)
)
go