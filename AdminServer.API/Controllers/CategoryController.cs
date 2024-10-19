using AdminServer.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using AdminServer.API.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;

namespace AdminServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : CustomBaseController
{

	private readonly ICategoryService _categoryService;

	public CategoryController(ICategoryService categoryService)
	{
		_categoryService = categoryService;
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getById")]
	public async Task<IActionResult> GetCategoryBydId([FromQuery] string id)
	{
		var result = await _categoryService.GetCategoryAsync(id);
		return ActionResultInstance(result);
	}

	[Authorize(Roles = "admin")]
	[HttpGet("getAll")]
	public async Task<IActionResult> GetAllCategories()
	{
		var result = await _categoryService.GetAllCategoriesAsync();
		return ActionResultInstance(result);
	}

	[Authorize(Roles = "admin")]
	[HttpPost("create")]
	public async Task<IActionResult> CreateCategoryAsync(CreateCategoryDto newCategory)
	{
		var result = await _categoryService.CreateCategoryAsync(newCategory);
		return ActionResultInstance(result);
	}

	[Authorize(Roles = "admin")]
	[HttpPut("update")]
	public async Task<IActionResult> UpdateCategoryAsync(UpdatedCategoryDto updatedCategory)
	{
		var result = await _categoryService.UpdateCategoryAsync(updatedCategory);
		return ActionResultInstance(result);
	}

	[Authorize(Roles = "admin")]
	[HttpDelete("remove")]
	public async Task<IActionResult> DeleteCategoryAsync([FromQuery] string id)
	{
		var result = await _categoryService.RemoveCategoryAsync(id);
		return ActionResultInstance(result);
	}
}
