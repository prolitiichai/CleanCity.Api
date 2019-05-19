using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrashMap.Api.DataBase;

namespace TrashMap.Api.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserManager _userManager;

		public UsersController(IUserManager userManager)
		{
			_userManager = userManager;
		}


	}
}
