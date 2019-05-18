using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase.SqlWrapping
{
	public class WrappedDbConnection : IWrappedDbConnecion
	{
		public WrappedDbConnection(IDatabase db, Type entityType)
		{
			this.db = db;
			this.entityType = entityType;
		}

		public T Get<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return Execute(() => db.Connection.Get<T>(id, transaction, commandTimeout));
		}

		public IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
		{
			return Execute(() => db.Connection.GetList<T>(predicate, sort, transaction, commandTimeout, buffered));
		}

		public IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class
		{
			return Execute(() => db.Connection.GetPage<T>(predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered));
		}

		public dynamic Insert<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return Execute(() => db.Connection.Insert(entity, transaction, commandTimeout));
		}

		public bool Update<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return Execute(() => db.Connection.Update(entity, transaction, commandTimeout));
		}

		public bool Delete<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return Execute(() => db.Connection.Delete(entity, transaction, commandTimeout));
		}

		public bool Delete<T>(object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return Execute(() => db.Connection.Delete<T>(predicate, transaction, commandTimeout));
		}

		public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			return Execute(() => db.Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType));
		}

		public IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			return Execute(() => db.Connection.Query(sql, param, transaction, buffered, commandTimeout, commandType));
		}

		public int Count<T>(object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
		{
			return Execute(() => db.Connection.Count<T>(predicate, transaction, commandTimeout));
		}

		public const int DefaultIdsChunkLength = 200;

		private IEnumerable<T> GetListByIdInternal<T>(IEnumerable<long> ids, IDbTransaction transaction = null, int? commandTimeout = null)
			where T : class, IHasId
		{
			var idsPredicate = Predicates.Field<T>(t => t.Id, Operator.Eq, ids);
			return GetList<T>(idsPredicate, null, transaction, commandTimeout);
		}

		private TResult Execute<TResult>(Func<TResult> func)
		{
			return func();
		}

		private readonly IDatabase db;
		private readonly Type entityType;
	}
}