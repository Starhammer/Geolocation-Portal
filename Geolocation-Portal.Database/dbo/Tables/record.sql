CREATE TABLE [dbo].[record]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [dataset_upload] DATETIME2 NOT NULL, 
    [dataset_modified_date] DATETIME2 NOT NULL,
	 [title] NVARCHAR(50) NOT NULL, 
	 [description] NTEXT NULL, 
	 [category_id] INT NULL,
	 [licence_id] INT NULL,
	 [publisher_id] INT NULL,
	 [rating] INT NOT NULL DEFAULT 0,
	 [role_id] INT NULL,
	 [record_active] BIT NOT NULL DEFAULT 0, 
    [location_id] INT NOT NULL, 
    [dia_data] BIT NOT NULL DEFAULT 0, 
    [geo_data] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_record_category] FOREIGN KEY (category_id) REFERENCES category(id), 
    CONSTRAINT [FK_record_publisher] FOREIGN KEY (publisher_id) REFERENCES publisher(id), 
    CONSTRAINT [FK_record_location] FOREIGN KEY (location_id) REFERENCES location(id),
	CONSTRAINT [FK_record_licence] FOREIGN KEY (licence_id) REFERENCES licence(id)

)
