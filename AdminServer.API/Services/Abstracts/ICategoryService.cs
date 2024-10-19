using AdminServer.API.Dtos;
using SharedLibrary.Dtos;
using SharedLibrary.Models;

namespace AdminServer.API.Services.Abstracts;

public interface ICategoryService
{
    Task<Response<Category>> GetCategoryAsync(string id);
    Task<Response<NoDataDto>>  CreateCategoryAsync(CreateCategoryDto newCategory);
    Task<Response<UpdatedCategoryDto>> UpdateCategoryAsync(UpdatedCategoryDto updatedCategory);
    Task<Response<IEnumerable<Category>>> GetAllCategoriesAsync();
    Task<Response<NoDataDto>> RemoveCategoryAsync(string id);
}
