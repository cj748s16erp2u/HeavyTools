if (select object_id('[dbo].[cif_ebank_khb_load]')) is not null
  drop procedure [dbo].[cif_ebank_khb_load]
go

create procedure [dbo].[cif_ebank_khb_load]
	@interfaceid varchar(50),
	@cmpcode varchar(12),
	@userid varchar(256)
AS
BEGIN
	UPDATE cif_errors 
	SET enduser=@userid, enddate=CURRENT_TIMESTAMP
	WHERE interfaceid=@interfaceid AND usr=@userid

	DECLARE @stdateflag BIT
	SELECT @stdateflag=stdate FROM cif_ebank_params WHERE interfaceid=@interfaceid AND cmpcode=@cmpcode AND enddate IS NULL

	UPDATE cif_ebank_khb_lines
	SET 
		ownacnum =
			CASE WHEN valueacc < 0 THEN 
				CASE WHEN LTRIM(RTRIM(ISNULL(chargedacnum, N''))) = N'' THEN srcacnum ELSE chargedacnum END 
			ELSE 
				CASE WHEN LTRIM(RTRIM(ISNULL(beneficialacnum, N''))) = N'' THEN destacnum ELSE beneficialacnum END 
			END,
		partneracnum =
			CASE WHEN valueacc > 0 THEN 
				CASE WHEN LTRIM(RTRIM(ISNULL(chargedacnum, N''))) = N'' THEN srcacnum ELSE chargedacnum END 
			ELSE 
				CASE WHEN LTRIM(RTRIM(ISNULL(beneficialacnum, N''))) = N'' THEN destacnum ELSE beneficialacnum END 
			END,
		trandate = CASE WHEN valueacc > 0 THEN beneficialdate ELSE chargeddate END
	WHERE interfaceid=@interfaceid AND username=@userid 

	UPDATE cif_ebank_khb_lines
	SET 
		ownacnum = 
			CASE WHEN LEN(ownacnum) >= 24 THEN 
				RIGHT(ownacnum, 24) 
			ELSE 
				CASE WHEN LEN(ownacnum) >= 16 THEN 
					RIGHT(ownacnum, 16) + '00000000' 
				ELSE 
					ownacnum 
				END
			END,
		partneracnum = 
			CASE WHEN LEN(partneracnum) >= 24 THEN 
				RIGHT(partneracnum, 24) 
			ELSE 
				CASE WHEN LEN(partneracnum) >= 16 THEN 
					RIGHT(partneracnum, 16) + '00000000' 
				ELSE 
					partneracnum 
				END
			END
	WHERE interfaceid=@interfaceid AND username=@userid 
	
	DECLARE @filename NVARCHAR(50)
	SELECT TOP 1 @filename=filname FROM cif_ebank_khb_header WITH (NOLOCK) WHERE interfaceid=@interfaceid AND username=@userid 

	IF NOT EXISTS (
		SELECT 1
		FROM cif_ebank_khb_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid 
	) OR NOT EXISTS (
		SELECT 1
		FROM cif_ebank_khb_lines WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid 
	) 
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @interfaceid, @filename, 1, 0, N'Nincs feldolgozható adat!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE IF EXISTS (
		SELECT 1
		FROM cif_ebank_khb_header WITH (NOLOCK)
		WHERE interfaceid=@interfaceid AND username=@userid AND startdate <> enddate
	)
		INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
		VALUES(@interfaceid, @filename, @filename, 1, 0, N'Napon átnyúló kivonat van a fájlban!', @userid, CURRENT_TIMESTAMP, 1)
	ELSE BEGIN
		DECLARE 
			@ok BIT,
			@cursor CURSOR,
			@acnum NVARCHAR(36)
		SELECT @ok = 1
		SET @cursor = CURSOR FAST_FORWARD FOR
			SELECT DISTINCT(acnum)
			FROM cif_ebank_khb_header WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid
		
		OPEN @cursor
		FETCH NEXT FROM @cursor INTO @acnum
		WHILE @@FETCH_STATUS = 0 /*AND @ok = 1*/ BEGIN
			DECLARE @count INT
			SELECT @count=COUNT(1)
			FROM cif_ebank_khb_header WITH (NOLOCK)
			WHERE interfaceid=@interfaceid AND username=@userid AND acnum=@acnum

			IF @count > 1 BEGIN
				INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
				VALUES(@interfaceid, @filename, @filename, 1, 0, N'Egy fájlban egy bankszámla csak egyszer szerepelhet!', @userid, CURRENT_TIMESTAMP, 1)
				SELECT @ok = 0
			END
			ELSE BEGIN
				DECLARE 
					@accur NVARCHAR(3),
					@year SMALLINT,
					@statement SMALLINT,
					@enddate DATETIME,
					@opening DECIMAL(24,8),
					@closing DECIMAL(24,8)

				SELECT @accur=accur, @year=year(startdate), @statement=statementno, @enddate=enddate, @opening=openbal/100, @closing=closebal/100 -- fix 2 tizedes miatt
				FROM cif_ebank_khb_header WITH (NOLOCK)
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
						SELECT @interfaceid, CONVERT(DATE, trandate, 102), @filename, 1, 0, N'A periódus meghatározása sikertelen!', @userid, CURRENT_TIMESTAMP, 1
						FROM cif_ebank_khb_lines L WITH (NOLOCK)
						LEFT OUTER JOIN oas_perlist P WITH (NOLOCK) ON
							P.cmpcode = @cmpcode
							AND P.enddate >= CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END
							AND P.period BETWEEN 1 AND 9997
						WHERE 
							L.interfaceid = @interfaceid 
							AND L.username = @userid 
							AND ownacnum = @acnum
							AND P.period IS NULL
					IF @@ROWCOUNT > 0 SELECT @ok = 0
					ELSE BEGIN
						INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
							SELECT @interfaceid, L.curacc, @filename, 1, 0, N'Érvénytelen pénznem!', @userid, CURRENT_TIMESTAMP, 1
							FROM cif_ebank_khb_lines L WITH (NOLOCK)
							LEFT OUTER JOIN oas_currency C WITH (NOLOCK) ON
								C.cmpcode = @cmpcode
								AND C.code = L.curacc
								AND C.deldate IS NULL
							WHERE 
								L.interfaceid = @interfaceid 
								AND L.username = @userid 
								AND ownacnum = @acnum
								AND C.code IS NULL
						IF @@ROWCOUNT > 0 SELECT @ok = 0
						ELSE BEGIN
							INSERT INTO cif_errors (interfaceid, linkcode, filename, step, messagecode, extramessage, usr, createtime, critical)
								SELECT @interfaceid, L.curdoc, @filename, 1, 0, N'Érvénytelen pénznem!', @userid, CURRENT_TIMESTAMP, 1
								FROM cif_ebank_khb_lines L WITH (NOLOCK)
								LEFT OUTER JOIN oas_currency C WITH (NOLOCK) ON
									C.cmpcode = @cmpcode
									AND C.code = L.curdoc
									AND C.deldate IS NULL
								WHERE 
									L.interfaceid = @interfaceid 
									AND L.username = @userid 
									AND ownacnum = @acnum
									AND ISNULL(L.curdoc,'') <> ''
									AND C.code IS NULL
							IF @@ROWCOUNT > 0 SELECT @ok = 0
						END
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
				SELECT @accur=accur, @year=year(startdate), @statement=statementno, @enddate=enddate, @opening=openbal/100, @closing=closebal/100 -- fix 2 tizedes miatt
				FROM cif_ebank_khb_header WITH (NOLOCK)
				WHERE interfaceid=@interfaceid AND username=@userid AND acnum=@acnum

				-- az egyenlegeket itt kell frissíteni, mert a check-ben nem lehet a többes feldolgozás miatt
				-- BEGIN BALANCE
					-- átlagárfolyam esetén eleve szigorú mód kell
					-- a '+' az 'OR' helyett van, mert szám mezők, nem logikai értékek
					DECLARE 
						@strict INT,
						@balance DECIMAL(24,8)
					SELECT @strict=CONVERT(INT, strictmode)+CONVERT(INT, avgrate)
					FROM cif_ebank_acnum_list WITH (NOLOCK) 
					WHERE interfaceid=@interfaceid AND cmpcode=@cmpcode AND sort+acnum=@acnum AND enddate IS NULL	         

					IF @strict <> 0 AND (@opening <> 0 OR @closing <> 0) BEGIN
						SELECT @balance=value FROM cif_ebank_balance WITH (NOLOCK) WHERE interfaceid=@interfaceid AND cmpcode=@cmpcode AND acnum=@acnum
						IF @balance IS NULL 
							INSERT INTO cif_ebank_balance (interfaceid, cmpcode, acnum, value) VALUES (@interfaceid, @cmpcode, @acnum, @closing)
						ELSE
							UPDATE cif_ebank_balance SET value=@closing WHERE interfaceid=@interfaceid AND cmpcode=@cmpcode AND acnum=@acnum
					END
					-- ha nincs egyenleg vizsgálat, akkor törlöm a rekordot, hogy ha ez kikapcsolás miatt van, akkor bekapcsoláskor ne fusson hibára
					ELSE DELETE FROM cif_ebank_balance WHERE interfaceid=@interfaceid AND cmpcode=@cmpcode AND acnum=@acnum
				-- END BALANCE

				declare @lnacclevel integer, @lcbankacccode varchar(79), @lnyr integer, @lnperiod integer

				/* Fokonyv elemszint */
				select @lnacclevel = 1

				/* Bank szamlakod, ebbol csak a fokonyv elemszintnek megfelelo elemkod kell */
				select @lcbankacccode=ob.defacc from oas_bank ob (nolock)
				left outer join efx_bankcmp bc (nolock) ON
					bc.cmpcode = ob.cmpcode and bc.bankcodacode = ob.code
				left outer join efx_bank eb (nolock) ON eb.bankid = bc.bankid
				where ob.cmpcode = @cmpcode and eb.interfaceid = @interfaceid

				/* Ev, idoszak */
				--select @lnyr=year(todate),@lnperiod=month(todate) from efx_banktranhead (nolock)
				--where banktranid = 73 /* Beolvasott kivonat szerint */

				-- elx mezőket és néhány extrefx mezőt itt még nem töltöm, NULL-ok lesznek
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
					--[rate],
					[origcur],
					[origvalue],
					[docdescr],
					[linedescr],
					[trtypecomment],
					[partnernamefrombank],
					[comment],
					[partneracnum],
					[postable],
					[destination],
					[matchdoccode],
					[matchdocnum],
					[matchdoclinenum],
					[matched],
					[createuser],
					[createdate],
					[errorcode],
					[extref1],
					[extref2],
					[extref3],
					[extref4],
					[extref5],
					[extref6],
					[openbalacc],
					[closebalacc],
					[extvalueacc]
				)
				SELECT 
					@interfaceid,
					@cmpcode,
					LEFT(filname, 50),
					' ', -- doccode
					' ', -- docnum
					--NULL, -- partnercode
					(
						SELECT TOP 1 e.elmcode
						FROM oas_elmbanklist e WITH (nolock)
						LEFT OUTER JOIN oas_element el WITH (NOLOCK) ON
							el.cmpcode = e.cmpcode
							AND el.elmlevel = e.elmlevel
							AND el.code = e.elmcode
							AND el.deldate is null
						WHERE UPPER(LTRIM(RTRIM(e.acname))) = 
						CASE SIGN(l.valueacc)
						  WHEN 1 THEN UPPER(LTRIM(RTRIM(l.srcpart)))
						  WHEN -1 THEN UPPER(LTRIM(RTRIM(l.destpart)))
						END
						AND e.cmpcode = @cmpcode
						AND e.elmlevel = 3
						/*
						-- elmlevel kiszedese, de ez erp tabla :(
						select valueint from ofc_companyline
						where cmpid = 1 and rectype = 2
						*/
					), -- partnercode
					--46, -- status
					111, -- status
					@acnum,
					@accur,
					@year,
					@statement,
					--trtype,
					CASE WHEN trtype = '802'
						THEN
							CASE WHEN valueacc > 0 
								THEN
									CASE WHEN UPPER(LTRIM(RTRIM(srcpart))) like '%BANKKÁRTYA%'
										THEN
											trtype + 'BK'
										ELSE
											trtype
									END
								ELSE
									CASE WHEN UPPER(LTRIM(RTRIM(destpart))) like '%BANKKÁRTYA%'
										THEN
											trtype + 'BK'
										ELSE
											trtype
									END
							END
						ELSE
							trtype
					END,  -- trtype (fugg a partnernamefrombank-tol)
					trid,
					CASE WHEN valueacc > 0 THEN 161 ELSE 160 END, -- debcred
					(
						SELECT MIN(10000*yr+period) / 10000
						FROM oas_perlist WITH (NOLOCK)
						WHERE 
							cmpcode = @cmpcode
							AND enddate >= CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END
							AND period BETWEEN 1 AND 9997
					), -- yr
					(
						SELECT MIN(10000*yr+period) % 10000
						FROM oas_perlist WITH (NOLOCK)
						WHERE 
							cmpcode = @cmpcode
							AND enddate >= CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END
							AND period BETWEEN 1 AND 9997
					), -- period
					CASE @stdateflag WHEN 0 THEN @enddate ELSE trandate END, -- doc
					CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END, -- due
					CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END, -- val
					curacc,
					--valueacc / 100, -- valuedoc, fix 2 tizedes miatt
					ROUND(valueacc / 100, ISNULL(CD.decnum,0)), -- valuedoc, fix 2 tizedes miatt
					valueacc / ABS(valueacc), -- amountsign
					--rate,
					CASE WHEN LTRIM(RTRIM(ISNULL(curdoc, ''))) = '' OR ISNULL(valuedoc, 0) = 0 THEN curacc ELSE curdoc END,
					--(CASE WHEN LTRIM(RTRIM(ISNULL(curdoc, ''))) = '' OR ISNULL(valuedoc, 0) = 0 OR curdoc=curacc /* jutalék belekavarhat, ezt védjük ki */ THEN valueacc ELSE valuedoc * valueacc / ABS(valueacc) END) / 100, -- osztás fix 2 tizedes miatt, ABS az előjel miatt, mert valuedoc-nak nincs
					ROUND((CASE WHEN LTRIM(RTRIM(ISNULL(curdoc, ''))) = '' OR ISNULL(valuedoc, 0) = 0 OR curdoc=curacc /* jutalék belekavarhat, ezt védjük ki */ THEN valueacc ELSE valuedoc * valueacc / ABS(valueacc) END) / 100, ISNULL(CD.decnum,0)), -- origvlaue, osztás fix 2 tizedes miatt, ABS az előjel miatt, mert valuedoc-nak nincs
					NULL, -- docdescr, majd fordítva kell tölteni, ha lesz benne adat
					--NULL, -- linedescr, majd fordítva kell tölteni, ha lesz benne adat
					CASE WHEN valueacc > 0 THEN srcpart ELSE destpart END, -- linedescr
					NULL, -- trtypecomment
					CASE WHEN valueacc > 0 THEN srcpart ELSE destpart END, -- partnernamefrombank
					comment,
					CASE WHEN LTRIM(RTRIM(ISNULL(partneracnum,N'')))=N'' THEN destacnum ELSE partneracnum END,
					1, -- postable,
					NULL, -- destination,
					NULL, -- matchdoccode,
					NULL, -- matchdocnum,
					NULL, -- matchdoclinenum,
					0, -- matched,
					@userid,
					CURRENT_TIMESTAMP,
					NULL, -- errorcode,
					'',	-- extref1
					CONVERT(NVARCHAR, @year) + '/' + RIGHT('000' + CONVERT(NVARCHAR, @statement), 3), -- extref2
					CONVERT(DATE, @enddate, 112), -- extref3
					'',	-- extref4
					LEFT(comment, 32), -- extref5
					id, -- extref6

					--CONVERT(NVARCHAR, @year) + '/' + RIGHT('000' + CONVERT(NVARCHAR, @statement), 3), -- extref1
					--LEFT(comment, 32), -- extref2
					--LEFT(CASE WHEN valueacc > 0 THEN srcpart ELSE destpart END, 32), -- extref3
					--CONVERT(DATE, @enddate, 112), -- extref4
					--'',	-- extref5
					--''	-- extref6
					@opening, -- openbalacc
					@closing, -- closebalacc
					ISNULL(
					(
						select sum(full_value) from oas_balance as b (nolock) join oas_company as c (nolock) on c.code = b.cmpcode
						where cmpcode = @cmpcode and el1 = @lcbankacccode and balcode = c.actual and b.curcode = c.homecur 
						and repbasis = @lnacclevel 
						and yr = CASE @stdateflag WHEN 0 THEN year(trandate) ELSE year(@enddate) END 
						and period <= CASE @stdateflag WHEN 0 THEN month(trandate) ELSE month(@enddate) END 
					), 0 ) -- extvalueacc
				FROM cif_ebank_khb_lines L WITH (NOLOCK)
				JOIN oas_currency CD WITH (NOLOCK) ON
					CD.cmpcode = @cmpcode
					AND CD.code = @accur
					AND CD.deldate IS NULL
				WHERE 
					interfaceid = @interfaceid 
					AND username = @userid 
					AND ownacnum = @acnum
					AND trtype <> 823	-- csekkes sorok nem tetelesen kellenek
				ORDER BY lineseqno
				
				-- Csekkes sorok osszevontan
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
					--[rate],
					[origcur],
					[origvalue],
					[docdescr],
					[linedescr],
					[trtypecomment],
					[partnernamefrombank],
					[comment],
					[partneracnum],
					[postable],
					[destination],
					[matchdoccode],
					[matchdocnum],
					[matchdoclinenum],
					[matched],
					[createuser],
					[createdate],
					[errorcode],
					[extref1],
					[extref2],
					[extref3],
					[extref4],
					[extref5],
					[extref6],
					[openbalacc],
					[closebalacc],
					[extvalueacc]
				)
				SELECT 
					@interfaceid,
					@cmpcode,
					LEFT(filname, 50),
					' ', -- doccode
					' ', -- docnum
					NULL, -- partnercode
					--46, -- status
					111, -- status
					@acnum,
					@accur,
					@year,
					@statement,
					trtype,
					trid,
					CASE WHEN valueacc > 0 THEN 161 ELSE 160 END, -- debcred
					(
						SELECT MIN(10000*yr+period) / 10000
						FROM oas_perlist WITH (NOLOCK)
						WHERE 
							cmpcode = @cmpcode
							AND enddate >= CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END
							AND period BETWEEN 1 AND 9997
					), -- yr
					(
						SELECT MIN(10000*yr+period) % 10000
						FROM oas_perlist WITH (NOLOCK)
						WHERE 
							cmpcode = @cmpcode
							AND enddate >= CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END
							AND period BETWEEN 1 AND 9997
					), -- period
					CASE @stdateflag WHEN 0 THEN @enddate ELSE trandate END, -- doc
					CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END, -- due
					CASE @stdateflag WHEN 0 THEN trandate ELSE @enddate END, -- val
					curacc,
					valueacc / 100, -- fix 2 tizedes miatt
					valueacc / ABS(valueacc), -- amountsign
					--rate,
					CASE WHEN LTRIM(RTRIM(ISNULL(curdoc, ''))) = '' OR ISNULL(valuedoc, 0) = 0 THEN curacc ELSE curdoc END,
					(CASE WHEN LTRIM(RTRIM(ISNULL(curdoc, ''))) = '' OR ISNULL(valuedoc, 0) = 0 OR curdoc=curacc /* jutalék belekavarhat, ezt védjük ki */ THEN valueacc ELSE valuedoc * valueacc / ABS(valueacc) END) / 100, -- osztás fix 2 tizedes miatt, ABS az előjel miatt, mert valuedoc-nak nincs
					NULL, -- docdescr, majd fordítva kell tölteni, ha lesz benne adat
					NULL, -- linedescr, majd fordítva kell tölteni, ha lesz benne adat
					NULL, -- trtypecomment
					CASE WHEN valueacc > 0 THEN srcpart ELSE destpart END, -- partnernamefrombank
					comment,
					CASE WHEN LTRIM(RTRIM(ISNULL(partneracnum,N'')))=N'' THEN destacnum ELSE partneracnum END,
					1, -- postable,
					NULL, -- destination,
					NULL, -- matchdoccode,
					NULL, -- matchdocnum,
					NULL, -- matchdoclinenum,
					0, -- matched,
					@userid,
					CURRENT_TIMESTAMP,
					NULL, -- errorcode,
					'',	-- extref1
					CONVERT(NVARCHAR, @year) + '/' + RIGHT('000' + CONVERT(NVARCHAR, @statement), 3), -- extref2
					CONVERT(DATE, @enddate, 112), -- extref3
					'',	-- extref4
					LEFT(comment, 32), -- extref5
					'', -- extref6
					@opening, -- openbalacc
					@closing, -- closebalacc
					ISNULL(
					(
						select sum(full_value) from oas_balance as b (nolock) join oas_company as c (nolock) on c.code = b.cmpcode
						where cmpcode = @cmpcode and el1 = @lcbankacccode and balcode = c.actual and b.curcode = c.homecur 
						and repbasis = @lnacclevel 
						and yr = CASE @stdateflag WHEN 0 THEN year(trandate) ELSE year(@enddate) END 
						and period <= CASE @stdateflag WHEN 0 THEN month(trandate) ELSE month(@enddate) END 
					), 0 ) -- extvalueacc

					--CONVERT(NVARCHAR, @year) + '/' + RIGHT('000' + CONVERT(NVARCHAR, @statement), 3), -- extref1
					--LEFT(comment, 32), -- extref2
					--LEFT(CASE WHEN valueacc > 0 THEN srcpart ELSE destpart END, 32), -- extref3
					--CONVERT(DATE, @enddate, 112) -- extref4
				FROM
					(SELECT
						filname, 
						trtype, 
						max(trid) trid, 
						max(trandate) trandate, 
						curacc, 
						SUM(valueacc) valueacc, 
						curdoc, 
						SUM(valuedoc) valuedoc, 
						srcpart, 
						destpart, 
						--max(comment) comment, 
						'Összevont csekkbefizetés sorok' comment,
						partneracnum, 
						destacnum
					FROM cif_ebank_khb_lines L WITH (NOLOCK)
					WHERE 
						interfaceid = @interfaceid 
						AND username = @userid 
						AND ownacnum = @acnum
						AND trtype = 823
					GROUP BY filname, trtype, curacc, curdoc, srcpart, destpart, partneracnum, destacnum) check_lines
					
				FETCH NEXT FROM @cursor INTO @acnum
			END
			CLOSE @cursor
			DEALLOCATE @cursor

            --- SEARCH partnercode
            -- order:
            -- 1. partneracnum = sort+acnum v. partneracnum = iban
            -- 2. partnernamefrombank = name

            DECLARE searchPartnerCode CURSOR FOR
              SELECT id, ltrim(rtrim(partnernamefrombank)), ltrim(rtrim(partneracnum))
              FROM cif_ebank_trans c (nolock)
              WHERE c.interfaceid = @interfaceid and c.cmpcode = @cmpcode and c.fileid = @filename and c.createuser = @userid and partnercode is null

            DECLARE
              @cit_id int,
              @partnnamefrombank nvarchar(400),
              @partnnamefrombank_like nvarchar(400),
              @partnacnum nvarchar(100),
              @partncode nvarchar(144)

            OPEN searchPartnerCode

            --Init
            SET @partnnamefrombank = ''
            SET @partnnamefrombank_like = ''
            SET @partnacnum = ''

            FETCH NEXT FROM searchPartnerCode INTO @cit_id, @partnnamefrombank, @partnacnum
            WHILE @@FETCH_STATUS = 0
              BEGIN

                SET @partncode = ''
                SET @partnnamefrombank_like = '%' + @partnnamefrombank + '%'

                IF (len(@partnacnum) = 16)
                  SET @partnacnum = @partnacnum + '00000000'

                IF isnull(@partnacnum,'') <> ''
                  BEGIN
                    SELECT @partncode = e.elmcode
                    FROM oas_elmbanklist e (nolock)
                      LEFT OUTER JOIN oas_element el WITH (NOLOCK) ON el.cmpcode = e.cmpcode 
                        AND el.elmlevel = e.elmlevel
                        AND el.code = e.elmcode
                        AND el.deldate is null
                      outer apply (select ltrim(rtrim(e.sort)) + ltrim(rtrim(e.acnum)) acno) [x1]
                      outer apply (select case when len(x1.acno) = 16 then x1.acno + '00000000' else x1.acno end acno) [x2]
                    WHERE e.cmpcode = @cmpcode AND e.elmlevel = 2
                          and (x2.acno = @partnacnum or e.iban = @partnacnum)
                  END

                IF isnull(@partncode,'') = '' and isnull(@partnnamefrombank,'') <> ''
                  BEGIN
                    SELECT @partncode = e.elmcode
                    FROM oas_elmbanklist e (nolock)
                      LEFT OUTER JOIN oas_element el WITH (NOLOCK) ON el.cmpcode = e.cmpcode  
                        AND el.elmlevel = e.elmlevel  
                        AND el.code = e.elmcode  
                        AND el.deldate is null  
                    WHERE e.cmpcode = @cmpcode AND e.elmlevel = 2 and UPPER(el.name) like @partnnamefrombank_like
                  END

                IF isnull(@partncode,'') <> ''
                  BEGIN
                    update cif_ebank_trans
                    set partnercode = @partncode
                    where id = @cit_id
                  END

                --SELECT @partncode, @partnnamefrombank, @partnnamefrombank_like, @partnacnum

                FETCH NEXT FROM searchPartnerCode INTO @cit_id, @partnnamefrombank, @partnacnum
              END

            CLOSE searchPartnerCode
            DEALLOCATE searchPartnerCode

			SELECT 0
			RETURN
		END
	END
	SELECT 1
END