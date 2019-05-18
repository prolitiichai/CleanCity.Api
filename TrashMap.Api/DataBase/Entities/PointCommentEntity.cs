using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase.Entities
{
	public class PointCommentEntity : IEntity, IHasId
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public string PhotoPath { get; set; }
		public bool PointStatus { get; set; }
		public int PlusCount { get; set; }
		public int MinusCount { get; set; }
	}
}
