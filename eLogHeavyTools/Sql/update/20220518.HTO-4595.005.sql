insert into [ols_typehead] (typegrpid, typekey, descr, protect, multipl, addusrid, adddate, delstat)
values (509, 'olc_partner.InvLangType', 'Partner tulajdonság: számla nyelve', 1, 0, 'dev', GETDATE(),0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (509, 10, 10, 'Angol', 'ENG', 'dev', getdate(), 0)

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (509, 20, 20, 'Magyar', 'HU', 'dev', getdate(), 0)