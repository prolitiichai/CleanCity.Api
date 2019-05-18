using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase.SqlWrapping
{
	public interface IWrappedDbConnecion
	{
		T Get<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;
		IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class;
		IEnumerable<T> GetPage<T>(object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false) where T : class;
		dynamic Insert<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;
		bool Update<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;
		bool Delete<T>(T entity, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;
		bool Delete<T>(object predicate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;
		IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
		IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
		int Count<T>(object predicate = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class;
	}
}