using AdminServer.API.Dtos;
using AdminServer.API.Models;
using AdminServer.API.Services.Abstracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using SharedLibrary.Repositories.Abstract;
using SharedLibrary.UnitOfWork.Abstract;
using System.Text.Json;

namespace AdminServer.API.Services.Concretes
{
	public class ResultService : IResultService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<AppDbContext, Result> _resultRepository;
		private readonly IGenericRepository<AppDbContext, ApplierForAdmin> _applierRepository;
		private readonly ILogger<CategoryService> _logger;
		private readonly IMapper _mapper;
		private readonly IGenericRepository<AppDbContext, Question> _questionRepository;

		public ResultService(IUnitOfWork unitOfWork, IGenericRepository<AppDbContext, Result> resultRepository, ILogger<CategoryService> logger, IMapper mapper, IGenericRepository<AppDbContext, Question> questionRepository)
		{
			_unitOfWork = unitOfWork;
			_resultRepository = resultRepository;
			_logger = logger;
			_mapper = mapper;
			_questionRepository = questionRepository;
		}

		public async Task<Response<IEnumerable<Result>>> GetAllResult()
		{
			try
			{
				var result = await _resultRepository.GetIQueryable().Include(x => x.Vacancy).Include(x => x.Applier).ToListAsync();
				if (result is not null && result.Any())
					return Response<IEnumerable<Result>>.Success(result, StatusCodes.Status200OK);

				return Response<IEnumerable<Result>>.Fail("Result not found", StatusCodes.Status404NotFound, isShow: true);
			}
			catch (Exception)
			{
				return Response<IEnumerable<Result>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
			}
		}

		public async Task<Response<Result>> GetResultByAppierId(AnswerForResultDto dto)
		{
			var result = await _resultRepository.GetIQueryable().Include(x => x.Vacancy).Include(x => x.Applier).FirstOrDefaultAsync(m => m.Applier.Id.ToString() == dto.AppierId);
			if (result is not null)
			{
				Dictionary<string, string> dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(dto.Answer)!;
				int count = 0;
				int correctAnswerCounter = 0;

				foreach (var item in dictionary)
				{
					var question = await _questionRepository.GetIQueryable().Include(x=>x.Answers).FirstOrDefaultAsync(x => x.Id.ToString() == item.Key);
					if(question.Answers.FirstOrDefault(x=>item.Value==x.Id.ToString())!.IsTrue)
						correctAnswerCounter++;

					count++;

				}

				result.Score = correctAnswerCounter;
				result.ScorePercent = (correctAnswerCounter / count) * 100;

				_resultRepository.Update(result);
				await _unitOfWork.CommitAsync();
				
				return Response<Result>.Success(result, StatusCodes.Status200OK);
			}

			return Response<Result>.Fail("Result not found", StatusCodes.Status404NotFound, isShow: true);
		}
	}
}
