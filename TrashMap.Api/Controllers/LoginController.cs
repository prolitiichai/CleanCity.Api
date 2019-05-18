using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TrashMap.Api.DataBase;
using TrashMap.Api.Model;

namespace TrashMap.Api.Controllers
{
	[Route("api/login")]
	[ApiController]
	[AllowAnonymous]
	public class LoginController : ControllerBase
	{
		private readonly IUserManager _userManager;

		public LoginController(IUserManager userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
		public IActionResult Post([FromBody] RegistrationInputModel inputModel)
		{
			return Login(inputModel.Username, inputModel.Password);
		}

		[HttpPost]
		public IActionResult PostBody(string login, string password)
		{
			return Login(login, password);
		}

		private IActionResult Login(string login, string password)
		{
			var userData = _userManager.GetByNickName(login);

			if (userData == null || userData.PasswordSalt != password)// TODO AF: Support salt
			{
				return StatusCode((int) HttpStatusCode.Unauthorized);
			}

			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, login),
				new Claim(ClaimTypes.Role, "user")
			};

			HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
			
			return StatusCode(200);
		}
	}
}
