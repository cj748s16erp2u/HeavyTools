if (select object_id('[dbo].[cif_ebank_khb_card_load]')) is not null
  drop procedure [dbo].[cif_ebank_khb_card_load]
go

create procedure [dbo].[cif_ebank_khb_card_load]
	@interfaceid varchar(50),
	@cmpcode varchar(12),
	@userid varchar(256)
AS
BEGIN
	UPDATE cif_errors 
	SET enduser=@userid, enddate=CURRENT_TIMESTAMP
	WHERE interfaceid=@interfaceid AND usr=@userid

	--DECLARE @stdateflag BIT
	--SELECT @stdateflag=stdate FROM cif_ebank_params WHERE interfaceid=@interfaceid AND cmpcode=@cmpcode AND enddate IS NULL

	DECLARE @filename NVARCHAR(50)
	SELECT TOP 1 @filename=filname FROM cif_ebank_khb_card_all WITH (NOLOCK) WHERE interfaceid=@interfaceid AND username=@userid 

	-- feldolgozas elso resze, sorok szetdobasa fej/tetel tablakba
	DELETE FROM cif_ebank_khb_card_header
	WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

	DELETE FROM cif_ebank_khb_card_lines
	WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

	INSERT INTO cif_ebank_khb_card_header (
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		generation_date,
		generation_time,
		number_of_file,
		number_of_records,
		transfer_date)
	SELECT
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		CONVERT(datetime, SUBSTRING(line,1,8)),
		SUBSTRING(line,9,4),
		CONVERT(int, SUBSTRING(line,13,8)),
		CONVERT(int, SUBSTRING(line,21,8)),
		CONVERT(datetime, SUBSTRING(line,29,8))
	FROM cif_ebank_khb_card_all (nolock)
	WHERE lineseqno = 1 and interfaceid=@interfaceid AND username=@userid and filname = @filename

	INSERT INTO cif_ebank_khb_card_lines (
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		cardno_trno,
		transaction_date,
		transaction_amount,
		msc_amount,
		dcc_reimb_amount,
		net_amount,
		mif_amount,
		blank,
		msc_mif_amount,
		terminal_id,
		merchant_id,
		transaction_number,
		authorization_number,
		currency_iso,
		currency,
		status,
		batch_number,
		transfer_number,
		brand,
		db_card_type,
		comm_card_type,
		region_of_issuance,
		merch_comm1,
		merch_comm2,
		trancated_pan,
		merch_reserv,
		transfer_id)
	SELECT
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		SUBSTRING(line,1,19),
		CONVERT(datetime, SUBSTRING(line,20,4) + '-' + SUBSTRING(line,24,2) + '-' + SUBSTRING(line,26,2) + ' ' + SUBSTRING(line,28,2) + ':' + SUBSTRING(line,30,2)),
		CASE WHEN ISNULL(SUBSTRING(line,32,10),'') <> '' THEN CONVERT(DECIMAL(24,8), SUBSTRING(line,32,10)) END,
		CASE WHEN ISNULL(SUBSTRING(line,42,10),'') <> '' THEN CONVERT(DECIMAL(24,8), SUBSTRING(line,42,10)) END,
		CASE WHEN ISNULL(SUBSTRING(line,52,10),'') <> '' THEN CONVERT(DECIMAL(24,8), SUBSTRING(line,52,10)) END,
		CASE WHEN ISNULL(SUBSTRING(line,62,10),'') <> '' THEN CONVERT(DECIMAL(24,8), SUBSTRING(line,62,10)) END,
		CASE WHEN ISNULL(SUBSTRING(line,72,10),'') <> '' THEN CONVERT(DECIMAL(24,8), SUBSTRING(line,72,10)) END,
		SUBSTRING(line,82,10),
		CASE WHEN ISNULL(SUBSTRING(line,92,10),'') <> '' AND LOWER(SUBSTRING(line,92,10)) <> 'n.k' THEN CONVERT(DECIMAL(24,8), SUBSTRING(line,92,10)) END,
		SUBSTRING(line,102,10),
		SUBSTRING(line,112,10),
		SUBSTRING(line,122,12),
		SUBSTRING(line,134,9),
		SUBSTRING(line,143,3),
		CASE 
			WHEN SUBSTRING(line,143,3) = '348' THEN 'HUF'
			WHEN SUBSTRING(line,143,3) = '840' THEN 'USD'
			WHEN SUBSTRING(line,143,3) = '978' THEN 'EUR'
		END,
		SUBSTRING(line,146,2),
		SUBSTRING(line,148,12),
		SUBSTRING(line,160,10),
		SUBSTRING(line,170,1),
		SUBSTRING(line,171,1),
		SUBSTRING(line,172,1),
		SUBSTRING(line,173,1),
		SUBSTRING(line,174,20),
		SUBSTRING(line,194,20),
		SUBSTRING(line,214,20),
		SUBSTRING(line,234,20),
		SUBSTRING(line,254,20)
	FROM cif_ebank_khb_card_all (nolock)
	WHERE lineseqno > 1 and interfaceid=@interfaceid AND username=@userid and filname = @filename
	
	IF NOT EXISTS (
		SELECT 1
		FROM cif_ebank_khb_card_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
	) OR NOT EXISTS (
		SELECT 1
		FROM cif_ebank_khb_card_lines WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @interfaceid, @filename, 1, 0, N'Nincs feldolgozható adat!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE IF EXISTS (
		SELECT 1
		FROM 
		(
			SELECT COUNT(DISTINCT(currency)) AS currency
			FROM cif_ebank_khb_card_lines WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
		) tmp
		WHERE currency > 1
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @interfaceid, @filename, 1, 0, N'Több pénznem egy fájlban!', @userid, CURRENT_TIMESTAMP, 1)
	--ELSE IF EXISTS (
	--	SELECT 1
	--	FROM 
	--	(
	--		SELECT CONVERT(date, MIN(transaction_date), 102) AS min_date, CONVERT(date, MAX(transaction_date), 102) AS max_date
	--		FROM cif_ebank_khb_card_lines WITH (NOLOCK)
	--		WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
	--	) tmp
	--	WHERE min_date <> max_date
	--) 
	--	INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
	--	VALUES(@interfaceid, @filename, @filename, 1, 0, N'Napon átnyúló kivonat van a fájlban!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE BEGIN
		DECLARE 
			@ok BIT,
			@acnum NVARCHAR(36),
			@accur NVARCHAR(3),
			@year SMALLINT,
			@statement SMALLINT,
			@enddate DATETIME,
			@opening DECIMAL(24,8),
			@closing DECIMAL(24,8)

		SELECT @ok = 1

		SELECT @accur = currency FROM 
			(SELECT DISTINCT(currency) AS currency
			FROM cif_ebank_khb_card_lines WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename) x

		UPDATE cif_ebank_khb_card_header
		SET acnum = CASE @accur
			WHEN 'HUF' THEN '104010555052665572801002'
			WHEN 'EUR' THEN '104010555052665572801019'
			ELSE NULL  
		END

		UPDATE cif_ebank_khb_card_lines
		SET ownacnum = CASE @accur
			WHEN 'HUF' THEN '104010555052665572801002'
			WHEN 'EUR' THEN '104010555052665572801019'
			ELSE NULL  
		END

		SET @acnum = CASE @accur WHEN 'HUF' THEN '104010555052665572801002' ELSE '104010555052665572801019' END

		SELECT @year=year(transfer_date), @statement=number_of_file, @enddate=transfer_date, @opening=0, @closing=0
		FROM cif_ebank_khb_card_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid AND acnum=@acnum

		DECLARE	@retval INT
		EXEC @retval = cif_ebank_check
			@interfaceid = @interfaceid,
			@cmpcode = @cmpcode,
			@username = @userid,
			@acnum = @acnum,
			@year = @year,
			@statement = @statement,
			@openingBal = @opening,
			@closingBal = @closing,
			@updateBal = 0,
			@filename = @filename

		IF @retval = 1 SELECT @ok = 0
		ELSE BEGIN
			INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
				SELECT @interfaceid, CONVERT(DATE, transaction_date, 102), @filename, 1, 0, N'A periódus meghatározása sikertelen!', @userid, CURRENT_TIMESTAMP, 1
				FROM cif_ebank_khb_card_lines L WITH (NOLOCK)
				LEFT OUTER JOIN oas_perlist P WITH (NOLOCK) ON
					P.cmpcode = @cmpcode
					AND P.enddate >= transaction_date
					AND P.period BETWEEN 1 AND 9997
				WHERE 
					L.interfaceid = @interfaceid 
					AND L.username = @userid 
					AND ownacnum = @acnum
					AND P.period IS NULL
			IF @@ROWCOUNT > 0 SELECT @ok = 0
			ELSE BEGIN
				INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
					SELECT @interfaceid, L.currency, @filename, 1, 0, N'Érvénytelen pénznem!', @userid, CURRENT_TIMESTAMP, 1
					FROM cif_ebank_khb_card_lines L WITH (NOLOCK)
					LEFT OUTER JOIN oas_currency C WITH (NOLOCK) ON
						C.cmpcode = @cmpcode
						AND C.code = L.currency
						AND C.deldate IS NULL
					WHERE 
						L.interfaceid = @interfaceid 
						AND L.username = @userid 
						AND ownacnum = @acnum
						AND C.code IS NULL
				IF @@ROWCOUNT > 0 SELECT @ok = 0
			END
		END

		IF @ok = 1 BEGIN

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
				[partneracnum],
				[postable],
				[matched],
				[createuser],
				[createdate],
				[extref1],
				[extref2],
				[extref3],
				[extref4],
				[extref5],
				[extref6],
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
				@accur, -- curaccount
				@year, -- year
				@statement,  -- statement
				status, -- trtypeid
				cardno_trno, -- trid
				CASE WHEN transaction_amount > 0 THEN 161 ELSE 160 END, -- debcred
				(
					SELECT MIN(10000*yr+period) / 10000
					FROM oas_perlist WITH (NOLOCK)
					WHERE 
						cmpcode = @cmpcode
						AND enddate >= transaction_date
						AND period BETWEEN 1 AND 9997
				), -- yr
				(
					SELECT MIN(10000*yr+period) % 10000
					FROM oas_perlist WITH (NOLOCK)
					WHERE 
						cmpcode = @cmpcode
						AND enddate >= transaction_date
						AND period BETWEEN 1 AND 9997
				), -- period
				transaction_date, -- doc
				transaction_date, -- due
				transaction_date, -- val
				currency, -- curcode
				--transaction_amount / 100, -- valuedoc fix 2 tizedes miatt
				ROUND(transaction_amount / 100, ISNULL(CD.decnum,0)), -- valuedoc, fix 2 tizedes miatt
				transaction_amount / ABS(transaction_amount), -- amountsign
				currency, -- origcur
				--transaction_amount / 100, -- origvalue
				ROUND(transaction_amount / 100, ISNULL(CD.decnum,0)), -- origvalue, fix 2 tizedes miatt
				cardno_trno, -- partneraccnum
				1, -- postable,
				0, -- matched,
				@userid,
				CURRENT_TIMESTAMP,
				authorization_number,	-- extref1
				transfer_id, -- extref2
				transfer_number, -- extref3
				transaction_number,	-- extref4
				merchant_id, -- extref5
				terminal_id, -- extref6
				--msc_amount / 100 -- valueacc
				ROUND(msc_amount / 100, ISNULL(CD.decnum,0)) -- valueacc, jutalek
			FROM cif_ebank_khb_card_lines L WITH (NOLOCK)
				JOIN oas_currency CD WITH (NOLOCK) ON
					CD.cmpcode = @cmpcode
					AND CD.code = @accur
					AND CD.deldate IS NULL
			WHERE 
				interfaceid = @interfaceid 
				AND username = @userid 
				AND ownacnum = @acnum
			ORDER BY lineseqno

			DELETE FROM cif_ebank_khb_card_header
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

			DELETE FROM cif_ebank_khb_card_lines
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

			SELECT 0
			RETURN
		END

	END

	SELECT 1
END