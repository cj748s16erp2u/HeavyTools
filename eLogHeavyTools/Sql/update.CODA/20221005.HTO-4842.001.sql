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

	DECLARE @filename NVARCHAR(50)
	SELECT TOP 1 @filename=filname FROM cif_ebank_akcenta_header WITH (NOLOCK) WHERE interfaceid=@interfaceid AND username=@userid 

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

	-- transaction amount (comma and two decimal places)
	UPDATE cif_ebank_akcenta_078
	SET 		
		transaction_info_orig=LTRIM(RTRIM(transaction_info1)),
		transaction_info_p1=LTRIM(RTRIM(LEFT(transaction_info1, CHARINDEX(' ', transaction_info1) - 1))),
		transaction_info1=LTRIM(RTRIM(RIGHT(transaction_info1, LEN(transaction_info1) - CHARINDEX(' ', transaction_info1))))
	WHERE interfaceid=@interfaceid AND username=@userid 

	-- transaction currency (e.g. EUR)
	UPDATE cif_ebank_akcenta_078
	SET 		
		transaction_info_p2=LTRIM(RTRIM(LEFT(transaction_info1, CHARINDEX(' ', transaction_info1) - 1))),
		transaction_info1=LTRIM(RTRIM(RIGHT(transaction_info1, LEN(transaction_info1) - CHARINDEX(' ', transaction_info1))))
	WHERE interfaceid=@interfaceid AND username=@userid 

	-- FX rate used for conversion (six decimal places)
	UPDATE cif_ebank_akcenta_078
	SET 		
		transaction_info_p3=LTRIM(RTRIM(transaction_info1)),
		transaction_info1=''
	WHERE interfaceid=@interfaceid AND username=@userid 

	IF NOT EXISTS (
		SELECT 1
		FROM cif_ebank_akcenta_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid 
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @interfaceid, @filename, 1, 0, N'Nincs feldolgozható adat!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE BEGIN
			DECLARE 
				@ok BIT,
				@cursor CURSOR,
				@acnum NVARCHAR(36)
			SELECT @ok = 1

			SET @cursor = CURSOR FAST_FORWARD FOR
				SELECT DISTINCT(accountnumber)
				FROM cif_ebank_akcenta_header WITH (NOLOCK)
				WHERE interfaceid=@interfaceid AND username=@userid

			OPEN @cursor
			FETCH NEXT FROM @cursor INTO @acnum
			WHILE @@FETCH_STATUS = 0  BEGIN
				DECLARE @count INT
				SELECT @count=COUNT(1)
				FROM cif_ebank_akcenta_header WITH (NOLOCK)
				WHERE interfaceid=@interfaceid AND username=@userid AND accountnumber=@acnum

				IF @count > 1 BEGIN
					INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
					VALUES(@interfaceid, @filename, @filename, 1, 0, N'Egy fájlban egy bankszámla csak egyszer szerepelhet!', @userid, CURRENT_TIMESTAMP, 1)
					SELECT @ok = 0
				END
				ELSE BEGIN
					DECLARE 
						@this INT,
						@next INT,
						@accur NVARCHAR(3),
						@year SMALLINT,
						@statement SMALLINT,
						@opening DECIMAL(24,8),
						@closing DECIMAL(24,8),
						@stdate DATETIME

					SELECT @this = MIN(l75.lineseqno), @next = ISNULL(CONVERT(INT, MIN(NXT.lineseqno)), 99999999) 
					FROM cif_ebank_akcenta_075 l75 WITH (NOLOCK) 
					LEFT OUTER JOIN cif_ebank_akcenta_075 NXT WITH (NOLOCK) ON NXT.interfaceid=@interfaceid AND NXT.username=@userid AND NXT.lineseqno > l75.lineseqno
					WHERE l75.interfaceid=@interfaceid AND l75.username=@userid AND l75.accountnumber=@acnum

					SELECT @statement=h.statementno, @year=year(l75.postingdate), @accur=l75.currency,
							@opening=CASE h.openbalsign WHEN N'+' THEN 1 ELSE -1 END * h.openbalance / 100, 
							@closing=CASE h.closebalsign WHEN N'+' THEN 1 ELSE -1 END * h.closebalance / 100,
							@stdate=h.statementdate
					FROM cif_ebank_akcenta_075 l75 WITH (NOLOCK) 
						JOIN cif_ebank_akcenta_header h WITH (NOLOCK) ON h.interfaceid = l75.interfaceid AND h.username = l75.username AND l75.lineseqno BETWEEN @this AND @next

					DECLARE	@retval INT, @fake_acnum NVARCHAR(36)
					set @fake_acnum = @acnum+LEFT(REPLACE('', '-', '') + '00000000', 16)
					EXEC @retval = cif_ebank_check
						@interfaceid = @interfaceid,
						@cmpcode = @cmpcode,
						@username = @userid,
						@acnum = @fake_acnum,
						@year = @year,
						@statement = @statement,
						@openingBal = @opening,
						@closingBal = @closing,
						@updateBal = 0,
						@filename = @filename

					IF @retval = 1 SELECT @ok = 0
					ELSE BEGIN
						INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
							SELECT @interfaceid, CONVERT(DATE, postingdate, 102), @filename, 1, 0, N'A periódus meghatározása sikertelen!', @userid, CURRENT_TIMESTAMP, 1
							FROM cif_ebank_akcenta_075 L WITH (NOLOCK)
							LEFT OUTER JOIN oas_perlist P WITH (NOLOCK) ON
								P.cmpcode = @cmpcode
								AND P.enddate >= postingdate
								AND P.period BETWEEN 1 AND 9997
							WHERE 
								L.interfaceid = @interfaceid 
								AND L.username = @userid 
								AND accountnumber = @acnum
								AND P.period IS NULL
						IF @@ROWCOUNT > 0 SELECT @ok = 0
						ELSE BEGIN
							INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
								SELECT @interfaceid, L.currency, @filename, 1, 0, N'Érvénytelen pénznem!', @userid, CURRENT_TIMESTAMP, 1
								FROM cif_ebank_akcenta_075 L WITH (NOLOCK)
								LEFT OUTER JOIN oas_currency C WITH (NOLOCK) ON
									C.cmpcode = @cmpcode
									AND C.code = L.currency
									AND C.deldate IS NULL
								WHERE 
									L.interfaceid = @interfaceid 
									AND L.username = @userid 
									AND accountnumber = @acnum
									AND C.code IS NULL
							IF @@ROWCOUNT > 0 SELECT @ok = 0
						END
					END
				END

				FETCH NEXT FROM @cursor INTO @acnum
			END
			CLOSE @cursor

			IF @ok = 1 BEGIN
				OPEN @cursor
				FETCH NEXT FROM @cursor INTO @acnum
				WHILE @@FETCH_STATUS = 0 BEGIN

					SELECT @this = MIN(l75.lineseqno), @next = ISNULL(CONVERT(INT, MIN(NXT.lineseqno)), 99999999) 
					FROM cif_ebank_akcenta_075 l75 WITH (NOLOCK) 
					LEFT OUTER JOIN cif_ebank_akcenta_075 NXT WITH (NOLOCK) ON NXT.interfaceid=@interfaceid AND NXT.username=@userid AND NXT.lineseqno > l75.lineseqno
					WHERE l75.interfaceid=@interfaceid AND l75.username=@userid AND l75.accountnumber=@acnum

					INSERT INTO cif_ebank_trans (
						[interfaceid],
						[cmpcode],
						[fileid],
						[doccode],
						[docnum],
						[partnercode],
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
						[comment],
						[partneracnum],
						[postable],
						[matched],
						[createuser],
						[createdate],
						[extref1],
						[openbalacc],
						[closebalacc]
					)
					SELECT 
						@interfaceid,
						@cmpcode,
						LEFT(l75.filname, 50),
						N' ', -- doccode
						N' ', -- docnum
						NULL, -- partnercode
						111, -- status
						@acnum,
						@accur,
						@year,
						@statement,
						CASE l75.accountingtype
							WHEN N'1' THEN 'debit (outgoing)'
							WHEN N'2' THEN 'credit (incoming)'
							WHEN N'4' THEN 'debit reversal'
							WHEN N'5' THEN 'credit reversal'
							ELSE ''
						END, -- trtype
						isnull(l76.transaction_bank_id, l75.transactionid), -- trid
						CASE l75.accountingtype
							WHEN N'1' then 160
							WHEN N'2' then 161
							WHEN N'4' then 160
							WHEN N'5' then 161
							ELSE 161
						END, -- debcred
						(
							SELECT MIN(10000*yr+period) / 10000
							FROM oas_perlist WITH (NOLOCK)
							WHERE 
								cmpcode = @cmpcode
								AND enddate >= l75.postingdate
								AND period BETWEEN 1 AND 9997
						), -- yr
						(
							SELECT MIN(10000*yr+period) % 10000
							FROM oas_perlist WITH (NOLOCK)
							WHERE 
								cmpcode = @cmpcode
								AND enddate >= l75.postingdate
								AND period BETWEEN 1 AND 9997
						), -- period
						l75.postingdate, -- doc
						l75.valuedate, -- due
						l75.valuedate, -- val
						l75.currency, -- curcode
						ROUND(l75.transactionamount / 100 *
						CASE l75.accountingtype
							WHEN N'1' then -1
							WHEN N'2' then 1
							WHEN N'4' then -1
							WHEN N'5' then 1
							ELSE 1
						END, ISNULL(CD.decnum,0)), -- valuedoc
						CASE l75.accountingtype
							WHEN N'1' then -1
							WHEN N'2' then 1
							WHEN N'4' then -1
							WHEN N'5' then 1
							ELSE 1
						END, -- amountsign
						--isnull(l78.transaction_info_p3,''), --rate,
						isnull(l78.transaction_info_p2, currency), -- origcur
						ROUND(case when LTRIM(RTRIM(ISNULL(l78.transaction_info_p1, N''))) = N''
							then convert(numeric(24,8), l75.transactionamount) / 100 *
								CASE l75.accountingtype
									WHEN N'1' then -1
									WHEN N'2' then 1
									WHEN N'4' then -1
									WHEN N'5' then 1
									ELSE 1
								END
							else CONVERT(numeric(24,8), REPLACE(l78.transaction_info_p1,',','.')) *
								CASE l75.accountingtype
									WHEN N'1' then -1
									WHEN N'2' then 1
									WHEN N'4' then -1
									WHEN N'5' then 1
									ELSE 1
								END
						end, ISNULL(CD.decnum,0)), -- origvalue
						CASE WHEN LTRIM(RTRIM(ISNULL(l76.countpnamecomment, N''))) = N''
							THEN LTRIM(RTRIM(ISNULL(l75.description, N'')))
							ELSE LTRIM(RTRIM(ISNULL(l76.countpnamecomment, N''))) + ' ' + LTRIM(RTRIM(ISNULL(l78.transaction_info1, N''))) +
								+ ' ' + LTRIM(RTRIM(ISNULL(l79.transaction_info4, N'')))
						END, -- comment
						--N'', -- partneraccnum
						CASE WHEN LTRIM(RTRIM(l75.countpaccountnumber)) = '0'
							THEN N''
							ELSE right('0000000000000000'+ l75.countpaccountnumber, 10)
						END, -- partneraccnum
						1, -- postable,
						0, -- matched,
						@userid,
						CURRENT_TIMESTAMP,
						l75.variablecode,
						@opening, -- openbalacc
						@closing -- closebalacc
					FROM cif_ebank_akcenta_075 l75 (nolock)
						left outer join cif_ebank_akcenta_076 l76 (nolock) on l76.interfaceid=l75.interfaceid AND l76.username=l75.username AND l76.lineseqno=l75.lineseqno+1
						left outer join cif_ebank_akcenta_078 l78 (nolock) on l78.interfaceid=l76.interfaceid AND l78.username=l76.username AND l78.lineseqno BETWEEN l76.lineseqno+1 AND l76.lineseqno+2
						left outer join cif_ebank_akcenta_079 l79 (nolock) on l79.interfaceid=l78.interfaceid AND l79.username=l78.username AND l79.lineseqno=l78.lineseqno+1
						JOIN oas_currency CD WITH (NOLOCK) ON
							CD.cmpcode = @cmpcode
							AND CD.code = @accur
							AND CD.deldate IS NULL
					WHERE 
						l75.interfaceid = @interfaceid 
						AND l75.username = @userid 
						AND l75.accountnumber = @acnum
						--AND l75.lineseqno BETWEEN @this AND @next
					ORDER BY l75.lineseqno

					FETCH NEXT FROM @cursor INTO @acnum
				END
				CLOSE @cursor
				DEALLOCATE @cursor

			SELECT 0
			RETURN

		END
	END

	SELECT 1
END
GO