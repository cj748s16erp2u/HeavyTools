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