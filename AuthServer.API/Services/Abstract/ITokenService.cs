using AuthServer.API.Dtos;
using AuthServer.API.Configurations;
using SharedLibrary.Models;

namespace AuthServer.API.Services.Abstract;

public interface ITokenService
{
	TokenDto CreateToken(User userApp);
	ClientTokenDto CreateTokenByClient(Client client);
}
