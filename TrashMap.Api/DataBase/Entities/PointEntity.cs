using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase.Entities
{
	public class PointEntity : IEntity, IHasId
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public long UserId { get; set; }
		public long Created { get; set; }
		public long Updated { get; set; }
		public int Complexity { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}
}
