/***************************************/
/* Szállítói rendelés kiegészítés          */
/***************************************/
create table [olc_pordhead] (
  [pordid]                    [int]             not null, -- primary key, foreign key
  [paritytypeid]              [int]                 null, -- paritás
  [advance]                   numeric(19,6)     	null, -- elõleg összege
  [clerkempid]                [int]                 null, -- ügyintézõ
  [locality]                  [varchar](50)     	null, -- helység
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_pordhead] primary key ([pordid])
)

alter table [olc_pordhead] add constraint [fk_olc_pordhead_pordid] foreign key ([pordid]) references [ols_pordhead] ([pordid])
alter table [olc_pordhead] add constraint [fk_olc_pordhead_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
go