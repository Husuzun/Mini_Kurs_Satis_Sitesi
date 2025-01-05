using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class CategoryDtoValidator : AbstractValidator<CategoryDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Kategori zorunludur")
                .MaximumLength(50).WithMessage("Kategori 50 karakterden uzun olamaz");
        }
    }
} 