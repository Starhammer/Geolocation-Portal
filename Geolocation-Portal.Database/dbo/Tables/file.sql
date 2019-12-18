CREATE TABLE [dbo].[file]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[record_id] INT NOT NULL,
	[file_upload_date] DATETIME2 NOT NULL,
	[download_count] INT NULL,
	[file_icon] NVARCHAR(50) NULL,
	[diagram_data] INT NULL ,
	[map_data] INT NULL,
	[file_size] FLOAT NULL, 
    CONSTRAINT [FK_file_record] FOREIGN KEY (record_id) REFERENCES record(id) 
)
