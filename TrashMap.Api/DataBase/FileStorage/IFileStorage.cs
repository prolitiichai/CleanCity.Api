using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.DataBase.FileStorage
{
	public interface IFileStorage
	{
		string SaveData(byte[] data);
		byte[] LoadData(string filePath);
	}
}
