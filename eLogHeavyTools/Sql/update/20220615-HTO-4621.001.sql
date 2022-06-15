exec sp_rename N'olc_partnaddr.bname', N'buildingname', N'COLUMN'
alter table olc_partnaddr alter column buildingname varchar(50) null
go

alter table olc_partncmp add constraint fk_olc_partncmp_partnid_cmpid foreign key (partnid, cmpid) references ols_partncmp (partnid, cmpid)
alter table olc_partncmp add constraint fk_olc_partncmp_secpaymid foreign key (secpaymid) references ols_paymethod (paymid)
go

exec sp_rename N'olc_partner.invoicelang', N'invlngid', N'COLUMN'
alter table olc_partner alter column invlngid varchar(12) null
go

delete tl from ols_typeline tl join ols_typehead th on th.typegrpid = tl.typegrpid where th.typekey = 'olc_partner.InvLangType'
delete th from ols_typehead th where th.typekey = 'olc_partner.InvLangType'
go

insert into [cfw_language] values ('sk-SK', 'Slovak')
insert into [cfw_language] values ('cs-CZ', 'Czech')
insert into [cfw_language] values ('ro-RO', 'Romanian')
go
