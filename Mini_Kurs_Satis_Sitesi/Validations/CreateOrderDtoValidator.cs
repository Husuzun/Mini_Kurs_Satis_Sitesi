using FluentValidation;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.API.Validations
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("En az bir kurs seçilmelidir")
                .Must(x => x != null && x.Count > 0).WithMessage("Sipariş en az bir kurs içermelidir");

            RuleForEach(x => x.OrderItems).SetValidator(new OrderItemCreateDtoValidator());
        }
    }

    public class OrderItemCreateDtoValidator : AbstractValidator<OrderItemCreateDto>
    {
        public OrderItemCreateDtoValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Lütfen geçerli bir kurs seçiniz");
        }
    }
} 