using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.DataBase.Providers
{
	public interface IDbConnectionProviderStorage
	{
		IDbConnectionProvider GetProvider(string databasePart);
	}
}
