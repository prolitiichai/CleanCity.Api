using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.DataBase.Providers
{
	public class DBConnectionProviderStorage : IDbConnectionProviderStorage
	{
		public DBConnectionProviderStorage(IEnumerable<IDbConnectionProvider> providers)
		{
			this.providers = providers.ToDictionary(provider => provider.Settings.Name);
		}

		public IDbConnectionProvider GetProvider(string databaseName)
		{
			return providers[databaseName];
		}

		private readonly Dictionary<string, IDbConnectionProvider> providers;
	}
}
