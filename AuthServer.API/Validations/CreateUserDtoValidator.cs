﻿using FluentValidation;
using AuthServer.API.Dtos;

namespace AuthServer.API.Validations;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
	public CreateUserDtoValidator()
	{
		RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is wrong");
		RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
	}
}
