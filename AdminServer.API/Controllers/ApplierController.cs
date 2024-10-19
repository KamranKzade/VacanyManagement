using AdminServer.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using AdminServer.API.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;

namespace AdminServer.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplierController : CustomBaseController
	{
		private readonly IApplierService _applierService;

		public ApplierController(IApplierService applierService)
		{
			_applierService = applierService;
		}

		[HttpPost]
		[Authorize()]
		public async Task<IActionResult> Post(CreateApplierDto createApplierDto)
		{
			var result = await _applierService.CreateApplierAsync(createApplierDto);
			return ActionResultInstance(result);
		}

		[Authorize(Roles = "admin")]
		[HttpGet("downloadFile")]
		public async Task<IActionResult> DownloadFile([FromQuery] DownloadFileRequestDto dto)
		{
			var result = await _applierService.DownloadFileAsync(dto);
			return File(result.Data.File, result.Data.ContentType, result.Data.FileName);
		}

		[Authorize(Roles = "admin")]
		[HttpGet("getAllAppliers")]
		public async Task<IActionResult> GetAllApplier()
		{
			var result = await _applierService.GetAllAppliersAsync();
			return ActionResultInstance(result);
		}
	}
}
