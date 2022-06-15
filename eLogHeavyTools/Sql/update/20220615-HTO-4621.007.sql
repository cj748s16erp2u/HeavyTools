-- drop table [imp_partndelstat]

create table [imp_partndelstat] (
  [key]                     [varchar](20)   not null,
  [name]                    [varchar](200)  not null,
  [u4delstat]               [varchar](30)       null,
)
go

insert into imp_partndelstat values ('A', 'Aktív', 'A')
insert into imp_partndelstat values ('I', 'Inaktív', 'R')
insert into imp_partndelstat values ('M', 'Megszűnt', 'R')
insert into imp_partndelstat values ('P', 'Potenciális', 'R')
insert into imp_partndelstat values ('K', 'Kulcsügyfél', 'R')
