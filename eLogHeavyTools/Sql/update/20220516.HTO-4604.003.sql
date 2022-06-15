insert ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (500, 'olc_whzone.pickingtype', 'Zóna szedés módja', 1, 0, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (500, 1, 1, 'Kézi', 'K', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (500, 2, 2, 'Targonca', 'T', 'dev', getdate(), 0)

insert ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (504, 'olc_whlocation.loctype', 'Helykód típusa', 1, 0, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (504, 1, 1, 'Normál', 'N', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (504, 2, 2, 'Mozgó', 'M', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (504, 3, 3, 'Átadó terület', 'T', 'dev', getdate(), 0)

insert ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (506, 'olc_whlocation.movloctype', 'Mozgó helykód típusa', 1, 0, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 1, 1, 'Szedőkocsi', 'SZK', 1, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 2, 2, 'Kocsi', 'K', null, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 3, 3, 'Raklap', 'RA', 2, 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, int1, addusrid, adddate, delstat)
values (506, 4, 4, 'Rekesz', 'RE', 2, 'dev', getdate(), 0)
