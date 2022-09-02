if object_id('cif_ebank_fio_header') is null
CREATE TABLE [cif_ebank_fio_header]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[accountnumber] [nvarchar](16) NULL,
	[accountowner] [nvarchar](20) NULL,
	[openbaldate] [datetime] NULL,
	[openbalance] [decimal](24,8) NULL,
	[openbalsign] [nvarchar](1) NULL,
	[closebalance] [decimal](24,8) NULL,
	[closebalsign] [nvarchar](1) NULL,
	[debittransbalance] [decimal](24,8) NULL,
	[debittranssign] [nvarchar](1) NULL,
	[creditransbalance] [decimal](24,8) NULL,
	[creditranssing] [nvarchar](1) NULL,
	[statementno] [int] NULL,
	[statementdate] [datetime] NULL,
	CONSTRAINT [PK_cif_ebank_fio_header] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_fio_075') is null
CREATE TABLE [cif_ebank_fio_075]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[accountnumber] [nvarchar](16) NULL,
	[countpaccountnumber] [nvarchar](16) NULL,
	[transactionid] [nvarchar](13) NULL,
	[transactionamount] [decimal](24,8) NULL,
	[accountingtype] [nvarchar](1) NULL,
	[variablecode] [nvarchar](10) NULL,
	[delimiter] [nvarchar](2) NULL,
	[countpbankcode] [nvarchar](4) NULL,
	[constcode] [nvarchar](4) NULL,
	[specificcode] [nvarchar](10) NULL,
	[valuedate] [datetime] NULL,
	[description] [nvarchar](20) NULL,
	[currency_code] [nvarchar](5) NULL,
	[currency] [nvarchar](3) NULL,
	[postingdate] [datetime] NULL,
	CONSTRAINT [PK_cif_ebank_fio_075] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_fio_076') is null
CREATE TABLE [cif_ebank_fio_076]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[transaction_bank_id] [nvarchar](26) NULL,
	[countpdebitdate] [datetime] NULL,
	[countpnamecomment] [nvarchar](92) NULL,
	CONSTRAINT [PK_cif_ebank_fio_076] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_fio_078') is null
CREATE TABLE [cif_ebank_fio_078]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[transaction_info1] [nvarchar](35) NULL,
	[countpaccountnumber] [nvarchar](35) NULL,
	[countpbank] [nvarchar](54) NULL,
	[transaction_info_orig] [nvarchar](35) NULL,
	[transaction_info_p1] [nvarchar](20) NULL,
	[transaction_info_p2] [nvarchar](3) NULL,
	[transaction_info_p3] [nvarchar](20) NULL,
	CONSTRAINT [PK_cif_ebank_fio_078] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_fio_079') is null
CREATE TABLE [cif_ebank_fio_079]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[transaction_info3] [nvarchar](35) NULL,
	[transaction_info4] [nvarchar](35) NULL,
	[transaction_info5] [nvarchar](35) NULL,
	CONSTRAINT [PK_cif_ebank_fio_079] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_paypal_lines') is null
CREATE TABLE [cif_ebank_paypal_lines]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[p_date] [nvarchar](10) NULL,
	[p_time] [nvarchar](10) NULL,
	[p_time_zone] [nvarchar](50) NULL,
	[p_description] [nvarchar](50) NULL,
	[p_currency] [nvarchar](5) NULL,
	[p_gross] [nvarchar](15) NULL,
	[p_fee] [nvarchar](15) NULL,
	[p_net] [nvarchar](15) NULL,
	[p_balance] [nvarchar](20) NULL,
	[p_transactionid] [nvarchar](20) NULL,
	[p_fromemailaddr] [nvarchar](100) NULL,
	[p_name] [nvarchar](150) NULL,
	[p_bankname] [nvarchar](150) NULL,
	[p_bankaccount] [nvarchar](100) NULL,
	[p_shipp_handl_amount] [nvarchar](15) NULL,
	[p_salestax] [nvarchar](15) NULL,
	[p_invoiceid] [nvarchar](20) NULL,
	[p_reftaxid] [nvarchar](20) NULL,
	[p_acnum] [nvarchar](24) NULL,
	CONSTRAINT [PK_cif_ebank_paypal_lines] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_akcenta_header') is null
CREATE TABLE [cif_ebank_akcenta_header]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[accountnumber] [nvarchar](16) NULL,
	[accountowner] [nvarchar](20) NULL,
	[openbaldate] [datetime] NULL,
	[openbalance] [decimal](24,8) NULL,
	[openbalsign] [nvarchar](1) NULL,
	[closebalance] [decimal](24,8) NULL,
	[closebalsign] [nvarchar](1) NULL,
	[debittransbalance] [decimal](24,8) NULL,
	[debittranssign] [nvarchar](1) NULL,
	[creditransbalance] [decimal](24,8) NULL,
	[creditranssing] [nvarchar](1) NULL,
	[statementno] [int] NULL,
	[statementdate] [datetime] NULL,
	CONSTRAINT [PK_cif_ebank_akcenta_header] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_akcenta_075') is null
CREATE TABLE [cif_ebank_akcenta_075]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[accountnumber] [nvarchar](16) NULL,
	[countpaccountnumber] [nvarchar](16) NULL,
	[transactionid] [nvarchar](13) NULL,
	[transactionamount] [decimal](24,8) NULL,
	[accountingtype] [nvarchar](1) NULL,
	[variablecode] [nvarchar](10) NULL,
	[delimiter] [nvarchar](2) NULL,
	[countpbankcode] [nvarchar](4) NULL,
	[constcode] [nvarchar](4) NULL,
	[specificcode] [nvarchar](10) NULL,
	[valuedate] [datetime] NULL,
	[description] [nvarchar](20) NULL,
	[currency_code] [nvarchar](5) NULL,
	[currency] [nvarchar](3) NULL,
	[postingdate] [datetime] NULL,
	CONSTRAINT [PK_cif_ebank_akcenta_075] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_akcenta_076') is null
CREATE TABLE [cif_ebank_akcenta_076]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[transaction_bank_id] [nvarchar](26) NULL,
	[countpdebitdate] [datetime] NULL,
	[countpnamecomment] [nvarchar](92) NULL,
	CONSTRAINT [PK_cif_ebank_akcenta_076] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_akcenta_078') is null
CREATE TABLE [cif_ebank_akcenta_078]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[transaction_info1] [nvarchar](35) NULL,
	[countpaccountnumber] [nvarchar](35) NULL,
	[countpbank] [nvarchar](54) NULL,
	[transaction_info_orig] [nvarchar](35) NULL,
	[transaction_info_p1] [nvarchar](20) NULL,
	[transaction_info_p2] [nvarchar](3) NULL,
	[transaction_info_p3] [nvarchar](20) NULL,
	CONSTRAINT [PK_cif_ebank_akcenta_078] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_akcenta_079') is null
CREATE TABLE [cif_ebank_akcenta_079]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[transaction_info3] [nvarchar](35) NULL,
	[transaction_info4] [nvarchar](35) NULL,
	[transaction_info5] [nvarchar](35) NULL,
	CONSTRAINT [PK_cif_ebank_akcenta_079] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

CREATE TABLE [dbo].[cif_ebank_otp_card_all](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[line] [nvarchar](1000) NOT NULL,
 CONSTRAINT [PK_cif_ebank_otp_card_all] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [cif_ebank_otp_card_header]
(
    [id] [int] IDENTITY(1,1) NOT NULL,
    [interfaceid] [nvarchar](50) NOT NULL,
    [filname] [nvarchar](50) NOT NULL,
    [username] [nvarchar](256) NOT NULL,
    [inputdate] [datetime] NOT NULL,
    [lineseqno] [smallint] NOT NULL,
	[constans] [nvarchar](2) NULL,
	[trader_code] [nvarchar](9) NULL,
	[trader_name] [nvarchar](30) NULL,
	[registration_number] [nvarchar](20) NULL,
    [creation_date] [datetime] NULL,
	[account_number] [nvarchar](30) NULL,
	[currency] [nvarchar](3) NULL,
	CONSTRAINT [PK_cif_ebank_otp_card_header] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

CREATE TABLE [cif_ebank_otp_card_lines]
(
    [id] [int] IDENTITY(1,1) NOT NULL,
    [interfaceid] [nvarchar](50) NOT NULL,
    [filname] [nvarchar](50) NOT NULL,
    [username] [nvarchar](256) NOT NULL,
    [inputdate] [datetime] NOT NULL,
    [lineseqno] [smallint] NOT NULL,
	[transaction_id] [nvarchar](16) NULL,
	[terminal_id] [nvarchar](8) NULL,
	[authorization_id] [nvarchar](6) NULL,
	[transaction_date] [datetime] NULL,
	[transaction_time] [nvarchar](6) NULL,
	[card_number] [nvarchar](19) NULL,
	[transaction_amount] [decimal](24,8) NULL,
	[intercharge_amount] [decimal](24,8) NULL,
	[systemcharge_amount] [decimal](24,8) NULL,
	[merchant_fee_amount] [decimal](24,8) NULL,
	[commission_amount] [decimal](24,8) NULL,
	[net_amount] [decimal](24,8) NULL,
	[transaction_flag] [nvarchar](1) NULL,
	[store_name] [nvarchar](50) NULL,
	[external_unique_id] [nvarchar](32) NULL,
	[binf] [nvarchar](2) NULL,
	[ownacnum] [nvarchar](36) NULL
	CONSTRAINT [PK_cif_ebank_otp_card_lines] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

CREATE TABLE [cif_ebank_otp_card_footer]
(
    [id] [int] IDENTITY(1,1) NOT NULL,
    [interfaceid] [nvarchar](50) NOT NULL,
    [filname] [nvarchar](50) NOT NULL,
    [username] [nvarchar](256) NOT NULL,
    [inputdate] [datetime] NOT NULL,
    [lineseqno] [smallint] NOT NULL,
	[constans] [nvarchar](2) NULL,
	[trader_code] [nvarchar](9) NULL,
    [creation_date] [datetime] NULL,
    [processed_records] [int] NULL,
	[net_value] [decimal](24,8) NULL,
	CONSTRAINT [PK_cif_ebank_otp_card_footer] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_khb_card_all') is null
CREATE TABLE [dbo].[cif_ebank_khb_card_all](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[line] [nvarchar](1000) NOT NULL,
	CONSTRAINT [PK_cif_ebank_khb_card_all] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

if object_id('cif_ebank_khb_card_header') is null
CREATE TABLE [cif_ebank_khb_card_header]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[generation_date] [datetime] NOT NULL,	/* File keszites datuma */
	[generation_time] [nvarchar](8) NULL,	/* File keszites idopontja */
	[number_of_file] [int] NULL,			/* File sorszama */
	[number_of_records] [int] NULL,			/* Tetelsorok szama */
	[transfer_date] [datetime] NOT NULL,	/* Utalas datuma */
	[acnum] [nvarchar](36) NULL				/* Szamlaszam */
	CONSTRAINT [PK_cif_ebank_khb_card_header] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

if object_id('cif_ebank_khb_card_lines') is null
CREATE TABLE [cif_ebank_khb_card_lines]
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[interfaceid] [nvarchar](50) NOT NULL,
	[filname] [nvarchar](50) NOT NULL,
	[username] [nvarchar](256) NOT NULL,
	[inputdate] [datetime] NOT NULL,
	[lineseqno] [smallint] NOT NULL,
	[cardno_trno] [nvarchar](40) NOT NULL,	/* Kartyaszam vagy tranzakcioszam */
	[transaction_date] [datetime] NULL,
	[transaction_amount] [decimal](24,8) NULL,
	[msc_amount] [decimal](24,8) NULL,
	[dcc_reimb_amount] [decimal](24,8) NULL,
	[net_amount] [decimal](24,8) NULL,
	[mif_amount] [decimal](24,8) NULL,
	[blank] [nvarchar](20) NULL,
	[msc_mif_amount] [decimal](24,8) NULL,
	[terminal_id] [nvarchar](20) NULL,
	[merchant_id] [nvarchar](20) NULL,
	[transaction_number] [nvarchar](20) NULL,
	[authorization_number] [nvarchar](20) NULL,
	[currency_iso] [nvarchar](6) NULL,
	[currency] [nvarchar](6) NULL,
	[status] [nvarchar](4) NULL,
	[batch_number] [nvarchar](24) NULL,
	[transfer_number] [nvarchar](20) NULL,
	[brand] [nvarchar](2) NULL,
	[db_card_type] [nvarchar](2) NULL,
	[comm_card_type] [nvarchar](2) NULL,
	[region_of_issuance] [nvarchar](2) NULL,
	[merch_comm1] [nvarchar](40) NULL,
	[merch_comm2] [nvarchar](40) NULL,
	[trancated_pan] [nvarchar](40) NULL,
	[merch_reserv] [nvarchar](40) NULL,
	[transfer_id] [nvarchar](40) NULL,
	[ownacnum] [nvarchar](36) NULL
	CONSTRAINT [PK_cif_ebank_khb_card_lines] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)
)
GO

create type oasdoclineids as table (cmpcode nvarchar(12) not null, doccode nvarchar(12) not null, docnum nvarchar(12) not null, doclinenum int not null)
go

create table ofc_reminderletterpayerid (
  id          int          identity(1, 1),
  doclineids  xml          null,
  payerid     char(16)     null,
  prlcode     nvarchar(28) null, 
  elmcode     nvarchar(36) null, 
  elmlevel    integer      null,
  severity    integer      null,
  remdate     datetime     null, 
  duedate     datetime     null,
  adddate     datetime not null,
  constraint pk_ofc_reminderletterpayerid primary key (id)
)
go

