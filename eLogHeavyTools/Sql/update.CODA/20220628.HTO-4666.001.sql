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