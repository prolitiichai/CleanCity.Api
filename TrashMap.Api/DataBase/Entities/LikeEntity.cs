using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase.Entities
{
	public class LikeEntity : IEntity, IHasId
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public long PointCommentEntityId { get; set; }
	}
}
