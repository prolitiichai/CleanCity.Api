using System.Collections.Generic;
using TrashMap.Api.DataBase.Entities.Interfaces;

namespace TrashMap.Api.DataBase
{
	public interface IIdManager<THasId> : IIdReadManager<THasId>, IIdWriteManager<THasId>
		where THasId : IHasId
	{
	}

	public interface IIdReadManager<out THasId> where THasId : IHasId
	{
		THasId FindBy(long id);
		THasId GetBy(long id);
		IEnumerable<THasId> GetAll();
		long Count();
	}

	public interface IIdWriteManager<THasId> where THasId : IHasId
	{
		long Add(THasId hasId);
		void Update(THasId hasId);
		THasId AddOrUpdate(THasId hasId);
		void Delete(THasId hasId);
		void DeleteAll();
	}
}