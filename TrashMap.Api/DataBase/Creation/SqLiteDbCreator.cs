using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrashMap.Api.DataBase.Providers;
using TrashMap.Api.Settings;

namespace TrashMap.Api.DataBase.Creation
{
	public class SQLiteDBCreator : BaseDBCreator
	{
		public SQLiteDBCreator(IDbSettings dbSettings, ILogger log)
			: base(dbSettings)
		{
			this.log = log;
		}

		public override bool DatabaseExists()
		{
			var fullDbPath = SQLiteConnectionProvider.GetFullDBPath(Settings);
			return File.Exists(fullDbPath);
		}

		public override void CreateEmptyDB()
		{
			var databaseLocation = SQLiteConnectionProvider.GetFullDBPath(Settings);
			log.LogDebug($"Создание пустой базы данных ({databaseLocation})");
			var directoryPath = Path.GetDirectoryName(databaseLocation);
			if (!string.IsNullOrWhiteSpace(directoryPath))
				Directory.CreateDirectory(directoryPath);
			SQLiteConnection.CreateFile(databaseLocation);
			var createCommandText = GetResourceAsString(GetType(), GetAppropriateCreateScript(Settings));
			using (var sqliteConnection = SQLiteConnectionProvider.GetConnectionFromSettings(Settings))
			{
				sqliteConnection.Execute(createCommandText);
			}
		}

		public override void Drop()
		{
			var databaseLocation = SQLiteConnectionProvider.GetFullDBPath(Settings);
			log.LogDebug($"Удаление базы данных ({databaseLocation})");
			if (!File.Exists(databaseLocation))
				return;
			File.Delete(databaseLocation);
		}

		private static string GetAppropriateCreateScript(IDbSettings settings)
		{
			return DbScripts[settings.Name];
		}

		public static Stream GetResource(Assembly assembly, string name)
		{
			var resources = assembly.GetManifestResourceNames();
			var matchedResource = resources.Where(r => r.EndsWith(name, StringComparison.InvariantCultureIgnoreCase));
			return assembly.GetManifestResourceStream(matchedResource.First());
		}

		public static Stream GetResource(Type type, string name)
		{
			var assembly = Assembly.GetAssembly(type);
			return GetResource(assembly, name);
		}

		public static string GetResourceAsString(Type type, string name)
		{
			var resourceStream = GetResource(type, name);
			return ToUTF8String(resourceStream);
		}

		public static string ToUTF8String(Stream stream)
		{
			var builder = new StringBuilder();
			using (var streamReader = new StreamReader(stream, new UTF8Encoding(true), true, 4096))
			{
				while (!streamReader.EndOfStream)
				{
					var line = streamReader.ReadLine();
					builder.AppendLine(line);
				}
			}
			return builder.ToString();
		}

		private readonly ILogger log;

		private static readonly Dictionary<string, string> DbScripts = new Dictionary<string, string>()
		{
			{ "main", "DataBase.Creation.Scripts.Create_Main.sql" }
		};
	}
}
