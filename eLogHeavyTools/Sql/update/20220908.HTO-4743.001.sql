insert into ols_stathead (statgrpid, statkey, descr, protect, addusrid, adddate, delstat)
values (501, 'oas_prllist.status', 'Finance utal�s csomag st�tusz', 1, 'dev', getdate(), 0)
go

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 211, 10, 'Gener�l�s', '211', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 212, 20, 'Gener�lt', '212', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 213, 30, 'Rendez�s', '213', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 214, 40, 'Rendezett', '214', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 215, 50, 'El��ll�t�s', '215', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 216, 60, 'El��ll�tott', '216', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 217, 70, 'Sz�moz�s', '217', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 218, 80, 'Sz�mozott', '218', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 219, 90, 'Enged�lyez�s', '219', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 220, 100, 'Enged�lyezett', '220', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 221, 110, 'Felad�s', '221', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 222, 120, 'R�szlegesen feladott', '222', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 223, 130, 'Feladott', '223', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 224, 140, 'Befejez�s', '224', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 225, 150, 'Visszal�p�s', '225', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 226, 160, 'Helyre�ll�t�s', '226', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 227, 170, 'Megszak�t�s', '227', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 228, 180, '�rv�nytelen', '228', 'dev', getdate(), 0)
go