DELETE FROM ols_typeline WHERE typegrpid = 1 -- hiaba lenne a delstat 1 a feluleten latszik tovabbra is

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 1, 20, 'Kisker', 'KIS', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 2, 30, 'Nagyker', 'NK', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 4, 40, 'Disztribútor', 'DI', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 8, 50, 'Key Accountant', 'KA', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 16, 60, 'Franchise', 'FR', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 32, 70, 'Közület', 'KOZ', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (1, 64, 80, 'Szállító', 'SZ', 'dev', getdate(), 0)

UPDATE ols_typeline SET abbr = 'SZH' WHERE typegrpid = 2 AND value = 1

UPDATE ols_typeline SET abbr = 'SZL' WHERE typegrpid = 2 AND value = 2

UPDATE ols_typeline SET abbr = 'SZT' WHERE typegrpid = 2 AND value = 32

DELETE FROM ols_typeline WHERE typegrpid = 2 AND value = 4 -- hiaba lenne a delstat 1 a feluleten latszik tovabbra is, ezert a delete

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (2, 64, null, 'Levelezési', 'LEV', 'dev', getdate(), 0)

DELETE FROM ols_typeline WHERE typegrpid = 11 -- hiaba lenne a delstat 1 a feluleten latszik tovabbra is, ezert a delete

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (11, 1, null, 'eSzámla', 'eSZ', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (11, 2, null, 'Pénzügy', 'PU', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (11, 4, null, 'Felszólítás', 'FEL', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (11, 8, null, 'Beszerzés', 'BESZ', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (11, 16, null, 'Ter. képviselő', 'TER', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (500, 1, 1, 'Kézi', 'K', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (500, 2, 2, 'Targonca', 'T', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (504, 1, 1, 'Normál', 'N', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (504, 2, 2, 'Mozgó', 'M', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (504, 3, 3, 'Átadó terület', 'T', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 1, 1, 'Szedőkocsi', 'SZK', 1, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 2, 2, 'Kocsi', 'K', null, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 3, 3, 'Raklap', 'RA', 2, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 4, 4, 'Rekesz', 'RE', 2, 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (509, 10, 10, 'Angol', 'ENG', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (509, 2, 20, 'Magyar', 'HU', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 1, 1, 'GLS: referencia1, érték, pénznem', '1', 'ref1,origvalue,origcur', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 2, 2, 'Foxpost: referencia1, referencia2, érték', '2', 'ref1,ref2,origvalue', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 3, 3, 'PPP: referencia1, érték, pénznem', '3', 'ref1,origvalue,origcur', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 4, 4, 'Avizó Hervis: referencia1, kedvezmény, érték', '4', 'ref1,valueacc,origvalue', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 5, 5, 'Avizó Intersport: referencia1, érték, kedvezmény', '5', 'ref1,origvalue,valueacc', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (511, 10, 10, 'Ex works', 'EXW', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (511, 20, 20, ' Free on board', 'FOB', 'dev', getdate(), 0)


insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 60, 60, 'K&H Bankkártya kivonat file', 'KHK', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 61, 61, 'OTP Bankkártya kivonat file', 'OTPK', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 62, 62, 'GLS avizó file', 'GLS', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 63, 63, 'FoxPost avizó file', 'FOXP', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 64, 64, 'Sprinter avizó file', 'PPP', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 65, 65, 'Hervis avizó file', 'HER', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (8001, 66, 66, 'Intersport avizó file', 'INT', 'dev', getdate(), 0)
go