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