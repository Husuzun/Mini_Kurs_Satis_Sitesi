using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using System.Text.RegularExpressions;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.FirstName));

            RuleFor(x => x.LastName)
                .Length(2, 50).WithMessage("Soyad 2 ile 50 karakter arasında olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.LastName));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Geçersiz e-posta formatı")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.UserName)
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.UserName));

            RuleFor(x => x.City)
                .MinimumLength(2).WithMessage("Şehir en az 2 karakter olmalıdır")
                .When(x => !string.IsNullOrEmpty(x.City));

            When(x => !string.IsNullOrEmpty(x.CurrentPassword) || !string.IsNullOrEmpty(x.NewPassword), () =>
            {
                RuleFor(x => x.CurrentPassword)
                    .NotEmpty().WithMessage("Mevcut şifre zorunludur");

                RuleFor(x => x.NewPassword)
                    .NotEmpty().WithMessage("Yeni şifre zorunludur")
                    .Must(password => Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$"))
                    .WithMessage("Şifre en az 6 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");
            });
        }
    }
} 