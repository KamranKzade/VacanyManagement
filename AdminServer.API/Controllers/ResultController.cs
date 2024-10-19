using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using AdminServer.API.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using AdminServer.API.Dtos;

namespace AdminServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResultController : CustomBaseController
{
	private readonly IResultService _resultService;

	public ResultController(IResultService resultService)
	{
		_resultService = resultService;
	}

	[HttpGet]
	[Authorize()]
	public async Task<IActionResult> Get([FromBody]AnswerForResultDto dto)
	{
		var result = await _resultService.GetResultByAppierId(dto);
		return ActionResultInstance(result);
	}

	[HttpGet("getAllResult")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> GetAllResult()
	{
		var result = await _resultService.GetAllResult();
		return ActionResultInstance(result);
	}

}
