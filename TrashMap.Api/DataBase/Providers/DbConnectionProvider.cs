using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TrashMap.Api.Settings;

namespace TrashMap.Api.DataBase.Providers
{
	public abstract class DBConnectionProvider : IDbConnectionProvider
	{
		protected DBConnectionProvider(IDbSettings settings, ILogger log)
		{
			Settings = settings;
			this.log = log;
		}

		public IDbConnection CreateConnection()
		{
			log.LogDebug($"Создаем подключение к SQLite, используя connectionString={ConnectionString}");
			try
			{
				return GetConnection();
			}
			catch (Exception ex)
			{
				var message = $"Не удалось создать подключение к SQLite, используя connectionString = '{ConnectionString}'";
				log.LogError(message, ex);
				throw new Exception(message, ex);
			}
		}

		protected abstract IDbConnection GetConnection();

		protected abstract string GetConnectionString();

		public IDbSettings Settings { get; }

		protected string ConnectionString => string.IsNullOrEmpty(connectionString) ? connectionString = GetConnectionString() : connectionString;

		protected readonly ILogger log;
		private string connectionString;
	}
}
