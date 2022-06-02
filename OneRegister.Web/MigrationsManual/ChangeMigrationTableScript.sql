CREATE TABLE [Application].[MigrationHistory](
	[MigrationId] [NVARCHAR](150) NOT NULL,
	[ProductVersion] [NVARCHAR](32) NOT NULL,
 CONSTRAINT [PK_MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO Application.MigrationHistory
(
    MigrationId,
    ProductVersion
)
SELECT MigrationId,
       ProductVersion
FROM dbo.__EFMigrationsHistory;

GO

DROP TABLE dbo.__EFMigrationsHistory;
Go