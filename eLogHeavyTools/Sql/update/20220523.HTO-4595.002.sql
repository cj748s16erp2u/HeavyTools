UPDATE ols_typeline SET abbr = 'SZH' WHERE typegrpid = 2 AND value = 1

UPDATE ols_typeline SET abbr = 'SZL' WHERE typegrpid = 2 AND value = 2

UPDATE ols_typeline SET abbr = 'SZT' WHERE typegrpid = 2 AND value = 32

DELETE FROM ols_typeline WHERE typegrpid = 2 AND value = 4 -- hiaba lenne a delstat 1 a feluleten latszik tovabbra is, ezert a delete

insert into [ols_typeline] ([typegrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (2, 64, null, 'Levelezési', 'LEV', 'dev', getdate(), 0)