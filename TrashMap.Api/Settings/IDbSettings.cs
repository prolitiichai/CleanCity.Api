using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashMap.Api.Settings
{
	public interface IDbSettings
	{
		string DataBasePath { get; set; }
		string Password { get; set; }
		string Name { get; set; }
	}
}
