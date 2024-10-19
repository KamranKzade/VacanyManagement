using AuthServer.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Controllers;
using AuthServer.API.Services.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : CustomBaseController
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("CreateUser")]
	public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
	{
		return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
	}

	[Authorize(Roles = "Admin")]
	[HttpPost("CreateUserRoles")]
	public async Task<IActionResult> CreateUserRoles(CreateUserRoleDto dto)
	{
		return ActionResultInstance(await _userService.CreateUserRoles(dto));
	}

}
