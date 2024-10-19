using AutoMapper;
using SharedLibrary.Dtos;
using AdminServer.API.Dtos;
using SharedLibrary.Models;
using AdminServer.API.Models;
using SharedLibrary.UnitOfWork.Abstract;
using AdminServer.API.Services.Abstracts;
using SharedLibrary.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using AdminServer.API.Dto;
using MimeKit;

namespace AdminServer.API.Services.Concretes;


public class ApplierService : IApplierService
{
	private readonly IGenericRepository<AppDbContext, ApplierForAdmin> _applierRepository;
	private readonly IUnitOfWork _unitOfWork;
	private IMapper _mapper;
	private ILogger<ApplierService> _logger;

	public ApplierService(IGenericRepository<AppDbContext, ApplierForAdmin> applierRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ApplierService> logger)
	{
		_applierRepository = applierRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Response<IEnumerable<ApplierForAdmin>>> GetAllAppliersAsync()
	{
		try
		{
			var appliers = await _applierRepository.GetAllAsync();

			if (appliers is not null && appliers.Any())
				return Response<IEnumerable<ApplierForAdmin>>.Success(appliers, StatusCodes.Status200OK);

			return Response<IEnumerable<ApplierForAdmin>>.Fail("Appliers not found", StatusCodes.Status404NotFound, isShow: true);

		}
		catch (Exception)
		{
			return Response<IEnumerable<ApplierForAdmin>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}

	public async Task<Response<NoDataDto>> CreateApplierAsync(CreateApplierDto newApplier)
	{
		try
		{
			var selectedApplier = await _applierRepository.GetIQueryable().FirstOrDefaultAsync(x => x.Email == newApplier.Email);
			if (selectedApplier == null)
			{
				var applier = _mapper.Map<ApplierForAdmin>(newApplier);
				await _applierRepository.AddAsync(applier);
				await _unitOfWork.CommitAsync();
				return Response<NoDataDto>.Success(StatusCodes.Status201Created);
			}
			else
			{
				selectedApplier.FilePath = newApplier.FilePath!;
				_applierRepository.Update(selectedApplier);
				await _unitOfWork.CommitAsync();
				return Response<NoDataDto>.Success(StatusCodes.Status204NoContent);
			}
		}
		catch (DbUpdateException)
		{
			return Response<NoDataDto>.Fail("Applier with this name already exists", StatusCodes.Status400BadRequest, isShow: true);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Undefined error while adding category");
			return Response<NoDataDto>.Fail("An error occurred while creating applier", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}

	public async Task<Response<DownloadFileDto>> DownloadFileAsync(DownloadFileRequestDto dto)
	{
		try
		{
			var applier = await _applierRepository.GetIQueryable().FirstOrDefaultAsync(x => x.Id == dto.ApplierId);
			if (applier is null)
				return Response<DownloadFileDto>.Fail("File with this id not found", StatusCodes.Status404NotFound, isShow: true);

			if (!File.Exists(applier.FilePath))
				return Response<DownloadFileDto>.Fail("File not exit in folder", StatusCodes.Status404NotFound, isShow: true);

			var fileName = Path.GetFileName(applier.FilePath);
			var contentType = MimeTypes.GetMimeType(fileName);
			var file = await File.ReadAllBytesAsync(applier.FilePath);

			var result = new DownloadFileDto()
			{
				ContentType = contentType,
				File = file,
				FileName = fileName
			};

			return Response<DownloadFileDto>.Success(result, StatusCodes.Status200OK);
		}
		catch (Exception)
		{
			return Response<DownloadFileDto>.Fail("Internal Server Error", StatusCodes.Status404NotFound, isShow: true);

		}
	}
}
