insert into ols_typehead (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (514, 'olc_whlocprio.priotype', 'Els�dleges vagy m�sodlagos helyk�d', 1, 1, 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, name, abbr, addusrid, adddate, delstat)
values (514, 1, 'Els�dleges helyk�d', 'E', 'dev', getdate(), 0)

insert into ols_typeline (typegrpid, value, name, abbr, addusrid, adddate, delstat)
values (514, 2, 'M�sodlagos helyk�d', 'M', 'dev', getdate(), 0)