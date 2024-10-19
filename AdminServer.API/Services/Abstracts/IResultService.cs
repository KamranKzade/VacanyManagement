using AdminServer.API.Dtos;
using SharedLibrary.Dtos;
using SharedLibrary.Models;

namespace AdminServer.API.Services.Abstracts;

public interface IResultService
{
	Task<Response<Result>> PostResultByAppierId(AnswerForResultDto dto);
	Task<Response<IEnumerable<Result>>> GetAllResult();
}
