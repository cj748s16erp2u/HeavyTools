 -- drop table olc_sordline_res
 
 create table olc_sordline_res (
  sordlineidres				int identity    not null,

  sordlineid				int				not null,  
  resid						int			    not null,
  preordersordlineid		int				null, 

  addusrid                  varchar(12)     not null,
  adddate                   datetime        not null,

  constraint pk_olc_sordline_res primary key (sordlineidres)
)

alter table olc_sordline_res add constraint fk_olc_sordline_res_presordersordlineid foreign key (presordersordlineid) references ols_sordline (sordlineid)
alter table olc_sordline_res add constraint fk_olc_sordline_res_sordlineid foreign key (sordlineid) references ols_sordline (sordlineid)
alter table olc_sordline_res add constraint fk_olc_sordline_res_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_sordline_res add constraint fk_olc_sordline_res_resid foreign key (resid) references ols_reserve (resid)
go
