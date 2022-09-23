insert into ols_stathead (statgrpid, statkey, descr, protect, addusrid, adddate, delstat)
values (501, 'oas_prllist.status', 'Finance utalás csomag státusz', 1, 'dev', getdate(), 0)
go

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 211, 10, 'Generálás', '211', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 212, 20, 'Generált', '212', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 213, 30, 'Rendezés', '213', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 214, 40, 'Rendezett', '214', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 215, 50, 'Elõállítás', '215', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 216, 60, 'Elõállított', '216', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 217, 70, 'Számozás', '217', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 218, 80, 'Számozott', '218', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 219, 90, 'Engedélyezés', '219', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 220, 100, 'Engedélyezett', '220', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 221, 110, 'Feladás', '221', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 222, 120, 'Részlegesen feladott', '222', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 223, 130, 'Feladott', '223', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 224, 140, 'Befejezés', '224', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 225, 150, 'Visszalépés', '225', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 226, 160, 'Helyreállítás', '226', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 227, 170, 'Megszakítás', '227', 'dev', getdate(), 0)

insert into [ols_statline] ([statgrpid], [value], [seqno], [name], [abbr], [addusrid], [adddate], [delstat])
values (501, 228, 180, 'Érvénytelen', '228', 'dev', getdate(), 0)
go