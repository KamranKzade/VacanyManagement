using AdminServer.API.Dtos;
using SharedLibrary.Dtos;
using SharedLibrary.Models;

namespace AdminServer.API.Services.Abstracts
{
    public interface IAnswerService
    {
        Task<Response<NoDataDto>> CreateAnswerAsync(CreateAnswerDto newAnswer);
        Task<Response<NoDataDto>> RemoveAnswerAsync(string id);
        Task<Response<UpdatedAnswerDto>> UpdateAnswerAsync(UpdatedAnswerDto updatedAnswer);
        Task<Response<Answer>> GetAnswerAsync(string id);
        Task<Response<IEnumerable<Answer>>> GetAnswersByQuestionIdAsync(string questionId);
    }
}
