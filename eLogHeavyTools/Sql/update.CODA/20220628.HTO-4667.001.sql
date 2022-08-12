if object_id('cif_ebank_otp_card_all') is null
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


if object_id('cif_ebank_otp_card_header') is null
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


if object_id('cif_ebank_otp_card_lines') is null
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


if object_id('cif_ebank_otp_card_footer') is null
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