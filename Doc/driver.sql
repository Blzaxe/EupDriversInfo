CREATE TABLE [dbo].[Driver](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[driverName] [nvarchar](10) NOT NULL,
	[account] [varchar](8) NOT NULL,
	[lastLoginTime] [datetime] NULL,
 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]