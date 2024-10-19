﻿using AuthServer.API.Dtos;
using System.Security.Claims;
using SharedLibrary.Services;
using SharedLibrary.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using AuthServer.API.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AuthServer.API.Services.Abstract;
using SharedLibrary.Models;

namespace AuthServer.API.Services.Concrete;

public class TokenService : ITokenService
{
	private readonly UserManager<User> _userManager;
	private readonly CustomTokenOption _tokenOption;
	private readonly ILogger<TokenService> _logger;

	public TokenService(UserManager<User> userManager, IOptions<CustomTokenOption> options, ILogger<TokenService> logger)
	{
		_userManager = userManager;
		_tokenOption = options.Value;
		_logger = logger;
	}


	// Reflesh Token yaradiriq
	private string CreateRefleshToken()
	{
		try
		{
			var numberByte = new Byte[32];

			// Random token aliriq, guidden daha etibarlidir
			using var rnd = RandomNumberGenerator.Create();

			rnd.GetBytes(numberByte);

			var refreshToken = Convert.ToBase64String(numberByte);

			_logger.LogInformation($"Refresh token created successfully: {refreshToken}");
			return refreshToken;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while creating a refresh token");
			throw;
		}
	}

	// Uygun uzere gore claımlerı alırıq
	private async Task<IEnumerable<Claim>> GetClaims(User userApp, List<string> audiences)
	{
		try
		{
			// userin role-larini aliriq
			var userRoles = await _userManager.GetRolesAsync(userApp);

			var userList = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userApp.Id),
				new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
				new Claim(ClaimTypes.Name, userApp.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
			userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
			userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

			_logger.LogInformation($"Claims generated successfully for user: {userApp.Id}");
			return userList;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while generating claims for user: {userApp.Id}");
			throw;
		}
	}

	private IEnumerable<Claim> GetClaimByClient(Client client)
	{
		try
		{
			var claims = new List<Claim>();
			claims.AddRange(client.Audiences!.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
			new Claim(JwtRegisteredClaimNames.Sub, client.Id!.ToString());

			_logger.LogInformation($"Generated claims for client: {client.Id}");
			return claims;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while generating claims for client: {client.Id}");
			throw;
		}
	}


	public TokenDto CreateToken(User userApp)
	{
		try
		{
			// Tokenle bagli olan melumatlar
			var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
			var refleshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefleshTokenExpiration);
			var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey!);

			// credentiallari yaradiriq
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			// jwt token aliriq
			var jwttoken = new JwtSecurityToken
				(
					issuer: _tokenOption.Issuer,
					expires: accessTokenExpiration,
					notBefore: DateTime.MinValue,
					claims: GetClaims(userApp, _tokenOption.Audience!).Result,
					signingCredentials: credentials
				);

			// token yaratmaq ucun lazim olan handler
			var handler = new JwtSecurityTokenHandler();

			// handler uzerinden token yazmaq
			var token = handler.WriteToken(jwttoken);

			var tokenDto = new TokenDto
			{
				AccessToken = token,
				RefleshToken = CreateRefleshToken(),
				AccessTokenExpiration = accessTokenExpiration,
				RefleshTokenExpiration = refleshTokenExpiration,
			};

			_logger.LogInformation($"Generated a token for user: {userApp.Id}");
			return tokenDto;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while generating a token for user: {userApp.Id}");
			throw;
		}
	}

	public ClientTokenDto CreateTokenByClient(Client client)
	{
		try
		{
			// Tokenle bagli olan melumatlar
			var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
			var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey!);

			// credentiallari yaradiriq
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			// jwt token aliriq
			var jwttoken = new JwtSecurityToken
				(
					issuer: _tokenOption.Issuer,
					expires: accessTokenExpiration,
					notBefore: DateTime.MinValue,
					claims: GetClaimByClient(client),
					signingCredentials: credentials
				);

			// token yaratmaq ucun lazim olan handler
			var handler = new JwtSecurityTokenHandler();

			// handler uzerinden token yazmaq
			var token = handler.WriteToken(jwttoken);

			var clientTokenDto = new ClientTokenDto
			{
				AccessToken = token,
				AccessTokenExpiration = accessTokenExpiration,
			};

			_logger.LogInformation($"Token generated successfully for client: {client.Id}");
			return clientTokenDto;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while generating a token for the client: {client.Id}");
			throw;
		}
	}
}
