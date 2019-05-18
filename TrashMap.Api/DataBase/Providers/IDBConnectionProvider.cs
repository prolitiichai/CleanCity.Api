using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.Settings;

namespace TrashMap.Api.DataBase.Providers
{
	public interface IDbConnectionProvider
	{
		IDbSettings Settings { get; }
		IDbConnection CreateConnection();
	}
}
