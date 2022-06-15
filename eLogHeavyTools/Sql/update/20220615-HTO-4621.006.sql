-- drop table [imp_partnvattyp]

create table [imp_partnvattyp] (
  [key]                     [varchar](20)   not null,
  [name]                    [varchar](200)  not null,
  [u4partnvattyp]           [varchar](30)       null,
)
go

insert into imp_partnvattyp values ('B', 'Belföldi adóalany', 'NORMAL')
insert into imp_partnvattyp values ('K', 'Külföldi, EU-n belül', 'EU')
insert into imp_partnvattyp values ('L', 'Külföldi, nem közösségi', '3DIK')
insert into imp_partnvattyp values ('M', 'Magánszemély', 'MAGAN')
insert into imp_partnvattyp values ('N', 'Nem adóalany', 'NEM')
