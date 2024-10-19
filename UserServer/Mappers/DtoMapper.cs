using AutoMapper;
using SharedLibrary.Models;
using UserServer.API.Dto;
using UserServer.Models;

namespace UserServer.API.Mappers;

public class DtoMapper : Profile
{
	public DtoMapper()
	{
		CreateMap<Applier, CreateApplierDto>().ReverseMap();
		CreateMap<Question, QuestionForUserDto>().ReverseMap();
		CreateMap<Answer, AnswerForUserDto>().ReverseMap();
	}
}
