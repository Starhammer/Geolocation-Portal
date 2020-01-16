/*
Vorlage für ein Skript nach der Bereitstellung							
--------------------------------------------------------------------------------------
 Diese Datei enthält SQL-Anweisungen, die an das Buildskript angefügt werden.		
 Schließen Sie mit der SQLCMD-Syntax eine Datei in das Skript nach der Bereitstellung ein.			
 Beispiel:   :r .\myfile.sql								
 Verwenden Sie die SQLCMD-Syntax, um auf eine Variable im Skript nach der Bereitstellung zu verweisen.		
 Beispiel:   :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
SET IDENTITY_INSERT [dbo].[role] ON
INSERT INTO [dbo].[role] ([Id], [name], [description]) VALUES (1, N'Administrator', N'Diese Rolle hat Zugriff auf alle Funktionen des Portals.')
INSERT INTO [dbo].[role] ([Id], [name], [description]) VALUES (2, N'Key-User', N'Diese Rolle unterstützt die Administrator-Rolle beim Verwalten von Datensätzen.')
INSERT INTO [dbo].[role] ([Id], [name], [description]) VALUES (3, N'Internen-User', N'Diese Rolle kann nicht öffentliche Datensätze einsehen.')
INSERT INTO [dbo].[role] ([Id], [name], [description]) VALUES (4, N'Externen-User', N'Diese Rolle kann alle öffentlichen Datensätze einsehen. Diese Rolle kann sich nicht anmelden.')
SET IDENTITY_INSERT [dbo].[role] OFF

SET IDENTITY_INSERT [dbo].[licence] ON
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (1, N'CC-BY', N'Creative Commons Namensnennung 4.0 (CC-BY): Der Lizenzgeber (Stadt Mosbach) erlaubt es, das Werk zu vervielfältigen, zu verbreiten, öffentlich zu zeigen bzw. zugänglich zu machen, aufzuführen und auszustellen sowie darauf aufbauende Bearbeitungen anzufertigen und zu verbreiten und zwar auch für kommerzielle Zwecke')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (2, N'CC-BY-SA', N'Creative Commons Namensnennung – Share-Alike 4.0 (CC-BY-SA) (Weitergabe unter gleichen Bedingungen): Der Lizenzgeber (Stadt Mosbach) erlaubt es, Bearbeitungen anzufertigen und zu verbreiten, allerdings letzteres nur unter derselben oder einer kompatiblen Lizenz.
Der Lizenzgeber (Stadt Mosbach) erlaubt es jedermann, das Werk zu vervielfältigen, zu verbreiten, öffentlich zu zeigen bzw. zugänglich zu machen sowie es aufzuführen und auszustellen, und zwar auch für kommerzielle Zwecke.
')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (3, N'CC-BY-ND', N'Creative Commons Namensnennung – Keine-Bearbeitungen 4.0 (CC-BY-ND): Der Lizenzgeber (Stadt Mosbach) erlaubt es, das Werk auch für kommerzielle Zwecke zu vervielfältigen, zu verbreiten, zu zeigen und aufzuführen, erlaubt jedoch nicht die Verbreitung von auf dem Werk aufbauende Bearbeitungen.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (4, N'CC-BY-NC', N'Creative Commons Namensnennung – Nicht-kommerziell 4.0 (CC-BY-NC): Der Lizenzgeber (Stadt Mosbach) erlaubt jedermann, das Werk für nicht kommerzielle Zwecke zu vervielfältigen, zu verbreiten, öffentlich zu zeigen bzw. öffentlich zugänglich zu machen, es aufzuführen und auszustellen sowie darauf aufbauende Bearbeitungen anzufertigen und zu verbreiten.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (5, N'CC-BY-NC-SA', N'Creative Commons Namensnennung – Nicht-kommerziell – Share Alike 4.0 (CC-BY-NC-SA) (Weitergabe unter gleichen Bedingungen): Der Lizenzgeber (Stadt Mosbach) erlaubt es, Bearbeitungen anzufertigen und zu verbreiten, allerdings letzteres nur unter derselben oder einer kompatiblen Lizenz.
Der Lizenzgeber (Stadt Mosbach) erlaubt jedermann, das Werk für nicht kommerzielle Zwecke zu vervielfältigen, zu verbreiten, öffentlich zu zeigen bzw. öffentlich zugänglich zu machen sowie es aufzuführen und auszustellen.
')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (6, N'CC-BY-NC-ND', N'Creative Commons Namensnennung – Nicht-kommerziell – Keine Bearbeitungen 4.0 (CC-BY-NC-ND): Der Lizenzgeber (Stadt Mosbach) erlaubt jedermann, das Werk für nicht kommerzielle Zwecke zu vervielfältigen, zu verbreiten, öffentlich zu zeigen bzw. öffentlich zugänglich zu machen sowie es aufzuführen und auszustellen, erlaubt jedoch nicht die Verbreitung von auf dem Werk aufbauende Bearbeitungen.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (7, N'CC0', N'Creative Commons Zero 1.0 (Public Domain Dedication) (CC0): Der Lizenzgeber (Stadt Mosbach) erlaubt es, das Werk zu kopieren, zu verbreiten und aufzuführen, sogar zu kommerziellen Zwecken, ohne dass er um weitere Erlaubnis gebeten werden muss.
In keiner Weise werden Patente oder Markenschutzrechte des Lizenzgebers (Stadt Mosbach) von dieser Lizenz angetastet. Dasselbe gilt für Rechte, welche andere Personen am Werk oder an seiner Verwendung geltend machen können.
')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (8, N'GeoNutzV ', N'GeoNutzV – Verordnung zur Festlegung der Nutzungsbestimmungen für die Bereitstellung von Geodaten des Bundes: Bereitgestellte Geodaten und zugehörigen Metadaten dürfen vervielfältigt, ausgedruckt, präsentiert, verändert, bearbeiten, an Dritte übermittelt, mit eigenen Daten und Daten Anderer zusammengeführt und zu selbstständig neuen Datensätzen verbunden und in interne und externe Geschäftsprozesse, Produkte und Anwendungen in öffentlichen und nicht öffentlichen elektronischen Netzwerken eingebunden werden. 
Bereitgestellte Geodatendienste dürfen mit eigenen Diensten und Diensten Anderer zusammengeführt und in interne und externe Geschäftsprozesse, Produkte und Anwendungen in öffentlichen und nicht öffentlichen elektronischen Netzwerken eingebunden werden.
')
SET IDENTITY_INSERT [dbo].[licence] OFF

SET IDENTITY_INSERT [dbo].[location] ON
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (1, N'Mosbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (2, N'Stadt Mosbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (15, N'Neckarelz')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (16, N'Diedesheim')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (17, N'Lohrbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (18, N'Reichenbuch')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (19, N'Sattelbach')
SET IDENTITY_INSERT [dbo].[location] OFF

SET IDENTITY_INSERT [dbo].[category] ON
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (1, 0, N'Bevölkerung', N'Bevölkerung', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (2, 0, N'Bildung und Wissenschaft', N'Bildung und Wissenschaft', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (3, 0, N'Geo-Daten', N'Datensätze, die Geo-Daten enthalten', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (4, 0, N'Gesetze und Justiz', N'Gesetze und Justiz', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (5, 0, N'Infrastruktur, Bauen und Wohnen', N'Infrastruktur, Bauen und Wohnen', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (6, 0, N'Kultur', N'Kultur', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (7, 0, N'Tourismus', N'Tourismus', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (8, 0, N'Verwaltung', N'Verwaltung', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (9, 0, N'Haushalt', N'Haushalt', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (10, 0, N'Steuern', N'Steuern', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (11, 0, N'Wahlen', N'Wahlen', NULL)
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (12, 0, N'Diagramm-Daten', N'Datensätze, die Diagramme enthalten', NULL)
SET IDENTITY_INSERT [dbo].[category] OFF

SET IDENTITY_INSERT [dbo].[publisher] ON
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (1, N'Stadtverwaltung Mosbach', N'Stadtverwaltung Mosbach')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (2, N'Finanzamt Mosbach', N'Finanzamt Mosbach')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (3, N'Landratsamt Mosbach', N'Landratsamt Mosbach')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (4, N'Gemeinde Obrigheim', N'Gemeinde Obrigheim')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (5, N'Rathaus Mosbach', N'Rathaus Mosbach')
SET IDENTITY_INSERT [dbo].[publisher] OFF

SET IDENTITY_INSERT [dbo].[user] ON
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (4, 1, 1, N'Julia', N'Kessler', N'JKessler', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (7, 1, 1, N'Manuel', N'Scheub', N'MScheub', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (8, 1, 1, N'Laura', N'Schmitt', N'LSchmitt', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (10, 1, 1, N'Michael', N'Kress', N'MKress', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (11, 2, 1, N'Jan', N'Schumacher', N'JSchumacher', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (12, 3, 1, N'Thilo', N'Krause', N'TKrause', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (13, 3, 1, N'Alex', N'Moritz', N'AMoritz', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[user] OFF