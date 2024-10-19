using AdminServer.API.Dtos;
using AutoMapper;
using SharedLibrary.Models;

namespace AdminServer.API.Mappers;

public class DtoMapper : Profile
{
    public DtoMapper()
    {
        CreateMap<CreateVacancyDto, Vacancy>();
        CreateMap<UpdatedVacancyDto, Vacancy>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdatedCategoryDto, Category>().ReverseMap();
        CreateMap<CreateAnswerDto, Answer>();
        CreateMap<UpdatedAnswerDto, Answer>().ReverseMap();
        CreateMap<CreateQuestionDto, Question>();
        CreateMap<UpdatedQuestionDto, Question>().ReverseMap();
        CreateMap<ApplierForAdmin, CreateApplierDto>().ReverseMap();
    }
}
