using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.Settings;

namespace TrashMap.Api.DataBase.Creation
{
	public abstract class BaseDBCreator : IDbCreator
	{
		protected BaseDBCreator(IDbSettings dbSettings)
		{
			Settings = dbSettings;
		}

		public abstract bool DatabaseExists();

		public abstract void CreateEmptyDB();

		public abstract void Drop();

		protected IDbSettings Settings { get; }
	}
}
