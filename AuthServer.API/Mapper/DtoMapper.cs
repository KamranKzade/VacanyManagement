using AutoMapper;
using AuthServer.API.Dtos;
using SharedLibrary.Models;

namespace AuthServer.API.Mapper;

public class DtoMapper : Profile
{
	public DtoMapper()
	{
		CreateMap<UserAppDto, User>().ReverseMap();
	}
}