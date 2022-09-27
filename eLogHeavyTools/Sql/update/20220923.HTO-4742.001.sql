if exists(select 1 from sys.columns where object_id = object_id('olc_partncmp') and name = 'scontoavoveaccno')
begin
  exec sp_rename 'olc_partncmp.scontoavoveaccno', 'scontoaboveaccno', 'COLUMN'
end
go