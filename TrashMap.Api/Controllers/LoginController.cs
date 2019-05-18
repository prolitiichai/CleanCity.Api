using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
		public IActionResult Post([FromQuery] string login, [FromQuery] string password)
		{
			//if (inputModel != null && inputModel.Username != null)
			//	return Login(inputModel.Username, inputModel.Password);
			return Login(login, password);
		}

		[HttpGet]
		public IActionResult Get([FromQuery] string login, [FromQuery] string password)
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
