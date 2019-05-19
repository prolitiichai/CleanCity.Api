using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using Microsoft.Extensions.Logging;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public class PointManager : IdManager<PointEntity>, IPointManager
	{
		public PointManager(IDatabase db, string entityName, ILogger<PointEntity> log) : base(db, entityName, log)
		{
		}

		public PointEntity[] SearchPointsInSquare(double leftUpperLongitude, double leftUpperLatitude, double rightLowerLongitude,
			double rightLowerLatitude, int count = 100)
		{
			var predicatesList = new List<IPredicate>
			{
				Predicates.Field<PointEntity>(en => en.Latitude, Operator.Ge, leftUpperLatitude),
				Predicates.Field<PointEntity>(en => en.Latitude, Operator.Le, rightLowerLatitude),
				Predicates.Field<PointEntity>(en => en.Longitude, Operator.Ge, leftUpperLongitude),
				Predicates.Field<PointEntity>(en => en.Longitude, Operator.Le, rightLowerLongitude),
				
			};

			var predicatesFilter = new List<IPredicate>
			{
				Predicates.Field<PointEntity>(en => en.IsFixed, Operator.Eq, true),
				Predicates.Field<PointEntity>(en => en.Created, Operator.Ge,
					((DateTimeOffset) (DateTime.UtcNow.AddDays(-7))).ToUnixTimeSeconds()),
			};

			var timeFilter = new List<IPredicate>
			{
				Predicates.Group(GroupOperator.And, predicatesFilter.ToArray()),
				Predicates.Field<PointEntity>(en => en.IsFixed, Operator.Eq, false)
			};

			predicatesList.Add(Predicates.Group(GroupOperator.And, timeFilter.ToArray()));

			var queueItemsPredicate = Predicates.Group(GroupOperator.And, predicatesList.ToArray());

			return database.GetPage<PointEntity>(
					queueItemsPredicate,
					new List<ISort>
					{
						new Sort {PropertyName = nameof(PointEntity.Created), Ascending = true},
					},
					0, count)
				.ToArray();
		}

		public PointEntity[] SearchPointsByUser(long userId)
		{
			return database.GetPage<PointEntity>(
					Predicates.Field<PointEntity>(en => en.UserId, Operator.Eq, userId),
					new List<ISort>
					{
						new Sort {PropertyName = nameof(PointEntity.Created), Ascending = true},
					},
					0, 100)
				.ToArray();
		}
	}
}
