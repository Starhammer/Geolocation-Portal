CREATE TABLE [dbo].[record]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [dataset_upload] DATETIME2 NOT NULL, 
    [dataset_modified_date] DATETIME2 NOT NULL,
	 [title] NVARCHAR(50) NOT NULL, 
	 [description] NTEXT NULL, 
	 [category_id] INT NULL,
	 [licence_id] INT NULL,
	 [publisher_id] INT NULL,
	 [rating] INT NULL,
	 [role_id] INT NULL,
	 [record_active] BIT NOT NULL DEFAULT 0

)
