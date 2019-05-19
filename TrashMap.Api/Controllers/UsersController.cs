using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.FileStorage;

namespace TrashMap.Api.Controllers
{
	[Route("api/users")]
	[Authorize]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserManager _userManager;
		private readonly IFileStorage _fileStorage;

		public UsersController(IUserManager userManager, IFileStorage fileStorage)
		{
			_userManager = userManager;
			_fileStorage = fileStorage;
		}

		[HttpPost("avatar")]
		public ActionResult<string> PostAvatar([FromBody] byte[] file)
		{
			var userData =
				_userManager.GetByNickName(
					HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
			if (userData == null) return StatusCode(400, "user not found");
			var avatarPath = _fileStorage.SaveData(file);
			userData.AvatarPath = avatarPath;
			_userManager.Update(userData);
			return new ActionResult<string>(avatarPath);
		}

	}
}
