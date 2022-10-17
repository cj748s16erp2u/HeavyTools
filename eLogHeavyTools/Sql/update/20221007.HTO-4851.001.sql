 -- drop table olc_sthead

 create table olc_sthead (
  stid						int				not null, 
  onroadtowhid              varchar(12)		null,
  onroadfromstid			int			    null,

  addusrid                  varchar(12)     not null,
  adddate                   datetime        not null,
  constraint pk_olc_sthead primary key (stid)
)

alter table olc_sthead add constraint fk_olc_sthead_stid foreign key (stid) references ols_sthead (stid)
alter table olc_sthead add constraint fk_olc_sthead_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_sthead add constraint fk_olc_sthead_towhid foreign key (onroadtowhid) references ols_warehouse (whid)
alter table olc_sthead add constraint fk_olc_sthead_onroadfromstid foreign key (onroadfromstid) references ols_sthead (stid)
go