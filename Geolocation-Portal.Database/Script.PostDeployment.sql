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
INSERT INTO [dbo].[role] ([Id], [name], [description]) VALUES (3, N'Interner-User', N'Diese Rolle kann nicht öffentliche Datensätze einsehen.')
INSERT INTO [dbo].[role] ([Id], [name], [description]) VALUES (4, N'Externer-User', N'Diese Rolle kann alle öffentlichen Datensätze einsehen. Diese Rolle kann sich nicht anmelden.')
SET IDENTITY_INSERT [dbo].[role] OFF

SET IDENTITY_INSERT [dbo].[licence] ON
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (1, N'CC-BY', N'Diese Lizenz erlaubt Dritten, ein Werk zu verbreiten, zu remixen, zu verbessern und darauf aufzubauen, auch kommerziell, solange der Urheber des Originals genannt wird.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (2, N'CC-BY-SA', N'Diese Lizenz erlaubt es Dritten, ein Werk zu verbreiten, zu remixen, zu verbessern und darauf aufzubauen, auch kommerziell, solange der Urheber des Originals genannt wird und die auf seinem Werk basierenden neuen Werke unter denselben Bedingungen veröffentlicht werden.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (3, N'CC-BY-ND', N'Diese Lizenz erlaubt Dritten die Weiterverbreitung des Werkes, kommerziell wie nicht-kommerziell, solange dies ohne Veränderungen und vollständig geschieht und der Urheber genannt wird.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (4, N'CC-BY-NC', N'Diese Lizenz erlaubt Dritten, ein Werk zu verbreiten, zu remixen, zu verbessern und darauf aufzubauen, nur nicht kommerziell, solange der Urheber des Originals genannt wird.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (5, N'CC-BY-NC-SA', N'Diese Lizenz erlaubt es Dritten, ein Werk zu verbreiten, zu remixen, zu verbessern und darauf aufzubauen, nur nicht kommerziell, solange der Urheber des Originals genannt wird und die auf seinem Werk basierenden neuen Werke unter denselben Bedingungen veröffentlicht werden.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (6, N'CC-BY-NC-ND', N'Diese Lizenz erlaubt Dritten die Weiterverbreitung des Werkes, nur nicht-kommerziell, solange dies ohne Veränderungen und vollständig geschieht und der Urheber genannt wird.')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (7, N'CC0', N'Verzicht auf alle urheberrechtlichen und verwandten Schutzrechte')
INSERT INTO [dbo].[licence] ([Id], [name], [description]) VALUES (8, N'GeoNutzV ', N'Bereitgestellte Geodaten und zugehörigen Metadaten dürfen vervielfältigt, ausgedruckt, präsentiert, verändert, bearbeiten, an Dritte übermittelt, mit eigenen Daten und Daten Anderer zusammengeführt und zu selbstständig neuen Datensätzen verbunden und in andere Dienste eingebunden werden.')
SET IDENTITY_INSERT [dbo].[licence] OFF

SET IDENTITY_INSERT [dbo].[location] ON
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (1, N'Mosbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (2, N'Stadt Mosbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (3, N'Neckarelz')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (4, N'Diedesheim')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (5, N'Lohrbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (6, N'Reichenbuch')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (7, N'Sattelbach')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (8, N'Waldstadt')
INSERT INTO [dbo].[location] ([Id], [name]) VALUES (2001, N'Nüstenbach')
SET IDENTITY_INSERT [dbo].[location] OFF

SET IDENTITY_INSERT [dbo].[category] ON
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (1, 0, N'Bevölkerung', N'Bevölkerung', N'Bevoelkerung.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (2, 0, N'Bildung und Wissenschaft', N'Bildung und Wissenschaft', N'Bildung.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (3, 0, N'Gesetze und Justiz', N'Gesetze und Justiz', N'Justiz.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (4, 0, N'Infrastruktur, Bauen und Wohnen', N'Infrastruktur, Bauen und Wohnen', N'Infrastruktur.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (5, 0, N'Kultur', N'Kultur', N'Kultur.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (6, 0, N'Tourismus', N'Tourismus', N'Tourismus.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (7, 0, N'Verwaltung', N'Verwaltung', N'Verwaltung.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (8, 0, N'Haushalt', N'Haushalt', N'Haushalt.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (9, 0, N'Steuern', N'Steuern', N'Steuern.png')
INSERT INTO [dbo].[category] ([Id], [parent_id], [name], [description], [icon]) VALUES (10, 0, N'Wahlen', N'Wahlen', N'Wahlen.png')
SET IDENTITY_INSERT [dbo].[category] OFF

SET IDENTITY_INSERT [dbo].[publisher] ON
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (1, N'Stadtverwaltung Mosbach', N'Die Dateien dieses Datensatzes wurden von der Stadtverwaltung Mosbach erhoben.')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (2, N'Finanzamt Mosbach', N'Die Dateien dieses Datensatzes wurden vom Finanzamt Mosbach erhoben.')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (3, N'Landratsamt Neckar-Odenwald-Kreis', N'Die Dateien dieses Datensatzes wurden vom Landratsamt Neckar-Odenwald-Kreis erhoben.')
INSERT INTO [dbo].[publisher] ([Id], [name], [description]) VALUES (4, N'Gemeinde Obrigheim', N'Die Dateien dieses Datensatzes wurden von der Gemeinde Obrigheim erhoben.')
SET IDENTITY_INSERT [dbo].[publisher] OFF


SET IDENTITY_INSERT [dbo].[user] ON
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (1, 1, 1, N'Julia', N'Kessler', N'JKessler', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (2, 1, 1, N'Manuel', N'Scheub', N'MScheub', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (3, 1, 1, N'Laura', N'Schmitt', N'LSchmitt', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (4, 1, 1, N'Michael', N'Kress', N'MKress', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (5, 2, 1, N'Jan', N'Schumacher', N'JSchumacher', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (6, 3, 1, N'Thilo', N'Krause', N'TKrause', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
INSERT INTO [dbo].[user] ([Id], [role_id], [department_id], [first_name], [last_name], [username], [password], [last_password_change], [create_date], [account_active], [login_attempts], [last_login]) VALUES (7, 3, 1, N'Alex', N'Moritz', N'AMoritz', N'098f6bcd4621d373cade4e832627b4f6', NULL, N'2019-12-20 00:00:00', 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[user] OFF

/* Beispiel Datensätze */
SET IDENTITY_INSERT [dbo].[record] ON
INSERT INTO [dbo].[record] ([Id], [dataset_upload], [dataset_modified_date], [title], [description], [category_id], [licence_id], [publisher_id], [rating], [role_id], [record_active], [location_id], [dia_data], [geo_data]) VALUES (1, N'2018-06-23 07:30:20', N'2018-06-23 07:30:20', N'Bildungseinrichtungen', N'Dieser Datensatz beinhaltet die Geolocation Daten aller Schulen innerhalb der Gemeinde Mosbach.', 3, 1, 1, 0, 4, 1, 1, 0, 1)
INSERT INTO [dbo].[record] ([Id], [dataset_upload], [dataset_modified_date], [title], [description], [category_id], [licence_id], [publisher_id], [rating], [role_id], [record_active], [location_id], [dia_data], [geo_data]) VALUES (2, N'2018-06-23 07:30:20', N'2018-06-23 07:30:20', N'Kindergartenübersicht', N'Die Gemeinde Mosbach besitzt in vielen Ortschaften einen Kindergarten. Dieser Geolocation Datensatz zeigt die Position jedes Kindergartens innerhalb der Gemeinde Mosbach.', 3, 1, 1, 0, 4, 1, 1, 0, 1)
INSERT INTO [dbo].[record] ([Id], [dataset_upload], [dataset_modified_date], [title], [description], [category_id], [licence_id], [publisher_id], [rating], [role_id], [record_active], [location_id], [dia_data], [geo_data]) VALUES (3, N'2018-06-23 07:30:20', N'2018-06-23 07:30:20', N'Parkhausübersicht in der Großen Kreisstadt Mosbach', N'Jede Stadt hat mit dem wachsenden Bedarf an Parkplätzen zu kämpfen. Damit Sie bei Ihrem Stadtbesuch besser planen können, sehen Sie in diesem Geolocation Datensatz die Positionen aller öffentlicher Parklätze in der Stadt Mosbach.', 3, 1, 1, 0, 4, 1, 2, 0, 1)
INSERT INTO [dbo].[record] ([Id], [dataset_upload], [dataset_modified_date], [title], [description], [category_id], [licence_id], [publisher_id], [rating], [role_id], [record_active], [location_id], [dia_data], [geo_data]) VALUES (4, N'2018-06-23 07:30:20', N'2018-06-23 07:30:20', N'Behörden in Mosbach', N'Dieser Datensatz zeigt die Position der Behörden in Mosbach.', 3, 1, 1, 0, 4, 1, 2, 0, 1)
SET IDENTITY_INSERT [dbo].[record] OFF

/* Beispiel Dateien */
SET IDENTITY_INSERT [dbo].[file] ON
INSERT INTO [dbo].[file] ([Id], [record_id], [file_upload_date], [download_count], [file_icon], [file_size], [name]) VALUES (1, 1, N'2018-06-23 07:30:20', 0, N'geojson', 7486, N'bildungseinrichtungen.geojson')
INSERT INTO [dbo].[file] ([Id], [record_id], [file_upload_date], [download_count], [file_icon], [file_size], [name]) VALUES (2, 2, N'2020-01-20 23:14:57', 0, N'geojson', 6802, N'kindergarten.geojson')
INSERT INTO [dbo].[file] ([Id], [record_id], [file_upload_date], [download_count], [file_icon], [file_size], [name]) VALUES (3, 3, N'2020-01-20 23:18:06', 0, N'geojson', 5377, N'parking.geojson')
INSERT INTO [dbo].[file] ([Id], [record_id], [file_upload_date], [download_count], [file_icon], [file_size], [name]) VALUES (4, 4, N'2020-01-20 23:21:30', 0, N'geojson', 8856, N'commercial.geojson')
SET IDENTITY_INSERT [dbo].[file] OFF


/* Beispiel Kommentare */
SET IDENTITY_INSERT [dbo].[comment] ON
INSERT INTO [dbo].[comment] ([Id], [title], [text], [person_name], [bewertung], [record_id]) VALUES (1, N'Top!', N'Die Geolocation Position jeder Schule ist optimal getroffen. Eine übersichtliche Ansicht aller Schulen der Gemeinde Mosbach. Zusätzlich ist auch die DHBW Mosbach zu sehen.', N'Max Musterman', 5, 1)
INSERT INTO [dbo].[comment] ([Id], [title], [text], [person_name], [bewertung], [record_id]) VALUES (2, N'Besser als in Google', N'Besonders toll dabei sind die zusätzlichen Informationen. Zu fast jeder Schule befindet sich der Link zur Webseite und Kontaktinformationen.', N'Hannah P.', 5, 1)
SET IDENTITY_INSERT [dbo].[comment] OFF
