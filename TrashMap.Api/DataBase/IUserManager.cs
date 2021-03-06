﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public interface IUserManager : IIdManager<UserEntity>
	{
		UserEntity GetByNickName(string user);
	}
}
