using UserServer.API.Dto;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using UserServer.API.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using UserServer.API.Dtos;

namespace UserServer.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplierContoller : CustomBaseController
	{
		private readonly IApplierService _applierService;

		public ApplierContoller(IApplierService applierService)
		{
			_applierService = applierService;
		}

		[Authorize()]
		[HttpPost("create")]
		public async Task<IActionResult> CreateApplierAsync(CreateApplierDto newApplier)
		{
			string authorizationToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			var result = await _applierService.CreateApplierAsync(newApplier, authorizationToken);
			return ActionResultInstance(result);
		}


		[HttpGet("getById")]
		public async Task<IActionResult> GetById([FromQuery] string id)
		{
			var result = await _applierService.GetApplierAsync(id);
			return ActionResultInstance(result);
		}

		[Authorize(Roles = "admin")]
		[HttpGet("getAllAppliers")]
		public async Task<IActionResult> GetAllApplier()
		{
			var result = await _applierService.GetAllAppliersAsync();
			return ActionResultInstance(result);
		}

		[Authorize()]
		[HttpPost("attachFile")]
		public async Task<IActionResult> UploadFile([FromForm] AttachFileDto fileDto)
		{
			string authorizationToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			var result = await _applierService.AttachFileAsync(fileDto, authorizationToken);
			return ActionResultInstance(result);
		}

		[Authorize(Roles = "admin")]
		[HttpGet("downloadFile")]
		public async Task<IActionResult> DownloadFile([FromQuery] DownloadFileRequestDto dto)
		{
			var result = await _applierService.DownloadFileAsync(dto);
			return File(result.Data.File, result.Data.ContentType, result.Data.FileName);
		}

		[Authorize()]
		[HttpGet("GetQuestionByCategoryRandomAsync")]
		public async Task<IActionResult> GetQuestionByCategoryRandomAsync([FromQuery] string categoryId, int questionCount)
		{
			string authorizationToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			return ActionResultInstance(await _applierService.GetQuestionByCategoryRandomAsync(categoryId, questionCount, authorizationToken));
		}


		[Authorize()]
		[HttpPost("SendAnswerToAdminServer")]
		public async Task<IActionResult> SendAnswerToAdminServer([FromBody]SendAnswerDto dto)
		{
			string authorizationToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

			var result = await _applierService.SendAnswerAsync(dto, authorizationToken);
			return ActionResultInstance(result);
		}
	}
}
