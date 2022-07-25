if exists(select 0 from sysobjects where name = 'cif_ebank_akcenta_load' and xtype = 'P')
  drop procedure dbo.cif_ebank_akcenta_load
go

CREATE PROCEDURE cif_ebank_akcenta_load
	@interfaceid varchar(50),
	@cmpcode varchar(12),
	@userid varchar(256)
AS
BEGIN
	UPDATE cif_errors 
	SET enduser=@userid, enddate=CURRENT_TIMESTAMP
	WHERE interfaceid=@interfaceid AND usr=@userid

	UPDATE cif_ebank_akcenta_075
	SET currency = CASE currency_code
		WHEN '00036' THEN 'AUD'
		WHEN '00124' THEN 'CAD'
		WHEN '00156' THEN 'CNY'
		WHEN '00203' THEN 'CZK'
		WHEN '00208' THEN 'DKK'
		WHEN '00978' THEN 'EUR'
		WHEN '00826' THEN 'GBP'
		WHEN '00191' THEN 'HRK'
		WHEN '00348' THEN 'HUF'
		WHEN '00756' THEN 'CHF'
		WHEN '00392' THEN 'JPY'
		WHEN '00578' THEN 'NOK'
		WHEN '00985' THEN 'PLN'
		WHEN '00946' THEN 'RON'
		WHEN '00643' THEN 'RUB'
		WHEN '00752' THEN 'SEK'
		WHEN '00949' THEN 'TRY'
		WHEN '00840' THEN 'USD'
		ELSE NULL 
	END
	WHERE interfaceid=@interfaceid AND username=@userid

	SELECT 0
END
GO