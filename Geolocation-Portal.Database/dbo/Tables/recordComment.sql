CREATE TABLE [dbo].[recordComment]
(
	[dataset_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [comment_id] INT NOT NULL, 
    CONSTRAINT [FK_recordComment_record] FOREIGN KEY (dataset_id) REFERENCES record(id), 
    CONSTRAINT [FK_recordComment_comment] FOREIGN KEY (comment_id) REFERENCES comment(id)
)
