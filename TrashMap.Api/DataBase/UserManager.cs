using DapperExtensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public class UserManager : IdManager<UserEntity>, IUserManager
	{
		public UserManager(IDatabase db, ILogger<UserEntity> log)
			: base(db, "пользователь", log)
		{
		}

		public UserEntity GetByNickName(string login)
		{
			var result = database.GetList<UserEntity>(new { Login = login }).Take(2).ToArray();
			return result.FirstOrDefault();
		}
	}
}
