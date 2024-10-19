using AdminServer.API.Dtos;
using SharedLibrary.Dtos;
using SharedLibrary.Models;

namespace AdminServer.API.Services.Abstracts;

public interface IVacancyService
{
    Task<Response<IEnumerable<Vacancy>>> GetAllVacanciesAsync();
    Task<Response<IEnumerable<Vacancy>>> GetVacanciesByCategoryAsync(string categoryId);
    Task<Response<Vacancy>> GetVacancyAsync(string id);
    Task<Response<NoDataDto>> RemoveVacancyAsync(string id);
    Task<Response<NoDataDto>> CreateVacanyAsync(CreateVacancyDto newVacancy);
    Task<Response<UpdatedVacancyDto>> UpdateVacancyAsync(UpdatedVacancyDto updatedVacancy);
    Task<Response<UpdatedVacancyDto>> ChangeVacancyStatusAsync(string id);
    Task<Response<IEnumerable<Vacancy>>> GetActiveVacanciesAsync();
}