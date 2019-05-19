using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrashMap.Api.DataBase;
using TrashMap.Api.DataBase.FileStorage;

namespace TrashMap.Api.Controllers
{
	[Route("api/photos")]
	[ApiController]
	[AllowAnonymous]
	public class PhotosController : ControllerBase
	{
		private readonly IFileStorage _fileStorage;

		public PhotosController(IFileStorage fileStorage)
		{
			_fileStorage = fileStorage;
		}

		[HttpGet("{fileName}")]
		public async Task<ActionResult> GetPhoto(string fileName)
		{
			return new FileContentResult(_fileStorage.LoadData(fileName),
				"image/jpeg");
		}
	}
}
