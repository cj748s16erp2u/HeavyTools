insert ols_statline (statgrpid, value, seqno, name, abbr, addusrid, adddate, delstat)
values (10, 50, 50, 'Szállítónak vissza', 'SZV', 'dev', getdate(), 0 ) 

insert into ols_statext (statkey, value, xmldata, addusrid, adddate)
values('pinvhead.pinvstat', 50, '<statext><pinvheadeditdisabled /><costlinesoapintfcreateallowed /><codapostdisabled /></statext>', 'dev', getdate())
go

--Szallitoi szamla statusz valtas (10->50, 50->10)
declare @statextid int, @xmldata xml

select @statextid = statextid, @xmldata = xmldata
from ols_statext
where statkey = 'pinvhead.pinvstat'
  and value is null

declare @id int, @xmldata1 xml

select @id = max(a.b.value('id[1]', 'int')) from @xmldata.nodes('/statext/statchange/sc') a(b)
if @xmldata.exist('/statext/statchange/sc[fromstat="10" and tostat="50"]') = 0 begin
  set @id = @id + 1
  set @xmldata1 = '<sc><id>'+convert(varchar, @id)+'</id><fromstat>10</fromstat><tostat>50</tostat></sc>'
  set @xmldata.modify('insert sql:variable("@xmldata1") as last into (/statext/statchange)[1]')
end
if @xmldata.exist('/statext/statchange/sc[fromstat="50" and tostat="10"]') = 0 begin
  set @id = @id + 1
  set @xmldata1 = '<sc><id>'+convert(varchar, @id)+'</id><fromstat>50</fromstat><tostat>10</tostat></sc>'
  set @xmldata.modify('insert sql:variable("@xmldata1") as last into (/statext/statchange)[1]')
end

update ols_statext
set xmldata = @xmldata
where statextid = @statextid
go