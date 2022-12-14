-- drop table [imp_countryregion]

create table [imp_countryregion] (
  [key]                     [int]           not null,
  [name]                    [varchar](200)  not null,
  [u4countryid]             [varchar](12)       null,
  [u4regionid]              [varchar](12)       null
)
go

insert into imp_countryregion values (0, 'Ismeretlen', 'HU', null)
insert into imp_countryregion values (1, 'Bács-Kiskun', 'HU', null)
insert into imp_countryregion values (2, 'Somogy', 'HU', null)
insert into imp_countryregion values (3, 'Békés', 'HU', null)
insert into imp_countryregion values (4, 'Borsod-AbaŁj-Zemplén', 'HU', null)
insert into imp_countryregion values (5, 'Csongrád', 'HU', null)
insert into imp_countryregion values (6, 'Fejér', 'HU', null)
insert into imp_countryregion values (7, 'Györ-Moson-Sopron', 'HU', null)
insert into imp_countryregion values (8, 'Hajdú-Bihar', 'HU', null)
insert into imp_countryregion values (9, 'Heves', 'HU', null)
insert into imp_countryregion values (10, 'Jász-Nagykun-Szolnok', 'HU', null)
insert into imp_countryregion values (11, 'Komárom-Esztergom', 'HU', null)
insert into imp_countryregion values (53, 'Románia', 'RO', null)
insert into imp_countryregion values (12, 'Nógrád', 'HU', null)
insert into imp_countryregion values (13, 'Pest', 'HU', null)
insert into imp_countryregion values (14, 'Zala', 'HU', null)
insert into imp_countryregion values (15, 'Szabolcs-Szatmár-Bereg', 'HU', null)
insert into imp_countryregion values (16, 'Tolna', 'HU', null)
insert into imp_countryregion values (17, 'Vas', 'HU', null)
insert into imp_countryregion values (18, 'Veszprém', 'HU', null)
insert into imp_countryregion values (19, 'Baranya', 'HU', null)
insert into imp_countryregion values (20, 'Budapest', 'HU', null)
insert into imp_countryregion values (40, 'Németország', 'DE', null)
insert into imp_countryregion values (41, 'Ausztria', 'AT', null)
insert into imp_countryregion values (42, 'Belgium', 'BE', null)
insert into imp_countryregion values (43, 'Lengyelország', 'PL', null)
insert into imp_countryregion values (44, 'Görögország', 'EL', null)
insert into imp_countryregion values (45, 'Nagy Britannia', 'GB', null)
insert into imp_countryregion values (46, 'Svédország', 'SE', null)
insert into imp_countryregion values (47, 'Hollandia', 'NL', null)
insert into imp_countryregion values (48, 'Olaszország', 'IT', null)
insert into imp_countryregion values (49, 'Franciaország', 'FR', null)
insert into imp_countryregion values (50, 'Dánia', 'DK', null)
insert into imp_countryregion values (51, 'Szlovákia', 'SK', null)
insert into imp_countryregion values (52, 'Switzerland', null, null)
insert into imp_countryregion values (54, 'Ciprus', 'CY', null)
insert into imp_countryregion values (55, 'Česká Republika', 'CZ', null)
insert into imp_countryregion values (56, 'Észtország', 'EE', null)
insert into imp_countryregion values (57, 'Spanyolország', 'ES', null)
insert into imp_countryregion values (58, 'Finnország', 'FI', null)
insert into imp_countryregion values (59, 'Írország', 'IE', null)
insert into imp_countryregion values (60, 'Litvánia', 'LT', null)
insert into imp_countryregion values (61, 'Luxemburg', 'LU', null)
insert into imp_countryregion values (62, 'Lettország', 'LV', null)
insert into imp_countryregion values (63, 'Málta', 'MT', null)
insert into imp_countryregion values (64, 'Portugália', 'PT', null)
insert into imp_countryregion values (65, 'Szlovénia', 'SI', null)
insert into imp_countryregion values (66, 'Bulgária', 'BG', null)
insert into imp_countryregion values (67, 'Norvégia', 'NO', null)
insert into imp_countryregion values (68, 'Ukrajna', 'UA', null)
insert into imp_countryregion values (69, 'USA', 'US', null)
insert into imp_countryregion values (70, 'Horvátország', 'HR', null)
go
