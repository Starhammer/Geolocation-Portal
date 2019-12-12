CREATE TABLE [dbo].[category]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[parent_id] INT NOT NULL,
	[name] NVARCHAR(50) NOT NULL,
	[description] NTEXT NULL,
	[icon] NVARCHAR(50) NULL
)
