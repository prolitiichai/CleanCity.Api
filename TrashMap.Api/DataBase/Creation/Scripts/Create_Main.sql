CREATE TABLE [Users] (
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[AvatarPath] NVARCHAR(256),
	[Login] NVARCHAR(256) NOT NULL,
	[PasswordSalt] NVARCHAR(256) NOT NULL,
	[NickName] NVARCHAR(256),
	[TrashFound] INTEGER,
	[TrashCleaned] INTEGER,
	[Karma] INTEGER
);

CREATE TABLE [Points] (
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[Title] NVARCHAR(256),
	[UserId] INTEGER,
	[Created] INTEGER,
	[Updated] INTEGER,
	[Complexity] INTEGER,
	[Latitude] FLOAT,
	[Longitude] FLOAT,
	[IsFixed] BIT
);

CREATE TABLE [PointComments] (
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[UserId] INTEGER,
	[PointId] INTEGER,
	[PhotoPath] NVARCHAR(256),
	[PointStatus] INTEGER,
	[PlusCount] INTEGER,
	[MinusCount] INTEGER
);

CREATE TABLE [Likes] (
	[Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	[UserId] INTEGER,
	[PointCommentEntityId] INTEGER,
	[IsLike] BIT
);

CREATE INDEX [idx_login] ON [Users]([Login]);

CREATE INDEX [idx_point_user_id] ON [Points]([UserId]);
CREATE INDEX [idx_point_search_dots] ON [Points]([IsFixed],[Latitude],[Longitude]);

CREATE INDEX [idx_point_comments] ON [PointComments]([PointId]);
CREATE INDEX [idx_user_comments] ON [PointComments]([UserId]);

CREATE INDEX [idx_user_likes] ON [Likes]([UserId]);
CREATE INDEX [idx_point_likes] ON [Likes]([PointCommentEntityId]);