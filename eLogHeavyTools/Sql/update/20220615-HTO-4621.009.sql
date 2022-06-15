-- drop table [imp_companyhierarchy]

create table [imp_companyhierarchy] (
  [whkey]                   [int]           not null,
  [key]                     [int]           not null,
  [name]                    [varchar](200)  not null,
  [mainkey]                 [int]           not null,
  [prconpurch]              [int]               null,
)
go

insert into imp_companyhierarchy values (1, 1, 'HT Hungary', 19, 0)
insert into imp_companyhierarchy values (2, 2, 'HT Retail', 23, 0)
insert into imp_companyhierarchy values (3, 3, 'Hervis', 6307, 0)
insert into imp_companyhierarchy values (4, 4, 'Goker', 6310, 0)
insert into imp_companyhierarchy values (5, 5, 'x', 0, 0)
insert into imp_companyhierarchy values (6, 6, 'Smart Centrum', 6928, 0)
insert into imp_companyhierarchy values (7, 7, 'Auchan', 6946, 0)
insert into imp_companyhierarchy values (8, 8, 'AS-CONSTRUCT KFT.', 1011, 0)
insert into imp_companyhierarchy values (9, 9, 'Intersport Debr-Miskolc', 8523, 0)
insert into imp_companyhierarchy values (10, 10, 'Intersport', 75717, 0)
insert into imp_companyhierarchy values (11, 11, 'Intersport SPG', 76872, 0)
insert into imp_companyhierarchy values (12, 12, 'Intersport Allée', 75712, 0)
insert into imp_companyhierarchy values (13, 13, 'Intersport Érd', 75714, 0)
insert into imp_companyhierarchy values (14, 14, 'Intersport Veszprém', 1002, 0)
insert into imp_companyhierarchy values (15, 15, 'Intersport Győr', 76876, 0)
insert into imp_companyhierarchy values (16, 16, 'Intersport Keszthely', 76877, 0)
insert into imp_companyhierarchy values (17, 17, 'Intersport Nyíregyháza', 75749, 0)
insert into imp_companyhierarchy values (18, 18, 'Deka Debrecen-Miskolc', 93741, 0)
insert into imp_companyhierarchy values (19, 19, 'Márkaboltok', 10, 0)
insert into imp_companyhierarchy values (20, 20, 'Intersport Alee', 201845, 0)
insert into imp_companyhierarchy values (21, 0, '', 0, 0)
insert into imp_companyhierarchy values (22, 21, 'Intersport Ausztria', 202048, 0)
insert into imp_companyhierarchy values (23, 22, 'Intersport CZ', 203165, 0)
insert into imp_companyhierarchy values (24, 23, 'Intersport HU Kft', 201845, 0)
insert into imp_companyhierarchy values (25, 24, 'Hervis Románia', 204770, 0)
insert into imp_companyhierarchy values (26, 25, 'Heavy Tools Slovakia', 203738, 0)
insert into imp_companyhierarchy values (27, 26, 'MOL', 209339, 0)
insert into imp_companyhierarchy values (28, 27, 'SZ+K', 25, 0)
insert into imp_companyhierarchy values (29, 28, 'Peek & Cloppenburg', 209860, 0)
insert into imp_companyhierarchy values (30, 29, 'Office Depot', 212243, 0)
insert into imp_companyhierarchy values (31, 30, 'Auchan központi raktár', 6946, 0)
insert into imp_companyhierarchy values (32, 31, 'Mountex', 1132, 1)
insert into imp_companyhierarchy values (33, 32, 'Divat és Sport', 77607, 1)
insert into imp_companyhierarchy values (34, 33, 'Sába gold', 209738, 1)
insert into imp_companyhierarchy values (35, 34, 'Premium Brand Line', 53223, 1)
insert into imp_companyhierarchy values (36, 35, 'Gigler Tibor', 71221, 1)
insert into imp_companyhierarchy values (37, 36, 'Sári Zoltán', 63182, 1)
insert into imp_companyhierarchy values (38, 37, 'Bocskoros 2005 Kft.', 221282, 1)
