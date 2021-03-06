﻿using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public class SqliteClassMapping
	{
		public class UserEntityClassMapper : ClassMapper<UserEntity>
		{
			public const string TableNameConst = "Users";

			public UserEntityClassMapper()
			{
				TableName = TableNameConst;
				Map(e => e.Id).Key(KeyType.Identity);
				AutoMap();
			}
		}

		public class LikeEntityClassMapper : ClassMapper<LikeEntity>
		{
			public const string TableNameConst = "Likes";

			public LikeEntityClassMapper()
			{
				TableName = TableNameConst;
				Map(e => e.Id).Key(KeyType.Identity);
				AutoMap();
			}
		}
		public class PointCommentEntityClassMapper : ClassMapper<PointCommentEntity>
		{
			public const string TableNameConst = "PointComments";

			public PointCommentEntityClassMapper()
			{
				TableName = TableNameConst;
				Map(e => e.Id).Key(KeyType.Identity);
				AutoMap();
			}
		}
		public class PointEntityClassMapper : ClassMapper<PointEntity>
		{
			public const string TableNameConst = "Points";

			public PointEntityClassMapper()
			{
				TableName = TableNameConst;
				Map(e => e.Id).Key(KeyType.Identity);
				AutoMap();
			}
		}
	}
}
