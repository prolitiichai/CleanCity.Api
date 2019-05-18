using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.Settings
{
	public class DbSettings : IDbSettings
	{
		public string DataBasePath { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
	}
}
