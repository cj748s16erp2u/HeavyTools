
if exists(select 0 from sysobjects where name = 'fn_olc_prctable_order' and xtype = 'FN')
  drop function dbo.fn_olc_prctable_order
go
create function dbo.fn_olc_prctable_order(@partnid int, @addrid	int, @isid varchar(12), @icid varchar(12), @itemid int, @prctype int) 
	returns int
as
begin
	declare @res int
	set @res=1
   
	if (@addrid is not null) 
		set  @res=1000

	if (@partnid is not null) 
		set  @res=@res+100

	if (@itemid is not null) 
		set  @res=@res+50

	if (@isid is not null) 
		set  @res=@res+20

	if (@icid is not null) 
		set  @res=@res+10

	if (@prctype<>1)
		set @res=-@res

/*
	Original = 0, // Eredeti
	Actual = 1,  // Aktuális
	BasisOfAction = 2,  // Akció alapja
*/
   return @res
end
go
