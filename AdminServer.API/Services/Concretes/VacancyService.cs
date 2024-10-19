using AdminServer.API.Dtos;
using AdminServer.API.Models;
using AdminServer.API.Services.Abstracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using SharedLibrary.Repositories.Abstract;
using SharedLibrary.UnitOfWork.Abstract;

namespace AdminServer.API.Services.Concretes;

public class VacancyService : IVacancyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<AppDbContext, Vacancy> _vacancyiesRepository;
    private readonly ILogger<VacancyService> _logger;
    private readonly IMapper _mapper;

    public VacancyService(IUnitOfWork unitOfWork, IGenericRepository<AppDbContext, Vacancy> vacancyiesRepositoryy, ILogger<VacancyService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _vacancyiesRepository = vacancyiesRepositoryy;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Response<UpdatedVacancyDto>> ChangeVacancyStatusAsync(string id)
    {
        var vacancy = await _vacancyiesRepository.GetByIdAsync(id);
        if (vacancy is not null)
        {
            vacancy.IsActive = !vacancy.IsActive;
            var updatedVacancy = _mapper.Map<UpdatedVacancyDto>(_vacancyiesRepository.Update(vacancy));
            await _unitOfWork.CommitAsync();
            return Response<UpdatedVacancyDto>.Success(updatedVacancy, StatusCodes.Status200OK);
        }

        return Response<UpdatedVacancyDto>.Fail("Vacancy not found", StatusCodes.Status404NotFound, isShow: true);
    }

    public async Task<Response<NoDataDto>> CreateVacanyAsync(CreateVacancyDto newVacancy)
    {
        try
        {
            var vacancy = _mapper.Map<Vacancy>(newVacancy);
            await _vacancyiesRepository.AddAsync(vacancy);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Undefined error while adding vacancy");
            return Response<NoDataDto>.Fail("An error occurred while creating vacancy", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<NoDataDto>> RemoveVacancyAsync(string id)
    {
        try
        {
            var vacancy = await _vacancyiesRepository.GetByIdAsync(id);
            if (vacancy is not null)
            {
                _vacancyiesRepository.Remove(vacancy);
                await _unitOfWork.CommitAsync();
                return Response<NoDataDto>.Success(StatusCodes.Status204NoContent);
            }

            return Response<NoDataDto>.Fail("Vacancy not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<IEnumerable<Vacancy>>> GetAllVacanciesAsync()
    {
        try
        {
            var vacancies = await _vacancyiesRepository.GetAllAsync();
            if (vacancies is not null && vacancies.Any())
                return Response<IEnumerable<Vacancy>>.Success(vacancies, StatusCodes.Status200OK);

            return Response<IEnumerable<Vacancy>>.Fail("Vacancies not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Vacancy>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }

    }

    public async Task<Response<IEnumerable<Vacancy>>> GetVacanciesByCategoryAsync(string categoryId)
    {
        try
        {
            var vacancies = await _vacancyiesRepository.Where(v => v.CategoryId.ToString() == categoryId).ToListAsync();
            if (vacancies is not null && vacancies.Any())
                return Response<IEnumerable<Vacancy>>.Success(vacancies, StatusCodes.Status200OK);

            return Response<IEnumerable<Vacancy>>.Fail("Vacancies for this category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Vacancy>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<Vacancy>> GetVacancyAsync(string id)
    {
        var vacancy = await _vacancyiesRepository.GetByIdAsync(id);
        if (vacancy is not null)
            return Response<Vacancy>.Success(vacancy, StatusCodes.Status200OK);

        return Response<Vacancy>.Fail("Vacancy not found", StatusCodes.Status404NotFound, isShow: true);
    }

    public async Task<Response<UpdatedVacancyDto>> UpdateVacancyAsync(UpdatedVacancyDto updatedVacancy)
    {
        try
        {
            var vacancy = await _vacancyiesRepository.GetByIdAsync(updatedVacancy.Id);
            if (vacancy is not null)
            {
                vacancy = _mapper.Map<Vacancy>(updatedVacancy);
                _vacancyiesRepository.Update(vacancy);
                await _unitOfWork.CommitAsync();
                return Response<UpdatedVacancyDto>.Success(updatedVacancy, StatusCodes.Status200OK);
            }

            return Response<UpdatedVacancyDto>.Fail("Category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<UpdatedVacancyDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<IEnumerable<Vacancy>>> GetActiveVacanciesAsync()
    {
        try
        {
            var vacancies = await _vacancyiesRepository.Where(v => v.IsActive).ToListAsync();
            if (vacancies is not null && vacancies.Any())
                return Response<IEnumerable<Vacancy>>.Success(vacancies, StatusCodes.Status200OK);

            return Response<IEnumerable<Vacancy>>.Fail("There is no active vacancies", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Vacancy>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }
}
