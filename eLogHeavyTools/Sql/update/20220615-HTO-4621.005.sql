-- drop table [imp_paymethod]

create table [imp_paymethod] (
  [key]                     [varchar](20)   not null,
  [name]                    [varchar](200)  not null,
  [u4paymid]                [varchar](12)       null,
)
go

insert into imp_paymethod values ('A', 'Atutalás', 'UT')
insert into imp_paymethod values ('B', 'Bizomány', 'KEZI')
insert into imp_paymethod values ('C', 'Compnezáció', 'KOMP')
insert into imp_paymethod values ('H', 'Hitelkártya', 'KARTYA')
insert into imp_paymethod values ('K', 'Készpénz', 'KP')
insert into imp_paymethod values ('U', 'Utánvét', 'KEZI')
