using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is wrong");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }
}