using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        private readonly string[] validStatuses = { "Beklemede", "İşleniyor", "Tamamlandı", "İptal Edildi", "Ödendi" };

        public UpdateOrderDtoValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Durum zorunludur")
                .Must(status => validStatuses.Contains(status))
                .WithMessage("Durum şunlardan biri olmalıdır: Beklemede, İşleniyor, Tamamlandı, İptal Edildi, Ödendi");
        }
    }
} 