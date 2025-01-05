using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using System.Text.RegularExpressions;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class CreatePaymentDtoValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentDtoValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Lütfen geçerli bir sipariş seçiniz");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Ödeme yöntemi zorunludur");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Kart numarası zorunludur")
                .Must(cardNumber => Regex.IsMatch(cardNumber, @"^\d{16}$"))
                .WithMessage("Kart numarası 16 haneli olmalıdır");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty().WithMessage("Son kullanma ayı zorunludur")
                .Must(month => Regex.IsMatch(month, @"^(0[1-9]|1[0-2])$"))
                .WithMessage("Son kullanma ayı 01-12 arasında olmalıdır");

            RuleFor(x => x.ExpiryYear)
                .NotEmpty().WithMessage("Son kullanma yılı zorunludur")
                .Must(year => Regex.IsMatch(year, @"^20\d{2}$"))
                .WithMessage("Lütfen geçerli bir yıl giriniz");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV zorunludur")
                .Must(cvv => Regex.IsMatch(cvv, @"^\d{3}$"))
                .WithMessage("CVV 3 haneli olmalıdır");

            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Kart sahibinin adı zorunludur")
                .Length(3, 100).WithMessage("Kart sahibinin adı 3 ile 100 karakter arasında olmalıdır");
        }
    }
} 