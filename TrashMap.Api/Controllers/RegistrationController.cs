using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.Entities;
using TrashMap.Api.Model;

namespace TrashMap.Api.Controllers
{
	[Route("api/register")]
	[ApiController]
	public class RegistrationController : ControllerBase
	{
		private readonly IUserManager _userManager;

		public RegistrationController(IUserManager userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
		public IActionResult Post([FromBody] RegistrationInputModel inputModel)
		{
			if (!Regex.IsMatch(inputModel.Username, "[A-Za-z0123456789\\-\\._@\\+]{3,}")
				|| inputModel.Password.Length < 6)
			{
				return BadRequest();
			}
			if (_userManager.GetByNickName(inputModel.Username) != null)
			{
				return BadRequest();
			}

			var result = _userManager.AddOrUpdate(CreateUserEntity(inputModel));

			if (result == null || result.Id <= 0)
				return StatusCode(500);
			return StatusCode(200);
		}

		private UserEntity CreateUserEntity(RegistrationInputModel model)
		{
			return new UserEntity
			{
				Login = model.Username,
				PasswordSalt = model.Password, // TODO AF: Put the salt in
			};
		}
	}
}
