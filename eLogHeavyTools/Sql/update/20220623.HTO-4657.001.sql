/***************************************/
/* Vevői rendelés kiegészítés          */
/***************************************/
create table [olc_sordhead] (
  [sordid]                    [int]             not null, -- primary key, foreign key
  [sordapprovalstat]          [int]             not null, -- jóváhagyás (jóváhagyandó, jóváhagyott, automatikusan jóváhagyott, elutasított)
  [loyaltycardno]             [varchar](50)         null, -- törzskáryta
  [transfcond]                [int]                 null, -- szállítási feltétel (GLS, FoxPost, Bolt, cím)
  [deliverylocation]          [varchar](200)        null, -- csomagpont
  [data]                      [xml]                 null, -- számlázási adatok (customer prefix: név, cím, szállítási cím, telefon, email; coupon prefix lista: kuponkód, ...)
  [regreprempid]    		  [int]                 null, -- területi képviselő 
  [clerkempid]                [int]                 null, -- ügyintéző
  [addusrid]                  [varchar](12)     not null,
  [adddate]                   [datetime]        not null,
  constraint [pk_olc_sordhead] primary key ([sordid])
)

alter table [olc_sordhead] add constraint [fk_olc_sordhead_sordid] foreign key ([sordid]) references [ols_sordhead] ([sordid])
alter table [olc_sordhead] add constraint [fk_olc_sordhead_addusrid] foreign key ([addusrid]) references [cfw_user] ([usrid])
go