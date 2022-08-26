insert into ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (512, 'olc_partncmp.scontoinvoice', 'Skontó plusszos számlák után', 1, 0, 'dev', getdate(), 0)

insert into ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (513, 'olc_partncmp.referencetype', 'Referencia típusa', 1, 0, 'dev', getdate(), 0)
go

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (512, 0, null, 'Nem', '0', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (512, 1, null, 'Igen', '1', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (512, 2, null, 'Nincs skonto', '2', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (513, 10, 10, 'Számla', 'SZLA', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (513, 20, 20, 'Szállítólevél szám', 'SZLE', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (513, 30, 30, 'Rendelészám', 'RSZ', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (513, 40, 40, 'Dátum', 'DAT', 'dev', getdate(), 0)
go