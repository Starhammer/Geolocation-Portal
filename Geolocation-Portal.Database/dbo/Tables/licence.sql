CREATE TABLE [dbo].[licence]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [name] NVARCHAR(50) NOT NULL, 
    [description] NTEXT NULL
)
