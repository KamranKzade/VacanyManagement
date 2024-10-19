using AdminServer.API.Dtos;
using AdminServer.API.Helpers;
using AdminServer.API.Models;
using AdminServer.API.Services.Abstracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using SharedLibrary.Repositories.Abstract;
using SharedLibrary.UnitOfWork.Abstract;

namespace AdminServer.API.Services.Concretes;

public class QuestionService : IQuestionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<AppDbContext, Question> _questionsRepository;
    private readonly ILogger<QuestionService> _logger;
    private readonly IMapper _mapper;

    public QuestionService(IUnitOfWork unitOfWork, IGenericRepository<AppDbContext, Question> questionsRepository, ILogger<QuestionService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _questionsRepository = questionsRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Response<NoDataDto>> CreateQuestionAsync(CreateQuestionDto newQuestion)
    {
        try
        {
            var question = _mapper.Map<Question>(newQuestion);
            await _questionsRepository.AddAsync(question);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(StatusCodes.Status201Created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Undefined error while adding category");
            return Response<NoDataDto>.Fail("An error occurred while creating category", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<IEnumerable<Question>>> GetAllQuestionsAsync()
    {
        try
        {
            var questions = await _questionsRepository.GetIQueryable().Include(q => q.Answers).ToListAsync();
            if (questions is not null && questions.Any())
                return Response<IEnumerable<Question>>.Success(questions, StatusCodes.Status200OK);

            return Response<IEnumerable<Question>>.Fail("Questions not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Question>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<Question>> GetQuestionAsync(string id)
    {
        var question = await _questionsRepository.GetIQueryable().Include(q => q.Answers).FirstOrDefaultAsync(x => x.Id.ToString() == id);
        if (question is not null)
            return Response<Question>.Success(question, StatusCodes.Status200OK);

        return Response<Question>.Fail("Category not found", StatusCodes.Status404NotFound, isShow: true);
    }

    public async Task<Response<IEnumerable<Question>>> GetQuestionsByCategoryAsync(string categoryId)
    {
        try
        {
            var questions = await _questionsRepository.GetIQueryable().Include(q => q.Answers).Where(v => v.CategoryId.ToString() == categoryId).ToListAsync();
            if (questions is not null && questions.Any())
                return Response<IEnumerable<Question>>.Success(questions, StatusCodes.Status200OK);

            return Response<IEnumerable<Question>>.Fail("Vacancies for this category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Question>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<IEnumerable<Question>>> GetQuestionsByCategoryRandomAsync(string categoryId, int questionCount)
    {
        try
        {
            var questions = await _questionsRepository.GetIQueryable().Include(q => q.Answers).Where(v => v.CategoryId.ToString() == categoryId).ToListAsync();
            if (questions is not null && questions.Any())
                return Response<IEnumerable<Question>>.Success(questions.Shuffle().Take(questionCount), StatusCodes.Status200OK);

            return Response<IEnumerable<Question>>.Fail("Vacancies for this category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<IEnumerable<Question>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<NoDataDto>> RemoveQuestionAsync(string id)
    {
        try
        {
            var question = await _questionsRepository.GetByIdAsync(id);
            if (question is not null)
            {
                _questionsRepository.Remove(question);
                await _unitOfWork.CommitAsync();
                return Response<NoDataDto>.Success(StatusCodes.Status204NoContent);
            }

            return Response<NoDataDto>.Fail("Question not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }

    public async Task<Response<UpdatedQuestionDto>> UpdateQuestionAsync(UpdatedQuestionDto updatedQuestion)
    {
        try
        {
            var question = await _questionsRepository.GetByIdAsync(updatedQuestion.Id);
            if (question is not null)
            {
                question = _mapper.Map<Question>(updatedQuestion);
                _questionsRepository.Update(question);
                await _unitOfWork.CommitAsync();
                return Response<UpdatedQuestionDto>.Success(updatedQuestion, StatusCodes.Status200OK);
            }

            return Response<UpdatedQuestionDto>.Fail("Category not found", StatusCodes.Status404NotFound, isShow: true);
        }
        catch (Exception)
        {
            return Response<UpdatedQuestionDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
        }
    }
}
