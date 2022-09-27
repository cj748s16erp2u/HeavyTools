if (object_id('sp_ofc_reminderletter_getpayerid') is not null)
  drop procedure sp_ofc_reminderletter_getpayerid
go
create procedure sp_ofc_reminderletter_getpayerid
  @doclineids oasdoclineids readonly,
  @prlcode varchar(60), 
  @elmcode varchar(72), 
  @elmlevel integer,
  @severity integer,
  @remdate DATE,
  @duedate DATE,
  @payerid char(16) output
as
begin
  declare @x varchar(max)
  set @x = (select * from @doclineids for xml path('doclineid'), root('xmldata'))

  declare @id int
  insert into ofc_reminderletterpayerid (doclineids, prlcode, elmcode, elmlevel, severity, remdate, duedate, adddate) values(@x, @prlcode, @elmcode, @elmlevel, @severity, @remdate, @duedate, getdate())
  select @id = @@identity

  set @payerid =
    '15' + -- fizetesi felszolitas
    '1' + -- verzio
    right('00000000' + convert(varchar(9), @id), 9) + -- adat (1mrd db)
    right('00' + convert(varchar(3), ((@id ^ 0x96a5) % 1000)), 3) --+ -- filler
    --'0' -- CDV todo!
  set @payerid = dbo.fn_getluhn(@payerid) --cdv-vel kiegészített payerid


  update ofc_reminderletterpayerid
  set payerid = @payerid
  where id = @id
end
go

