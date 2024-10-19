using AdminServer.API.Dtos;
using AdminServer.API.Models;
using AdminServer.API.Services.Abstracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using SharedLibrary.Repositories.Abstract;
using SharedLibrary.UnitOfWork.Abstract;

namespace AdminServer.API.Services.Concretes
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<AppDbContext, Answer> _answersRepository;
        private readonly ILogger<AnswerService> _logger;
        private readonly IMapper _mapper;

        public AnswerService(IUnitOfWork unitOfWork, IGenericRepository<AppDbContext, Answer> answersRepository, ILogger<AnswerService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _answersRepository = answersRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response<NoDataDto>> CreateAnswerAsync(CreateAnswerDto newAnswer)
        {
            try
            {
                var answer = _mapper.Map<Answer>(newAnswer);
                await _answersRepository.AddAsync(answer);
                await _unitOfWork.CommitAsync();
                return Response<NoDataDto>.Success(StatusCodes.Status201Created);
            }
            catch (DbUpdateException)
            {
                return Response<NoDataDto>.Fail("Question id not found", StatusCodes.Status400BadRequest, isShow: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Undefined error while adding category");
                return Response<NoDataDto>.Fail("An error occurred while creating category", StatusCodes.Status500InternalServerError, isShow: true);
            }
        }

        public async Task<Response<Answer>> GetAnswerAsync(string id)
        {
            var answer = await _answersRepository.GetIQueryable().Include(a => a.Question).FirstOrDefaultAsync(a => a.Id.ToString() == id);
            if (answer is not null)
                return Response<Answer>.Success(answer, StatusCodes.Status200OK);

            return Response<Answer>.Fail("Answer not found", StatusCodes.Status404NotFound, isShow: true);
        }

        public async Task<Response<IEnumerable<Answer>>> GetAnswersByQuestionIdAsync(string questionId)
        {
            try
            {
                var answers = await _answersRepository.GetIQueryable().Include(a => a.Question).Where(v => v.QuestionId.ToString() == questionId).ToListAsync();
                if (answers is not null && answers.Any())
                    return Response<IEnumerable<Answer>>.Success(answers, StatusCodes.Status200OK);

                return Response<IEnumerable<Answer>>.Fail("Question id is not correct or question does not have answers yet", StatusCodes.Status404NotFound, isShow: true);
            }
            catch (Exception)
            {
                return Response<IEnumerable<Answer>>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
            }
        }

        public async Task<Response<NoDataDto>> RemoveAnswerAsync(string id)
        {
            try
            {
                var answer = await _answersRepository.GetByIdAsync(id);
                if (answer is not null)
                {
                    _answersRepository.Remove(answer);
                    await _unitOfWork.CommitAsync();
                    return Response<NoDataDto>.Success(StatusCodes.Status204NoContent);
                }

                return Response<NoDataDto>.Fail("Asnwer not found", StatusCodes.Status404NotFound, isShow: true);
            }
            catch (Exception)
            {
                return Response<NoDataDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
            }
        }

        public async Task<Response<UpdatedAnswerDto>> UpdateAnswerAsync(UpdatedAnswerDto updatedAnswer)
        {
            try
            {
                var answer = await _answersRepository.GetByIdAsync(updatedAnswer.Id);
                if (answer is not null)
                {
                    _mapper.Map(updatedAnswer, answer);
                    _answersRepository.Update(answer);
                    await _unitOfWork.CommitAsync();
                    return Response<UpdatedAnswerDto>.Success(updatedAnswer, StatusCodes.Status200OK);
                }

                return Response<UpdatedAnswerDto>.Fail("Answer not found", StatusCodes.Status404NotFound, isShow: true);
            }
            catch (Exception)
            {
                return Response<UpdatedAnswerDto>.Fail("Internal server error", StatusCodes.Status500InternalServerError, isShow: true);
            }
        }
    }
}
