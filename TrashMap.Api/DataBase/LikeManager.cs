using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using Microsoft.Extensions.Logging;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public class LikeManager : IdManager<LikeEntity>, ILikeManager
	{
		public LikeManager(IDatabase db, string entityName, ILogger<LikeEntity> log) : base(db, entityName, log)
		{
		}
	}
}
