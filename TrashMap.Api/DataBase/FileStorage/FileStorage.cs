using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.DataBase.FileStorage
{
	public class FileStorage : IFileStorage
	{
		private readonly string _root;

		public FileStorage(string root)
		{
			_root = root;
			Directory.CreateDirectory(_root);
		}

		public string SaveData(byte[] data)
		{
			var fileName = Guid.NewGuid().ToString()+".jpg";
			var filePath = Path.Combine(_root, fileName);
			File.WriteAllBytes(filePath, data);

			return fileName;
		}

		public byte[] LoadData(string fileName)
		{
			var filePath = Path.Combine(_root, fileName);
			return File.ReadAllBytes(filePath);
		}
	}
}
