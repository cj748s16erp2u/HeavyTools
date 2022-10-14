
create table olc_sp_ols_reserve_reservestock (
    id			int not null,
	result		int null,
	errkey		varchar(1000)
)
go

if (select object_id('[dbo].[sp_olc_sp_ols_reserve_reservestock]')) is not null
  drop procedure [dbo].[sp_olc_sp_ols_reserve_reservestock]
go

create procedure [dbo].[sp_olc_sp_ols_reserve_reservestock]
 @storeId int, @resid int, @qty decimal(19, 6), @userid varchar(12)
as

	declare @Result int, @ErrKey varchar(40)
	exec @Result =	dbo.sp_ols_reserve_reservestock @resid, @qty, @userid, @ErrKey out
	
	insert into olc_sp_ols_reserve_reservestock
	 select @storeId, @Result result, @ErrKey errkey

go


 insert into ols_recid values ('sp_ols_reserve_reservestock',0)
