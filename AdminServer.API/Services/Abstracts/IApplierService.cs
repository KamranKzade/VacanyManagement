using SharedLibrary.Dtos;
using AdminServer.API.Dtos;
using AdminServer.API.Dto;
using SharedLibrary.Models;

namespace AdminServer.API.Services.Abstracts;

public interface IApplierService
{
	Task<Response<NoDataDto>> CreateApplierAsync(CreateApplierDto newApplier);
	Task<Response<DownloadFileDto>> DownloadFileAsync(DownloadFileRequestDto dto);
	Task<Response<IEnumerable<ApplierForAdmin>>> GetAllAppliersAsync();
}
