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

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<AppDbContext, Category> _catgoriesRepository;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork unitOfWork, IGenericRepository<AppDbContext, Category> catgoriesRepository, ILogger<CategoryService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _catgoriesRepository = catgoriesRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Response<NoDataDto>> CreateCategoryAsync(CreateCategoryDto newCategory)
    {
        try
        {
            var category = _mapper.Map<Category>(newCategory);
            await _catgoriesRepository.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(StatusCodes.Status201Created);
        }
        catch (DbUpdateException)
        {
            return Response<NoDataDto>.Fail("Category with this name already exists", StatusCodes.Status400BadRequest, isShow: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Undefined error while adding category");
            return Response<NoDataDto>.Fail("An error occurred while creating category", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<IEnumerable<Category>>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _catgoriesRepository.GetAllAsync();
            if (categories is not null && categories.Any())
                return Response<IEnumerable<Category>>.Success(categories, StatusCodes.Status200OK);

            return Response<IEnumerable<Category>>.Fail("Categories not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Category>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<Category>> GetCategoryAsync(string id)
    {
        var category = await _catgoriesRepository.GetByIdAsync(id);
        if (category is not null)
            return Response<Category>.Success(category, StatusCodes.Status200OK);

        return Response<Category>.Fail("Category not found", StatusCodes.Status404NotFound, isShow: true);
    }

    public async Task<Response<NoDataDto>> RemoveCategoryAsync(string id)
    {
        try
        {
            var category = await _catgoriesRepository.GetByIdAsync(id);
            if (category is not null)
            {
                _catgoriesRepository.Remove(category);
                await _unitOfWork.CommitAsync();
                return Response<NoDataDto>.Success(StatusCodes.Status204NoContent);
            }

            return Response<NoDataDto>.Fail("Category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<UpdatedCategoryDto>> UpdateCategoryAsync(UpdatedCategoryDto updatedCategory)
    {
        try
        {
            var category = await _catgoriesRepository.GetByIdAsync(updatedCategory.Id);
            if (category is not null)
            {
                category = _mapper.Map<Category>(updatedCategory);
                _catgoriesRepository.Update(category);
                await _unitOfWork.CommitAsync();
                return Response<UpdatedCategoryDto>.Success(updatedCategory,StatusCodes.Status200OK);
            }

            return Response<UpdatedCategoryDto>.Fail("Category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch(DbUpdateException) 
        {
            return Response<UpdatedCategoryDto>.Fail("Category with this name already exists", StatusCodes.Status400BadRequest, isShow: true);
        }
        catch (Exception)
        {
            return Response<UpdatedCategoryDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }
}
