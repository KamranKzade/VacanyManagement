using SharedLibrary.Dtos;
using SharedLibrary.Models;
using UserServer.API.Dto;
using UserServer.API.Dtos;
using UserServer.Models;

namespace UserServer.API.Services.Abstracts;

public interface IApplierService
{
	Task<Response<NoDataDto>> CreateApplierAsync(CreateApplierDto newApplier, string authorizationToken);
	Task<Response<IEnumerable<Applier>>> GetAllAppliersAsync();
	Task<Response<Applier>> GetApplierAsync(string id);
	Task<Response<NoDataDto>> AttachFileAsync(AttachFileDto dto, string authorizationToken);
	Task<Response<DownloadFileDto>> DownloadFileAsync(DownloadFileRequestDto dto);

	Task<Response<IEnumerable<QuestionForUserDto>>> GetQuestionByCategoryRandomAsync(string categoryId, int questionCount, string authorizationToken);
	Task<Response<NoDataDto>> SendAnswerAsync(SendAnswerDto dto, string authorizationToken);


}
