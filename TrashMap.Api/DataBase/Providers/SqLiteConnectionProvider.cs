using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SQLite;
using TrashMap.Api.Settings;

namespace TrashMap.Api.DataBase.Providers
{
	public class SQLiteConnectionProvider : DBConnectionProvider
	{
		public SQLiteConnectionProvider(IDbSettings dbSettings, ILogger log)
			: base(dbSettings, log)
		{
		}

		public static string ConnectionStringFromSettings(IDbSettings settings)
		{
			var builder = new SQLiteConnectionStringBuilder
			{
				DataSource = settings.DataBasePath,
				Version = 3,
				ForeignKeys = true,
				DateTimeKind = DateTimeKind.Utc
			};
			return builder.ToString();
		}

		public static string GetFullDBPath(IDbSettings settings)
		{
			return settings.DataBasePath;
		}

		public static IDbConnection GetConnectionFromSettings(IDbSettings settings)
		{
			var connectionString = ConnectionStringFromSettings(settings);
			return new SQLiteConnection(connectionString);
		}

		protected override IDbConnection GetConnection()
		{
			return GetConnectionFromSettings(Settings);
		}

		protected override string GetConnectionString()
		{
			return ConnectionStringFromSettings(Settings);
		}
	}
}
