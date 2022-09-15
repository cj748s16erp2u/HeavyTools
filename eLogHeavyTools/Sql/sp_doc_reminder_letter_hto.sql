if (select object_id('[dbo].[sp_doc_reminder_letter_hto]')) is not null
  drop procedure [dbo].[sp_doc_reminder_letter_hto]
go

create procedure [dbo].[sp_doc_reminder_letter_hto] @guid uniqueidentifier
as
  declare @lcstatement nvarchar(4000)
  declare @repparams table (name  varchar(25) not null,
                            value sql_variant     null)

  declare 
    @codadb nvarchar(100),
	@cmpcode nvarchar(12),
	@prlcode nvarchar(28),
	@language nvarchar(50),
	@remdate nvarchar(50),
	@duedate_param nvarchar(50),
	@partner_from nvarchar(72),
	@partner_to nvarchar(72),
	@user nvarchar(50),
    @cheque int

  insert into @repparams select name,value from cfw_repparams
   where repcallguid = @guid

  select @cmpcode=convert(varchar,value) from @repparams
   where name = 'cmpcode'

  select @codadb=convert(varchar,value) from @repparams
   where name = 'codadb'

  select @prlcode=convert(varchar,value) from @repparams
   where name = 'prlcode'

  select @language=convert(varchar,value) from @repparams
   where name = 'language'

  select @remdate=convert(varchar,value) from @repparams
   where name = 'remdate'

  select @duedate_param=convert(varchar,value) from @repparams
   where name = 'duedate_param'

  select @partner_from=convert(varchar,value) from @repparams
   where name = 'partner_from'

  select @partner_to=convert(varchar,value) from @repparams
   where name = 'partner_to'       
  
  select @user=convert(varchar,value) from @repparams
   where name = 'user'    

  select @cheque=convert(int,value) from @repparams
   where name = 'action'  

/* Összeállítjuk a paraméterezett tárolt eljárást hívást */
set @lcstatement = @codadb + '..' + 'rep2_reminder_letter_HTO ' +
'@cmpcode = ' +  isnull('''' + @cmpcode + '''', 'null') + 
', @prlcode = ' + isnull('''' + @prlcode + '''', 'null') +
', @language = ' + isnull('''' + @language + '''', 'null') +
', @remdate = ' + isnull('''' + @remdate + '''', 'null') +
', @duedate_param = ' + isnull('''' + @duedate_param + '''', 'null') +
', @partner_from = ' + isnull('''' + @partner_from + '''', 'null') +
', @partner_to = ' + isnull('''' + @partner_to + '''', 'null') +
', @user = ' + isnull('''' + @user + '''', 'null') +
', @cheque = ' + isnull('' + convert(varchar, @cheque) + '', 'null')


print @lcstatement
exec (@lcstatement)

/* RIPORT NYELV: */
select convert(varchar(10), value) langid
  from cfw_repparams 
 where repcallguid = @guid 
   and name = 'lngid'
