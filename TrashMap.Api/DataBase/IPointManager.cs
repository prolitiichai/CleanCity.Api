using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public interface IPointManager : IIdManager<PointEntity>
	{
		PointEntity[] SearchPointsInSquare(double leftUpperLongitude, double leftUpperLatitude,
			double rightLowerLongitude, double rightLowerLatitude, int count);

		PointEntity[] SearchPointsByUser(long userId);
	}
}
