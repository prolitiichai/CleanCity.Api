using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.Entities;
using TrashMap.Api.DataBase.FileStorage;

namespace TrashMap.Api.Controllers
{
	[Route("api/points")]
	[Authorize]
	[ApiController]
	public class PointCommentController : ControllerBase
	{
		private readonly IFileStorage _fileStorage;
		private readonly IPointManager _pointManager;
		private readonly IUserManager _userManager;
		private readonly IPointCommentEntityManager _pointCommentEntityManager;

		public PointCommentController(IFileStorage fileStorage, IPointManager pointManager, IUserManager userManager, IPointCommentEntityManager pointCommentEntityManager)
		{
			_fileStorage = fileStorage;
			_userManager = userManager;
			_pointManager = pointManager;
			_pointCommentEntityManager = pointCommentEntityManager;
		}

		[HttpPost("{id}/comment-photo")]
		public ActionResult<string> PostPhoto(long id, [FromBody] byte[] file)
		{
			var userData =
				_userManager.GetByNickName(
					HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
			if (userData == null) return StatusCode(400, "user not found");
			var photoPath = _fileStorage.SaveData(file);
			return new ActionResult<string>(photoPath);
		}

		[HttpPost("{id}/comment")]
		public ActionResult CreateComment(long id, [FromBody] CreatePointCommentDTO creationDto)
		{
			var userData =
				_userManager.GetByNickName(
					HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
			if (userData == null) return StatusCode(400, "user not found");
			var pointData = _pointManager.FindBy(id);
			
			var pointComment = new PointCommentEntity()
			{
				Created = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds(),
				MinusCount = 0,
				PhotoPath = creationDto.PhotoPath,
				PlusCount = 0,
				PointId = id,
				PointStatus = creationDto.IsFixed,
				UserId = userData.Id,
			};

			if (pointData.IsFixed != pointComment.PointStatus)
			{
				pointData.IsFixed = pointComment.PointStatus;
			}

			if (pointData.IsFixed)
			{
				userData.TrashCleaned += 1;
			}
			else
			{
				userData.TrashFound += 1;
			}
			_userManager.Update(userData);

			pointComment = _pointCommentEntityManager.AddOrUpdate(pointComment);

			pointData.Updated = ((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds();
			_pointManager.Update(pointData);
			return StatusCode(200, pointComment.Id);
		}

		[HttpGet("{id}/comments")]
		public ActionResult GetComments(long id)
		{
			var userData =
				_userManager.GetByNickName(
					HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
			if (userData == null) return StatusCode(400, "user not found");
			var pointData = _pointManager.FindBy(id);

			var result = _pointCommentEntityManager.GetPointComments(id);

			return StatusCode(200, result);
		}

		public class CreatePointCommentDTO
		{
			public string PhotoPath { get; set; }
			public bool IsFixed { get; set; }
		}
	}
}
