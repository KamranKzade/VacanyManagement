using AdminServer.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using AdminServer.API.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;

namespace AdminServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionController : CustomBaseController
{
	private readonly IQuestionService _questionService;

	public QuestionController(IQuestionService questionService)
	{
		_questionService = questionService;
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getAll")]
	public async Task<IActionResult> GetAllQuestionsAsync()
	{
		var restult = await _questionService.GetAllQuestionsAsync();
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getById")]
	public async Task<IActionResult> GetQuestionById([FromQuery] string id)
	{
		var restult = await _questionService.GetQuestionAsync(id);
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getByCategory")]
	public async Task<IActionResult> GetQuestionsByCategory([FromQuery] string categoryId)
	{
		var restult = await _questionService.GetQuestionsByCategoryAsync(categoryId);
		return ActionResultInstance(restult);
	}

	[Authorize()]
	[HttpGet("getRandomByCategory")]
	public async Task<IActionResult> GetRandomQuestionsByCategory([FromQuery] string categoryId, int questionCount)
	{
		var restult = await _questionService.GetQuestionsByCategoryRandomAsync(categoryId, questionCount);
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpPost("create")]
	public async Task<IActionResult> CreateQuestionAsync(CreateQuestionDto newQuestion)
	{
		var restult = await _questionService.CreateQuestionAsync(newQuestion);
		return ActionResultInstance(restult);
	}

	[Authorize(Roles = "admin")]
	[HttpPut("update")]
	public async Task<IActionResult> UpdateQuestionAsync(UpdatedQuestionDto updatedQuestion)
	{
		var result = await _questionService.UpdateQuestionAsync(updatedQuestion);
		return ActionResultInstance(result);
	}

	[Authorize(Roles = "admin")]
	[HttpDelete("remove")]
	public async Task<IActionResult> DeleteQuestionAsync([FromQuery] string id)
	{
		var result = await _questionService.RemoveQuestionAsync(id);
		return ActionResultInstance(result);
	}
}
