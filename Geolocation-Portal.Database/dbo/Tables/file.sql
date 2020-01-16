CREATE TABLE [dbo].[file]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[record_id] INT NOT NULL,
	[file_upload_date] DATETIME2 NOT NULL,
	[download_count] INT NULL,
	[file_icon] NVARCHAR(50) NULL,
	[diagram_data] BIT NULL ,
	[map_data] BIT NULL,
	[file_size] FLOAT NULL, 
    [name] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_file_record] FOREIGN KEY (record_id) REFERENCES record(id) 
)
