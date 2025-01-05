using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class UpdatePaymentDtoValidator : AbstractValidator<UpdatePaymentDto>
    {
        private readonly string[] validStatuses = { "İşleniyor", "Başarılı", "Başarısız", "Doğrulandı" };

        public UpdatePaymentDtoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Durum zorunludur")
                .Must(status => validStatuses.Contains(status))
                .WithMessage("Durum şunlardan biri olmalıdır: İşleniyor, Başarılı, Başarısız, Doğrulandı");
        }
    }
} 