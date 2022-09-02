/*
https://www.sqlteam.com/forums/topic.asp?TOPIC_ID=76195
*/

if (object_id('dbo.fn_getluhn') is not null)
  drop function dbo.fn_getluhn
go

create function	dbo.fn_getluhn
(
	@luhn char(15)
)
returns char(16)
as

begin
	if @luhn like '%[^0-9]%'
		return @luhn

	declare	@index smallint,
		@multiplier tinyint,
		@sum int,
		@plus tinyint

	select	@index = len(@luhn),
		@multiplier = 2,
		@sum = 0

	while @index >= 1
		select	@plus = @multiplier * cast(substring(@luhn, @index, 1) as tinyint),
			@multiplier = 3 - @multiplier,
			@sum = @sum + @plus / 10 + @plus % 10,
			@index = @index - 1

	return	@luhn + case when @sum % 10 = 0 then '0' else cast(10 - @sum % 10 as char) end
end
