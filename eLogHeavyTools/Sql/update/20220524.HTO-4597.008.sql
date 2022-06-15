
CREATE PROCEDURE [dbo].[sp_olc_generatae_ean] @isStore int, @EAN varchar(100) OUTPUT
AS
begin
	declare @EANCode1 int, @EANCode2 int, @Number varchar(15), @i int, @sum int 
	 
	exec @EANCode1=sp_ols_getnewid 'Ean.Code.Pre', 0,0,0
	exec @EANCode2=sp_ols_getnewid 'Ean.Code', @isStore,0,1

	set @Number = LTrim(Str(@EANCode1-1)) + Right('00000'+LTrim(Str(@EANCode2)), 5)
	 
	set @i = 1
	set @sum = 0
	while (@i<=len(@Number))
	begin 
	  if @i % 2 = 0
		set @sum = @sum + CAST(Substring(@Number,@i,1) AS INT)*3
	  else
		set @sum = @sum + CAST(Substring(@Number,@i,1) AS INT)
	  set @i=@i+1
	end
	if @sum % 10 = 0
		set @sum = 0
	else
		set @sum = 10 - ( @sum % 10 )

	select @EAN=RTrim(@Number) + Substring(LTrim(Str(@sum)), 1, 1)
end
GO



