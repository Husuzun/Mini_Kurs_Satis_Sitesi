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
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur")
                .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur")
                .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı zorunludur")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta zorunludur")
                .EmailAddress().WithMessage("Geçersiz e-posta formatı");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre zorunludur")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");

            RuleFor(x => x.City)
                .MinimumLength(2).WithMessage("Şehir en az 2 karakter olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.City));
        }
    }
}