using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.Entities;
using TrashMap.Api.Model;

namespace TrashMap.Api.Controllers
{
	[Route("api/registration")]
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
			if (inputModel.Username == null || inputModel.Password == null)
			{
				return BadRequest("Username is empty");
			}

			if (!Regex.IsMatch(inputModel.Username, "[A-Za-z0123456789\\-\\._@\\+]{3,}"))
			{
				return BadRequest("Username should conains only letters or numbers or _@.+.");
			}
			
			if (inputModel.Password.Length <= 6)
			{
				return BadRequest("Password should contains more than 6 symbols");
			}
			
			if (_userManager.GetByNickName(inputModel.Username) != null)
			{
				return BadRequest("Username is already exists");
			}

			var result = _userManager.AddOrUpdate(CreateUserEntity(inputModel));

			if (result == null || result.Id <= 0)
				return StatusCode(500);

			return Login(inputModel.Username, inputModel.Password);
		}

		private UserEntity CreateUserEntity(RegistrationInputModel model)
		{
			return new UserEntity
			{
				Login = model.Username,
				PasswordSalt = model.Password, // TODO AF: Put the salt in
			};
		}

		private IActionResult Login(string login, string password) // TODO AF: Вынести в отдельный класс
		{
			var userData = _userManager.GetByNickName(login);

			if (userData == null || userData.PasswordSalt != password)// TODO AF: Support salt
			{
				return StatusCode((int)HttpStatusCode.Unauthorized);
			}

			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, login),
				new Claim(ClaimTypes.NameIdentifier, userData.Id.ToString())
			};

			var identity = new ClaimsIdentity(claims, "Cookies");
			identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
			var principal = new ClaimsPrincipal(identity);
			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
			{
				IsPersistent = true,
				ExpiresUtc = DateTime.UtcNow.AddYears(1)
			}).Wait();

			return StatusCode(200);
		}
	}
}
