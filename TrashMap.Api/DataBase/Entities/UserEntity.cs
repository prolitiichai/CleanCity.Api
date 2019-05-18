using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase.Entities
{
	public class UserEntity : IEntity, IHasId
	{
		public long Id { get; set; }
		public string AvatarPath { get; set; }
		public string Login { get; set; }
		public string PasswordSalt { get; set; }
		public string NickName { get; set; }
		public int TrashFound { get; set; }
		public int TrashCleaned { get; set; }
		public int Karma { get; set; }
	}
}
