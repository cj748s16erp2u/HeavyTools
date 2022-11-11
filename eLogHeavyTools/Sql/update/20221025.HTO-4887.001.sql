
create table olc_sp_ols_getnewsordnum (
    id			int not null,
	result		int null,
	docnum		varchar(12)
)
go

if (select object_id('[dbo].[sp_olc_sp_ols_getnewsordnum]')) is not null
  drop procedure [dbo].[sp_olc_sp_ols_getnewsordnum]
go

create procedure [dbo].[sp_olc_sp_ols_getnewsordnum]
 @storeId int, @sordDocId  varchar(12), @cmpId int, @date datetime, @store bit 
as 
	declare @result int, @docnum varchar(12)
    exec @result = sp_ols_getnewsordnum @sordDocId, @cmpId, @docnum out, @store, @date
    
	insert into olc_sp_ols_getnewsordnum
	select	 @storeId, @result result, @docnum docnum 
go


 insert into ols_recid values ('sp_ols_getnewsordnum',0)


  