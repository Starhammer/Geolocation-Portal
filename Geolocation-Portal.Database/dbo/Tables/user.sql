CREATE TABLE [dbo].[user]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[role_id] INT NOT NULL,
	[department_id] INT NOT NULL,
	[first_name] NVARCHAR(50) NULL,
	[last_name] NVARCHAR(50) NULL,
	[username] NVARCHAR(50) NOT NULL,
	[password] NVARCHAR(MAX) NOT NULL,
	[last_password_change]DATETIME2 NULL, 
	[create_date] DATETIME2 NOT NULL,
	[account_active] BIT NOT NULL DEFAULT 0,
	[login_attempts] INT NULL,
	[last_login] DATETIME2 NULL
)

