-- drop table [imp_partnertype]

create table [imp_partnertype] (
  [key]                     [varchar](50)   not null,
  [name]                    [varchar](200)  not null,
  [u4partntype]             [varchar](12)       null,
)
go

insert into imp_partnertype values ('BSZ', 'Beszállító', 'SZ')
insert into imp_partnertype values ('KIS', 'Kisker vevő', 'KIS')
insert into imp_partnertype values ('VIP', 'VIP vevő', 'KIS')
insert into imp_partnertype values ('NET', 'WEB áruházas vevő', 'KIS')
insert into imp_partnertype values ('NKV', 'Nagyker vevő', 'NK')
insert into imp_partnertype values ('JLK', 'Julika', 'KIS')
insert into imp_partnertype values ('UK', 'Üzletkötő', 'KA')
insert into imp_partnertype values ('HT', 'Heavy Tools boltok', 'FR')
insert into imp_partnertype values (null, 'TXT-ből importálva', 'KIS')
insert into imp_partnertype values ('NE', 'Nagyker egyéni vevő', 'NK')
insert into imp_partnertype values ('SZK', 'Szlovák kisker vevő', 'KIS')
insert into imp_partnertype values ('CZK', 'Cseh kisker vevő', 'KIS')
insert into imp_partnertype values ('SNV', 'Szlovák nagyker vevő', 'NK')
insert into imp_partnertype values ('EX', 'Export partner', 'KIS')
insert into imp_partnertype values ('FR', 'Franchise partner', 'FR')
insert into imp_partnertype values ('MK', 'Marketing partner', 'KIS')
insert into imp_partnertype values ('CO', 'Közület partner', 'KOZ')
insert into imp_partnertype values ('RNV', 'Román nagyker vevő', 'NK')
insert into imp_partnertype values ('RKV', 'Román kisker vevő', 'KIS')
