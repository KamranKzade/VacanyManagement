using SharedLibrary.Dtos;
using AuthServer.API.Dtos;

namespace AuthServer.API.Services.Abstract;

public interface IUserService
{
	Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
	Task<Response<UserAppDto>> GetUserByEmailAsync(string email);
	Task<Response<NoDataDto>> CreateUserRoles(CreateUserRoleDto dto);
}
