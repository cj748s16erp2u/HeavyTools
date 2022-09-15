create type oasdoclineids as table (cmpcode nvarchar(12) not null, doccode nvarchar(12) not null, docnum nvarchar(12) not null, doclinenum int not null)
go

create table ofc_reminderletterpayerid (
  id          int          identity(1, 1),
  doclineids  xml          null,
  payerid     char(16)     null,
  prlcode     nvarchar(28) null, 
  elmcode     nvarchar(36) null, 
  elmlevel    integer      null,
  severity    integer      null,
  remdate     datetime     null, 
  duedate     datetime     null,
  adddate     datetime not null,
  constraint pk_ofc_reminderletterpayerid primary key (id)
)
go