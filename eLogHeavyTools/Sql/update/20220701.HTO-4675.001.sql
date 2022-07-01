insert ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (510, 'cif_ebank_trans.importfields', 'Import Fields', 1, 0, 'dev', getdate(), 0)
go

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
go