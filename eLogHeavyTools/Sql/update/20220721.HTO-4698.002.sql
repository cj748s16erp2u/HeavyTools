insert ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (511, 'olc_pordhead.paritytype', 'Szállítói rendelés paritás', 1, 0, 'dev', getdate(), 0)
go

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (511, 10, 10, 'Ex works', 'EXW', 'dev', getdate(), 0)

insert ols_typeline (typegrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (511, 20, 20, ' Free on board', 'FOB', 'dev', getdate(), 0)
