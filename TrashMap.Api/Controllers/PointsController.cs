using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.Entities;

namespace TrashMap.Api.Controllers
{
	[Route("api/points")]
	[Authorize]
	[ApiController]
	public class PointsController : ControllerBase
	{
		private readonly IPointManager _pointManager;
		private readonly IUserManager _userManager;

		public PointsController(IPointManager pointManager, IUserManager userManager)
		{
			_userManager = userManager;
			_pointManager = pointManager;
		}

		[HttpPost("create")]
		public ActionResult CreatePoint([FromBody] PointCreationDTO pointCreationDTO)
		{
			var userData =
				_userManager.GetByNickName(
					HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
			if (userData == null) return StatusCode(400, "user not found");

			var result = _pointManager.AddOrUpdate(new PointEntity()
			{
				Complexity = pointCreationDTO.Complexity,
				Title = pointCreationDTO.Title,
				UserId = userData.Id,
				Created = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds(),
				Updated = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds(),
				Latitude = pointCreationDTO.Latitude,
				Longitude = pointCreationDTO.Longitude,
				IsFixed = false,
			});

			return StatusCode(200, result.Id);
		}
		
		[HttpGet("{id}")]
		public ActionResult GetPoint(long id)
		{
			var result = _pointManager.GetBy(id);
			
			return StatusCode(200, result);
		}

		[HttpGet("find/{leftUpperLatitude)/{leftUpperLongitude}/{rightLowerLatitude}/{rightLowerLongitude}")]
		public ActionResult GetPoint(double leftUpperLatitude, double leftUpperLongitude, double rightLowerLatitude, double rightLowerLongitude)
		{
			var result = _pointManager.SearchPointsInSquare(leftUpperLongitude, leftUpperLatitude, rightLowerLongitude, rightLowerLatitude, 100);

			return StatusCode(200, result);
		}

		[HttpGet("find/{username}")]
		public ActionResult GetPoint(string username)
		{
			var userData =
				_userManager.GetByNickName(
					HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
			if (userData == null) return StatusCode(400, "user not found");

			var result = _pointManager.SearchPointsByUser(userData.Id);

			return StatusCode(200, result);
		}

		public class PointCreationDTO
		{
			public string Title { get; set; }
			public int Complexity { get; set; }
			public double Latitude { get; set; }
			public double Longitude { get; set; }
		}
	}
}
