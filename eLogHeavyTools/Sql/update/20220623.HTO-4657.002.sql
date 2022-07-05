/***************************************/
/* Vevői rendelés tétel kiegészítés    */
/***************************************/
create table [olc_sordline] (
  [sordlineid]                [int]             not null, -- primary key, foreign key
  [confqty]                   [numeric](19, 6)      null, -- visszaigazolt mennyiség
  [confdeldate]               [datetime]            null, -- visszaigazolt szállítási határidő
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_sordline] primary key ([sordlineid])
)

alter table [olc_sordline] add constraint [fk_olc_sordline_sordlineid] foreign key ([sordlineid]) references [ols_sordline] ([sordlineid])
alter table [olc_sordline] add constraint [fk_olc_sordline_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
go
