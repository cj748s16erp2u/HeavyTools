if exists(select 0 from sysobjects where name = 'cif_ebank_paypal_load' and xtype = 'P')
  drop procedure dbo.cif_ebank_paypal_load
go

CREATE PROCEDURE cif_ebank_paypal_load
	@interfaceid varchar(50),
	@cmpcode varchar(12),
	@userid varchar(256)
AS
BEGIN
	UPDATE cif_errors 
	SET enduser=@userid, enddate=CURRENT_TIMESTAMP
	WHERE interfaceid=@interfaceid AND usr=@userid

	DECLARE @filename NVARCHAR(50)
	SELECT TOP 1 @filename=filname FROM cif_ebank_paypal_lines WITH (NOLOCK) WHERE interfaceid=@interfaceid AND username=@userid 

	UPDATE cif_ebank_paypal_lines
	SET p_acnum = 'PAYPAL' + LTRIM(RTRIM(p_currency))
	WHERE interfaceid=@interfaceid AND username=@userid 

	DECLARE 
		@CURSOR_UPDATE_VALUE cursor,
		@pgross NVARCHAR(15),
		@pfee NVARCHAR(15),
		@pnet NVARCHAR(15),
		@new_pgross NVARCHAR(15),
		@new_pfee NVARCHAR(15),
		@new_pnet NVARCHAR(15),
		@position INT,
		@recid INT

		SET @CURSOR_UPDATE_VALUE = CURSOR FAST_FORWARD FOR
			SELECT p_gross, p_fee, p_net, id
			FROM cif_ebank_paypal_lines WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

		OPEN @CURSOR_UPDATE_VALUE
		FETCH NEXT FROM @CURSOR_UPDATE_VALUE INTO @pgross, @pfee, @pnet, @recid
		WHILE @@FETCH_STATUS = 0 
			BEGIN
				SET @position = 1;
				SET @new_pgross = '';

				WHILE @position <= DATALENGTH(@pgross)  
				   BEGIN
					   IF ASCII(SUBSTRING(@pgross, @position, 1)) <> 194	-- NULL
							BEGIN
								SET @new_pgross += CHAR(ASCII(SUBSTRING(@pgross, @position, 1)))
							END
						--SELECT ASCII(SUBSTRING(@pgross, @position, 1)),
						--CHAR(ASCII(SUBSTRING(@pgross, @position, 1)))
						SET @position = @position + 1  
				   END;

				SET @position = 1;
				SET @new_pfee = '';

				WHILE @position <= DATALENGTH(@pfee)  
				   BEGIN
					   IF ASCII(SUBSTRING(@pfee, @position, 1)) <> 194	-- NULL
							BEGIN
								SET @new_pfee += CHAR(ASCII(SUBSTRING(@pfee, @position, 1)))
							END
						SET @position = @position + 1  
				   END;

				SET @position = 1;
				SET @new_pnet = '';

				WHILE @position <= DATALENGTH(@pnet)  
				   BEGIN
					   IF ASCII(SUBSTRING(@pnet, @position, 1)) <> 194	-- NULL
							BEGIN
								SET @new_pnet += CHAR(ASCII(SUBSTRING(@pnet, @position, 1)))
							END
						SET @position = @position + 1  
				   END;

				   UPDATE cif_ebank_paypal_lines 
				   SET p_gross = @new_pgross, p_fee = @new_pfee, p_net = @new_pnet
				   WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename AND id = @recid
				FETCH NEXT FROM @CURSOR_UPDATE_VALUE INTO @pgross, @pfee, @pnet, @recid
			END
		CLOSE @CURSOR_UPDATE_VALUE
		DEALLOCATE @CURSOR_UPDATE_VALUE

	IF NOT EXISTS (
		SELECT 1
		FROM cif_ebank_paypal_lines WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid 
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @interfaceid, @filename, 1, 0, N'Nincs feldolgozhat?? adat!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE BEGIN

		DECLARE 
			@ok BIT,
			@cursor CURSOR,
			@acnum NVARCHAR(36)

		SELECT @ok = 1

		SET @cursor = CURSOR FAST_FORWARD FOR
			SELECT DISTINCT(p_acnum)
			FROM cif_ebank_paypal_lines WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

		OPEN @cursor
		FETCH NEXT FROM @cursor INTO @acnum
		WHILE @@FETCH_STATUS = 0 
			BEGIN

				INSERT INTO cif_ebank_trans (
					[interfaceid],
					[cmpcode],
					[fileid],
					[doccode],
					[docnum],
					[status],
					[ownacnum],
					[curaccount],
					[year],
					[statement],
					[trtypeid],
					[trid],
					[debcred],
					[yr],
					[period],
					[docdate],
					[duedate],
					[valdate],
					[curcode],
					[valuedoc],
					[amountsign],
					[origcur],
					[origvalue],
					[partnernamefrombank],
					[comment],
					[partneracnum],
					[postable],
					[matched],
					[createuser],
					[createdate],
					[extref1],
					[valueacc]
				)
				SELECT 
					@interfaceid,  -- interfaceid
					@cmpcode, -- cmpcode
					LEFT(filname, 50), --fileid
					' ', -- doccode
					' ', -- docnum
					111, -- status
					@acnum, -- ownacnum
					p_currency, -- curaccount
					DATEPART(year, convert(datetime, p_date, 102)), -- year
					1,  -- statement
					p_description, -- trtypeid
					p_transactionid, -- trid
					CASE WHEN convert(decimal(24,8), p_gross / 100) > 0 THEN 161 ELSE 160 END, -- debcred
					(
						SELECT MIN(10000*yr+period) / 10000
						FROM oas_perlist WITH (NOLOCK)
						WHERE 
							cmpcode = @cmpcode
							AND enddate >= convert(datetime, p_date, 102)
							AND period BETWEEN 1 AND 9997
					), -- yr
					(
						SELECT MIN(10000*yr+period) % 10000
						FROM oas_perlist WITH (NOLOCK)
						WHERE 
							cmpcode = @cmpcode
							AND enddate >= convert(datetime, p_date, 102)
							AND period BETWEEN 1 AND 9997
					), -- period
					convert(datetime, p_date, 102), -- doc
					convert(datetime, p_date, 102), -- due
					convert(datetime, p_date, 102), -- val
					p_currency, -- curcode
					--convert(decimal(24,8), p_gross) / 100, -- valuedoc fix 2 tizedes miatt
					ROUND(convert(decimal(24,8), p_gross) / 100, ISNULL(CD.decnum,0)), -- valuedoc fix 2 tizedes miatt
					convert(decimal(24,8), p_gross) / ABS(convert(decimal(24,8), p_gross)), -- amountsign
					p_currency, -- origcur
					--convert(decimal(24,8), p_gross) / 100, -- origvalue
					ROUND(convert(decimal(24,8), p_gross) / 100, ISNULL(CD.decnum,0)), -- origvalue
					p_name, -- partnernamefrombank
					p_description, -- comment
					p_fromemailaddr, -- partneraccnum
					1, -- postable,
					0, -- matched,
					@userid,
					CURRENT_TIMESTAMP,
					l.p_invoiceid,
					ROUND(CASE WHEN convert(decimal(24,8), p_gross / 100) > 0 
						THEN convert(decimal(24,8), p_fee / 100) * -1 
						ELSE convert(decimal(24,8), p_fee / 100) 
					END, ISNULL(CD.decnum,0)) -- valueacc
				FROM cif_ebank_paypal_lines L WITH (NOLOCK)
					JOIN oas_currency CD WITH (NOLOCK) ON
						CD.cmpcode = @cmpcode
						AND CD.code = l.p_currency
						AND CD.deldate IS NULL
				WHERE 
					interfaceid = @interfaceid 
					AND username = @userid 
					AND p_acnum = @acnum
				ORDER BY lineseqno

				FETCH NEXT FROM @cursor INTO @acnum
			END
		CLOSE @cursor
		DEALLOCATE @cursor

		SELECT 0
		RETURN

	END

	SELECT 1
END
GO