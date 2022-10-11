 -- drop table olc_stline

 create table olc_stline (
  stlineid					int				not null,  
  origstlineid				int			    null,

  addusrid                  varchar(12)     not null,
  adddate                   datetime        not null,
  constraint pk_olc_stline primary key (stlineid)
)

alter table olc_stline add constraint fk_olc_stline_stid foreign key (stlineid) references ols_stline (stlineid)
alter table olc_stline add constraint fk_olc_stline_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_stline add constraint fk_olc_stline_origstlineid foreign key (origstlineid) references ols_stline (stlineid)
go
