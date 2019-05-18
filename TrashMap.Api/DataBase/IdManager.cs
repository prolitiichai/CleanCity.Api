using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrashMap.Api.DataBase.Entities.Interfaces;
using TrashMap.Api.DataBase.SqlWrapping;

namespace TrashMap.Api.DataBase
{
	public abstract class IdManager<THasId> : IIdManager<THasId> where THasId : class, IHasId
	{
		protected IdManager(IDatabase db, string entityName, ILogger<THasId> log)
		{
			this.db = db;
			this.entityName = entityName;
			this.log = log;
			database = new WrappedDbConnection(db, typeof(THasId));
		}

		public long Add(THasId hasId)
		{
			return InsertInternal(hasId).Id;
		}

		public void Update(THasId hasId)
		{
			log.LogDebug($"{typeof(THasId).Name} Update id = '{hasId.Id}'");
			database.Update(hasId);
		}

		public THasId AddOrUpdate(THasId hasId)
		{
			var existingEntityObject = db.Connection.Get<THasId>(hasId.Id);

			if (existingEntityObject != null)
				database.Update(hasId);
			else
				InsertInternal(hasId);
			return hasId;
		}

		public void Delete(THasId hasId)
		{
			log.LogDebug($"{typeof(THasId).Name} Delete id = '{hasId.Id}'");
			database.Delete(hasId);
		}

		public void DeleteAll()
		{
			log.LogDebug($"{typeof(THasId).Name} Delete all");
			database.Delete<THasId>(new { });
		}

		public THasId FindBy(long id)
		{
			log.LogDebug($"{typeof(THasId).Name} FindBy id = '{id}'");
			return database.Get<THasId>(id);
		}

		public THasId GetBy(long id)
		{
			log.LogDebug($"{typeof(THasId).Name} GetBy id = '{id}'");
			var entity = database.Get<THasId>(id);
			if (entity == null)
				throw new System.Exception($"Не найден {entityName} с идентификатором id = '{id}'");
			return entity;
		}

		public IEnumerable<THasId> GetAll()
		{
			log.LogDebug($"GetAll {typeof(THasId).Name} items");
			return database.GetList<THasId>();
		}

		public long Count()
		{
			log.LogDebug($"Count of {typeof(THasId).Name} items");
			return database.Count<THasId>();
		}

		// TODO ponkin. Подумать над более универсальным и элегантным решением
		protected THasId FindBy(string column, string value, object whereConditions)
		{
			log.LogDebug($"{typeof(THasId).Name} FindBy '{column}' = '{value}'");
			var result = database
				.GetList<THasId>(whereConditions)
				.OrderBy(item => item.Id)
				.Take(2)
				.ToArray();
			if (result.Length > 1)
				log.LogError($"Найдено более одного {entityName} с '{column}' = '{value}'");
			return result.FirstOrDefault();
		}

		private THasId InsertInternal(THasId hasId)
		{
			// DapperExtensions внутри метода Insert() для получения ID использует небезопасный "SELECT LAST_INSERT_ROWID() AS [Id]",
			// который при одновременной вставке в разные таблицы может даже вернуть ROW_ID для другой таблицы
			// и такие прецеденты уже были :-/
			dynamic id;
			lock (IdManagerLock.Insert)
			{
				id = database.Insert(hasId);
			}
			if (id == null)
				throw new System.Exception($"Can't save {typeof(THasId).Name}. Insert operation returned 'null' primary key.");
			hasId.Id = id;
			log.LogDebug($"{typeof(THasId).Name} saved with id '{hasId.Id}'");
			return hasId;
		}

		private readonly IDatabase db;

		protected readonly IWrappedDbConnecion database;
		protected readonly string entityName;
		protected readonly ILogger log;
	}

	internal static class IdManagerLock
	{
		public static object Insert = new object();
	}
}