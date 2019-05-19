using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using Microsoft.Extensions.Logging;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public class PointCommentEntityManager : IdManager<PointCommentEntity>, IPointCommentEntityManager
	{
		public PointCommentEntityManager(IDatabase db, ILogger<PointCommentEntity> log) : base(db, "pointCommentEntityManager", log)
		{
		}

		public PointCommentEntity[] GetPointComments(long pointId)
		{
			return database.GetPage<PointCommentEntity>(
				Predicates.Field<PointCommentEntity>(en => en.PointId, Operator.Eq, pointId),
				new List<ISort>
				{
					new Sort {PropertyName = nameof(PointCommentEntity.Created), Ascending = false},
				}, 0, 100).ToArray();
		}
	}
}
