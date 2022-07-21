if (select object_id('[dbo].[cif_ebank_otp_card_load]')) is not null
  drop procedure [dbo].[cif_ebank_otp_card_load]
go

create procedure [dbo].[cif_ebank_otp_card_load]
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
	SELECT TOP 1 @filename=filname FROM cif_ebank_otp_card_all WITH (NOLOCK) WHERE interfaceid=@interfaceid AND username=@userid 

	-- feldolgozas elso resze, sorok szetdobasa fej/tetel tablakba
	DELETE FROM cif_ebank_otp_card_header
	WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

	DELETE FROM cif_ebank_otp_card_lines
	WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

	DELETE FROM cif_ebank_otp_card_footer
	WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

	declare
		@h_1 varchar(2),
		@h_2 varchar(9),
		@h_3 varchar(30),
		@h_4 varchar(10),
		@h_5 varchar(10),
		@h_6 varchar(30),
		@h_7 varchar(3),
		@f_max int

	select
		@h_1=(SELECT strval FROM cif_sys_split(line, ';') where id=1),
		@h_2=(SELECT strval FROM cif_sys_split(line, ';') where id=2),
		@h_3=(SELECT strval FROM cif_sys_split(line, ';') where id=3),
		@h_4=(SELECT strval FROM cif_sys_split(line, ';') where id=4),
		@h_5=(SELECT strval FROM cif_sys_split(line, ';') where id=5),
		@h_6=(SELECT strval FROM cif_sys_split(line, ';') where id=6),
		@h_7=(SELECT strval FROM cif_sys_split(line, ';') where id=7)
	from cif_ebank_otp_card_all (nolock)
	WHERE lineseqno = 1 and interfaceid=@interfaceid AND username=@userid and filname = @filename

	INSERT INTO cif_ebank_otp_card_header (
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		constans,
		trader_code,
		trader_name,
		registration_number,
		creation_date,
		account_number,
		currency)
	SELECT
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		@h_1,
		@h_2,
		@h_3,
		@h_4,
		convert(datetime, @h_5, 102),
		@h_6,
		@h_7
	from cif_ebank_otp_card_all (nolock)
	WHERE lineseqno = 1 and interfaceid=@interfaceid AND username=@userid and filname = @filename

	SELECT @f_max = MAX(lineseqno) from cif_ebank_otp_card_all (nolock)

	INSERT INTO cif_ebank_otp_card_lines (
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		transaction_id,
		terminal_id,
		authorization_id,
		transaction_date,
		transaction_time,
		card_number,
		transaction_amount,
		intercharge_amount,
		systemcharge_amount,
		merchant_fee_amount,
		commission_amount,
		net_amount,
		transaction_flag,
		store_name,
		external_unique_id,
		binf,
		ownacnum)
	SELECT
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		(SELECT strval FROM cif_sys_split(line, ';') where id=1),
		(SELECT strval FROM cif_sys_split(line, ';') where id=2),
		(SELECT strval FROM cif_sys_split(line, ';') where id=3),
		convert(datetime, (SELECT strval FROM cif_sys_split(line, ';') where id=4), 102),
		(SELECT strval FROM cif_sys_split(line, ';') where id=5),
		(SELECT strval FROM cif_sys_split(line, ';') where id=6),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM((SELECT strval FROM cif_sys_split(line, ';') where id=7))), N'.', N''), N',', N'.')),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM((SELECT strval FROM cif_sys_split(line, ';') where id=8))), N'.', N''), N',', N'.')),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM((SELECT strval FROM cif_sys_split(line, ';') where id=9))), N'.', N''), N',', N'.')),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM((SELECT strval FROM cif_sys_split(line, ';') where id=10))), N'.', N''), N',', N'.')),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM((SELECT strval FROM cif_sys_split(line, ';') where id=11))), N'.', N''), N',', N'.')),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM((SELECT strval FROM cif_sys_split(line, ';') where id=12))), N'.', N''), N',', N'.')),
		(SELECT strval FROM cif_sys_split(line, ';') where id=13),
		(SELECT strval FROM cif_sys_split(line, ';') where id=14),
		(SELECT strval FROM cif_sys_split(line, ';') where id=15),
		(SELECT strval FROM cif_sys_split(line, ';') where id=16),
		REPLACE(@h_6, '-', '')
	from cif_ebank_otp_card_all (nolock)
	WHERE lineseqno BETWEEN 2 AND @f_max - 1 and interfaceid=@interfaceid AND username=@userid and filname = @filename

	DECLARE
		@f_1 varchar(2),
		@f_2 varchar(9),
		@f_3 varchar(10),
		@f_4 varchar(8),
		@f_5 varchar(15)

	select
		@f_1=(SELECT strval FROM cif_sys_split(line, ';') where id=1),
		@f_2=(SELECT strval FROM cif_sys_split(line, ';') where id=2),
		@f_3=(SELECT strval FROM cif_sys_split(line, ';') where id=3),
		@f_4=(SELECT strval FROM cif_sys_split(line, ';') where id=4),
		@f_5=(SELECT strval FROM cif_sys_split(line, ';') where id=5)
	from cif_ebank_otp_card_all (nolock)
	WHERE lineseqno = @f_max and interfaceid=@interfaceid AND username=@userid and filname = @filename

	INSERT INTO cif_ebank_otp_card_footer (
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		constans,
		trader_code,
		creation_date,
		processed_records,
		net_value)
	SELECT
		interfaceid,
		filname,
		username,
		inputdate,
		lineseqno,
		@f_1,
		@f_2,
		convert(datetime, @f_3, 102),
		convert(int, @f_4),
		CONVERT(DECIMAL(24,8), REPLACE(REPLACE(LTRIM(RTRIM(@f_5)), N'.', N''), N',', N'.'))
	from cif_ebank_otp_card_all (nolock)
	WHERE lineseqno = @f_max and interfaceid=@interfaceid AND username=@userid and filname = @filename
	
	IF NOT EXISTS (
		SELECT 1
		FROM cif_ebank_otp_card_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
	) OR NOT EXISTS (
		SELECT 1
		FROM cif_ebank_otp_card_lines WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @interfaceid, @filename, 1, 0, N'Nincs feldolgozható adat!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE IF EXISTS (
		SELECT 1
		FROM 
		(
			SELECT CONVERT(date, MIN(transaction_date), 102) AS min_date, CONVERT(date, MAX(transaction_date), 102) AS max_date
			FROM cif_ebank_khb_card_lines WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid and filname = @filename
		) tmp
		WHERE min_date <> max_date
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @filename, @filename, 1, 0, N'Napon átnyúló kivonat van a fájlban!', @userid, CURRENT_TIMESTAMP, 1)
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

		SELECT @year=year(creation_date), @statement=cast(format(convert(datetime, registration_number, 102),'MMdd') as int), 
				@acnum=REPLACE(account_number, '-', ''), @accur=currency, @enddate=creation_date, @opening=0, @closing=0
		FROM cif_ebank_otp_card_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

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
				FROM cif_ebank_otp_card_lines L WITH (NOLOCK)
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
					SELECT @interfaceid, H.currency, @filename, 1, 0, N'Érvénytelen pénznem!', @userid, CURRENT_TIMESTAMP, 1
					FROM cif_ebank_otp_card_header H WITH (NOLOCK)
					LEFT OUTER JOIN oas_currency C WITH (NOLOCK) ON
						C.cmpcode = @cmpcode
						AND C.code = H.currency
						AND C.deldate IS NULL
					WHERE 
						H.interfaceid = @interfaceid 
						AND H.username = @userid 
						AND H.account_number = @acnum
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
				LEFT(L.filname, 50), --fileid
				' ', -- doccode
				' ', -- docnum
				111, -- status
				@acnum, -- ownacnum
				@accur, -- curaccount
				@year, -- year
				@statement,  -- statement
				transaction_flag, -- trtypeid
				card_number, -- trid
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
				transaction_amount, -- valuedoc fix 2 tizedes miatt
				transaction_amount / ABS(transaction_amount), -- amountsign
				currency, -- origcur
				transaction_amount, -- origvalue
				card_number, -- partneraccnum
				1, -- postable,
				0, -- matched,
				@userid,
				CURRENT_TIMESTAMP,
				authorization_id,	-- extref1
				'', -- extref2
				--external_unique_id,	-- extref3
				store_name, -- extref3
				transaction_id,	-- extref4
				--store_name, -- extref5
				external_unique_id, -- extref5
				terminal_id, -- extref6
				commission_amount -- valueacc
			FROM cif_ebank_otp_card_lines L WITH (NOLOCK)
				JOIN cif_ebank_otp_card_header H WITH (NOLOCK) ON H.interfaceid=L.interfaceid AND H.username=L.username AND H.filname = L.filname
			WHERE 
				L.interfaceid = @interfaceid 
				AND L.username = @userid 
				AND L.ownacnum = @acnum
			ORDER BY L.lineseqno

			DELETE FROM cif_ebank_otp_card_header
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

			DELETE FROM cif_ebank_otp_card_lines
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

			DELETE FROM cif_ebank_otp_card_footer
			WHERE interfaceid=@interfaceid AND username=@userid AND filname = @filename

			SELECT 0
			RETURN

		END

	END
	
	SELECT 1
END