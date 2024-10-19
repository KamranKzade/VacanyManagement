using SharedLibrary.Dtos;
using AuthServer.API.Dtos;
using SharedLibrary.Models;
using AuthServer.API.Mapper;
using Microsoft.AspNetCore.Identity;
using AuthServer.API.Services.Abstract;

namespace AuthServer.API.Services.Concrete;

public class UserService : IUserService
{
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly ILogger<UserService> _logger;

	public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_logger = logger;
	}

	public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
	{
		try
		{
			var user = new User
			{
				Email = createUserDto.Email,
				Name = createUserDto.FirstName,
				Surname = createUserDto.LastName,
				PhoneNumber = createUserDto.PhoneNumber,
				UserName = createUserDto.UserName
			};

			var result = await _userManager.CreateAsync(user, createUserDto.Password);

			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(x => x.Description).ToList();
				_logger.LogWarning($"Failed to create user: {createUserDto.Email}\nErrors: {string.Join(", ", errors)}");
				return Response<UserAppDto>.Fail(new ErrorDto(errors, true), StatusCodes.Status400BadRequest);
			}

			if (result.Succeeded && createUserDto.UserName == "admin")
				await CreateUserRoles(new CreateUserRoleDto() { UserName = createUserDto.UserName, RoleName = "Admin" });


			_logger.LogInformation($"User created successfully: {createUserDto.Email}");
			return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), StatusCodes.Status200OK);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while creating a user: {createUserDto.Email}");
			throw;
		}
	}

	public async Task<Response<UserAppDto>> GetUserByEmailAsync(string email)
	{
		try
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
			{
				_logger.LogInformation($"User with UserName '{email}' not found");
				return Response<UserAppDto>.Fail("UserName not found", StatusCodes.Status404NotFound, true);
			}
			_logger.LogInformation($"User retrieved successfully by UserName: {email}");
			return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), StatusCodes.Status200OK);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"An error occurred while retrieving user by UserName: {email}");
			throw;
		}
	}

	public async Task<Response<NoDataDto>> CreateUserRoles(CreateUserRoleDto dto)
	{
		try
		{
			if (!await _roleManager.RoleExistsAsync(dto.RoleName))
			{
				await _roleManager.CreateAsync(new IdentityRole { Name = dto.RoleName!.ToLower() });
				_logger.LogInformation($"Role '{dto.RoleName}' created successfully.");
			}

			var user = await _userManager.FindByNameAsync(dto.UserName);

			if (user == null)
			{
				_logger.LogInformation($"User with UserName '{dto.UserName}' not found.");
				return Response<NoDataDto>.Fail("UserName not found", StatusCodes.Status404NotFound, true);
			}

			await _userManager.AddToRoleAsync(user, dto.RoleName!.ToLower());
			_logger.LogInformation($"User '{user.UserName}' assigned to role '{dto.RoleName}' successfully.");

			return Response<NoDataDto>.Success(StatusCodes.Status201Created);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while creating user roles");
			throw;
		}
	}

}
