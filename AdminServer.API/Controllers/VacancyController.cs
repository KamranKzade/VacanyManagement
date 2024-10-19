using AdminServer.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using AdminServer.API.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;

namespace AdminServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VacancyController : CustomBaseController
{
    private readonly IVacancyService _vacancyService;

    public VacancyController(IVacancyService vacancyService)
    {
        _vacancyService = vacancyService;
    }


	[Authorize(Roles = "admin")]
	[HttpGet("getAll")]
    public async Task<IActionResult> GetAllVacanciesAsync()
    {
        var restult = await _vacancyService.GetAllVacanciesAsync();
        return ActionResultInstance(restult);
    }

	[Authorize(Roles = "admin")]
	[HttpGet("getById")]
    public async Task<IActionResult> GetVacancyById([FromQuery] string id)
    {
        var restult = await _vacancyService.GetVacancyAsync(id);
        return ActionResultInstance(restult);
    }

	[Authorize(Roles = "admin")]
	[HttpGet("getByCategory")]
    public async Task<IActionResult> GetVacancyByCategory([FromQuery] string categoryId)
    {
        var restult = await _vacancyService.GetVacanciesByCategoryAsync(categoryId);
        return ActionResultInstance(restult);
    }

	[Authorize(Roles = "admin")]
	[HttpPost("create")]
    public async Task<IActionResult> CreateVacancyAsync(CreateVacancyDto newVacancy)
    {
        var restult = await _vacancyService.CreateVacanyAsync(newVacancy);
        return ActionResultInstance(restult);
    }

	[Authorize(Roles = "admin")]
	[HttpPut("update")]
    public async Task<IActionResult> UpdateVacancyAsync(UpdatedVacancyDto updatedVacancy)
    {
        var result = await _vacancyService.UpdateVacancyAsync(updatedVacancy);
        return ActionResultInstance(result);
    }

	[Authorize(Roles = "admin")]
	[HttpDelete("remove")]
    public async Task<IActionResult> DeleteVacancyAsync([FromQuery] string id)
    {
        var result = await _vacancyService.RemoveVacancyAsync(id);
        return ActionResultInstance(result);
    }
}
