using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kurs adı zorunludur")
                .MaximumLength(100).WithMessage("Kurs adı 100 karakterden uzun olamaz");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Kurs açıklaması zorunludur")
                .MaximumLength(1000).WithMessage("Kurs açıklaması 1000 karakterden uzun olamaz");

            RuleFor(x => x.Price)
                .NotNull().WithMessage("Kurs fiyatı zorunludur")
                .GreaterThanOrEqualTo(0).WithMessage("Kurs fiyatı negatif olamaz");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Kategori zorunludur")
                .MaximumLength(50).WithMessage("Kategori 50 karakterden uzun olamaz");
        }
    }
} 