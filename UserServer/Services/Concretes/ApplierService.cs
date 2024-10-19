using UserServer.Models;
using SharedLibrary.Dtos;
using UserServer.API.Dto;
using UserServer.API.Config;
using UserServer.API.Extentions;
using UserServer.API.Services.Abstracts;
using SharedLibrary.UnitOfWork.Abstract;
using SharedLibrary.Repositories.Abstract;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using SharedLibrary.Helpers;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using UserServer.API.Dtos;
using System.Collections.Generic;

namespace UserServer.Services.Concretes;

public class ApplierService : IApplierService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<AppDbContext, Applier> _applierRepository;
	private readonly ILogger<ApplierService> _logger;
	private readonly UploadConfig _uploadConfig;
	private readonly AttachmentConfig _attachmentConfig;
	private readonly IMapper _mapper;
	private readonly IConfiguration _configuration;
	private readonly IHttpClientFactory _httpClientFactory;

	public ApplierService(UploadConfig uploadConfig, AttachmentConfig attachmentConfig, IGenericRepository<AppDbContext, Applier> applierRepository, ILogger<ApplierService> logger, IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpClientFactory httpClientFactory)
	{
		_uploadConfig = uploadConfig;
		_attachmentConfig = attachmentConfig;
		_applierRepository = applierRepository;
		_logger = logger;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_configuration = configuration;
		_httpClientFactory = httpClientFactory;
	}

	public async Task<Response<NoDataDto>> AttachFileAsync(AttachFileDto dto, string authorizationToken)
	{
		try
		{
			int size = dto.File.GetFileSizeInMB();
			if (size > _uploadConfig.SizeLimitInMegabytes)
				throw new ApplicationException($"File size must be smaller than {_uploadConfig.SizeLimitInMegabytes} MB");


			var userItem = await _applierRepository.GetIQueryable().FirstOrDefaultAsync(m => m.Id == dto.Id);

			if (userItem == null)
				return Response<NoDataDto>.Fail("Not found any user item with this id", StatusCodes.Status404NotFound, isShow: true);


			var totalPath = String.Empty;
			string fileExtenstion = Path.GetExtension(dto.File.FileName);

			if (!(fileExtenstion != "docs" || fileExtenstion != "pdf"))
				return Response<NoDataDto>.Fail("File format wrong sending", StatusCodes.Status404NotFound, isShow: true);


			string currentFileNameWithoutExtension = Path.GetFileNameWithoutExtension(dto.File.FileName).Replace(" ", "");

			string fileName = currentFileNameWithoutExtension + "_" + DateTime.Now.ToString(_attachmentConfig.DateFormat) + fileExtenstion;
			if (fileName.Length > 100)
				return Response<NoDataDto>.Fail("File name is too long", StatusCodes.Status404NotFound, isShow: true);

			totalPath = Path.Combine(totalPath, _attachmentConfig.BasePath);

			if (!Directory.Exists(totalPath))
				Directory.CreateDirectory(totalPath);

			string filePath = Path.Combine(totalPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await dto.File.CopyToAsync(stream);
			}

			userItem.FilePath = filePath;

			_applierRepository.Update(userItem);
			await _unitOfWork.CommitAsync();

			var updateApplierResult = await CreateApplierAsync(userItem, authorizationToken);

			if (updateApplierResult.IsSuccessfull)
				return Response<NoDataDto>.Success(StatusCodes.Status201Created);
			return Response<NoDataDto>.Fail("Applier don't update AdminServer", StatusCodes.Status400BadRequest, isShow: true);
		}
		catch (Exception)
		{
			return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);

		}

	}

	public async Task<Response<NoDataDto>> CreateApplierAsync(CreateApplierDto newApplier, string authorizationToken)
	{
		try
		{
			var applier = _mapper.Map<Applier>(newApplier);
			applier.AppliedDate = DateTime.Now;
			await _applierRepository.AddAsync(applier);
			await _unitOfWork.CommitAsync();

			var createdResult = await CreateApplierAsync(applier, authorizationToken);

			if (createdResult.IsSuccessfull)
				return Response<NoDataDto>.Success(StatusCodes.Status201Created);
			return Response<NoDataDto>.Fail("Applier don't created AdminServer", StatusCodes.Status400BadRequest, isShow: true);
		}
		catch (DbUpdateException)
		{
			return Response<NoDataDto>.Fail("Applier with this name already exists", StatusCodes.Status400BadRequest, isShow: true);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Undefined error while adding applier");
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

	public async Task<Response<IEnumerable<Applier>>> GetAllAppliersAsync()
	{
		try
		{
			var appliers = await _applierRepository.GetAllAsync();

			if (appliers is not null && appliers.Any())
				return Response<IEnumerable<Applier>>.Success(appliers, StatusCodes.Status200OK);

			return Response<IEnumerable<Applier>>.Fail("Appliers not found", StatusCodes.Status404NotFound, isShow: true);

		}
		catch (Exception)
		{
			return Response<IEnumerable<Applier>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}

	public async Task<Response<Applier>> GetApplierAsync(string id)
	{
		try
		{
			var applier = await _applierRepository.GetIQueryable().FirstOrDefaultAsync(v => v.Id.ToString() == id);
			if (applier is not null)
				return Response<Applier>.Success(applier, StatusCodes.Status200OK);

			return Response<Applier>.Fail("Applier for this id not found", StatusCodes.Status404NotFound, isShow: true);
		}
		catch (Exception)
		{
			return Response<Applier>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}

	public async Task<Response<IEnumerable<QuestionForUserDto>>> GetQuestionByCategoryRandomAsync(string categoryId, int questionCount, string authorizationToken)
	{
		var policy = RetryPolicyHelper.GetRetryPolicy();

		try
		{
			string identityServiceBaseUrl = _configuration["Microservices:AdminServer"];
			string getQuestionByCategoryEndPoint = $"{identityServiceBaseUrl}/api/Question/getRandomByCategory?categoryId={categoryId}&questionCount={questionCount}";

			var client = _httpClientFactory.CreateClient();

			using (client)
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

				var responce = await policy.ExecuteAsync(async () =>
				{
					return await client.GetAsync(getQuestionByCategoryEndPoint);
				});

				if (responce.IsSuccessStatusCode)
				{
					var responseContent = await responce.Content.ReadAsStringAsync();

					var questionDto = JsonConvert.DeserializeObject<Response<IEnumerable<QuestionForUserDto>>>(responseContent);

					return Response<IEnumerable<QuestionForUserDto>>.Success(questionDto.Data, StatusCodes.Status200OK);

				}
				else
					return Response<IEnumerable<QuestionForUserDto>>.Fail("Questions could not be obtained", StatusCodes.Status404NotFound, isShow: true);

			}
		}
		catch (Exception)
		{
			return Response<IEnumerable<QuestionForUserDto>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}


	public async Task<Response<NoDataDto>> CreateApplierAsync(Applier applier, string authorizationToken)
	{
		var policy = RetryPolicyHelper.GetRetryPolicy();

		try
		{
			string identityServiceBaseUrl = _configuration["Microservices:AdminServer"];
			string createApplier = $"{identityServiceBaseUrl}/api/applier";

			var client = _httpClientFactory.CreateClient();

			var jsonContent = JsonConvert.SerializeObject(applier);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


			using (client)
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

				var responce = await policy.ExecuteAsync(async () =>
				{
					return await client.PostAsync(createApplier, content);
				});

				if (responce.IsSuccessStatusCode)
					return Response<NoDataDto>.Success(StatusCodes.Status201Created);
				else
					return Response<NoDataDto>.Fail("Applied do not created", StatusCodes.Status404NotFound, isShow: true);

			}
		}
		catch (Exception)
		{
			return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}

	public async Task<Response<NoDataDto>> SendAnswerAsync(SendAnswerDto dto, string authorizationToken)
	{
		var applier = await _applierRepository.GetIQueryable().FirstOrDefaultAsync(m => m.Id.ToString() == dto.AppierId);

		applier.SendAnswerResponse = string.Join(",", dto.Answer.Select(kvp => $"{kvp.Key}={kvp.Value}"));
		_applierRepository.Update(applier);
		await _unitOfWork.CommitAsync();


		var policy = RetryPolicyHelper.GetRetryPolicy();

		try
		{
			string identityServiceBaseUrl = _configuration["Microservices:AdminServer"];
			string createApplier = $"{identityServiceBaseUrl}/api/Result";

			var client = _httpClientFactory.CreateClient();

			var jsonContent = JsonConvert.SerializeObject(dto);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


			using (client)
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

				var responce = await policy.ExecuteAsync(async () =>
				{
					return await client.PostAsync(createApplier, content);
				});

				if (responce.IsSuccessStatusCode)
					return Response<NoDataDto>.Success(StatusCodes.Status201Created);
				else
					return Response<NoDataDto>.Fail("Answer do not send AdminService", StatusCodes.Status404NotFound, isShow: true);

			}
		}
		catch (Exception)
		{
			return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
		}
	}
}

