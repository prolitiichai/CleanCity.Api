﻿CREATE TABLE [Users] (
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[AvatarPath] NVARCHAR(256),
	[Login] NVARCHAR(256) NOT NULL,
	[PasswordSalt] NVARCHAR(256) NOT NULL,
	[NickName] NVARCHAR(256),
	[TrashFound] INTEGER,
	[TrashCleaned] INTEGER,
	[Karma] INTEGER
);