CREATE TABLE [dbo].[fileSearchtag]
(
	[file_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY , 
    [search_tag_id] INT NOT NULL, 
    CONSTRAINT [FK_fileSearchtag_file] FOREIGN KEY ([file_id]) REFERENCES [file](id), 
    CONSTRAINT [FK_fileSearchtag_searchStag] FOREIGN KEY (search_tag_id) REFERENCES [searchtag](id)
)
