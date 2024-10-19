using AdminServer.API.Dtos;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
namespace AdminServer.API.Services.Abstracts;

public interface IQuestionService
{
    Task<Response<NoDataDto>> CreateQuestionAsync(CreateQuestionDto newQuestion);
    Task<Response<UpdatedQuestionDto>> UpdateQuestionAsync(UpdatedQuestionDto updatedQuestion);
    Task<Response<IEnumerable<Question>>> GetAllQuestionsAsync();
    Task<Response<Question>> GetQuestionAsync(string id);
    Task<Response<IEnumerable<Question>>> GetQuestionsByCategoryAsync(string categoryId);
    Task<Response<IEnumerable<Question>>> GetQuestionsByCategoryRandomAsync(string categoryId,int questionCount);
    Task<Response<NoDataDto>> RemoveQuestionAsync(string id);
}
