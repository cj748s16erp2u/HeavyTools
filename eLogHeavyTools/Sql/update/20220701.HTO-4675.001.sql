insert ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (510, 'cif_ebank_trans.importfields', 'Import Fields', 1, 0, 'dev', getdate(), 0)
go

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 1, 1, 'GLS: referencia1, �rt�k, p�nznem', '1', 'ref1,origvalue,origcur', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 2, 2, 'Foxpost: referencia1, referencia2, �rt�k', '2', 'ref1,ref2,origvalue', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 3, 3, 'PPP: referencia1, �rt�k, p�nznem', '3', 'ref1,origvalue,origcur', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 4, 4, 'Aviz� Hervis: referencia1, kedvezm�ny, �rt�k', '4', 'ref1,valueacc,origvalue', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, str1, addusrid, adddate, delstat)
values (510, 5, 5, 'Aviz� Intersport: referencia1, �rt�k, kedvezm�ny', '5', 'ref1,origvalue,valueacc', 'dev', getdate(), 0)
go