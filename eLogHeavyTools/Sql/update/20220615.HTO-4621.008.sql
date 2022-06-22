-- drop table [imp_partninvlngid]

create table [imp_partninvlngid] (
  [key]                     [varchar](20)   not null,
  [name]                    [varchar](200)  not null,
  [u4lngid]                 [varchar](30)       null,
)
go

insert into imp_partninvlngid values ('SK', 'Szlovák', 'sk-SK')
insert into imp_partninvlngid values ('CZ', 'Cseh', 'cs-CZ')
insert into imp_partninvlngid values ('RO', 'Román', 'ro-RO')
insert into imp_partninvlngid values ('ENG', 'Angol', 'en-US')
insert into imp_partninvlngid values ('HUN', 'Magyar', 'hu-HU')
