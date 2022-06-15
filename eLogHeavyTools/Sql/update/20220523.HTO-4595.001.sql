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