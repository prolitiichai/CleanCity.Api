using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.DataBase.Creation
{
	public interface IDbCreator
	{
		bool DatabaseExists();
		void CreateEmptyDB();
		void Drop();
	}
}
