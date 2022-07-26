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