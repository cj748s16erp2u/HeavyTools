insert into ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (514, 'olc_whlocprio.priotype', 'Elsõdleges vagy másodlagos helykód', 1, 1, 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, name, abbr, addusrid, adddate, delstat)
values (514, 1, 'Elsõdleges helykód', 'E', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, name, abbr, addusrid, adddate, delstat)
values (514, 2, 'Másodlagos helykód', 'M', 'dev', getdate(), 0)