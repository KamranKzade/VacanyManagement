using AdminServer.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using Microsoft.AspNetCore.Authorization;
using AdminServer.API.Services.Abstracts;

namespace AdminServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswerController : CustomBaseController
{
	private readonly IAnswerService _answerService;

	public AnswerController(IAnswerService answerService)
	{
		_answerService = answerService;
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getById")]
	public async Task<IActionResult> GetAnswerById([FromQuery] string id)
	{
		var restult = await _answerService.GetAnswerAsync(id);
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getByQuestionId")]
	public async Task<IActionResult> GetAnswersByQuestion([FromQuery] string questionId)
	{
		var restult = await _answerService.GetAnswersByQuestionIdAsync(questionId);
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpPost("create")]
	public async Task<IActionResult> CreateAnswerAsync(CreateAnswerDto newAnswer)
	{
		var restult = await _answerService.CreateAnswerAsync(newAnswer);
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpPut("update")]
	public async Task<IActionResult> UpdateAnswerAsync(UpdatedAnswerDto updatedAnswer)
	{
		var result = await _answerService.UpdateAnswerAsync(updatedAnswer);
		return ActionResultInstance(result);
	}

	[Authorize(Roles = "admin")]
	[HttpDelete("remove")]
	public async Task<IActionResult> DeleteAnswerAsync([FromQuery] string id)
	{
		var result = await _answerService.RemoveAnswerAsync(id);
		return ActionResultInstance(result);
	}
}
