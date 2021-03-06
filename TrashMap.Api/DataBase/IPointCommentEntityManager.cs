﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.DataBase
{
	public interface IPointCommentEntityManager : IIdManager<PointCommentEntity>
	{
		PointCommentEntity[] GetPointComments(long pointId);
	}
}
