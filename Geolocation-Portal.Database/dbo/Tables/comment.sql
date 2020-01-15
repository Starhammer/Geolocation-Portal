CREATE TABLE [dbo].[comment]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[title] NVARCHAR(50) NULL,
	[text] NTEXT NULL,
	[person_name] NVARCHAR(50) NULL,
	[bewertung] INT NOT NULL 
)

